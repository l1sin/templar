using UnityEngine;
using UnityEngine.Audio;

public class Player : BaseEntity
{
    [Header("References")]
    [SerializeField] private BodyController _bodyController;
    [SerializeField] private Renderer _energyShield;
    [SerializeField] private Material _materialBlue;
    [SerializeField] private Material _materialRed;
    [SerializeField] public Transform Target;
    [SerializeField] private AudioClip[] _chargeUp;
    [SerializeField] private AudioClip[] _chargeDown;
    [SerializeField] private AudioMixerGroup _energyMixerGroup;

    [Header("Preferences")]
    [SerializeField] public float MaxEnergy;
    [SerializeField] public float CurrentEnergy;
    [SerializeField] private float _energyRegeneration;
    [SerializeField] private float _energyRegenerationPU;
    [SerializeField] private float _powerupTimer;

    [Header("Hidden Values")]
    [HideInInspector] public bool Powerup;
    [HideInInspector] public float EnergyRegenerationCurrent;
    [HideInInspector] public bool CanUseEnergy;
    [HideInInspector] public bool GameOver;
    [HideInInspector] public bool LevelComplete;

    public static Player Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        SetSingleton();
        ChangeColorBlue();
        ApplyMaterial();
        CurrentEnergy = MaxEnergy;
        CanUseEnergy = true;
    }

    protected override void Update()
    {
        base.Update();
        if (Powerup)
        {
            PowerupCountdown();
            EnergyRegenerationCurrent = _energyRegenerationPU;
        }
        else
        {
            EnergyRegenerationCurrent = _energyRegeneration;
        }
        EnergyRegeneration();
        RestorePower();
        if (Input.GetKeyDown(KeyCode.P)) FinishLevel();
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        HUD.Instance.ChangeHealth(CurrentHealth, MaxHealth);
    }

    public override void GetHeal(float heal)
    {
        base.GetHeal(heal);
        HUD.Instance.ChangeHealth(CurrentHealth, MaxHealth);
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
        CurrentMaterial = _materialBlue;
        TempMaterial = _materialBlue;
    }
    private void ChangeColorRed()
    {
        CurrentMaterial = _materialRed;
        TempMaterial = _materialBlue;
    }

    protected override void ApplyMaterial()
    {
        _bodyController.HeadRenderer.material = CurrentMaterial;
        _bodyController.BodyRenderer.material = CurrentMaterial;

        _bodyController.LeftHandActiveBackRRenderer.material = CurrentMaterial;
        _bodyController.LeftHandInactiveBackRRenderer.material = CurrentMaterial;
        _bodyController.RightHandActiveFrontRRenderer.material = CurrentMaterial;
        _bodyController.RightHandInactiveFrontRRenderer.material = CurrentMaterial;

        _bodyController.LeftHandActiveFrontLRenderer.material = CurrentMaterial;
        _bodyController.LeftHandInactiveFrontLRenderer.material = CurrentMaterial;
        _bodyController.RightHandActiveBackLRenderer.material = CurrentMaterial;
        _bodyController.RightHandInactiveBackLRenderer.material = CurrentMaterial;

        if (CurrentMaterial != DamageBlinkMaterial) _energyShield.material = CurrentMaterial;
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

    public void PowerOff()
    {
        CanUseEnergy = false;
        RenderEnergyLine();
        AudioManager.Instance.MakeSound(transform, _chargeDown, _energyMixerGroup);
    }

    public void UseEnergy(float energy)
    {
        CurrentEnergy -= energy;
        if (CurrentEnergy <= 0)
        {
            CurrentEnergy = 0;
            PowerOff();
        }
    }

    private void EnergyRegeneration()
    {
        CurrentEnergy += EnergyRegenerationCurrent * Time.deltaTime;
        if (CurrentEnergy > MaxEnergy)
        {
            CurrentEnergy = MaxEnergy;
        }
        if (CurrentEnergy < 0)
        {
            CurrentEnergy = 0;
        }
        RenderEnergyLine();
    }
    public void RenderEnergyLine()
    {
        HUD.Instance.ChangeEnergy(CurrentEnergy, MaxEnergy);
    }

    public override void Die()
    {
        PauseManager.Instance.TogglePause();
        UI.Instance.InstantiateMenuNoQueue(UI.Instance.GameOverMenu);
        AudioManager.Instance.StartLossSound();
        GameOver = true;
    }

    public void FinishLevel()
    {
        PauseManager.Instance.TogglePause();
        UI.Instance.InstantiateMenuNoQueue(UI.Instance.LevelCompleteMenu);
        AudioManager.Instance.StartWinSound();
        LevelComplete = true;
    }

    public void ApplyBattery(float energy)
    {
        CurrentEnergy += energy;
        CanUseEnergy = true;
        HUD.Instance.ChangeEnergyBarColor(HUD.Instance.CanUseEnergyColor);
        if (CurrentEnergy > MaxEnergy)
        {
            CurrentEnergy = MaxEnergy;
        }
    }

    private void RestorePower()
    {
        if (!CanUseEnergy)
        {
            HUD.Instance.ChangeEnergyBarColor(HUD.Instance.NoEnergyColor);
            if (CurrentEnergy == MaxEnergy)
            {
                CanUseEnergy = true;
                AudioManager.Instance.MakeSound(transform, _chargeUp, _energyMixerGroup);
                HUD.Instance.ChangeEnergyBarColor(HUD.Instance.CanUseEnergyColor);
            }
        }
    }
}
