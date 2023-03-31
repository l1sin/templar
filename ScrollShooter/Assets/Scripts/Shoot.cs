using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Main preferences")]
    [SerializeField] private Transform _shootingPoint;

    [Header("Pistol preferences")]
    [SerializeField] private GameObject _pistolProjectilePrefab;
    [SerializeField] private float _pistolFirePeriod;
    [SerializeField] private float _pistolDamage;
    [SerializeField] private float _pistolProjectileSpeed;

    [Header("Railgun preferences")]  
    [SerializeField] private GameObject _railgunProjectilePrefab;
    [SerializeField] private float _railgunFirePeriod;
    [SerializeField] private float _railgunDamage;
    [SerializeField] private float _railgunProjectileSpeed;
    [SerializeField] private float _recoilForce;
    
    [Header("Component references")]
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [Header("Hidden")]
    [HideInInspector] private float _nextFirePistol;
    [HideInInspector] private float _nextFireRailgun;

    private void Start()
    {
        _nextFirePistol = 0;
        _nextFireRailgun = 0;
    }

    private void Update()
    {
        FirePistol();
        FireRailgun();
    }

    private void FirePistol()
    {
        if (Input.GetMouseButton(0) && Time.time >= _nextFirePistol)
        {
            GameObject pistolProjectile = Instantiate(_pistolProjectilePrefab, _shootingPoint.position, Quaternion.Euler(transform.localEulerAngles));
            PlayerProjectile projectileParameters = pistolProjectile.GetComponent<PlayerProjectile>();
            projectileParameters.Damage = _pistolDamage;
            projectileParameters.Speed = _pistolProjectileSpeed;

            _nextFirePistol = Time.time + _pistolFirePeriod;
        }
    }

    private void FireRailgun()
    {
        if (Input.GetMouseButton(1) && Time.time >= _nextFireRailgun)
        {
            GameObject railgunProjectile = Instantiate(_railgunProjectilePrefab, _shootingPoint.position, Quaternion.Euler(transform.localEulerAngles));
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
}
