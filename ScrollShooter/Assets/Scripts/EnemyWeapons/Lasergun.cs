using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Lasergun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] _laserguns;
    [SerializeField] private Animator[] _lasergunAnimators;
    [SerializeField] private Transform[] _lasergunShootingPoints;
    [SerializeField] private LineRenderer[] _lineRenderers;
    [SerializeField] private SpriteRenderer[] _laserImpactRenderers;
    [SerializeField] private GameObject _laserImpactVFX;

    [Header("Preferences")]
    [SerializeField] private Vector2 _beamSize;
    [SerializeField] private LayerMask _stopLaserLayerMask;
    [SerializeField] private LayerMask _hitLaserLayerMask;
    [SerializeField] private float _laserDamage;
    [SerializeField] private float _laserDamagePeriod;
    [SerializeField] private float _laserBurstOnPeriod;
    [SerializeField] private float _laserBurstOffPeriod;

    [Header("Internal Values")]
    [HideInInspector] private float _laserBurstOnTimer;
    [HideInInspector] private float _laserBurstOffTimer;
    [HideInInspector] private float _laserNextDamageTimer;
    [HideInInspector] private bool _laserBurst = false;
    [HideInInspector] private Transform Target;

    private void Awake()
    {
        _laserBurstOnTimer = _laserBurstOnPeriod;
        _laserBurstOffTimer = _laserBurstOffPeriod;
        foreach (SpriteRenderer laserImpactRenderer in _laserImpactRenderers) laserImpactRenderer.enabled = false;
        Target = Player.Instance.Target;
    }

    private void RotateWeapons()
    {
        foreach (Transform lasergun in _laserguns) lasergun.transform.rotation = Quaternion.Euler(0, 0, Calculator.CalculateRotationZDeg(lasergun, Target));
    }

    private void Update()
    {
        RotateWeapons();
        ResetTimers();

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

    private void RenderLaser()
    {
        for (int i = 0; i < _laserguns.Length; i++)
        {
            _lineRenderers[i].enabled = true;
            _laserImpactRenderers[i].enabled = true;
            RaycastHit2D hit = Physics2D.BoxCast(_lasergunShootingPoints[i].position, _beamSize, 0,  Calculator.CalculateDirection(_lasergunShootingPoints[i], Target), Mathf.Infinity, _stopLaserLayerMask);
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
            RaycastHit2D hit = Physics2D.BoxCast(_lasergunShootingPoints[i].position, _beamSize, 0, Calculator.CalculateDirection(_lasergunShootingPoints[i], Target), Mathf.Infinity, _hitLaserLayerMask);
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
            foreach(Animator lasergunAnimator in _lasergunAnimators) lasergunAnimator.SetBool(GlobalStrings.Burst, _laserBurst);
            _laserBurstOffTimer = _laserBurstOffPeriod;
            StopRenderingLaser();
        }
    }
    private void StartBurst()
    {
        if (_laserBurstOffTimer <= 0)
        {
            _laserBurst = true;
            foreach (Animator lasergunAnimator in _lasergunAnimators) lasergunAnimator.SetBool(GlobalStrings.Burst, _laserBurst);
            _laserBurstOnTimer = _laserBurstOnPeriod;
        }
    }

    private void ResetTimers()
    {
        _laserNextDamageTimer -= Time.deltaTime;
        if (_laserBurst) _laserBurstOnTimer -= Time.deltaTime;
        else _laserBurstOffTimer -= Time.deltaTime;
    }
}