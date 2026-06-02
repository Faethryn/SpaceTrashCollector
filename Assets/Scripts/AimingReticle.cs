
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    private Gradient _blueGradient;

    [SerializeField]
    private Gradient _redGradient;

    [SerializeField]
    private GameObject _laserPrefabRed;

    [SerializeField]
    private GameObject _laserPrefabBlue;

    [SerializeField]
    private Transform _opponent;

    [SerializeField]
    private float _homingCircleRadius = 0.2f;

    [SerializeField]
    private Image _reticle;

    [SerializeField]
    private Color _lockOnColor;
    [SerializeField]
    private Color _lockOffColor;

    private bool _lockedOn = false;

    [SerializeField]
    private Camera _myCam;

    private ShipPlayer _player;

    [SerializeField]
    private Transform _laserOrigin;

    public UnityEvent HasShotLaser;

    private void Start()
    {
        _lineRenderer.positionCount = _steps;
    }

    public void SetPlayer(ShipPlayer player)
    {
        _player = player;

        switch (player)
        {
            case ShipPlayer.Red:
                _lineRenderer.colorGradient = _redGradient;
                break;
            case ShipPlayer.Blue:
                _lineRenderer.colorGradient = _blueGradient;
                break;
        }

        
    }

    private void LateUpdate()
    {
        UpdateEnemyLocation();
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
        else
        {
            _endReticle.forward = _spaceShip.transform.forward;
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(_opponent.position, 5.0f);
        //Gizmos.DrawSphere(_myCam.transform.position, 5);
    }

    private void UpdateEnemyLocation()
    {
        if(_opponent == null)
        {

            _opponent = GameManager.Instance.GetOpponent((int)_player);
            return;
        }

        Vector3 enemyScreenPosition = _myCam.WorldToViewportPoint(_opponent.position);

        Vector2 enemyScreenPositionXY = enemyScreenPosition;

        if (enemyScreenPosition.z < 0)
        {
            

            enemyScreenPositionXY.Normalize();

            enemyScreenPosition.x = enemyScreenPositionXY.x;
            enemyScreenPosition.y = enemyScreenPositionXY.y;
            enemyScreenPosition *= 3.0f;
        }

        enemyScreenPosition.x = Mathf.Clamp(enemyScreenPosition.x, 0, 1);
        enemyScreenPosition.y = Mathf.Clamp(enemyScreenPosition.y, 0, 1);

        float xFromMiddle = enemyScreenPosition.x - 0.5f;

        float yFromMiddle = enemyScreenPosition.y - 0.5f;
        if(Mathf.Abs(yFromMiddle) < _homingCircleRadius && Mathf.Abs(xFromMiddle) < _homingCircleRadius)
        {
            _lockedOn = true;
            _reticle.color = _lockOnColor;
        }
        else
        {
            _lockedOn = false;
            _reticle.color = _lockOffColor;

        }

        enemyScreenPosition.x = _reticle.canvas.pixelRect.width * enemyScreenPosition.x;
        enemyScreenPosition.y = _reticle.canvas.pixelRect.height * enemyScreenPosition.y;

        

        _reticle.rectTransform.anchoredPosition = enemyScreenPosition;
    }

    public void SpawnProjectile()
    {
        _laserOrigin.forward = _endReticle.forward;
        GameObject objectToInstantiate = null;

        switch (_player)
        {
            case ShipPlayer.Red:
                objectToInstantiate = _laserPrefabRed;
                break;
            case ShipPlayer.Blue:
                objectToInstantiate = _laserPrefabBlue;
                break;
        }

        HasShotLaser?.Invoke();

        if (_lockedOn)
        {
            Vector3 lockedOnDirection = _opponent.position - _laserOrigin.position;

            lockedOnDirection.Normalize();

            Quaternion aimDirection = Quaternion.LookRotation(lockedOnDirection, Vector3.up);

            GameObject LockedOnObject = Instantiate(objectToInstantiate, _laserOrigin.position, aimDirection);

            if (LockedOnObject.TryGetComponent<Laser>(out var Lockedlaser))
            {
                Lockedlaser.SetStartVelocity(_spaceShip.linearVelocity.magnitude);
            }
            return;
        }

        GameObject tempObject = Instantiate(objectToInstantiate, _laserOrigin.position, _laserOrigin.rotation);

        if(tempObject.TryGetComponent<Laser>(out var laser))
        {
            laser.SetStartVelocity(_spaceShip.linearVelocity.magnitude);
        }
        
    }
}
