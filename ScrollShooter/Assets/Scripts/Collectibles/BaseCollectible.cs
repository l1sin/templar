using UnityEngine;

public class BaseCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Collect(collision);
        }
    }

    protected virtual void Collect(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
