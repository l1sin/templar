using UnityEngine;

public class Spike : Enemy
{
    [SerializeField] private Transform _target;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _acceleration = 6f;
    [SerializeField] private float _maxVelocity = 6f;
    [SerializeField] private float _contactDamage = 1f;
    [SerializeField] private float _attackCooldown = 1f;
    private float _attackCooldownTimer;

    [SerializeField] private float _spotDistance;
    [SerializeField] private Vector2 _patrolDistance;
    private Vector2[] _patrolPoints;
    private Vector2 _playerDistance;
    private bool _goForth;
    private bool _spotted;

    private void Start()
    {
        _goForth = true;
        _spotted = false;
        _patrolPoints = new Vector2[2];
        _patrolPoints[0] = (Vector2)transform.position - _patrolDistance;
        _patrolPoints[1] = (Vector2)transform.position + _patrolDistance;
        _playerDistance = _target.position - transform.position;
    }

    protected override void Update()
    {
        base.Update();
        _attackCooldownTimer -= Time.deltaTime;
        _playerDistance = _target.position - transform.position;
    }

    private void FixedUpdate()
    {
        ClampVelocity();
        if (!_spotted)
        {
            CheckForPlayer();
            Patrol();
        }
        else
        {
            MoveToTarget();
        }
       
    }

    private void MoveToTarget()
    {
        if (_target.position.x - transform.position.x > 0)
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
        if (collision.gameObject.layer == 7 && _attackCooldownTimer <= 0)
        {
            collision.gameObject.GetComponent<Player>().GetDamage(_contactDamage);
            _attackCooldownTimer = _attackCooldown;
        }
        else if (collision.gameObject.layer == 10 && _attackCooldownTimer <= 0)
        {

        }
    }

    private void Patrol()
    {
        if (_goForth) MoveRight();
        else MoveLeft();

        if (Mathf.Abs((_patrolPoints[0] - (Vector2)transform.position).x) < 0.1f) _goForth = true;
        else if (Mathf.Abs((_patrolPoints[1] - (Vector2)transform.position).x) < 0.1f) _goForth = false;
    }

    private void CheckForPlayer()
    {
        if (_spotDistance > _playerDistance.magnitude) _spotted = true;
    }

    private void ClampVelocity()
    {
        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _maxVelocity);
    }
}
