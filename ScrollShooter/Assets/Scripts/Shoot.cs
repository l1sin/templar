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

    [Header("Pistol powerup")]
    [SerializeField] private float _pistolFirePeriodPU;
    [SerializeField] private float _pistolDamagePU;
    [SerializeField] private float _pistolProjectileSpeedPU;

    private float _pistolFirePeriodCurrent;
    private float _pistolDamageCurrent;
    private float _pistolProjectileSpeedCurrent;


    [Header("Railgun preferences")]
    [SerializeField] private Transform _railgunShootingPointR;
    [SerializeField] private Transform _railgunShootingPointL;
    [SerializeField] private GameObject _railgunProjectilePrefab;
    [SerializeField] private float _railgunFirePeriod;
    [SerializeField] private float _railgunDamage;
    [SerializeField] private float _railgunProjectileSpeed;
    [SerializeField] private float _recoilForce;

    [Header("Railgun powerup")]
    [SerializeField] private float _railgunFirePeriodPU;
    [SerializeField] private float _railgunDamagePU;
    [SerializeField] private float _railgunProjectileSpeedPU;
    [SerializeField] private float _recoilForcePU;

    private float _railgunFirePeriodCurrent;
    private float _railgunDamageCurrent;
    private float _railgunProjectileSpeedCurrent;
    private float _recoilForceCurrent;

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
            GameObject pistolProjectile = Instantiate(_pistolProjectilePrefab, _pistolShootingPoint.position, Quaternion.Euler(new Vector3(0, 0, AimGun.RotationZ)));
            PlayerProjectile projectileParameters = pistolProjectile.GetComponent<PlayerProjectile>();
            projectileParameters.Damage = _pistolDamageCurrent;
            projectileParameters.Speed = _pistolProjectileSpeedCurrent;

            _nextFirePistol = Time.time + _pistolFirePeriodCurrent;
        }
    }

    private void FireRailgun()
    {
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1) && !Input.GetMouseButton(2) && Time.time >= _nextFireRailgun)
        {
            GameObject railgunProjectile = Instantiate(_railgunProjectilePrefab, _railgunShootingPoint.position, Quaternion.Euler(new Vector3(0, 0, AimGun.RotationZ)));
            PlayerProjectile projectileParameters = railgunProjectile.GetComponent<PlayerProjectile>();
            projectileParameters.Damage = _railgunDamageCurrent;
            projectileParameters.Speed = _railgunProjectileSpeedCurrent;

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
        if (_player._powerUp)
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
        _pistolFirePeriodCurrent = _pistolFirePeriodPU;
        _pistolDamageCurrent = _pistolDamagePU;
        _pistolProjectileSpeedCurrent = _pistolProjectileSpeedPU;
        _railgunFirePeriodCurrent = _railgunFirePeriodPU;
        _railgunDamageCurrent = _railgunDamagePU;
        _railgunProjectileSpeedCurrent = _railgunProjectileSpeedPU;
        _recoilForceCurrent = _recoilForcePU;
    }
    private void SetPowerUpOff()
    {
        _pistolFirePeriodCurrent = _pistolFirePeriod;
        _pistolDamageCurrent = _pistolDamage;
        _pistolProjectileSpeedCurrent = _pistolProjectileSpeed;
        _railgunFirePeriodCurrent = _railgunFirePeriod;
        _railgunDamageCurrent = _railgunDamage;
        _railgunProjectileSpeedCurrent = _railgunProjectileSpeed;
        _recoilForceCurrent = _recoilForce;
    }
}
