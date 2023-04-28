using UnityEngine;

public class Player : BaseEntity
{
    [SerializeField] public bool Powerup;
    [SerializeField] private BodyController _bodyController;
    [SerializeField] private Renderer _energyShield;
    [SerializeField] private Material _materialBlue;
    [SerializeField] private Material _materialRed;
    [SerializeField] private float _powerupTimer;
    private Material _currentMaterial;

    private void Update()
    {
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
            Color();
        }
    }

    public void ActivatePowerup(float powerupTime)
    {
        _powerupTimer = powerupTime;
        Powerup = true;
        ChangeColorRed();
        Color();
    }

    private void ChangeColorBlue()
    {
        _currentMaterial = _materialBlue;
    }
    private void ChangeColorRed()
    {
        _currentMaterial = _materialRed;
    }

    private void Color()
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

        _energyShield.material = _currentMaterial;
    }
}
