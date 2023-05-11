using UnityEngine;

public class Powerup : BaseCollectible
{
    [SerializeField] private float _powerupTime;

    protected override void Collect(Collider2D collision)
    {
        collision.GetComponent<Player>().ActivatePowerup(_powerupTime);
        base.Collect(collision);
    }
}
