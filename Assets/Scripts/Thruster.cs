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

    [SerializeField]
    private ParticleSystem _fireSystem;

    [SerializeField]
    private Vector2 _emissionRateRange = new Vector2(0f, 80f);

    

    

    private void FixedUpdate()
    {
       
    }


    public void UpdateThrusterOrient(Vector2 inputVector)
    {
        float targetAngleX = MathUtilHelpers.RemapF(-1f, 1f, _thrusterAngleConstraints.x, _thrusterAngleConstraints.y, inputVector.y);
        float targetAngleY = MathUtilHelpers.RemapF(-1f, 1f, _thrusterAngleConstraints.x, _thrusterAngleConstraints.y, inputVector.x);


        Vector3 targetAngle = new Vector3(targetAngleX, targetAngleY, 0);

        transform.localEulerAngles = targetAngle;
    }

    public void AddThrustForce(float inputFloat)
    {
        UpdateParticleSystem(inputFloat);

        float targetForce = MathUtilHelpers.RemapF(0f, 1f, 0f, _thrusterForce, inputFloat);

        Vector3 addedForce = this.transform.forward * targetForce * Time.fixedDeltaTime;

        _spaceShipBody.AddForceAtPosition(addedForce, this.transform.position);
    }

    private void UpdateParticleSystem(float lerpFactor)
    {
        float amountOfEmission = MathUtilHelpers.RemapF(0, 1, _emissionRateRange.x, _emissionRateRange.y, lerpFactor);

        var emissionModule = _fireSystem.emission;

        emissionModule.rateOverTime = amountOfEmission;
    }
}

public enum ThrusterSide
{
    Left,
    Right
}
