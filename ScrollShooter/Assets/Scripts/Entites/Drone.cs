using UnityEngine;

public class Drone : Enemy
{
    [Header("Main references")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _minigun;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private Transform _target;
    [SerializeField] private LineRenderer _lineRenderer;

    [Header("Shooting")]
    
    [SerializeField] private float _firePeriod;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _damage;
    [SerializeField] private float _distance;
    [SerializeField] private LayerMask _layerMask;
    private float _nextFire;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _spotDistance;
    [SerializeField] private Vector2 _patrolDistance;
    [SerializeField] private float _maxVelocity;
    private Vector2[] _patrolPoints;
    private Vector3 _direction;
    private bool _goForth;
    private bool _spotted;

    [Header("Rotation")]
    private float _rotationZDeg;
    private Vector3 _difference;

    [Header("Trace")]
    [SerializeField] private float _traceLength;
    private float _traceTimer;







    private void Start()
    {
        _difference = _target.transform.position + Vector3.up - transform.position;
        _goForth = true;
        _spotted = false;
        _patrolPoints = new Vector2[2];
        _patrolPoints[0] = (Vector2)transform.position - _patrolDistance;
        _patrolPoints[1] = (Vector2)transform.position + _patrolDistance;
    }

    private void Update()
    {
        RotateMinigun();
        Flip();
        Shoot();
        Trace();
        
    }

    private void FixedUpdate()
    {
        ClampVelocity();
        if (!_spotted)
        {
            Patrol();
            CheckForPlayer();
        }
        else MoveToDestination();
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

        if (_difference.magnitude < _distance && Time.time >= _nextFire)
        {
            var hit = Physics2D.Raycast(_shootingPoint.position, _difference, _distance, _layerMask);
            if (hit)
            {
                _lineRenderer.SetPosition(1, new Vector3(Mathf.Abs((hit.point - (Vector2)_shootingPoint.transform.position).magnitude), 0, 0));

                _traceTimer = _traceLength;

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

    private void Trace()
    {
        _traceTimer -= Time.deltaTime;
        if (_traceTimer > 0)
        {
            _lineRenderer.enabled = true;
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }

    private void MoveToDestination()
    {
        _direction = (_target.position + Vector3.up * 3 - transform.position).normalized;
        _rigidbody2D.AddForce(_direction * _moveSpeed * Time.deltaTime, ForceMode2D.Force);
    }

    private void Patrol()
    {

        if (_goForth) _direction = (_patrolPoints[0] - (Vector2)transform.position).normalized;
        else _direction = (_patrolPoints[0] - (Vector2)transform.position).normalized;

        _rigidbody2D.AddForce(_direction * _moveSpeed * Time.deltaTime, ForceMode2D.Force);

        if ((_patrolPoints[0] - (Vector2)transform.position).magnitude < 0.1f) _goForth = false;
        else if ((_patrolPoints[1] - (Vector2)transform.position).magnitude < 0.1f) _goForth = true;
    }

    private void CheckForPlayer()
    {
        if (_spotDistance > _difference.magnitude) _spotted = true;
    }

    private void ClampVelocity()
    {
        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _maxVelocity);
    }
}