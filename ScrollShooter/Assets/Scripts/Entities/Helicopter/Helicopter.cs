using UnityEngine;
using UnityEngine.UI;

public class Helicopter : Enemy
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private GameObject _winTimerPrefab;

    [Header("Preferences")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _waitTime;
    [SerializeField] private float _altitude;
    [SerializeField] private float _maxRandomX;
    [SerializeField] private float _maxRandomY;
    [SerializeField] private float _minRandomX;
    [SerializeField] private float _minRandomY;
    [SerializeField] private float _reachDistance;

    [Header("Hidden Values")]
    [HideInInspector] private Vector2 _destination;
    [HideInInspector] private Vector2 _direction;
    [HideInInspector] private float _distance;
    [HideInInspector] private float _waitTimer;
    [HideInInspector] private bool _stop;
    [HideInInspector] private GameObject _healthBar;
    [HideInInspector] private Image _healthBarImage;

    protected override void Awake()
    {
        base.Awake();
        ChooseNewDestination();
        _healthBar = UI.Instance.InstantiateMenuNoQueue(UI.Instance.BossHPBar);
        _healthBarImage = _healthBar.transform.GetChild(1).GetComponentInChildren<Image>();
        _healthBarImage.fillAmount = 1;
    }

    protected override void Update()
    {
        base.Update();
        Flip();
        if (_distance < _reachDistance)
        {
            Stop();
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0) ChooseNewDestination();
        }
    }

    private void FixedUpdate()
    {
        if (_waitTimer <= 0)
        {
            MoveToDestination();
            _stop = false;
        }
        ClampVelocity();
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        _healthBarImage.fillAmount = CurrentHealth / MaxHealth;
    }

    public override void Die()
    {
        Instantiate(Drop, transform.position, Quaternion.identity);
        Instantiate(VFX, transform.position, Quaternion.identity);
        Instantiate(_winTimerPrefab, transform.position, Quaternion.identity);
        AudioManager.Instance.MakeSound(transform, _deathSounds, _deathMixerGroup);
        Destroy(gameObject);
    }

    private void ClampVelocity()
    {
        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _maxVelocity);
    }

    private void Flip()
    {
        if (Target.transform.position.x - transform.position.x > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        else transform.rotation = Quaternion.Euler(0, 180, 0);
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

    private void MoveToDestination()
    {
        _direction = (_destination - (Vector2)transform.position).normalized;
        _distance = (_destination - (Vector2)transform.position).magnitude;
        _rigidbody2D.AddForce(_direction * _acceleration * _rigidbody2D.mass, ForceMode2D.Force);
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

    private void OnDestroy()
    {
        Destroy(_healthBar);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _groundCheckRadius);
    }
}