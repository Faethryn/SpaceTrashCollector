using UnityEngine;
using UnityEngine.UI;

public class FuelManager : MonoBehaviour
{
    [SerializeField]
    private float _fuel = 500f;

    public float Fuel
    {
        get { return _fuel; }
    }

    private float _maxFuel = 500f;

    [SerializeField]
    private Image _fuelBar;

    private void Awake()
    {
        _maxFuel = _fuel;
    }

    public void ThrustFuelReduction(float thrustValue)
    {
        _fuel -= thrustValue;
        UpdateFuelBar();
    }

    public void RefillFuel(float fuelRefill)
    {
        _fuel += fuelRefill;
    }

    private void UpdateFuelBar()
    {
        _fuelBar.fillAmount = _fuel / _maxFuel;
    }
}
