
using UnityEngine;
using UnityEngine.InputSystem;

public class AimingReticle : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _spaceShip;

    [SerializeField]
    private int _steps = 10;

    [SerializeField]
    private Transform _endReticle;

    [SerializeField]
    private LineRenderer _lineRenderer;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private Transform _laserOrigin;

    private void Start()
    {
        _lineRenderer.positionCount = _steps;
    }

    private void LateUpdate()
    {
        UpdateReticle();
        //ShootUpdate();
    }

    private void UpdateReticle()
    { 
        float distancePerStep = _spaceShip.linearVelocity.magnitude / (float)_steps;

        Vector3 lastPosition = this.transform.position;

        Vector3 lastForward = _spaceShip.transform.forward;

        Quaternion convertedAngular = Quaternion.Euler(_spaceShip.angularVelocity * _spaceShip.angularVelocity.magnitude);

        for (int i = 0; i < _steps; i++)
        {

            Vector3 nextForward = convertedAngular * lastForward;

            Vector3 nextPosition = lastPosition + ( nextForward * distancePerStep );

            //Debug.DrawLine(lastPosition, nextPosition, Color.red, 0.1f);

            _lineRenderer.SetPosition(i, lastPosition);

            lastPosition = nextPosition;
            lastForward = nextForward;
        }

        Vector3 endrecticleForward = Vector3.Normalize(lastPosition - this.transform.position);

        if(endrecticleForward.magnitude > 0.1f)
        {
            _endReticle.forward = endrecticleForward;
        }
    }

    public void SpawnProjectile()
    {
        _laserOrigin.forward = _endReticle.forward;
        GameObject tempObject = Instantiate(_laserPrefab, _laserOrigin.position, _laserOrigin.rotation);

        if(tempObject.TryGetComponent<Laser>(out var laser))
        {
            laser.SetStartVelocity(_spaceShip.linearVelocity.magnitude);
        }
    }
}
