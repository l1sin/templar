using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [Header("UI prefabs")]
    [SerializeField] public GameObject MainMenu;
    [SerializeField] public GameObject PauseMenu;
    [SerializeField] public GameObject PauseOptions;
    [SerializeField] public GameObject DarkBG;
    [SerializeField] public GameObject HUD;
    [SerializeField] public GameObject GameOverMenu;
    [SerializeField] public GameObject LevelCompleteMenu;
    [SerializeField] public GameObject BossHPBar;

    [Header("Cursors")]
    [SerializeField] private Texture2D _systemCursor;
    [SerializeField] private Texture2D _crosshair;

    [Header("Menu Queue")]
    [SerializeField] public List<GameObject> MenuQueue;

    public static UI Instance { get; private set; }

    private void Awake()
    {
        SetSingleton();
    }

    public GameObject InstantiateMenu(GameObject menu)
    {
        GameObject newMenu = Instantiate(menu);

        if (MenuQueue.Count == 0)
        {
            newMenu.transform.SetParent(transform, false);
            GameObject newBackground =  Instantiate(DarkBG);
            newBackground.transform.SetParent(newMenu.transform, false);
            newBackground.transform.SetSiblingIndex(0);
        }    
        else newMenu.transform.SetParent(MenuQueue[MenuQueue.Count - 1].transform, false);
        MenuQueue.Add(newMenu);
        return newMenu;
    } 

    public GameObject InstantiateMenuNoQueue(GameObject menu)
    {
        GameObject newMenu = Instantiate(menu);
        newMenu.transform.SetParent(transform, false);
        return newMenu;
    }

    public void CloseLastMenu()
    {
        Destroy(MenuQueue[MenuQueue.Count - 1]);
        MenuQueue.RemoveAt(MenuQueue.Count - 1);
        if (MenuQueue.Count == 0)
        {
            PauseManager.Instance.TogglePause();
            if (SceneManager.GetActiveScene().buildIndex != 0) SetCrosshair();
        } 
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

    public void SetSystemCursor()
    {
        Vector2 hotspot = new Vector2(0, 0);
        Cursor.SetCursor(_systemCursor, hotspot, CursorMode.Auto);
    }

    public void SetCrosshair()
    {
        Vector2 hotspot = new Vector2(_crosshair.width * 0.5f, _crosshair.height * 0.5f);
        Cursor.SetCursor(_crosshair, hotspot, CursorMode.Auto);
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
        for (int i = 0; i < transform.transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        MenuQueue.Clear();

        if (scene.buildIndex == 0)
        {
            GameObject newMenu = Instantiate(MainMenu);
            newMenu.transform.SetParent(transform, false);
            SetSystemCursor();
        }
        else
        {
            GameObject hud = Instantiate(HUD);
            hud.transform.SetParent(transform, false);
            SetCrosshair();
        }
    }
}
