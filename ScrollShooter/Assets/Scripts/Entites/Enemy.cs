using UnityEngine;

public class Enemy : BaseEntity
{
    [SerializeField] protected GameObject Drop;
    [SerializeField] protected GameObject VFX;
    [SerializeField] protected SpriteRenderer[] SpriteRenderers;
    protected Transform Target;

    protected override void Awake()
    {
        base.Awake();
        CurrentMaterial = SpriteRenderers[0].material;
        ApplyMaterial();
        Target = Player.Instance.Target;
    }
    protected override void Die()
    {
        Instantiate(Drop, transform.position, Quaternion.identity);
        Instantiate(VFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected override void ApplyMaterial()
    {
        foreach (Renderer renderer in SpriteRenderers)
        {
            renderer.material = CurrentMaterial;
        }
    }
}
