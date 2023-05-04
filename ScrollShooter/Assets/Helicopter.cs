using UnityEngine;

public class Helicopter : Enemy
{
    [Header("Components References")]
    [SerializeField] private Transform[] _miniguns;
    [SerializeField] private Transform[] _laserguns;

    [SerializeField] private Animator[] _minigunAnimators;
    [SerializeField] private Animator[] _lasergunAnimators;

    [Header("Minigun Preferences")]
    [SerializeField] private float _minigunAnimationSpeed;
    [SerializeField] private float _missDeg;
    [SerializeField] private float _minigunDamage;

    [SerializeField] private float _burstAmount;
    [SerializeField] private float _shotAmount;

    [SerializeField] private float _burstReloadTimer;
    [SerializeField] private float _burstReloadLength;
    [SerializeField] private float _nextFireTimer;
    [SerializeField] private float _firePeriod;

    [SerializeField] private Transform[] _minigunShootingPoints;
    [SerializeField] private GameObject _tracerPrefab;
    [SerializeField] private GameObject _tracerReflectedPrefab;

    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private LayerMask _stopMask;

    [SerializeField] private float _reflectLength;
    [SerializeField] private float _shootDistance;

    private bool _shotPerformed;

    protected override void Awake()
    {
        base.Awake();
        _burstReloadTimer = _burstReloadLength;
        _nextFireTimer = 0;        
    }
    protected override void Update()
    {
        base.Update();
        RotateWeapons();
        ResetTimer();

        if (_shotAmount < _burstAmount)
        {
            Shoot();
        }
        else
        {
            foreach(Animator minigunAnimator in _minigunAnimators) minigunAnimator.SetFloat(GlobalStrings.MinigunSpeed, 0);
            _burstReloadTimer -= Time.deltaTime;
            if (_burstReloadTimer < 0)
            {
                _shotAmount = 0;
                _burstReloadTimer = _burstReloadLength;
            }
        }
    }
    private void RotateWeapons()
    {
        foreach (Transform minigun in _miniguns) minigun.transform.rotation = Quaternion.Euler(0, 0, CalculateRotationZDeg(minigun, Target));
        foreach (Transform lasergun in _laserguns) lasergun.transform.rotation = Quaternion.Euler(0, 0, CalculateRotationZDeg(lasergun, Target));
    }
    private void Shoot()
    {
        for (int i = 0; i < _miniguns.Length; i++)
        {
            _minigunAnimators[i].SetFloat(GlobalStrings.MinigunSpeed, _minigunAnimationSpeed);

            if (_nextFireTimer <= 0)
            {
                var shootingDirection = RandomizeShootingDirection(_miniguns[i], Target, _missDeg);
                RaycastHit2D hit = Physics2D.Raycast(_minigunShootingPoints[i].position, shootingDirection, _shootDistance, _enemyMask);


                if (hit)
                {
                    Trace(_minigunShootingPoints[i], hit.point - (Vector2)_minigunShootingPoints[i].position);
                    if (hit.collider.gameObject.layer == 7)
                    {
                        hit.collider.GetComponent<BaseEntity>().GetDamage(_minigunDamage);
                    }
                    else if (hit.collider.gameObject.layer == 10)
                    {
                        Player.Instance.gameObject.GetComponent<EnergyShield>().AbsorbDamage(_minigunDamage);
                        TraceReflect(_minigunShootingPoints[i], hit.point, hit.normal);
                    }
                }
                else
                {
                    Trace(_minigunShootingPoints[i], shootingDirection * _shootDistance);
                }
                _shotPerformed = true;
            }
        }
        if (_shotPerformed)
        {
            _shotPerformed = false;
            _nextFireTimer = _firePeriod;
            _shotAmount++;
        }
    }
    private void ResetTimer()
    {
        _nextFireTimer -= Time.deltaTime;
    }
    private LineRenderer Trace(Transform shootingPoint, Vector2 endpos)
    {
        var trace = Instantiate(_tracerPrefab, shootingPoint.position, Quaternion.identity);
        var lineRenderer = trace.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(1, endpos);
        return lineRenderer;
    }

    private LineRenderer TraceReflect(Transform shootingPoint, Vector2 hitPoint,  Vector3 normal)
    {
        GameObject trace = Instantiate(_tracerReflectedPrefab, hitPoint, Quaternion.identity);
        LineRenderer lineRenderer = trace.GetComponent<LineRenderer>();
        Vector2 reflectionDirection = Vector2.Reflect(hitPoint - (Vector2)shootingPoint.position, normal).normalized;
        float reflectionLength = Random.Range(0, _reflectLength);
        lineRenderer.SetPosition(1, reflectionDirection * reflectionLength);
        return lineRenderer;
    }

    private Vector3 CalculateDirection(Transform myTransform, Transform targetTransform)
    {
        Vector3 direction = (targetTransform.position - myTransform.position).normalized;
        return direction;
    }
    private float CalculateRotationZRad(Transform myTransform, Transform targetTransform)
    {
        Vector3 direction;
        float rotationZRad;

        direction = (targetTransform.position - myTransform.position).normalized;
        rotationZRad = Mathf.Atan2(direction.normalized.y, direction.normalized.x);

        return rotationZRad;
    }
    private float CalculateRotationZDeg(Transform myTransform, Transform targetTransform)
    {
        Vector3 direction;
        float rotationZRad;
        float rotationZDeg;

        direction = (targetTransform.position - myTransform.position).normalized;
        rotationZRad = Mathf.Atan2(direction.normalized.y, direction.normalized.x);
        rotationZDeg = rotationZRad * Mathf.Rad2Deg;

        return rotationZDeg;
    }
    private Vector3 RandomizeShootingDirection(Transform myTransform, Transform targetTransform, float missDeg)
    {
        float newShootingAngle = CalculateRotationZDeg(myTransform, targetTransform) + Random.Range(-missDeg, missDeg);
        Random.InitState(System.DateTime.Now.Millisecond);
        Vector3 newShootingDirection = new Vector2(Mathf.Cos(newShootingAngle * Mathf.Deg2Rad), Mathf.Sin(newShootingAngle * Mathf.Deg2Rad));

        return newShootingDirection;
    }
}
