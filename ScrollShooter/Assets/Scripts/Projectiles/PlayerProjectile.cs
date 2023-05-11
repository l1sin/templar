using UnityEngine;
using UnityEngine.Audio;

public class PlayerProjectile : BaseProjectile
{
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private bool _isRailgunProjectile;
    [SerializeField] private Vector3 _normal;
    [SerializeField] private AudioClip[] _reflectSounds;
    [SerializeField] private AudioMixerGroup _reflectMixerGroup;
    [SerializeField] private AudioClip[] _pistolImpacts;
    [SerializeField] private AudioClip[] _railgunImpacts;
    [SerializeField] private AudioMixerGroup _impactMixerGroup;

    protected override void MakeSweepTest2D()
    {
        var hit = Physics2D.Raycast(transform.position, Direction, Speed * Time.deltaTime, HitMask);
        if (hit)
        {
            transform.position = hit.point;
            _normal = hit.normal;
            MakeCollision(hit.collider);
            _trail.AddPosition(hit.point);
        }
    }
    protected override void MakeCollision(Collider2D collision)
    {
        

        if ((HitMask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            if (collision.gameObject.layer == 11 && collision.GetComponentInParent<ForceField>() != null)
            {
                if (_isRailgunProjectile)
                {
                    collision.GetComponentInParent<ForceField>().GetDamage(Damage);
                    Instantiate(HitEffect, transform.position, transform.rotation);
                    DestroyProjectile();
                } 
                else Reflect();
            }
            else
            {
                Instantiate(HitEffect, transform.position, transform.rotation);

                if (collision.TryGetComponent<BaseEntity>(out BaseEntity baseEntity))
                {
                    baseEntity.GetDamage(Damage);          
                }
                if (collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
                {
                    rigidbody2D.AddForce(Direction * StoppingAction, ForceMode2D.Impulse);
                }
                if (_isRailgunProjectile) AudioManager.Instance.MakeSound(transform, _railgunImpacts, _impactMixerGroup);
                else AudioManager.Instance.MakeSound(transform, _pistolImpacts, _impactMixerGroup);
                DestroyProjectile();
            }
        }
    }
    private void Reflect()
    {
        AudioManager.Instance.MakeSound(transform, _reflectSounds, _reflectMixerGroup);
        Direction = Vector2.Reflect(Direction, _normal);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Calculator.Vector2ToDeg(Direction));
    }

    private void DestroyProjectile()
    {
        foreach (Effect effect in Effects)
        {
            effect.SeparateParent();
        }
        Destroy(gameObject);
    }
}
