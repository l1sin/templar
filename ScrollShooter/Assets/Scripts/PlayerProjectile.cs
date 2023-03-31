using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;

    private void Update()
    {
        transform.Translate(Vector2.right * _speed * Time.deltaTime, Space.Self);
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

}
