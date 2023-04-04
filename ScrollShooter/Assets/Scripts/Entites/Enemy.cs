using UnityEngine;

public class Enemy : BaseEntity
{
    public override void Die()
    {
        Destroy(gameObject);
    }
}
