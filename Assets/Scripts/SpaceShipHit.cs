using UnityEngine;
using UnityEngine.Events;

public class SpaceShipHit : MonoBehaviour
{
    public UnityEvent ShipHasBeenHit;
    
    public void HitShip()
    {
        ShipHasBeenHit?.Invoke();
    }
}
