using UnityEngine;

public class Heal : BaseCollectible
{
    [SerializeField] private float _healAmount;

    protected override void Collect(Collider2D collision)
    {
        collision.gameObject.GetComponent<Player>().GetHeal(_healAmount);
        base.Collect(null);
    }
}
