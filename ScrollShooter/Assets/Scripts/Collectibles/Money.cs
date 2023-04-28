using UnityEngine;

public class Money : BaseCollectible
{
    [SerializeField] private float _moneyValue;
    protected override void Collect(Collider2D collision)
    {
        UI.Instance.AddMoney(_moneyValue);
        base.Collect(null);
    }
}
