using UnityEngine;

public class Battery : BaseCollectible
{
    [SerializeField] private float _energyAmount;

    protected override void Collect(Collider2D collision)
    {
        collision.gameObject.GetComponent<Player>().ApplyBattery(_energyAmount);
        base.Collect(null);
    }
}
