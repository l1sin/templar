using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private float _money;
    public static UI Instance { get; private set; }

    private void Start()
    {
        SetSingleton();
    }

    public void AddMoney(float money)
    {
        _money += money;
        _moneyText.text = _money.ToString();
    }

    public void SetSingleton()
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
}
