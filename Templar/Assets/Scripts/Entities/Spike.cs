using UnityEngine;

public class Spike : Enemy
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [Header("Preferences")]
    [SerializeField] private float _acceleration = 6f;
    [SerializeField] private float _maxVelocity = 6f;
    [SerializeField] private float _contactDamage = 1f;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _spotDistance;
    [SerializeField] private Vector2 _patrolDistance;
    [SerializeField] private float _reachDistance = 0.2f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _groundCheckRadius;

    [Header("Hidden Values")]
    [HideInInspector] private float _attackCooldownTimer;
    [HideInInspector] private Vector2[] _patrolPoints;
    [HideInInspector] private Vector2 _playerDistance;
    [HideInInspector] private bool _goForth;
    [HideInInspector] private bool _isGrounded;
    [HideInInspector] public bool Spotted = false;

    protected override void Awake()
    {
        base.Awake();
        _goForth = true;
        _patrolPoints = new Vector2[2];
        _patrolPoints[0] = (Vector2)transform.position - _patrolDistance;
        _patrolPoints[1] = (Vector2)transform.position + _patrolDistance;
        _playerDistance = Target.position - transform.position;
    }

    protected override void Update()
    {
        base.Update();
        CheckGround();
        ResetTimers();
        _playerDistance = Target.position - transform.position;
    }

    private void FixedUpdate()
    {
        ClampVelocity();
        if (!Spotted)
        {
            CheckForPlayer();
            if (_isGrounded) Patrol();
        }
        else
        {
            if (_isGrounded) MoveToTarget();
        }
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        Spotted = true;
    }

    private void MoveToTarget()
    {
        if (Target.position.x - transform.position.x > 0)
        {
            MoveRight();
        }
        else
        {
            MoveLeft();
        }
    }

    private void MoveRight()
    {
        _rigidbody2D.AddForce(Vector2.right * _acceleration * _rigidbody2D.mass, ForceMode2D.Force);
    }

    private void MoveLeft()
    {
        _rigidbody2D.AddForce(Vector2.left * _acceleration * _rigidbody2D.mass, ForceMode2D.Force);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Collider2D thisCollider = collision.GetContact(0).collider;
        if (thisCollider.gameObject.layer == 7 && _attackCooldownTimer <= 0)
        {
            thisCollider.gameObject.GetComponent<BaseEntity>().GetDamage(_contactDamage);
            _attackCooldownTimer = _attackCooldown;
        }
        else if (thisCollider.gameObject.layer == 10 && _attackCooldownTimer <= 0)
        {
            Player.Instance.gameObject.GetComponent<EnergyShield>().AbsorbDamage(_contactDamage);
            _attackCooldownTimer = _attackCooldown;
        }
    }

    private void Patrol()
    {
        if (_goForth) MoveRight();
        else MoveLeft();

        if (Mathf.Abs((_patrolPoints[0] - (Vector2)transform.position).x) < _reachDistance)
        {
            _goForth = true;
        }
        else if (Mathf.Abs((_patrolPoints[1] - (Vector2)transform.position).x) < _reachDistance)
        {
            _goForth = false;
        }
    }

    private void CheckForPlayer()
    {
        if (_spotDistance > _playerDistance.magnitude) Spotted = true;
    }

    private void ClampVelocity()
    {
        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _maxVelocity);
    }

    private void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(transform.position, _groundCheckRadius, _groundMask);
    }

    private void ResetTimers()
    {
        _attackCooldownTimer -= Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _groundCheckRadius);
    }
}
