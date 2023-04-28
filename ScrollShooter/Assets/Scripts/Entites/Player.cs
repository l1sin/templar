using UnityEngine;

public class Player : BaseEntity
{
    [SerializeField] public bool Powerup;
    [SerializeField] private BodyController _bodyController;
    [SerializeField] private Renderer _energyShield;
    [SerializeField] private Material _materialBlue;
    [SerializeField] private Material _materialRed;
    [SerializeField] private float _powerupTimer;

    protected override void Awake()
    {
        base.Awake();
        ChangeColorBlue();
        ApplyMaterial();
    }

    protected override void Update()
    {
        base.Update();
        if (Powerup)
        {
            PowerupCountdown();
        }
    }

    private void PowerupCountdown()
    {
        _powerupTimer -= Time.deltaTime;
        if (_powerupTimer <= 0)
        {
            Powerup = false;
            ChangeColorBlue();
            ApplyMaterial();
        }
    }

    public void ActivatePowerup(float powerupTime)
    {
        _powerupTimer = powerupTime;
        Powerup = true;
        ChangeColorRed();
        ApplyMaterial();
    }

    private void ChangeColorBlue()
    {
        _currentMaterial = _materialBlue;
        _tempMaterial = _materialBlue;
    }
    private void ChangeColorRed()
    {
        _currentMaterial = _materialRed;
        _tempMaterial = _materialBlue;
    }

    protected override void ApplyMaterial()
    {
        _bodyController.HeadRenderer.material = _currentMaterial;
        _bodyController.BodyRenderer.material = _currentMaterial;

        _bodyController.LeftHandActiveBackRRenderer.material = _currentMaterial;
        _bodyController.LeftHandInactiveBackRRenderer.material = _currentMaterial;
        _bodyController.RightHandActiveFrontRRenderer.material = _currentMaterial;
        _bodyController.RightHandInactiveFrontRRenderer.material = _currentMaterial;

        _bodyController.LeftHandActiveFrontLRenderer.material = _currentMaterial;
        _bodyController.LeftHandInactiveFrontLRenderer.material = _currentMaterial;
        _bodyController.RightHandActiveBackLRenderer.material = _currentMaterial;
        _bodyController.RightHandInactiveBackLRenderer.material = _currentMaterial;

        if (_currentMaterial != _damageBlinkMaterial) _energyShield.material = _currentMaterial;
    }
}
