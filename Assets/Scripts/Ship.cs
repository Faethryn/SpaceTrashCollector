using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ship : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _shipBody;

    [SerializeField]
    private CameraSetup _cameraSetupCockpit;

    [SerializeField]
    private CameraSetup _cameraSetupBehind;

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
    private AimingReticle _aimingReticle;

    [SerializeField]
    private float _addedTorqueInCombat = 100f;

    private IASpaceShip _shipControls;

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

    private PerspectiveState _currentPerspective = PerspectiveState.Back;

    private MovementMode _movementMode = MovementMode.Mobility;

    private void Awake()
    {
        _shipControls = new IASpaceShip();

        InitializeCamera();
    }

    private void OnEnable()
    {
        _perspectiveChange = _shipControls.SpaceShip.ChangePerspective;
        _perspectiveChange.Enable();
        _perspectiveChange.performed += OnPerspectiveChange;

        _movementModeChange = _shipControls.SpaceShip.MovementModeToggle;
        _movementModeChange.Enable();
        _movementModeChange.performed += OnMovementModeChange;

        _thrustRight = _shipControls.SpaceShip.ThrustRight;
        _thrustRight.Enable();

        _thrustLeft = _shipControls.SpaceShip.ThrustLeft;
        _thrustLeft.Enable();

        _orientLeft = _shipControls.SpaceShip.OrientLeft;
        _orientLeft.Enable();

        _orientRight = _shipControls.SpaceShip.OrientRight;
        _orientRight.Enable();

        _shoot = _shipControls.SpaceShip.Shoot;
        _shoot.Enable();
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
        UpdateFuel(inputThrustRight);
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
        if (_shoot.ReadValue<float>() > 0.1f)
        {
            float timeSinceLastShot = Time.time - _lastShotTime;
            if (timeSinceLastShot >= _delayBetweenShots)
            {
                _lastShotTime = Time.time;
                _aimingReticle.SpawnProjectile();
            }
        }
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
                _movementMode = MovementMode.Combat;
                break;
            case MovementMode.Combat:
                _movementMode = MovementMode.SingleThrust;
                break;
            case MovementMode.SingleThrust:
                _movementMode = MovementMode.Mobility;
                break;
        }
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
