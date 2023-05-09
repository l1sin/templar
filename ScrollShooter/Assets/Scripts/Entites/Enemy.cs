using UnityEngine;
using UnityEngine.Audio;

public class Enemy : BaseEntity
{
    [SerializeField] public GameObject Drop;
    [SerializeField] protected GameObject VFX;
    [SerializeField] protected SpriteRenderer[] SpriteRenderers;
    [SerializeField] protected AudioClip[] _deathSounds;
    [SerializeField] protected AudioMixerGroup _deathMixerGroup;
    protected Transform Target;

    protected override void Awake()
    {
        base.Awake();
        CurrentMaterial = SpriteRenderers[0].material;
        ApplyMaterial();
        Target = Player.Instance.Target;
    }
    public override void Die()
    {
        Instantiate(Drop, transform.position, Quaternion.identity);
        Instantiate(VFX, transform.position, Quaternion.identity);
        AudioManager.Instance.MakeSound(transform.position, _deathSounds, _deathMixerGroup);
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
