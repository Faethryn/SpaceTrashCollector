using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private ColliderEvents _cylinderCollider;

    [SerializeField]
    private float _forwardSpeed = 20f;

    [SerializeField]
    private float _lifeTime = 10f;

    private float _timeSpawned;

    private float _actualSpeed = 0.0f;

    private void Awake()
    {
        _timeSpawned = Time.time;
    }


    private void OnEnable()
    {
        _cylinderCollider.OnTriggerEnterEvent.AddListener(OnCollisionTrigger);
    }

    private void OnDisable()
    {
        _cylinderCollider.OnTriggerEnterEvent.RemoveListener(OnCollisionTrigger);
    }

    private void OnCollisionTrigger(Collider other)
    {
       if(other.TryGetComponent<Asteroid>(out var asteroid))
        {
            asteroid.RemoveHealth(1);
            Destroy(this.gameObject);
        }

        if (other.TryGetComponent<SpaceShipHit>(out var Ship))
        {
            Ship.HitShip();
            Destroy(this.gameObject);
        }
    }

    public void SetStartVelocity(float speed)
    {
        _actualSpeed = speed + _forwardSpeed;
    }

    private void FixedUpdate()
    {
        MoveForward();
        CheckLifetime();
    }

    private void MoveForward()
    {
        transform.position = transform.position + (transform.forward * _actualSpeed * Time.fixedDeltaTime);
    }

    private void CheckLifetime()
    {
        float timeAlive = Time.time - _timeSpawned;

        if(timeAlive >= _lifeTime)
        {
            Destroy(this.gameObject) ;
        }
    }
}
