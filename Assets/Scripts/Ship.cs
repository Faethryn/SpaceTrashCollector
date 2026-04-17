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

    private IASpaceShip _shipControls;

    private InputAction _perspectiveChange;

    private PerspectiveState _currentPerspective = PerspectiveState.Back;

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
    }

    private void OnDisable()
    {
        _perspectiveChange.performed -= OnPerspectiveChange;
        _perspectiveChange.Disable();
    }


    private void FixedUpdate()
    {
        UpdateCameraSetup();
        UpdateThrustSound();
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

    private enum PerspectiveState
    {
        Back,
        CockPit
    }
}
