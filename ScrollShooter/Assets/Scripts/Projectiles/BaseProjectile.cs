using UnityEngine;
using UnityEngine.Serialization;

public class BaseProjectile : MonoBehaviour
{
    [Header("Main preferences")]
    [SerializeField] public float Speed;
    [SerializeField] public float Damage;
    [SerializeField] public float StoppingAction;
    [SerializeField] protected float LifeTime;
    [SerializeField] protected GameObject HitEffect;
    [SerializeField] protected Effect[] Effects;
    [SerializeField]protected LayerMask HitMask;
    protected Vector2 Direction;

    private void Awake()
    {
        Direction = Calculator.DegToVector2(transform.localEulerAngles.z);
    }

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        MakeSweepTest2D();
        Move();
        DestroyOnTimer();
    }

    private void Move()
    {
        transform.Translate(Vector2.right * Speed * Time.deltaTime, Space.Self);
    }

    private void DestroyOnTimer()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void MakeSweepTest2D()
    {
        var hit = Physics2D.Raycast(transform.position, Direction, Speed * Time.deltaTime, HitMask);
        if (hit)
        {
            transform.position = hit.point;
            MakeCollision(hit.collider);
        }
    }

    protected virtual void MakeCollision(Collider2D collision)
    {
        if ((HitMask.value & (1 << collision.transform.gameObject.layer)) > 0)
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

            foreach (Effect effect in Effects)
            {
                effect.SeparateParent();
            }

            Destroy(gameObject);
        }
    }
}
