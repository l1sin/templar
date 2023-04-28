using UnityEngine;

public class Enemy : BaseEntity
{
    protected override void Die()
    {
        Destroy(gameObject);
    }
}
