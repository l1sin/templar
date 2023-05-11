using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private Image _healthLine;
    [SerializeField] private Image _energyLine;
    [SerializeField] public Color CanUseEnergyColor;
    [SerializeField] public Color NoEnergyColor;

    public static HUD Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeMoney(float money)
    {
        _moneyText.text = money.ToString();
    }

    public void ChangeHealth(float current, float max)
    {
        _healthLine.fillAmount = current / max;
    }
    public void ChangeEnergy(float current, float max)
    {
        _energyLine.fillAmount = current / max;
    }

    public void ChangeEnergyBarColor(Color color)
    {
        _energyLine.color = color;
    }
}
