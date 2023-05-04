using UnityEngine;
using static UnityEngine.ParticleSystem;

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

    [SerializeField] private float _minigunBurstAmount;
    [SerializeField] private float _minigunShotAmount;

    [SerializeField] private float _minigunBurstReloadTimer;
    [SerializeField] private float _minigunBurstReloadLength;
    [SerializeField] private float _minigunNextFireTimer;
    [SerializeField] private float _minigunFirePeriod;

    [SerializeField] private Transform[] _minigunShootingPoints;
    [SerializeField] private GameObject _tracerPrefab;
    [SerializeField] private GameObject _tracerReflectedPrefab;

    [SerializeField] private LayerMask _railgunHitLayerMask;

    [SerializeField] private float _reflectLength;
    [SerializeField] private float _shootDistance;

    private bool _shotPerformed;

    [SerializeField] private bool _minigunEnabled;

    [Header("Laser Preferences")]
    [SerializeField] private LineRenderer[] _lineRenderers;
    [SerializeField] private SpriteRenderer[] _laserImpactRenderers;
    [SerializeField] private Transform[] _lasergunShootingPoints;
    [SerializeField] private Vector2 _beamSize;
    [SerializeField] private LayerMask _stopLaserLayerMask;
    [SerializeField] private LayerMask _hitLaserLayerMask;
    [SerializeField] private GameObject _laserImpactVFX;

    [SerializeField] private float _laserDamage;
    [SerializeField] private float _laserDamagePeriod;
    [SerializeField] private float _laserBurstOnPeriod;
    [SerializeField] private float _laserBurstOffPeriod;

    [SerializeField] private float _laserBurstOnTimer;
    [SerializeField] private float _laserBurstOffTimer;
    [SerializeField] private float _laserNextDamageTimer;
    [SerializeField] private bool _laserBurst = false;

    [SerializeField] private bool _lasergunEnabled;

    protected override void Awake()
    {
        base.Awake();
        _minigunBurstReloadTimer = _minigunBurstReloadLength;
        _minigunNextFireTimer = 0;

        _laserBurstOnTimer = _laserBurstOnPeriod;
        _laserBurstOffTimer = _laserBurstOffPeriod;
        foreach(SpriteRenderer laserImpactRenderer in _laserImpactRenderers) laserImpactRenderer.enabled = false;
    }
    protected override void Update()
    {
        base.Update();
        RotateWeapons();
        ResetTimers();

        if (_lasergunEnabled)
        {
            if (_laserBurst)
            {
                RenderLaser();
                DamageLaser();
                StopBurst();
            }
            else
            {
                StartBurst();
            }
        }
        

        if (_minigunEnabled)
        {
            if (_minigunShotAmount < _minigunBurstAmount)
            {
                ShootMinigun();
            }
            else
            {
                foreach (Animator minigunAnimator in _minigunAnimators) minigunAnimator.SetFloat(GlobalStrings.MinigunSpeed, 0);
                _minigunBurstReloadTimer -= Time.deltaTime;
                if (_minigunBurstReloadTimer < 0)
                {
                    _minigunShotAmount = 0;
                    _minigunBurstReloadTimer = _minigunBurstReloadLength;
                }
            }
        }
    }
    private void RotateWeapons()
    {
        foreach (Transform minigun in _miniguns) minigun.transform.rotation = Quaternion.Euler(0, 0, CalculateRotationZDeg(minigun, Target));
        foreach (Transform lasergun in _laserguns) lasergun.transform.rotation = Quaternion.Euler(0, 0, CalculateRotationZDeg(lasergun, Target));
    }
    private void ShootMinigun()
    {
        for (int i = 0; i < _miniguns.Length; i++)
        {
            _minigunAnimators[i].SetFloat(GlobalStrings.MinigunSpeed, _minigunAnimationSpeed);

            if (_minigunNextFireTimer <= 0)
            {
                var shootingDirection = RandomizeShootingDirection(_miniguns[i], Target, _missDeg);
                RaycastHit2D hit = Physics2D.Raycast(_minigunShootingPoints[i].position, shootingDirection, _shootDistance, _railgunHitLayerMask);


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
            _minigunNextFireTimer = _minigunFirePeriod;
            _minigunShotAmount++;
        }
    }

    private void RenderLaser()
    {
        for (int i = 0; i < _laserguns.Length; i++)
        {
            _lineRenderers[i].enabled = true;
            _laserImpactRenderers[i].enabled = true;
            RaycastHit2D hit = Physics2D.BoxCast(_lasergunShootingPoints[i].position, _beamSize, 0, CalculateDirection(_lasergunShootingPoints[i], Target), Mathf.Infinity, _stopLaserLayerMask);
            if (hit)
            {
                _lineRenderers[i].SetPosition(1, new Vector3(Mathf.Abs((hit.point - (Vector2)_lasergunShootingPoints[i].transform.position).magnitude), 0, 0));
                float newRotationZ = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg;
                _laserImpactVFX.transform.rotation = Quaternion.Euler(0, 0, newRotationZ + 180);
                _laserImpactVFX.transform.localPosition = new Vector3(Mathf.Abs((hit.point - (Vector2)_lasergunShootingPoints[i].transform.position).magnitude), 0, 0);
            }
        }
    }

    private void StopRenderingLaser()
    {
        Debug.Log("Stop render");
        for (int i = 0; i < _laserguns.Length; i++)
        {
            _lineRenderers[i].enabled = false;
            _laserImpactRenderers[i].enabled = false;
        }   
    }

    private void DamageLaser()
    {
        for (int i = 0; i < _laserguns.Length; i++)
        {
            RaycastHit2D hit = Physics2D.BoxCast(_lasergunShootingPoints[i].position, _beamSize, 0, CalculateDirection(_lasergunShootingPoints[i], Target), Mathf.Infinity, _hitLaserLayerMask);
            if (hit && _laserNextDamageTimer <= 0)
            {
                if (hit.collider.gameObject.layer == 7)
                {
                    hit.collider.GetComponent<BaseEntity>().GetDamage(_laserDamage);
                }
                else if (hit.collider.gameObject.layer == 10)
                {
                    Player.Instance.gameObject.GetComponent<EnergyShield>().AbsorbDamage(_laserDamage);
                }
                _laserNextDamageTimer = _laserDamagePeriod;
            }
        }
    }
    private void StopBurst()
    {
        if (_laserBurstOnTimer <= 0)
        {
            _laserBurst = false;
            _laserBurstOffTimer = _laserBurstOffPeriod;
            StopRenderingLaser();
        }
    }
    private void StartBurst()
    {
        if (_laserBurstOffTimer <= 0)
        {
            _laserBurst = true;
            _laserBurstOnTimer = _laserBurstOnPeriod;
        }
    }

    private void ResetTimers()
    {
        _minigunNextFireTimer -= Time.deltaTime;

        _laserNextDamageTimer -= Time.deltaTime;
        if (_laserBurst) _laserBurstOnTimer -= Time.deltaTime;
        else _laserBurstOffTimer -= Time.deltaTime;
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
