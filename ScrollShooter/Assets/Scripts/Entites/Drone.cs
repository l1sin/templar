using UnityEngine;

public class Drone : Enemy
{
    [SerializeField] private Transform _minigun;
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _target;



    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _shootingPoint;
    private float _nextFire;
    [SerializeField] private float _firePeriod;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _damage;
    [SerializeField] private float _distance;

    [SerializeField] private float _rotationZDeg;
    [SerializeField] private Vector3 _difference;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private LineRenderer _lineRenderer;

    private void Start()
    {

    }

    private void Update()
    {
        RotateMinigun();
        Flip();
        Shoot();
    }

    private void RotateMinigun()
    {
        _difference = _target.transform.position + Vector3.up - transform.position;
        var RotationZRad = Mathf.Atan2(_difference.normalized.y, _difference.normalized.x);
        _rotationZDeg = RotationZRad * Mathf.Rad2Deg;

        _minigun.transform.rotation = Quaternion.Euler(0, 0, _rotationZDeg);
    }

    private void Flip()
    {
        if (_target.transform.position.x - transform.position.x > 0)
        {
            _body.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            _body.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void Shoot()
    {
        
        if (Input.GetKey(KeyCode.V) && Time.time >= _nextFire)
        {
            var hit = Physics2D.Raycast(_shootingPoint.position, _difference, _distance, _layerMask);

            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(1, new Vector3(Mathf.Abs(hit.point.x - _shootingPoint.transform.position.x), 0, 0));

            if (hit.collider.gameObject.layer == 7)
            {
                hit.collider.gameObject.GetComponent<BaseEntity>().GetDamage(_damage);
                Debug.Log("Hit");
            }
            else if (hit.collider.gameObject.layer == 10)
            {
                Debug.Log("Deflect");
            }

            _nextFire = Time.time + _firePeriod;
        }
    }
}
