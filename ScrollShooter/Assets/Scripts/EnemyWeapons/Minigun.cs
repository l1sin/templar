using UnityEngine;

public class Minigun : MonoBehaviour
{
    [SerializeField] private Transform[] _miniguns;
    [SerializeField] private Animator[] _minigunAnimators;
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

    [SerializeField] private LayerMask _minigunHitLayerMask;

    [SerializeField] private float _reflectLength;
    [SerializeField] private float _shootDistance;

    private bool _shotPerformed;
    private Transform Target;

    private void Awake()
    {
        _minigunBurstReloadTimer = _minigunBurstReloadLength;
        _minigunNextFireTimer = 0;
        Target = Player.Instance.Target;
    }

    private  void Update()
    {
        RotateWeapons();
        ResetTimers();
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
    private void RotateWeapons()
    {
        foreach (Transform minigun in _miniguns) minigun.transform.rotation = Quaternion.Euler(0, 0, Calculator.CalculateRotationZDeg(minigun, Target));
    }

    private void ShootMinigun()
    {
        for (int i = 0; i < _miniguns.Length; i++)
        {
            _minigunAnimators[i].SetFloat(GlobalStrings.MinigunSpeed, _minigunAnimationSpeed);

            if (_minigunNextFireTimer <= 0)
            {
                var shootingDirection =  Calculator.RandomizeShootingDirection(_miniguns[i], Target, _missDeg);
                RaycastHit2D hit = Physics2D.Raycast(_minigunShootingPoints[i].position, shootingDirection, _shootDistance, _minigunHitLayerMask);


                if (hit)
                {
                    Tracer.Trace(_tracerPrefab, _minigunShootingPoints[i], hit.point - (Vector2)_minigunShootingPoints[i].position);
                    if (hit.collider.gameObject.layer == 7)
                    {
                        hit.collider.GetComponent<BaseEntity>().GetDamage(_minigunDamage);
                    }
                    else if (hit.collider.gameObject.layer == 10)
                    {
                        Player.Instance.gameObject.GetComponent<EnergyShield>().AbsorbDamage(_minigunDamage);
                        Tracer.TraceReflect(_tracerReflectedPrefab, _minigunShootingPoints[i], hit.point, hit.normal, _reflectLength);
                    }
                }
                else
                {
                    Tracer.Trace(_tracerPrefab, _minigunShootingPoints[i], shootingDirection * _shootDistance);
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

    private void ResetTimers()
    {
        _minigunNextFireTimer -= Time.deltaTime;
    }
}
