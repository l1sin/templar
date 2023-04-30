using UnityEngine;

public class LaserTrap : Enemy
{
    [SerializeField] private LayerMask _layerGroundMask;
    [SerializeField] private LayerMask _layerHitMask;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Vector2 _beamSize;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _damage;
    [SerializeField] private float _damagePeriod;
    [SerializeField] private float _burstOnPeriod;
    [SerializeField] private float _burstOffPeriod;

     private float _burstOnTimer;
     private float _burstOffTimer;
     private float _nextDamageTimer;

     private bool _burst = false;

    protected override void Awake()
    {
        base.Awake();
        _burstOnTimer = _burstOnPeriod;
        _burstOffTimer = _burstOffPeriod;
        _animator.SetFloat(GlobalStrings.Recharge, 1f / _burstOffPeriod);
        _animator.SetFloat(GlobalStrings.Discharge, 1f / _burstOnPeriod);
    }

    protected override void Update()
    {
        base.Update();
        ResetTimers();
        if (_burst)
        {
            RenderLaser();
            DamageTargets();
            StopBurst();
        }
        else
        {
            StartBurst();
        }
    }

    private void RenderLaser()
    {
        _lineRenderer.enabled = true;
        var hit = Physics2D.BoxCast(_shootingPoint.position, _beamSize, 0, (Vector2)transform.TransformDirection(Vector2.right), Mathf.Infinity, _layerGroundMask);
        if (hit)
        {
            _lineRenderer.SetPosition(1, new Vector3(Mathf.Abs((hit.point - (Vector2)_shootingPoint.transform.position).magnitude), 0, 0));
        }
    }

    private void StopRenderingLaser()
    {
        _lineRenderer.enabled = false;
    }

    private void DamageTargets()
    {
        var hit = Physics2D.BoxCast(_shootingPoint.position, _beamSize, 0, (Vector2)transform.TransformDirection(Vector2.right), Mathf.Infinity, _layerHitMask);
        if (hit && _nextDamageTimer <= 0)
        {
            if (hit.collider.gameObject.layer == 7)
            {
                hit.collider.GetComponent<BaseEntity>().GetDamage(_damage);
            }
            else if (hit.collider.gameObject.layer == 10)
            {

            }
            _nextDamageTimer = _damagePeriod;
        }
    }

    private void ResetTimers()
    {
        _nextDamageTimer -= Time.deltaTime;
        if (_burst) _burstOnTimer -= Time.deltaTime;
        else _burstOffTimer -= Time.deltaTime;
    }

    private void StopBurst()
    {
        if (_burstOnTimer <= 0)
        {
            _burst = false;
            _animator.SetBool(GlobalStrings.Burst, _burst);
            _burstOffTimer = _burstOffPeriod;
            StopRenderingLaser();
        }
    }
    private void StartBurst()
    {
        if (_burstOffTimer <= 0)
        {
            _burst = true;
            _animator.SetBool(GlobalStrings.Burst, _burst);
            _burstOnTimer = _burstOnPeriod;
        }
    }
}
