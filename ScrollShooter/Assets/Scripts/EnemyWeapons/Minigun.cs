using UnityEngine;
using UnityEngine.Audio;

public class Minigun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] _miniguns;
    [SerializeField] private Animator[] _minigunAnimators;
    [SerializeField] private Transform[] _minigunShootingPoints;
    [SerializeField] private GameObject _tracerPrefab;
    [SerializeField] private GameObject _tracerReflectedPrefab;
    
    [Header("Preferences")]
    [SerializeField] private LayerMask _minigunHitLayerMask;
    [SerializeField] private float _minigunDamage;
    [SerializeField] private float _minigunBurstAmount;
    [SerializeField] private float _minigunFirePeriod;
    [SerializeField] private float _minigunBurstReloadLength;
    [SerializeField] private float _shootDistance;
    [SerializeField] private float _minigunAnimationSpeed;
    [SerializeField] private float _missDeg;
    [SerializeField] private float _reflectLength;

    [Header("SFX")]
    [SerializeField] private AudioClip[] _shots;
    [SerializeField] private AudioClip[] _richochets;
    [SerializeField] private AudioMixerGroup _shotMixerGroup;

    [Header("Internal Values")]
    [HideInInspector] private float _minigunShotAmount;
    [HideInInspector] private float _minigunBurstReloadTimer;
    [HideInInspector] private float _minigunNextFireTimer;
    [HideInInspector] private bool _shotPerformed;
    [HideInInspector] private Transform Target;


    private void Awake()
    {
        ResetThis();
        Target = Player.Instance.Target;
    }

    private void Update()
    {
        if (PauseManager.IsPaused) return;
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
    public void ResetThis()
    {
        foreach (Animator minigunAnimator in _minigunAnimators) minigunAnimator.SetFloat(GlobalStrings.MinigunSpeed, 0);
        foreach (Transform minigun in _miniguns) minigun.transform.localRotation = Quaternion.identity;
        _minigunShotAmount = _minigunBurstAmount;
        _minigunBurstReloadTimer = _minigunBurstReloadLength;
        _minigunNextFireTimer = 0;
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
                AudioManager.Instance.MakeSound(transform.position, _shots, _shotMixerGroup);

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
                        AudioManager.Instance.MakeSound(transform.position, _richochets, _shotMixerGroup);
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
