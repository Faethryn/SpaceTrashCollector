using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using MathUtil;

public class Thruster : MonoBehaviour
{
    [SerializeField]
    private ThrusterSide _thrusterSide;

    [SerializeField]
    private float _thrusterForce = 2f;

    [SerializeField]
    private Vector2 _thrusterAngleConstraints = new Vector2(-60, 60);

    [SerializeField]
    private Rigidbody _spaceShipBody;

    private IASpaceShip _shipActions;

    private InputAction _thrustRight;

    private InputAction _thrustLeft;

    private InputAction _orientLeft;

    private InputAction _orientRight;


    private float _currentAngleY;

    private float _currentAngleX;

    private void OnEnable()
    {
        _shipActions = new IASpaceShip();

        _thrustRight = _shipActions.SpaceShip.ThrustRight;
        _thrustRight.Enable();

        _thrustLeft = _shipActions.SpaceShip.ThrustLeft;
        _thrustLeft.Enable();

        _orientLeft = _shipActions.SpaceShip.OrientLeft;
        _orientLeft.Enable();

        _orientRight = _shipActions.SpaceShip.OrientRight;
        _orientRight.Enable();

    }

    private void OnDisable()
    {
        _thrustRight.Disable();

        _thrustLeft.Disable();

        _orientLeft.Disable();

        _orientRight.Disable();
    }

    private void FixedUpdate()
    {
        UpdateThrusterOrient();
        AddThrustForce();
    }

    private void UpdateThrusterOrient()
    {
        Vector2 inputVector = new Vector2();

        switch(_thrusterSide)
        {
            case ThrusterSide.Left:
                inputVector = _orientLeft.ReadValue<Vector2>();
                break;
            case ThrusterSide.Right:
                inputVector = _orientRight.ReadValue<Vector2>();
                break;
        }

        float targetAngleX = MathUtilHelpers.RemapF(-1f, 1f, _thrusterAngleConstraints.x, _thrusterAngleConstraints.y, inputVector.y);
        float targetAngleY = MathUtilHelpers.RemapF(-1f, 1f, _thrusterAngleConstraints.x, _thrusterAngleConstraints.y, inputVector.x);


        Vector3 targetAngle = new Vector3(targetAngleX, targetAngleY, 0);

        _currentAngleX = targetAngleX;
        _currentAngleY = targetAngleY;

        transform.localEulerAngles = targetAngle;
    }

    private void AddThrustForce()
    {
        float inputFloat = 0f;

        switch(_thrusterSide)
        {
            case ThrusterSide.Left:
                inputFloat = _thrustLeft.ReadValue<float>();
                break;
            case ThrusterSide.Right:
                inputFloat =_thrustRight.ReadValue<float>();
                break;
        }

        float targetForce = MathUtilHelpers.RemapF(0f, 1f, 0f, _thrusterForce, inputFloat);

        Vector3 addedForce = this.transform.forward * targetForce * Time.fixedDeltaTime;

        _spaceShipBody.AddForceAtPosition(addedForce, this.transform.position);
    }
}

public enum ThrusterSide
{
    Left,
    Right
}
