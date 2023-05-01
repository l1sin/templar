using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private Transform _canvas;

    [Header("UI prefabs")]
    [SerializeField] public GameObject MainMenu;
    [SerializeField] public GameObject PauseMenu;
    [SerializeField] public GameObject PauseOptions;
    [SerializeField] public GameObject DarkBG;
    [SerializeField] public GameObject HUD;

    public List<GameObject> MenuQueue;

    public static UI Instance { get; private set; }

    private void Awake()
    {
        SetSingleton();
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
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        for (int i = 0; i < _canvas.transform.childCount; i++)
        {
            Destroy(_canvas.GetChild(i).gameObject);
        }
        MenuQueue.Clear();

        if (scene.buildIndex == 0)
        {
            GameObject newMenu = Instantiate(MainMenu);
            newMenu.transform.SetParent(_canvas, false);
        }
        else
        {
            GameObject hud = Instantiate(HUD);
            hud.transform.SetParent(_canvas, false);
        }
    }
}
