

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    [SerializeField]
    private ColliderEvents _vacuumCone;

    [SerializeField]
    private ColliderEvents _vacuumTarget;

    [SerializeField]
    private float _vacuumStrength = 10.0f;

    [SerializeField]
    private FuelManager _fuelmanager;

    [SerializeField]
    private float _fuelPerCube = 10.0f;

    private List<Collider> _collidersInCone = new List<Collider>();

    private void OnEnable()
    {
        _vacuumCone.OnTriggerEnterEvent.AddListener(OnEnterCone);
        _vacuumCone.OnTriggerExitEvent.AddListener(OnExitCone);

        _vacuumTarget.OnTriggerEnterEvent.AddListener(OnEnterTarget);
    }

    private void OnDisable()
    {
        _vacuumCone.OnTriggerEnterEvent.RemoveListener(OnEnterCone);
        _vacuumCone.OnTriggerExitEvent.RemoveListener(OnExitCone);

        _vacuumTarget.OnTriggerEnterEvent.RemoveListener(OnEnterTarget);
    }

    private void FixedUpdate()
    {
        VaccuumUpdate();
    }

    private void VaccuumUpdate()
    {
        foreach(Collider collider in _collidersInCone)
        {
            if(collider == null)
            {
                _collidersInCone.Remove(collider);
                return;
            }

            Vector3 direction = _vacuumTarget.transform.position - collider.transform.position ;

            direction.Normalize();

            if(collider.gameObject.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.linearVelocity = direction * _vacuumStrength;
            }
        }
    }

    private void OnEnterCone(Collider collider)
    {
        if(_collidersInCone.Contains(collider))
        {
            return;
        }

        _collidersInCone.Add(collider);
    }

    private void OnExitCone(Collider collider)
    {
        //_collidersInCone.Remove(collider);
    }

    private void OnEnterTarget(Collider collider)
    {
        _collidersInCone.Remove(collider);

        Destroy(collider.gameObject);
        Refuel();
    }

    private void Refuel()
    {
        _fuelmanager.RefillFuel(_fuelPerCube);
    }
}
