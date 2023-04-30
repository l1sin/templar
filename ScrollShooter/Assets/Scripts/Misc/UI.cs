using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private float _money;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Transform _canvas;
    private GameObject _menuInstance;
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

    public void InstantiatePauseMenu()
    {
        _menuInstance = Instantiate(_pauseMenu);
        _menuInstance.transform.SetParent(_canvas, false);
    }

    public void ClosePauseMenu()
    {
        Destroy(_menuInstance);
        _menuInstance = null;
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
