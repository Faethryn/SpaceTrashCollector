using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Ship : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _shipBody;

    [Header("Cameras")]
    [SerializeField]
    private CameraSetup _cameraSetupCockpit;

    [SerializeField]
    private CameraSetup _cameraSetupBehind;

    [Header("Thrusters")]
    [SerializeField]
    private ThrustSound _thrustSound;

    [SerializeField]
    private Thruster _leftThruster;

    [SerializeField]
    private Thruster _rightThruster;

    [SerializeField]
    private FuelManager _fuelManager;

    [SerializeField]
    private float _fuelConsumptionPerThrust = 1f;

    [SerializeField]
    private TextMeshProUGUI _ModeText;

    [SerializeField]
    private AimingReticle _aimingReticle;

    [SerializeField]
    private float _addedTorqueInCombat = 100f;

    [SerializeField]
    private PlayerInput _input;

    [Header("HP")]
    [SerializeField]
    private SpaceShipHit _hitComponent;

    [SerializeField]
    private float _fuelLostPerHit = 50.0f;

    private InputActionMap _spaceShipActionMap;

    private InputAction _thrustRight;

    private InputAction _thrustLeft;

    private InputAction _orientLeft;

    private InputAction _orientRight;

    private InputAction _perspectiveChange;

    private InputAction _movementModeChange;

    private InputAction _shoot;

    [SerializeField]
    private float _delayBetweenShots = 1f;

    private float _lastShotTime = 0f;

    private Camera _cam;

    private PerspectiveState _currentPerspective = PerspectiveState.Back;

    private MovementMode _movementMode = MovementMode.Mobility;

    [Header("Teams")]
    [SerializeField]
    private ShipPlayer _player = ShipPlayer.Blue;

    [SerializeField]
    private Material _redMaterial;

    [SerializeField]
    private Material _blueMaterial;

    [SerializeField]
    private MeshRenderer _renderer;

    [SerializeField]
    private TrailRenderer _trailRight;
    [SerializeField]
    private TrailRenderer _trailLeft;

    [SerializeField]
    private Gradient _trailGradientRed;

    [SerializeField]
    private Gradient _trailGradientBlue;

    private void Awake()
    {
        _spaceShipActionMap = _input.actions.FindActionMap("SpaceShip", throwIfNotFound: true);

        _thrustRight = _spaceShipActionMap.FindAction("ThrustRight", throwIfNotFound: true);
        _thrustLeft = _spaceShipActionMap.FindAction("ThrustLeft", throwIfNotFound: true);
        _orientLeft = _spaceShipActionMap.FindAction("OrientLeft", throwIfNotFound: true);
        _orientRight = _spaceShipActionMap.FindAction("OrientRight", throwIfNotFound: true);
        _perspectiveChange = _spaceShipActionMap.FindAction("ChangePerspective", throwIfNotFound: true);
        _shoot = _spaceShipActionMap.FindAction("Shoot", throwIfNotFound: true);
        _movementModeChange = _spaceShipActionMap.FindAction("MovementModeToggle", throwIfNotFound: true);
        _cam = _input.camera;

        PlayerCounter.Instance.PlayerJoined.AddListener(OnPlayerJoined);


        int playerId = _input.playerIndex;

        GameManager.Instance.PlayerHasJoined(playerId, this);

        if (playerId == 0)
        {
            _cam.rect = new Rect(0, 0.5f, 1f, 1f);
            _player = ShipPlayer.Red;
            _renderer.material = _redMaterial;
            _trailRight.colorGradient = _trailGradientRed;
            _trailLeft.colorGradient = _trailGradientRed;
            _aimingReticle.SetPlayer(_player);

        }

        if (playerId == 1)
        {
            _cam.rect = new Rect(0, 0, 1f, 0.5f);
            _player = ShipPlayer.Blue;
            _renderer.material = _blueMaterial;
            _trailRight.colorGradient = _trailGradientBlue;
            _trailLeft.colorGradient = _trailGradientBlue;
            _aimingReticle.SetPlayer(_player);
        }


        InitializeCamera();
    }

    private void OnDestroy()
    {
        if(PlayerCounter.Instance == null)
        {
            return;
        }

        PlayerCounter.Instance.PlayerJoined.RemoveListener(OnPlayerJoined);

    }

    private void OnPlayerJoined()
    {
        if (PlayerCounter.Instance._players == 2)
        {
            int playerId = _input.playerIndex;

            if (playerId == 0)
            {
                _cam.rect = new Rect(0, 0.5f, 1f, 1f);

            }

            if (playerId == 1)
            {
                _cam.rect = new Rect(0, 0, 1f, 0.5f);

            }
        }
    }

    private void OnEnable()
    {
        _perspectiveChange.Enable();
        _perspectiveChange.performed += OnPerspectiveChange;
        
        _movementModeChange.Enable();
        _movementModeChange.performed += OnMovementModeChange;
        
        _thrustRight.Enable();
        
        _thrustLeft.Enable();
       
        _orientLeft.Enable();
        
        _orientRight.Enable();
        
        _shoot.Enable();

        _hitComponent.ShipHasBeenHit.AddListener(OnSpaceShipHit);

        _fuelManager.OnFuelEmpty.AddListener(OnFuelEmpty);

    }

    private void OnDisable()
    {
        _perspectiveChange.performed -= OnPerspectiveChange;
        _perspectiveChange.Disable();

        _movementModeChange.performed -= OnMovementModeChange;
        _movementModeChange.Disable();

        _thrustRight.Disable();

        _thrustLeft.Disable();

        _orientLeft.Disable();

        _orientRight.Disable();

        _shoot.Disable();

        _hitComponent.ShipHasBeenHit.RemoveListener(OnSpaceShipHit);
        _fuelManager.OnFuelEmpty.RemoveListener(OnFuelEmpty);
    }

    private void Update()
    {
        ShootUpdate();
    }

    private void FixedUpdate()
    {
        UpdateCameraSetup();
        UpdateThrustSound();
        UpdateThrusters();
    }

    private void UpdateThrusters()
    {
        if(_fuelManager.Fuel < 0)
        {
            return;
        }

        switch (_movementMode)
        {
            case MovementMode.Mobility:
                UpdateThrustersMobility();
                break;
            case MovementMode.Combat:
                UpdateThrustersCombat(); 
                break;
            case MovementMode.SingleThrust:
                UpdateThrustersSingleThrust();
                break;
        }
    }

    private void OnFuelEmpty()
    {
        GameManager.Instance.OnFuelEmpty(_input.playerIndex);
    }

    private void UpdateThrustersMobility()
    {
        float inputThrustLeft = _thrustLeft.ReadValue<float>();
        float inputThrustRight = _thrustRight.ReadValue<float>();

        Vector2 inputOrientLeft = _orientLeft.ReadValue<Vector2>();

        Vector2 inputOrientRight = _orientRight.ReadValue<Vector2>();

        _leftThruster.UpdateThrusterOrient(inputOrientLeft);
        _rightThruster.UpdateThrusterOrient(inputOrientRight);

        _leftThruster.AddThrustForce(inputThrustLeft);
        _rightThruster.AddThrustForce(inputThrustRight);

        UpdateFuel(inputThrustLeft);
        UpdateFuel(inputThrustRight);
    }

    private void UpdateThrustersSingleThrust()
    {
        float inputThrustLeft = _thrustLeft.ReadValue<float>();
        float inputThrustRight = _thrustRight.ReadValue<float>();

        Vector2 inputOrientLeft = _orientLeft.ReadValue<Vector2>();

        Vector2 inputOrientRight = _orientRight.ReadValue<Vector2>();

        _leftThruster.UpdateThrusterOrient(inputOrientLeft);
        _rightThruster.UpdateThrusterOrient(inputOrientRight);

        _leftThruster.AddThrustForce(inputThrustLeft);
        _rightThruster.AddThrustForce(inputThrustLeft);

        UpdateFuel(inputThrustLeft);
        UpdateFuel(inputThrustLeft);
    }

    private void UpdateThrustersCombat()
    {
        float inputThrustLeft = _thrustLeft.ReadValue<float>();

        Vector2 inputOrientLeft = _orientLeft.ReadValue<Vector2>();

        _leftThruster.UpdateThrusterOrient(inputOrientLeft);
        _rightThruster.UpdateThrusterOrient(inputOrientLeft);

        _leftThruster.AddThrustForce(inputThrustLeft);
        _rightThruster.AddThrustForce(inputThrustLeft);

        UpdateFuel(inputThrustLeft);
        UpdateFuel(inputThrustLeft);

        float rollInput = _orientRight.ReadValue<Vector2>().x;

        Vector3 rollAxis = transform.forward * rollInput * _addedTorqueInCombat;

        _shipBody.AddTorque(rollAxis);
    }

    private void ShootUpdate()
    {
        bool isShooting = (_shoot.ReadValue<float>() > 0.1f);

        if (_movementMode == MovementMode.SingleThrust)
        {
            isShooting = (isShooting || (_thrustRight.ReadValue<float>() > 0.1f));
        }

        if (isShooting)
        {
            float timeSinceLastShot = Time.time - _lastShotTime;
            if (timeSinceLastShot >= _delayBetweenShots)
            {
                _lastShotTime = Time.time;
                _aimingReticle.SpawnProjectile();
            }
        }
    }

    private void OnSpaceShipHit()
    {
        UpdateFuel(_fuelLostPerHit);
    }

    private void InitializeCamera()
    {
        switch (_currentPerspective)
        {
            case PerspectiveState.Back:
                _cameraSetupBehind.enabled = true;
                _cameraSetupCockpit.enabled = false;
                break;
            case PerspectiveState.CockPit:
                _cameraSetupBehind.enabled = false;
                _cameraSetupCockpit.enabled = true;
                break;
        }
    }

    private void UpdateCameraSetup()
    {
        _cameraSetupCockpit.SetSpeed(_shipBody.linearVelocity.magnitude);
        _cameraSetupBehind.SetSpeed(_shipBody.linearVelocity.magnitude);
    }

    private void UpdateThrustSound()
    {
        _thrustSound.UpdateSpeed(_shipBody.linearVelocity.magnitude);
    }

    private void UpdateFuel(float input)
    {
        _fuelManager.ThrustFuelReduction(input * _fuelConsumptionPerThrust * Time.fixedDeltaTime);
    }

    private void OnPerspectiveChange(InputAction.CallbackContext _)
    {
        switch (_currentPerspective)
        {
            case PerspectiveState.Back:
                _currentPerspective = PerspectiveState.CockPit;
                break;
            case PerspectiveState.CockPit:
                _currentPerspective = PerspectiveState.Back;

                break;
        }

        InitializeCamera();
    }

    private void OnMovementModeChange(InputAction.CallbackContext _)
    {
        switch(_movementMode)
        {
            case MovementMode.Mobility:
                _movementMode = MovementMode.SingleThrust;
                break;
            case MovementMode.Combat:
                _movementMode = MovementMode.SingleThrust;
                break;
            case MovementMode.SingleThrust:
                _movementMode = MovementMode.Mobility;
                break;
        }
        _ModeText.text = _movementMode.ToString();

    }

    private enum PerspectiveState
    {
        Back,
        CockPit
    }

    private enum MovementMode
    {
        Mobility,
        Combat,
        SingleThrust
    }
}

public enum ShipPlayer
{
    Red = 0,
    Blue = 1
}
