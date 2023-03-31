using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] public float Speed;
    [SerializeField] public float Damage;
    [SerializeField] protected float _lifeTime;

    private void Update()
    {
        transform.Translate(Vector2.right * Speed * Time.deltaTime, Space.Self);
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
