using UnityEngine;

public class LaserTrap : Enemy
{
    [SerializeField] private LayerMask _layerGroundMask;
    [SerializeField] private LayerMask _layerHitMask;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Vector2 _beamSize;
    [SerializeField] private float _damage;
    [SerializeField] private float _damagePeriod;
    [SerializeField] private float _damageTimer;
    private float _nextFire;

    protected override void Update()
    {
        base.Update();
        RenderLaser();
        DamageTargets();
    }

    private void RenderLaser()
    {
        var hit = Physics2D.BoxCast(_shootingPoint.position, _beamSize, 0, (Vector2)transform.TransformDirection(Vector2.right), Mathf.Infinity, _layerGroundMask);
        if (hit)
        {
            _lineRenderer.SetPosition(1, new Vector3(Mathf.Abs((hit.point - (Vector2)_shootingPoint.transform.position).magnitude), 0, 0));
        }
    }

    private void DamageTargets()
    {
        var hit = Physics2D.BoxCast(_shootingPoint.position, _beamSize, 0, (Vector2)transform.TransformDirection(Vector2.right), Mathf.Infinity, _layerHitMask);
        if (hit)
        {
            hit.collider.GetComponent<Player>().GetDamage(_damage);
        }
    }

}
