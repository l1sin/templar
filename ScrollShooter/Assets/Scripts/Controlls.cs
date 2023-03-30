using UnityEngine;

public class Controlls : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Movement Stuff")]
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _jumpHeight;


    [Header("IsGrounded Stuff")]
    [SerializeField] private bool _isGrounded;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform _overlapBoxPoint;
    [SerializeField] private Vector2 _overlapBoxSize;

    [Header("Component references")]
    [SerializeField] private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _isGrounded = false;
    }

    private void Update()
    {
        CheckGround();
    }

    private void FixedUpdate()
    {
        Jump();
        Move();
        Brake();
    }

    private void Jump()
    {
        if (PlayerInput.Jump && _isGrounded)
        {
            float jumpForce = Mathf.Sqrt(_jumpHeight * -2 * (Physics2D.gravity.y * _rigidbody2D.gravityScale));
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Move()
    {
        if (_rigidbody2D.velocity.x < _maxVelocity && PlayerInput.Movement > 0)
        {
            _rigidbody2D.AddForce(new Vector2(PlayerInput.Movement * _acceleration, 0), ForceMode2D.Force);
        }
        else if (_rigidbody2D.velocity.x > -_maxVelocity && PlayerInput.Movement < 0)
        {
            _rigidbody2D.AddForce(new Vector2(PlayerInput.Movement * _acceleration, 0), ForceMode2D.Force);
        }
        else if (_rigidbody2D.velocity.x > _maxVelocity)
        {
            _rigidbody2D.velocity = new Vector2(_maxVelocity, _rigidbody2D.velocity.y);
        }
        else if (_rigidbody2D.velocity.x < -_maxVelocity)
        {
            _rigidbody2D.velocity = new Vector2(-_maxVelocity, _rigidbody2D.velocity.y);
        }

        if (AimGun.MousePosOffset.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        if (AimGun.MousePosOffset.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void CheckGround()
    {
        if (Physics2D.OverlapBox(_overlapBoxPoint.position, _overlapBoxSize, 0, _whatIsGround))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    private void Brake()
    {
        if (PlayerInput.Movement == 0 && _isGrounded)
        {
            _rigidbody2D.AddForce(new Vector2(-_rigidbody2D.velocity.x * _deceleration, 0));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_overlapBoxPoint.position, _overlapBoxSize);
    }
}
