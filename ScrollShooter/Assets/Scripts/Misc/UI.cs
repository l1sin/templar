using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private float _money;
    [SerializeField] private Transform _canvas;
    
    [Header("UI prefabs")]
    [SerializeField] public GameObject PauseMenu;
    [SerializeField] public GameObject PauseOptions;
    [SerializeField] public GameObject DarkBG;

    public List<GameObject> MenuQueue;

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

    public void InstantiateMenu(GameObject menu)
    {
        GameObject newMenu = Instantiate(menu);

        if (MenuQueue.Count == 0)
        {
            newMenu.transform.SetParent(_canvas, false);
            GameObject newBackground =  Instantiate(DarkBG);
            newBackground.transform.SetParent(newMenu.transform, false);
            newBackground.transform.SetSiblingIndex(0);
        }
        
        else newMenu.transform.SetParent(MenuQueue[MenuQueue.Count - 1].transform, false);
        MenuQueue.Add(newMenu);
    }

    public void CloseLastMenu()
    {
        Destroy(MenuQueue[MenuQueue.Count - 1]);
        MenuQueue.RemoveAt(MenuQueue.Count - 1);
        if (MenuQueue.Count == 0) PauseManager.Instance.TogglePause();
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
