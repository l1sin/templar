using UnityEngine;

public class Helicopter : Enemy
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _acceleration;
    [SerializeField] private Vector2 _destination;
    [SerializeField] private float _distance;
    [SerializeField] private Vector2 _direction;

    [SerializeField] private float _altitude;
    [SerializeField] private float _maxRandomX;
    [SerializeField] private float _maxRandomY;
    [SerializeField] private float _minRandomX;
    [SerializeField] private float _minRandomY;
    [SerializeField] private float _reachDistance;

    [SerializeField] private float _waitTime;
    [SerializeField] private float _waitTimer;

    [SerializeField] private bool _stop;
    [SerializeField] private float _deceleration;
    protected override void Awake()
    {
        base.Awake();
        ChooseNewDestination();
    }

    protected override void Update()
    {
        base.Update();
        Flip();
        ClampVelocity();
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
        Debug.Log($"{random1} * {random2} = {randomX}");
        float randomY = Random.Range(0, _maxRandomY);
        Vector3 randomPosition = new Vector3(randomX, randomY, 0);
        _destination = Target.transform.position + Vector3.up * _altitude + randomPosition; 
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
}