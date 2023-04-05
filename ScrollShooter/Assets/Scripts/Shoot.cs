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

    [Header("Railgun preferences")]
    [SerializeField] private Transform _railgunShootingPointR;
    [SerializeField] private Transform _railgunShootingPointL;
    [SerializeField] private GameObject _railgunProjectilePrefab;
    [SerializeField] private float _railgunFirePeriod;
    [SerializeField] private float _railgunDamage;
    [SerializeField] private float _railgunProjectileSpeed;
    [SerializeField] private float _recoilForce;
    
    [Header("Component references")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private BodyController _bodyController;
    [SerializeField] private AimGun _aimGun;

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
        ChangeShootingPoint();
        FirePistol();
        FireRailgun();
    }

    private void FirePistol()
    {
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1) && Time.time >= _nextFirePistol)
        {
            GameObject pistolProjectile = Instantiate(_pistolProjectilePrefab, _pistolShootingPoint.position, Quaternion.Euler(_aimGun.ActiveHand.transform.localEulerAngles));
            
            if (!(_bodyController.HeadAndBody.transform.rotation.y == 0))
            {
                pistolProjectile.transform.rotation = Quaternion.Euler(_aimGun.ActiveHand.transform.localEulerAngles + new Vector3(0, 0, 180));
            }
            PlayerProjectile projectileParameters = pistolProjectile.GetComponent<PlayerProjectile>();
            projectileParameters.Damage = _pistolDamage;
            projectileParameters.Speed = _pistolProjectileSpeed;

            _nextFirePistol = Time.time + _pistolFirePeriod;
        }
    }

    private void FireRailgun()
    {
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1) && Time.time >= _nextFireRailgun)
        {
            GameObject railgunProjectile = Instantiate(_railgunProjectilePrefab, _railgunShootingPoint.position, Quaternion.Euler(_aimGun.ActiveHand.transform.localEulerAngles));
            if (!(_bodyController.HeadAndBody.transform.rotation.y == 0))
            {
                railgunProjectile.transform.rotation = Quaternion.Euler(_aimGun.ActiveHand.transform.localEulerAngles + new Vector3(0, 0, 180));
            }
            PlayerProjectile projectileParameters = railgunProjectile.GetComponent<PlayerProjectile>();
            projectileParameters.Damage = _railgunDamage;
            projectileParameters.Speed = _railgunProjectileSpeed;

            _rigidbody2D.AddForce(CalculateRecoilDirection() * _recoilForce, ForceMode2D.Impulse);

            _nextFireRailgun = Time.time + _railgunFirePeriod;
        }
    }

    private Vector2 CalculateRecoilDirection()
    {
        Vector2 direction;
        if (transform.localEulerAngles.y == 0)
        {
            direction.x = Mathf.Cos(transform.localEulerAngles.z * Mathf.Deg2Rad);
        }
        else
        {
            direction.x = -Mathf.Cos(transform.localEulerAngles.z * Mathf.Deg2Rad);
        }
        direction.y = Mathf.Sin(transform.localEulerAngles.z * Mathf.Deg2Rad);
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
}
