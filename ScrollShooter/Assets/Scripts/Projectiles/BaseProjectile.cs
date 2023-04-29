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
            Instantiate(_hitEffect, transform.position, transform.rotation);
            if (collision.gameObject.layer != 6)
            {
                collision.GetComponent<BaseEntity>().GetDamage(Damage);
                Vector2 move;
                move.x = Mathf.Cos(transform.localRotation.eulerAngles.z * Mathf.Deg2Rad);
                move.y = Mathf.Sin(transform.localRotation.eulerAngles.z * Mathf.Deg2Rad);
                Debug.Log(transform.localRotation.eulerAngles.z);
                Debug.Log(move);
                collision.GetComponent<Rigidbody2D>().AddForce(move * StoppingAction, ForceMode2D.Impulse);
            }
            else 
            {

            }
            foreach (Effect effect in _effects)
            {
                effect.SeparateParent();
            }
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + _overlapBoxOffset, _overlapBoxSize);
    }
}
