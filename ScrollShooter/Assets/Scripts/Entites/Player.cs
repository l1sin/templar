using UnityEngine;

public class Player : BaseEntity
{
    [SerializeField] public bool Powerup;
    [SerializeField] private BodyController _bodyController;
    [SerializeField] private Renderer _energyShield;
    [SerializeField] private Material _materialBlue;
    [SerializeField] private Material _materialRed;
    [SerializeField] private float _powerupTimer;

    [SerializeField] public float MaxEnergy;
    [SerializeField] public float EnergyCurrent;
    [SerializeField] private float _energyRegeneration;
    [SerializeField] private float _energyRegenerationPU;
    [HideInInspector] public float _energyRegenerationCurrent;

    [SerializeField] public bool CanUseEnergy;

    [SerializeField] public bool GameOver;
    [SerializeField] public bool LevelComplete;

    public static Player Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        SetSingleton();
        ChangeColorBlue();
        ApplyMaterial();
        EnergyCurrent = MaxEnergy;
    }

    protected override void Update()
    {
        base.Update();
        if (Powerup)
        {
            PowerupCountdown();
            _energyRegenerationCurrent = _energyRegenerationPU;
        }
        else
        {
            _energyRegenerationCurrent = _energyRegeneration;
        }
        EnergyRegeneration();
        RestorePower();
        if (Input.GetKeyDown(KeyCode.P)) FinishLevel();
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        HUD.Instance.ChangeHealth(_currentHealthPoints, _maxHealthPoints);
    }

    public override void GetHeal(float heal)
    {
        base.GetHeal(heal);
        HUD.Instance.ChangeHealth(_currentHealthPoints, _maxHealthPoints);
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

    private void SetSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void EnergyRegeneration()
    {
        EnergyCurrent += _energyRegenerationCurrent * Time.deltaTime;
        if (EnergyCurrent > MaxEnergy)
        {
            EnergyCurrent = MaxEnergy;
        }
        if (EnergyCurrent < 0)
        {
            EnergyCurrent = 0;
        }
        RenderEnergyLine();
    }
    public void RenderEnergyLine()
    {
        HUD.Instance.ChangeEnergy(EnergyCurrent, MaxEnergy);
    }

    protected override void Die()
    {
        PauseManager.Instance.TogglePause();
        UI.Instance.InstantiateMenuNoQueue(UI.Instance.GameOverMenu);
        GameOver = true;
    }

    public void FinishLevel()
    {
        PauseManager.Instance.TogglePause();
        UI.Instance.InstantiateMenuNoQueue(UI.Instance.LevelCompleteMenu);
        LevelComplete = true;
    }

    public void RestorePower()
    {
        if (!CanUseEnergy)
        {
            HUD.Instance.ChangeEnergyBarColor(HUD.Instance.NoEnergyColor);
            if (EnergyCurrent == MaxEnergy)
            {
                CanUseEnergy = true;
                HUD.Instance.ChangeEnergyBarColor(HUD.Instance.CanUseEnergyColor);
            }
        }
    }
}
