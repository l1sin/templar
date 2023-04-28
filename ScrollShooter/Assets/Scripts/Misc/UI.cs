using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private float _money;
    public static UI Instance { get; private set; }

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddMoney(float money)
    {
        _money += money;
        _moneyText.text = _money.ToString();
    }
}
