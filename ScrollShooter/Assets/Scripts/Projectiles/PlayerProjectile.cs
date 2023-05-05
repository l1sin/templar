using UnityEngine;

public class PlayerProjectile : BaseProjectile
{
    [SerializeField] private bool _isRailgunProjectile;
    [SerializeField] private Vector3 _normal;
    [SerializeField] private TrailRenderer _trail;

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
            if (collision.GetComponentInParent<ForceField>() != null)
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

                DestroyProjectile();
            }
        }
    }
    private void Reflect()
    {
        Direction = Vector2.Reflect(Direction, _normal);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Calculator.Vector2ToDeg(Direction));
        Debug.Log("Reflect");
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
