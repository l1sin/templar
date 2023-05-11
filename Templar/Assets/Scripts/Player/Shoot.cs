using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class Shoot : MonoBehaviour
{
    [Header("Pistol preferences")]
    [SerializeField] private Transform _pistolShootingPointR;
    [SerializeField] private Transform _pistolShootingPointL;
    [SerializeField] private GameObject _pistolProjectilePrefab;
    [SerializeField] private float _pistolFirePeriod;
    [SerializeField] private float _pistolDamage;
    [SerializeField] private float _pistolProjectileSpeed;
    [SerializeField] private float _pistiolStoppingAction;
    [SerializeField] private float _pistolEnergyCost;
    [SerializeField] private GameObject _pistolEffect;
    [SerializeField] private AudioClip[] _pistolSound;

    [Header("Pistol powerup")]
    [SerializeField] private GameObject _pistolProjectilePrefabPU;
    [SerializeField] private float _pistolFirePeriodPU;
    [SerializeField] private float _pistolDamagePU;
    [SerializeField] private float _pistolProjectileSpeedPU;
    [SerializeField] private float _pistolStoppingActionPU;
    [SerializeField] private float _pistolEnergyCostPU;
    [SerializeField] private GameObject _pistolEffectPU;

    [HideInInspector] private GameObject _pistolProjectilePrefabCurrent;
    [HideInInspector] private float _pistolFirePeriodCurrent;
    [HideInInspector] private float _pistolDamageCurrent;
    [HideInInspector] private float _pistolProjectileSpeedCurrent;
    [HideInInspector] private float _pistolStoppingActionCurrent;
    [HideInInspector] private float _pistolEnergyCostCurrent;
    [HideInInspector] private GameObject _pistolEffectCurrent;

    [Header("Railgun preferences")]
    [SerializeField] private Transform _railgunShootingPointR;
    [SerializeField] private Transform _railgunShootingPointL;
    [SerializeField] private GameObject _railgunProjectilePrefab;
    [SerializeField] private float _railgunFirePeriod;
    [SerializeField] private float _railgunDamage;
    [SerializeField] private float _railgunProjectileSpeed;
    [SerializeField] private float _recoilForce;
    [SerializeField] private float _railgunStoppingAction;
    [SerializeField] private float _railgunEnergyCost;
    [SerializeField] private GameObject _railgunEffect;
    [SerializeField] private AudioClip[] _railgunSound;

    [Header("Railgun powerup")]
    [SerializeField] private GameObject _railgunProjectilePrefabPU;
    [SerializeField] private float _railgunFirePeriodPU;
    [SerializeField] private float _railgunDamagePU;
    [SerializeField] private float _railgunProjectileSpeedPU;
    [SerializeField] private float _recoilForcePU;
    [SerializeField] private float _railgunStoppingActionPU;
    [SerializeField] private float _railgunEnergyCostPU;
    [SerializeField] private GameObject _railgunEffectPU;

    [HideInInspector] private GameObject _railgunProjectilePrefabCurrent;
    [HideInInspector] private float _railgunFirePeriodCurrent;
    [HideInInspector] private float _railgunDamageCurrent;
    [HideInInspector] private float _railgunProjectileSpeedCurrent;
    [HideInInspector] private float _recoilForceCurrent;
    [HideInInspector] private float _railgunStoppingActionCurrent;
    [HideInInspector] private float _railgunEnergyCostCurrent;
    [HideInInspector] private GameObject _railgunEffectCurrent;

    [Header("Component references")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private BodyController _bodyController;
    [SerializeField] private AimGun _aimGun;
    [SerializeField] private Player _player;
    [SerializeField] private AudioMixerGroup _mixerGroup;

    [Header("Hidden")]
    [HideInInspector] private float _nextFirePistolTimer;
    [HideInInspector] private float _nextFireRailgunTimer;
    [HideInInspector] private Transform _pistolShootingPoint;
    [HideInInspector] private Transform _railgunShootingPoint;

    private void Start()
    {
        _nextFirePistolTimer = 0;
        _nextFireRailgunTimer = 0;
    }

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        CheckIfPowerUp();
        ChangeShootingPoint();
        ResetFireTimer();
        FirePistol();
        FireRailgun();
    }

    private void FirePistol()
    {
        if (PlayerInput.Mouse0 && !PlayerInput.Mouse1 && !PlayerInput.Mouse2 && _nextFirePistolTimer <= 0 && Player.Instance.CanUseEnergy && Player.Instance.CurrentEnergy > 0)
        {
            GameObject effect = Instantiate(_pistolEffectCurrent, _pistolShootingPoint.position, Quaternion.Euler(new Vector3(0, 0, AimGun.RotationZ)));
            effect.transform.SetParent(_pistolShootingPoint);

            GameObject pistolProjectile = Instantiate(_pistolProjectilePrefabCurrent, _pistolShootingPoint.position, Quaternion.Euler(new Vector3(0, 0, AimGun.RotationZ)));
            PlayerProjectile projectileParameters = pistolProjectile.GetComponent<PlayerProjectile>();
            projectileParameters.Damage = _pistolDamageCurrent;
            projectileParameters.Speed = _pistolProjectileSpeedCurrent;
            projectileParameters.StoppingAction = _pistolStoppingActionCurrent;

            _nextFirePistolTimer = _pistolFirePeriodCurrent;
            Player.Instance.UseEnergy(_pistolEnergyCostCurrent);
            AudioManager.Instance.MakeSound(transform, _pistolSound, _mixerGroup, false, false);
        }
    }

    private void FireRailgun()
    {
        if (PlayerInput.Mouse0 && PlayerInput.Mouse1 && !PlayerInput.Mouse2 && _nextFireRailgunTimer <= 0 && Player.Instance.CanUseEnergy && Player.Instance.CurrentEnergy > 0)
        {
            GameObject effect = Instantiate(_railgunEffectCurrent, _railgunShootingPoint.position, Quaternion.Euler(new Vector3(0, 0, AimGun.RotationZ)));
            effect.transform.SetParent(_railgunShootingPoint);

            GameObject railgunProjectile = Instantiate(_railgunProjectilePrefabCurrent, _railgunShootingPoint.position, Quaternion.Euler(new Vector3(0, 0, AimGun.RotationZ)));
            PlayerProjectile projectileParameters = railgunProjectile.GetComponent<PlayerProjectile>();
            projectileParameters.Damage = _railgunDamageCurrent;
            projectileParameters.Speed = _railgunProjectileSpeedCurrent;
            projectileParameters.StoppingAction = _railgunStoppingActionCurrent;

            _rigidbody2D.AddForce(CalculateRecoilDirection() * _recoilForceCurrent, ForceMode2D.Impulse);

            _nextFireRailgunTimer = _railgunFirePeriodCurrent;
            Player.Instance.UseEnergy(_railgunEnergyCostCurrent);
            AudioManager.Instance.MakeSound(transform, _railgunSound, _mixerGroup, false, false);
        }
    }

    private Vector2 CalculateRecoilDirection()
    {
        Vector2 direction;
        direction.x = Mathf.Cos(AimGun.RotationZ * Mathf.Deg2Rad);
        direction.y = Mathf.Sin(AimGun.RotationZ * Mathf.Deg2Rad);
        return -direction;
    }

    private void ChangeShootingPoint()
    {
        if (_bodyController.HeadAndBody.transform.rotation.y == 0)
        {
            _pistolShootingPoint = _pistolShootingPointR;
            _railgunShootingPoint = _railgunShootingPointR;
        }
        else
        {
            _pistolShootingPoint = _pistolShootingPointL;
            _railgunShootingPoint = _railgunShootingPointL;
        }
    }

    private void CheckIfPowerUp()
    {
        if (_player.Powerup)
        {
            SetPowerUpOn();
        }
        else
        {
            SetPowerUpOff();
        }
    }

    private void ResetFireTimer()
    {
        _nextFirePistolTimer -= Time.deltaTime;
        _nextFireRailgunTimer -= Time.deltaTime;
    }

    private void SetPowerUpOn()
    {
        // Pistol PU
        _pistolProjectilePrefabCurrent = _pistolProjectilePrefabPU;
        _pistolFirePeriodCurrent = _pistolFirePeriodPU;
        _pistolDamageCurrent = _pistolDamagePU;
        _pistolProjectileSpeedCurrent = _pistolProjectileSpeedPU;
        _pistolStoppingActionCurrent = _pistolStoppingActionPU;
        _pistolEnergyCostCurrent = _pistolEnergyCostPU;
        _pistolEffectCurrent = _pistolEffectPU;

        // Railgun PU
        _railgunProjectilePrefabCurrent = _railgunProjectilePrefabPU;
        _railgunFirePeriodCurrent = _railgunFirePeriodPU;
        _railgunDamageCurrent = _railgunDamagePU;
        _railgunProjectileSpeedCurrent = _railgunProjectileSpeedPU;
        _recoilForceCurrent = _recoilForcePU;
        _railgunStoppingActionCurrent = _railgunStoppingActionPU;
        _railgunEnergyCostCurrent = _railgunEnergyCostPU;
        _railgunEffectCurrent = _railgunEffectPU;
    }
    private void SetPowerUpOff()
    {
        // Pistol Common
        _pistolProjectilePrefabCurrent = _pistolProjectilePrefab;
        _pistolFirePeriodCurrent = _pistolFirePeriod;
        _pistolDamageCurrent = _pistolDamage;
        _pistolProjectileSpeedCurrent = _pistolProjectileSpeed;
        _pistolStoppingActionCurrent = _pistiolStoppingAction;
        _pistolEnergyCostCurrent = _pistolEnergyCost;
        _pistolEffectCurrent = _pistolEffect;

        // Railgun Common
        _railgunProjectilePrefabCurrent = _railgunProjectilePrefab;
        _railgunFirePeriodCurrent = _railgunFirePeriod;
        _railgunDamageCurrent = _railgunDamage;
        _railgunProjectileSpeedCurrent = _railgunProjectileSpeed;
        _recoilForceCurrent = _recoilForce;
        _railgunStoppingActionCurrent = _railgunStoppingAction;
        _railgunEnergyCostCurrent = _railgunEnergyCost;
        _railgunEffectCurrent = _railgunEffect;
    }
}
