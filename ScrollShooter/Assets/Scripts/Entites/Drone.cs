using UnityEngine;

public class Drone : Enemy
{
    [Header("Main references")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Minigun _minigun;

    [Header("Movement")]
    [SerializeField] private float _spotDistance;
    [SerializeField] private Vector2 _patrolDistance;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _altitude;
    [SerializeField] private float _maxRandomX;
    [SerializeField] private float _maxRandomY;
    [SerializeField] private float _minRandomX;
    [SerializeField] private float _minRandomY;
    [SerializeField] private float _reachDistance;
    [SerializeField] private float _tooFarDistance;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _correctionDeceleration;
    [SerializeField] private float _waitTime;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _groundCheckRadius;
    
    [Header("Hidden")]

    private Vector2[] _patrolPoints;
    private Vector2 _direction;
    private Vector2 _destination;
    private float _distance;
    private bool _stop;
    private bool _goForth;
    private bool _spotted;
    private float _waitTimer;
    [Header("Rotation")]
    private Vector3 _difference;

    protected override void Awake()
    {
        base.Awake();
        _difference = Target.position - transform.position;
        _goForth = true;
        _spotted = false;
        _minigun.enabled = _spotted;
        _patrolPoints = new Vector2[2];
        _patrolPoints[0] = (Vector2)transform.position - _patrolDistance;
        _patrolPoints[1] = (Vector2)transform.position + _patrolDistance;
    }

    protected override void Update()
    {
        base.Update();
        Flip();
        ResetTimers();
        if (!_spotted) CheckForPlayer();
        else
        {
            if (_distance < _reachDistance && _waitTimer <= 0) ChooseNewDestination();
            if ((Target.position - transform.position).magnitude > _tooFarDistance) ChooseNewDestination();
        }
        
    }

    private void FixedUpdate()
    {
        if (!_spotted) Patrol();
        else if (!_stop)
        {
            MoveToDestination();
        } 

        if (_distance < _reachDistance)
        {
            Stop();
        }
        if (_waitTimer <= 0)
        {
            _stop = false;
        }

        ClampVelocity();
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        SpotPlayer();
    }

    private void Flip()
    {
        if (!_spotted)
        {
            if (_direction.x > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
            else transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            if (Target.transform.position.x - transform.position.x > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
            else transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void ChooseNewDestination()
    {
        float random1 = Random.Range(_minRandomX, _maxRandomX);
        float random2 = Random.Range(0, 2) * 2 - 1;
        float randomX = random1 * random2;
        float randomY = Random.Range(0, _maxRandomY);
        Vector3 randomPosition = new Vector3(randomX, randomY, 0);
        _destination = Target.transform.position + Vector3.up * _altitude + randomPosition;
        if (Physics2D.OverlapCircle(_destination, _groundCheckRadius, _groundMask)) ChooseNewDestination();
    }

    private void Patrol()
    {

        if (_goForth) _destination = _patrolPoints[1];
        else _destination = _patrolPoints[0];

        MoveToDestination();

        if ((_patrolPoints[0] - (Vector2)transform.position).magnitude < 0.1f) _goForth = true;
        else if ((_patrolPoints[1] - (Vector2)transform.position).magnitude < 0.1f) _goForth = false;
    }

    private void MoveToDestination()
    {
        _direction = (_destination - (Vector2)transform.position).normalized;
        _distance = (_destination - (Vector2)transform.position).magnitude;
        _rigidbody2D.AddForce(_direction * _acceleration * _rigidbody2D.mass, ForceMode2D.Force);
        CorrectDirection();
    }


    private void CorrectDirection()
    {
        _rigidbody2D.AddForce(new Vector2(-_rigidbody2D.velocity.x * _correctionDeceleration, -_rigidbody2D.velocity.y * _correctionDeceleration), ForceMode2D.Force);
    }
    private void Stop()
    {
        _rigidbody2D.AddForce(new Vector2(-_rigidbody2D.velocity.x * _deceleration, -_rigidbody2D.velocity.y * _deceleration), ForceMode2D.Force);
        if (!_stop)
        {
            _stop = true;
            _waitTimer = _waitTime;
        }
    }

    private void CheckForPlayer()
    {
        if (_spotDistance > _difference.magnitude)
        {
            SpotPlayer();
        } 
    }

    private void SpotPlayer()
    {
        _spotted = true;
        _minigun.enabled = _spotted;
    }

    private void ClampVelocity()
    {
        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _maxVelocity);
    }

    private void ResetTimers()
    {
        _waitTimer -= Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _groundCheckRadius);
    }
}