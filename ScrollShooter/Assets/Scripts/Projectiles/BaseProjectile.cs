using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [Header("Main preferences")]
    [SerializeField] public float Speed;
    [SerializeField] public float Damage;
    [SerializeField] protected float _lifeTime;

    [Header("Other preferences")]
    [SerializeField] private Vector3 _overlapBoxOffset;
    [SerializeField] private Vector3 _overlapBoxSize;
    [SerializeField] private LayerMask _layers;

    private void Update()
    {
        Move();
        DestroyOnTimer();
        CheckCollision();
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

    private void CheckCollision()
    {
        Collider2D collision = Physics2D.OverlapBox(transform.position + _overlapBoxOffset, _overlapBoxSize, 0, _layers);
        if (collision != null)
        {
            collision.GetComponent<BaseEntity>().GetDamage(Damage);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + _overlapBoxOffset, _overlapBoxSize);
    }
}
