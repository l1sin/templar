using UnityEngine;

public class Controlls : MonoBehaviour
{

    [Header("Movement Stuff")]
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _maxWalkVelocity;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private bool _goingFront;
    [SerializeField] private bool _isRunning;


    [Header("IsGrounded Stuff")]
    [SerializeField] private bool _isGrounded;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform _overlapBoxPoint;
    [SerializeField] private Vector2 _overlapBoxSize;

    [Header("Component references")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Animator _animator;
    [SerializeField] private BodyController _bodyController;

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
        Animate();
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
        // Accelerate
        if (_goingFront && !Input.GetKey(KeyCode.LeftShift))
        {
            if (_rigidbody2D.velocity.x < _maxVelocity && PlayerInput.Movement > 0)
            {
                _rigidbody2D.AddForce(new Vector2(PlayerInput.Movement * _acceleration, 0), ForceMode2D.Force);
            }
            else if (_rigidbody2D.velocity.x > -_maxVelocity && PlayerInput.Movement < 0)
            {
                _rigidbody2D.AddForce(new Vector2(PlayerInput.Movement * _acceleration, 0), ForceMode2D.Force);
            }
            _isRunning = true;
        }
        else
        {
            if (_rigidbody2D.velocity.x < _maxWalkVelocity && PlayerInput.Movement > 0)
            {
                _rigidbody2D.AddForce(new Vector2(PlayerInput.Movement * _acceleration, 0), ForceMode2D.Force);
            }
            else if (_rigidbody2D.velocity.x > -_maxWalkVelocity && PlayerInput.Movement < 0)
            {
                _rigidbody2D.AddForce(new Vector2(PlayerInput.Movement * _acceleration, 0), ForceMode2D.Force);
            }
            _isRunning = false;
        }
        
        // Limit acceleration
        if (_isGrounded)
        {
            if (_goingFront && !Input.GetKey(KeyCode.LeftShift))
            {
                if (_rigidbody2D.velocity.x > _maxVelocity)
                {
                    _rigidbody2D.velocity = new Vector2(_maxVelocity, _rigidbody2D.velocity.y);
                }
                else if (_rigidbody2D.velocity.x < -_maxVelocity)
                {
                    _rigidbody2D.velocity = new Vector2(-_maxVelocity, _rigidbody2D.velocity.y);
                }
            }
            else
            {
                if (_rigidbody2D.velocity.x > _maxWalkVelocity)
                {
                    _rigidbody2D.velocity = new Vector2(_maxWalkVelocity, _rigidbody2D.velocity.y);
                }
                else if (_rigidbody2D.velocity.x < -_maxWalkVelocity)
                {
                    _rigidbody2D.velocity = new Vector2(-_maxWalkVelocity, _rigidbody2D.velocity.y);
                }
            }
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

    private void Animate()
    {
        if (PlayerInput.Movement == 0)
        {
            _animator.SetBool(GlobalStrings.XIsInput, false);
        }
        else
        {
            _animator.SetBool(GlobalStrings.XIsInput, true);
        }

        if ((_bodyController.HeadAndBody.transform.rotation.y == 0 && PlayerInput.Movement > 0) || (_bodyController.HeadAndBody.transform.rotation.y != 0 && PlayerInput.Movement < 0))
        {
            _goingFront = true;
            _animator.SetBool(GlobalStrings.GoingFront, _goingFront);
        }
        else
        {
            _goingFront = false;
            _animator.SetBool(GlobalStrings.GoingFront, _goingFront);
        }

        _animator.SetBool(GlobalStrings.IsGrounded, _isGrounded);
        _animator.SetBool(GlobalStrings.IsRunning, _isRunning);
        _animator.SetFloat(GlobalStrings.YVelocity, _rigidbody2D.velocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_overlapBoxPoint.position, _overlapBoxSize);
    }
}
