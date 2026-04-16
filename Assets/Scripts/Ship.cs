using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _shipBody;

    [SerializeField]
    private CameraSetup _cameraSetup;

    [SerializeField]
    private ThrustSound _thrustSound;


    private void FixedUpdate()
    {
        UpdateCameraSetup();
        UpdateThrustSound();
    }

    private void UpdateCameraSetup()
    {
        _cameraSetup.SetSpeed(_shipBody.linearVelocity.magnitude);
    }

    private void UpdateThrustSound()
    {
        _thrustSound.UpdateSpeed(_shipBody.linearVelocity.magnitude);
    }
}
