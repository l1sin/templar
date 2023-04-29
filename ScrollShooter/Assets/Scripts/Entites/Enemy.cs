using UnityEngine;

public class Enemy : BaseEntity
{
    [SerializeField] private GameObject _drop;
    [SerializeField] private GameObject _vFX;
    [SerializeField] private Renderer[] _renderers;

    protected override void Awake()
    {
        base.Awake();
        _currentMaterial = _renderers[0].material;
        ApplyMaterial();
    }
    protected override void Die()
    {
        Instantiate(_drop, transform.position, Quaternion.identity);
        Instantiate(_vFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected override void ApplyMaterial()
    {
        foreach (Renderer renderer in _renderers)
        {
            renderer.material = _currentMaterial;
        }
    }
}
