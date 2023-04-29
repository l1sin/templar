using UnityEngine;

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
    [SerializeField] private GameObject _pistolEffect;

    [Header("Pistol powerup")]
    [SerializeField] private GameObject _pistolProjectilePrefabPU;
    [SerializeField] private float _pistolFirePeriodPU;
    [SerializeField] private float _pistolDamagePU;
    [SerializeField] private float _pistolProjectileSpeedPU;
    [SerializeField] private float _pistolStoppingActionPU;
    [SerializeField] private GameObject _pistolEffectPU;

    private GameObject _pistolProjectilePrefabCurrent;
    private float _pistolFirePeriodCurrent;
    private float _pistolDamageCurrent;
    private float _pistolProjectileSpeedCurrent;
    private float _pistolStoppingActionCurrent;
    private GameObject _pistolEffectCurrent;

    [Header("Railgun preferences")]
    [SerializeField] private Transform _railgunShootingPointR;
    [SerializeField] private Transform _railgunShootingPointL;
    [SerializeField] private GameObject _railgunProjectilePrefab;
    [SerializeField] private float _railgunFirePeriod;
    [SerializeField] private float _railgunDamage;
    [SerializeField] private float _railgunProjectileSpeed;
    [SerializeField] private float _recoilForce;
    [SerializeField] private float _railgunStoppingAction;
    [SerializeField] private GameObject _RailgunEffect;

    [Header("Railgun powerup")]
    [SerializeField] private GameObject _railgunProjectilePrefabPU;
    [SerializeField] private float _railgunFirePeriodPU;
    [SerializeField] private float _railgunDamagePU;
    [SerializeField] private float _railgunProjectileSpeedPU;
    [SerializeField] private float _recoilForcePU;
    [SerializeField] private float _railgunStoppingActionPU;
    [SerializeField] private GameObject _RailgunEffectPU;

    private GameObject _railgunProjectilePrefabCurrent;
    private float _railgunFirePeriodCurrent;
    private float _railgunDamageCurrent;
    private float _railgunProjectileSpeedCurrent;
    private float _recoilForceCurrent;
    private float _railgunStoppingActionCurrent;
    private GameObject _RailgunEffectCurrent;

    [Header("Component references")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private BodyController _bodyController;
    [SerializeField] private AimGun _aimGun;
    [SerializeField] private Player _player;

    [Header("Hidden")]
    [HideInInspector] private float _nextFirePistol;
    [HideInInspector] private float _nextFireRailgun;
    [HideInInspector] private Transform _pistolShootingPoint;
    [HideInInspector] private Transform _railgunShootingPoint;


    private void Start()
    {
        _nextFirePistol = 0;
        _nextFireRailgun = 0;
    }

    private void Update()
    {
        CheckIfPowerUp();
        ChangeShootingPoint();
        FirePistol();
        FireRailgun();
    }

    private void FirePistol()
    {
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2) && Time.time >= _nextFirePistol)
        {
            GameObject effect = Instantiate(_pistolEffectCurrent, _pistolShootingPoint.position, Quaternion.Euler(new Vector3(0, 0, AimGun.RotationZ)));
            effect.transform.SetParent(_pistolShootingPoint);

            GameObject pistolProjectile = Instantiate(_pistolProjectilePrefabCurrent, _pistolShootingPoint.position, Quaternion.Euler(new Vector3(0, 0, AimGun.RotationZ)));
            PlayerProjectile projectileParameters = pistolProjectile.GetComponent<PlayerProjectile>();
            projectileParameters.Damage = _pistolDamageCurrent;
            projectileParameters.Speed = _pistolProjectileSpeedCurrent;
            projectileParameters.StoppingAction = _pistolStoppingActionCurrent;

            _nextFirePistol = Time.time + _pistolFirePeriodCurrent;
        }
    }

    private void FireRailgun()
    {
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1) && !Input.GetMouseButton(2) && Time.time >= _nextFireRailgun)
        {
            GameObject effect = Instantiate(_RailgunEffectCurrent, _railgunShootingPoint.position, Quaternion.Euler(new Vector3(0, 0, AimGun.RotationZ)));
            effect.transform.SetParent(_railgunShootingPoint);

            GameObject railgunProjectile = Instantiate(_railgunProjectilePrefabCurrent, _railgunShootingPoint.position, Quaternion.Euler(new Vector3(0, 0, AimGun.RotationZ)));
            PlayerProjectile projectileParameters = railgunProjectile.GetComponent<PlayerProjectile>();
            projectileParameters.Damage = _railgunDamageCurrent;
            projectileParameters.Speed = _railgunProjectileSpeedCurrent;
            projectileParameters.StoppingAction = _railgunStoppingActionCurrent;

            _rigidbody2D.AddForce(CalculateRecoilDirection() * _recoilForceCurrent, ForceMode2D.Impulse);

            _nextFireRailgun = Time.time + _railgunFirePeriodCurrent;
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

    private void SetPowerUpOn()
    {
        // Pistol PU
        _pistolProjectilePrefabCurrent = _pistolProjectilePrefabPU;
        _pistolFirePeriodCurrent = _pistolFirePeriodPU;
        _pistolDamageCurrent = _pistolDamagePU;
        _pistolProjectileSpeedCurrent = _pistolProjectileSpeedPU;
        _pistolStoppingActionCurrent = _pistolStoppingActionPU;
        _pistolEffectCurrent = _pistolEffectPU;

        // Railgun PU
        _railgunProjectilePrefabCurrent = _railgunProjectilePrefabPU;
        _railgunFirePeriodCurrent = _railgunFirePeriodPU;
        _railgunDamageCurrent = _railgunDamagePU;
        _railgunProjectileSpeedCurrent = _railgunProjectileSpeedPU;
        _recoilForceCurrent = _recoilForcePU;
        _railgunStoppingActionCurrent = _railgunStoppingActionPU;
        _RailgunEffectCurrent = _RailgunEffectPU;
    }
    private void SetPowerUpOff()
    {
        // Pistol Common
        _pistolProjectilePrefabCurrent = _pistolProjectilePrefab;
        _pistolFirePeriodCurrent = _pistolFirePeriod;
        _pistolDamageCurrent = _pistolDamage;
        _pistolProjectileSpeedCurrent = _pistolProjectileSpeed;
        _pistolStoppingActionCurrent = _pistiolStoppingAction;
        _pistolEffectCurrent = _pistolEffect;
        
        // Railgun Common
        _railgunProjectilePrefabCurrent =_railgunProjectilePrefab;
        _railgunFirePeriodCurrent = _railgunFirePeriod;
        _railgunDamageCurrent = _railgunDamage;
        _railgunProjectileSpeedCurrent = _railgunProjectileSpeed;
        _recoilForceCurrent = _recoilForce;
        _railgunStoppingActionCurrent = _railgunStoppingAction;
        _RailgunEffectCurrent = _RailgunEffect;
    }
}
