using MathUtil;
using UnityEngine;


public class CameraSetup : MonoBehaviour
{
    [SerializeField]
    private Transform _closeAnchor;

    [SerializeField]
    private Transform _farAnchor;

    [SerializeField] 
    private GameObject _cameraHolder;

    [SerializeField]
    private Vector2 _speedRange = new Vector2(10f, 50f);

    [SerializeField]
    private Vector2 _dampSpeed = new Vector2(0.1f, 5.0f);

    [SerializeField]
    private Vector2 _dampingDistance = new Vector2(0.05f, 0.5f);

    private float _currentSpeed = 10.0f;

    private Camera _ownCam;

    private void OnEnable()
    {
        if (_ownCam == null)
        {
            _ownCam = _cameraHolder.GetComponentInChildren<Camera>();
        }
        _ownCam.enabled = true;
    }

    private void OnDisable()
    {
        if (_ownCam == null)
        {
            _ownCam = _cameraHolder.GetComponentInChildren<Camera>();
        }

        _ownCam.enabled = false;
    }

    private void FixedUpdate()
    {
        UpdateCameraLocation();
    }

    private void UpdateCameraLocation()
    {
        float lerpFactor = MathUtilHelpers.RemapF(_speedRange.x, _speedRange.y, 0f, 1f, _currentSpeed);
        Vector3 currentTargetLocation = Vector3.Lerp(_closeAnchor.position, _farAnchor.position, lerpFactor);

        float distanceThisFrame = Vector3.Distance(_cameraHolder.transform.position, currentTargetLocation);

        if (distanceThisFrame >= _dampingDistance.x)
        {
            float distanceToSpeedFactor = MathUtilHelpers.RemapF(_dampingDistance.x, _dampingDistance.y, 0f, 1f, distanceThisFrame);

            float speed = Mathf.Lerp(_dampSpeed.x, _dampSpeed.y, distanceToSpeedFactor);

            Vector3 direction = currentTargetLocation - _cameraHolder.transform.position;

            direction.Normalize();

            _cameraHolder.transform.position = _cameraHolder.transform.position + (direction * speed * Time.fixedDeltaTime);
        }
    }

    public void SetSpeed(float speed)
    {
        _currentSpeed = speed;
    }
}
