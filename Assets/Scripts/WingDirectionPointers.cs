using UnityEngine;

public class WingDirectionPointers : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _spaceShip;

    [SerializeField]
    private int _steps = 10;

    [SerializeField]
    private float _lengthPerSpeedUnit = 0.1f;

    [SerializeField]
    private float _reticleDistanceMultiplier = 5f;

    [SerializeField]
    private LineRenderer _lineRenderer;

    [SerializeField]
    private Transform _decalProjector;

    private void Start()
    {
        _lineRenderer.positionCount = _steps;
    }

    private void LateUpdate()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        float distancePerStep = _spaceShip.linearVelocity.magnitude * _lengthPerSpeedUnit / (float)_steps;

        Vector3 lastPosition = this.transform.position;

        for (int i = 0; i < _steps; i++)
        {
            Vector3 nextPosition = lastPosition + (_spaceShip.linearVelocity.normalized * distancePerStep);

            //Debug.DrawLine(lastPosition, nextPosition, Color.red, 0.1f);

            _lineRenderer.SetPosition(i, lastPosition);

            lastPosition = nextPosition;
            
        }

        if(_spaceShip.linearVelocity.magnitude > 0.1f)
        {
            _decalProjector.forward = _spaceShip.linearVelocity.normalized;
        }

        //_decalProjector.localScale = new Vector3(_decalProjector.localScale.x, _decalProjector.localScale.y, _spaceShip.linearVelocity.magnitude * _reticleDistanceMultiplier);
    }
}
