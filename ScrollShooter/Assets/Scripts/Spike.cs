using UnityEngine;

public class Spike : Enemy
{
    [SerializeField] private Transform _target;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _noMoveDistance = 0.5f;
    [SerializeField] private float _contactDamage = 1f;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _attackCooldownTimer;

    void Update()
    {
        MoveToTarget();
        _attackCooldownTimer -= Time.deltaTime;
    }

    private void MoveToTarget()
    {
        if (Mathf.Abs(_target.position.x - transform.position.x) >= _noMoveDistance)
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
    }

    private void MoveRight()
    {
        _rigidbody2D.velocity = new Vector2(_speed, _rigidbody2D.velocity.y);
    }

    private void MoveLeft()
    {
        _rigidbody2D.velocity = new Vector2(-_speed, _rigidbody2D.velocity.y);
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
}
