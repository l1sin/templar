using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [Header("Main preferences")]
    [SerializeField] public float Speed;
    [SerializeField] public float Damage;
    [SerializeField] public float StoppingAction;
    [SerializeField] protected float _lifeTime;
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private Effect[] _effects;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    private Vector2 _direction;

    private void Awake()
    {
        _direction = CalculateDirectionVector();
    }

    private void Update()
    {
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
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void MakeSweepTest2D()
    {
        var hit = Physics2D.Raycast(transform.position, _direction, Speed * Time.deltaTime, _layerMask);
        if (hit)
        {
            transform.position = hit.point;
            MakeCollision(hit.collider);
        }
    }

    private Vector2 CalculateDirectionVector()
    {
        Vector2 direction;
        direction.x = Mathf.Cos(transform.localRotation.eulerAngles.z * Mathf.Deg2Rad);
        direction.y = Mathf.Sin(transform.localRotation.eulerAngles.z * Mathf.Deg2Rad);
        return direction;
    }

    private void MakeCollision(Collider2D collision)
    {
        if ((_layerMask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            Instantiate(_hitEffect, transform.position, transform.rotation);

            if (collision.TryGetComponent<BaseEntity>(out BaseEntity baseEntity))
            {
                baseEntity.GetDamage(Damage);
            }

            if (collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
            {
                rigidbody2D.AddForce(_direction * StoppingAction, ForceMode2D.Impulse);
            }

            foreach (Effect effect in _effects)
            {
                effect.SeparateParent();
            }

            Destroy(gameObject);
        }
    }
}
