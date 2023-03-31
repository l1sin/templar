using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _railPrefab;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private float _firePeriodPistol;
    [SerializeField] private float _firePeriodRailgun;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _recoil;
    [HideInInspector] private float _nextFirePistol;
    [HideInInspector] private float _nextFireRailgun;

    private void Start()
    {
        _nextFirePistol = 0;
        _nextFireRailgun = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= _nextFirePistol)
        {
            Instantiate(_projectilePrefab, _shootingPoint.position, Quaternion.Euler(transform.localEulerAngles));
            _nextFirePistol = Time.time +_firePeriodPistol;
        }

        if (Input.GetMouseButton(1) && Time.time >= _nextFireRailgun)
        {
            Instantiate(_railPrefab, _shootingPoint.position, Quaternion.Euler(transform.localEulerAngles));
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
            Debug.Log(direction);
            _rigidbody2D.AddForce(-direction * _recoil, ForceMode2D.Impulse);
            _nextFireRailgun = Time.time + _firePeriodRailgun;
        }
    }
}
