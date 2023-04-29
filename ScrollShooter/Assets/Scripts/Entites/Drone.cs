using UnityEngine;

public class Drone : Enemy
{
    [Header("Main references")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _minigun;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private LineRenderer _laserPointer;
    [SerializeField] private GameObject _tracePrefab;
    [SerializeField] private Animator _minigunAnimator;

    [Header("Shooting")]
    [SerializeField] private float _firePeriod;
    [SerializeField] private float _damage;
    [SerializeField] private float _distance;
    [SerializeField] private float _missRad;
    private float _shootingAngle;
    private Vector2 _shootingDirection;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _minigunAnimationSpeed = 20f;
    [SerializeField] private float _burstAmount = 5;
    private float _shotAmount;
    [SerializeField] private float _burstReloadLength = 2;
    private float _burstReloadTimer;

    private float _nextFire;
    private RaycastHit2D _laserTrackHit;
    private RaycastHit2D _hit;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _spotDistance;
    [SerializeField] private Vector2 _patrolDistance;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private bool _canMove;
    private Vector2[] _patrolPoints;
    private Vector3 _direction;
    private bool _goForth;
    private bool _spotted;

    [Header("Rotation")]
    private float _rotationZDeg;
    private Vector3 _difference;

    [Header("Trace")]
    [SerializeField] private float _traceLength;

    private void Start()
    {
        _burstReloadTimer = _burstReloadLength;
        _minigunAnimator.SetFloat(GlobalStrings.MinigunSpeed, 0);
        _difference = _target.transform.position + Vector3.up - transform.position;
        _goForth = true;
        _spotted = false;
        _patrolPoints = new Vector2[2];
        _patrolPoints[0] = (Vector2)transform.position - _patrolDistance;
        _patrolPoints[1] = (Vector2)transform.position + _patrolDistance;
    }

    protected override void Update()
    {
        base.Update();
        _difference = _target.transform.position + Vector3.up - transform.position;

        if (!_spotted) CheckForPlayer();
        else
        {
            RotateMinigun();
            TrackTarget();
            if (_shotAmount < _burstAmount)
            {
                Shoot();
            }
            else
            {
                _minigunAnimator.SetFloat(GlobalStrings.MinigunSpeed, 0);
                _burstReloadTimer -= Time.deltaTime;
                if (_burstReloadTimer < 0)
                {
                    _shotAmount = 0;
                    _burstReloadTimer = _burstReloadLength;
                }
            }
            
        }
        Flip(); 
    }

    private void FixedUpdate()
    {
        if (!_canMove) return;
        ClampVelocity();
        if (!_spotted) Patrol();
        else MoveToDestination();
    }

    private void RotateMinigun()
    {
        var RotationZRad = Mathf.Atan2(_difference.normalized.y, _difference.normalized.x);
        _rotationZDeg = RotationZRad * Mathf.Rad2Deg;

        _minigun.transform.rotation = Quaternion.Euler(0, 0, _rotationZDeg);
    }

    private void Flip()
    {
        if (!_spotted)
        {
            if (_direction.x > 0)
            {
                _body.transform.rotation = Quaternion.Euler(0, 0, 0);
                _minigun.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                _body.transform.rotation = Quaternion.Euler(0, 180, 0);
                _minigun.transform.rotation = Quaternion.Euler(0, 180, 0);
            } 
        }
        else
        {
            if (_target.transform.position.x - transform.position.x > 0) _body.transform.rotation = Quaternion.Euler(0, 0, 0);
            else _body.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void Shoot()
    {
        _minigunAnimator.SetFloat(GlobalStrings.MinigunSpeed, _minigunAnimationSpeed);

        if (_difference.magnitude < _distance && Time.time >= _nextFire)
        {
            _shootingAngle = _rotationZDeg + Random.Range(-_missRad, _missRad);
            _shootingDirection = new Vector2(Mathf.Cos(_shootingAngle * Mathf.Deg2Rad), Mathf.Sin(_shootingAngle * Mathf.Deg2Rad));

            _hit = Physics2D.Raycast(_shootingPoint.position, _shootingDirection, _distance, _layerMask);

            _shotAmount++;

            if (_hit)
            {
                var trace = Trace(_hit.point - (Vector2)_shootingPoint.position);
                if (_hit.collider.gameObject.layer == 7)
                {
                    _hit.collider.gameObject.GetComponent<BaseEntity>().GetDamage(_damage);
                }
                else if (_hit.collider.gameObject.layer == 10)
                {
                    Vector3 normal = _hit.normal;
                    trace.positionCount++;
                    var newRay = Physics2D.Raycast(trace.GetPosition(1), Vector2.Reflect(_shootingDirection, normal).normalized * _distance, Mathf.Infinity, _layerMask);
                    trace.SetPosition(2, newRay.point);
                }
                _nextFire = Time.time + _firePeriod;
            }
            else
            {
                Trace(_shootingDirection * _distance);
            }
        }
    }

    private void TrackTarget()
    {
        _laserPointer.enabled = true;
        _laserTrackHit = Physics2D.Raycast(_shootingPoint.position, _difference, Mathf.Infinity, _layerMask);
        _laserPointer.SetPosition(1, new Vector3(Mathf.Abs((_laserTrackHit.point - (Vector2)_shootingPoint.transform.position).magnitude), 0, 0));
    }

    private void MoveToDestination()
    {
        _direction = (_target.position + Vector3.up * 3 - transform.position).normalized;
        _rigidbody2D.AddForce(_direction * _moveSpeed, ForceMode2D.Force);
    }

    private LineRenderer Trace(Vector2 endpos)
    {
        var trace = Instantiate(_tracePrefab, _shootingPoint.position, Quaternion.identity);
        var lineRenderer = trace.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(1, endpos);
        return lineRenderer;
    }

    private void Patrol()
    {

        if (_goForth) _direction = (_patrolPoints[1] - (Vector2)transform.position).normalized;
        else _direction = (_patrolPoints[0] - (Vector2)transform.position).normalized;

        _rigidbody2D.AddForce(_direction * _moveSpeed, ForceMode2D.Force);

        if ((_patrolPoints[0] - (Vector2)transform.position).magnitude < 0.1f) _goForth = true;
        else if ((_patrolPoints[1] - (Vector2)transform.position).magnitude < 0.1f) _goForth = false;
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