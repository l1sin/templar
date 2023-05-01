using UnityEngine;

public class EnergyShield : MonoBehaviour
{
    [SerializeField] private GameObject _energyShield;
    [SerializeField] private Transform _pivot;
    [SerializeField] private Camera _cam;
    [SerializeField] private float _distanceMulitplier;
    [SerializeField] public bool IsActive;
    [SerializeField] private Renderer _energyShieldRenderer;
    [SerializeField] private Collider2D _collider;

    [SerializeField] private float _energyDrain;
    [SerializeField] private float _energyDrainPU;
    [HideInInspector] private float _energyDrainCurrent;

    [SerializeField] private float _absorbDamageMultiplier;
    [SerializeField] private float _absorbDamageMultiplierPU;
    [HideInInspector] private float _absorbDamageMultiplierCurrent;

    [SerializeField] private GameObject _shieldBreakEffect;
    [SerializeField] private GameObject _shieldBreakEffectPU;
    [HideInInspector] private GameObject _shieldBreakEffectCurrent;

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        RotateAndPos();
        CheckIfPowerup();
        Activate();
    }

    private void Activate()
    {
        if (!PlayerInput.Mouse0 && PlayerInput.Mouse1 && PlayerInput.Mouse2 && Player.Instance.CanUseEnergy && Player.Instance.EnergyCurrent > 0)
        {
            IsActive = true;
            _collider.enabled = true;
            _energyShieldRenderer.enabled = true;
            Player.Instance.EnergyCurrent -= Player.Instance._energyRegenerationCurrent * Time.deltaTime + _energyDrainCurrent * Time.deltaTime;
            if (Player.Instance.EnergyCurrent < 0) Player.Instance.CanUseEnergy = false;
            Player.Instance.RenderEnergyLine();
        }
        else
        {
            IsActive = false;
            _collider.enabled = false;
            _energyShieldRenderer.enabled = false;
        }
    }

    private void RotateAndPos()
    {
        var mousePos = _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        var difference = mousePos - _pivot.position;
        var RotationZRad = Mathf.Atan2(difference.normalized.y, difference.normalized.x);
        var RotationZDeg = RotationZRad * Mathf.Rad2Deg;

        _energyShield.transform.rotation = Quaternion.Euler(0, 0, RotationZDeg);
        _energyShield.transform.position = new Vector2(Mathf.Cos(RotationZRad), Mathf.Sin(RotationZRad)) * _distanceMulitplier + (Vector2)_pivot.position;
    }

    private void CheckIfPowerup()
    {
        if (Player.Instance.Powerup)
        {
            _energyDrainCurrent = _energyDrainPU;
            _absorbDamageMultiplierCurrent = _absorbDamageMultiplierPU;
            _shieldBreakEffectCurrent = _shieldBreakEffectPU;
        }
        else
        {
            _energyDrainCurrent = _energyDrain;
            _absorbDamageMultiplierCurrent = _absorbDamageMultiplier;
            _shieldBreakEffectCurrent = _shieldBreakEffect;
        }
    }

    public void AbsorbDamage(float damage)
    {
        if (Player.Instance.CanUseEnergy) Player.Instance.EnergyCurrent -= damage * _absorbDamageMultiplierCurrent;
        if (Player.Instance.EnergyCurrent < 0) Instantiate(_shieldBreakEffectCurrent, _energyShield.transform.position, _energyShield.transform.rotation);
    }
}
