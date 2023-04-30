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

    [Header("Powerup stats")]
    [SerializeField] private float _accelerationPU;
    [SerializeField] private float _maxVelocityPU;
    [SerializeField] private float _maxWalkVelocityPU;
    [SerializeField] private float _decelerationPU;
    [SerializeField] private float _jumpHeightPU;

    private float _accelerationCurrent;
    private float _maxVelocityCurrent;
    private float _maxWalkVelocityCurrent;
    private float _decelerationCurrent;
    private float _jumpHeightCurrent;

    [Header("IsGrounded Stuff")]
    [SerializeField] private bool _isGrounded;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Vector2 _overlapBoxPoint;
    [SerializeField] private Vector2 _overlapBoxSize;

    [Header("Component references")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Animator _animator;
    [SerializeField] private BodyController _bodyController;
    [SerializeField] private Player _player;

    [Header("Animation preferences")]
    [SerializeField] private float _animationSpeed = 1f;
    [SerializeField] private float _animationSpeedPU = 1.5f;
    private float _animationSpeedCurrent;

    private void Awake()
    {
        _isGrounded = false;
    }

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        CheckGround();
        CheckPowerUp();
    }

    private void FixedUpdate()
    {
        Jump();
        Move();
        Brake();
        Animate();
    }

    private void SetPowerupOn()
    {
        _accelerationCurrent = _accelerationPU;
        _maxVelocityCurrent = _maxVelocityPU;
        _maxWalkVelocityCurrent = _maxWalkVelocityPU;
        _decelerationCurrent = _decelerationPU;
        _jumpHeightCurrent = _jumpHeightPU;

        _animationSpeedCurrent = _animationSpeedPU;
    }
    private void SetPowerupOff()
    {
        _accelerationCurrent = _acceleration;
        _maxVelocityCurrent = _maxVelocity;
        _maxWalkVelocityCurrent = _maxWalkVelocity;
        _decelerationCurrent = _deceleration;
        _jumpHeightCurrent = _jumpHeight;

        _animationSpeedCurrent = _animationSpeed;
    }
    private void CheckPowerUp()
    {
        if (_player.Powerup)
        {
            SetPowerupOn();
        }
        else 
        {
            SetPowerupOff();
        }
    }

    private void Jump()
    {
        if (PlayerInput.Jump && _isGrounded)
        {
            float jumpForce = Mathf.Sqrt(_jumpHeightCurrent * -2 * (Physics2D.gravity.y * _rigidbody2D.gravityScale));
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Move()
    {
        // Accelerate
        if (_goingFront && !PlayerInput.LShift)
        {
            if (_rigidbody2D.velocity.x < _maxVelocityCurrent && PlayerInput.Movement > 0)
            {
                _rigidbody2D.AddForce(new Vector2(PlayerInput.Movement * _accelerationCurrent, 0), ForceMode2D.Force);
            }
            else if (_rigidbody2D.velocity.x > -_maxVelocityCurrent && PlayerInput.Movement < 0)
            {
                _rigidbody2D.AddForce(new Vector2(PlayerInput.Movement * _accelerationCurrent, 0), ForceMode2D.Force);
            }
            _isRunning = true;
        }
        else
        {
            if (_rigidbody2D.velocity.x < _maxWalkVelocityCurrent && PlayerInput.Movement > 0)
            {
                _rigidbody2D.AddForce(new Vector2(PlayerInput.Movement * _accelerationCurrent, 0), ForceMode2D.Force);
            }
            else if (_rigidbody2D.velocity.x > -_maxWalkVelocityCurrent && PlayerInput.Movement < 0)
            {
                _rigidbody2D.AddForce(new Vector2(PlayerInput.Movement * _accelerationCurrent, 0), ForceMode2D.Force);
            }
            _isRunning = false;
        }
        
        // Limit acceleration
        if (_isGrounded)
        {
            if (_goingFront && !PlayerInput.LShift)
            {
                if (_rigidbody2D.velocity.x > _maxVelocityCurrent)
                {
                    _rigidbody2D.velocity = new Vector2(_maxVelocityCurrent, _rigidbody2D.velocity.y);
                }
                else if (_rigidbody2D.velocity.x < -_maxVelocityCurrent)
                {
                    _rigidbody2D.velocity = new Vector2(-_maxVelocityCurrent, _rigidbody2D.velocity.y);
                }
            }
            else
            {
                if (_rigidbody2D.velocity.x > _maxWalkVelocityCurrent)
                {
                    _rigidbody2D.velocity = new Vector2(_maxWalkVelocityCurrent, _rigidbody2D.velocity.y);
                }
                else if (_rigidbody2D.velocity.x < -_maxWalkVelocityCurrent)
                {
                    _rigidbody2D.velocity = new Vector2(-_maxWalkVelocityCurrent, _rigidbody2D.velocity.y);
                }
            }
        }
    }

    private void CheckGround()
    {
        if (Physics2D.OverlapBox((Vector2)transform.position + _overlapBoxPoint, _overlapBoxSize, 0, _whatIsGround))
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
            _rigidbody2D.AddForce(new Vector2(-_rigidbody2D.velocity.x * _decelerationCurrent, 0));
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

        _animator.SetFloat(GlobalStrings.PlayerAnimationSpeed, _animationSpeedCurrent);
        _animator.SetFloat(GlobalStrings.PlayerAnimationWalkSpeed, _animationSpeedCurrent * 0.75f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + _overlapBoxPoint, _overlapBoxSize);
    }
}
