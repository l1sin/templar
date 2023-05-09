using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour
{
    [SerializeField] public float Money;
    [SerializeField] public float TimeInSeconds;
    [SerializeField] public bool _mainMenu;

    public static Stats Instance { get; private set; }

    private void Awake()
    {
        SetSingleton();
    }

    private void Update()
    {
        if (PauseManager.IsPaused || _mainMenu) return;
        if (!Player.Instance.LevelComplete || !Player.Instance.GameOver)
        {
            TimeInSeconds += Time.deltaTime;
        }  
    }

    public void AddMoney(float money)
    {
        Money += money;
        HUD.Instance.ChangeMoney(Money);
    }

    private void SetSingleton()
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
        if (scene.buildIndex == 0) _mainMenu = true;
        else _mainMenu = false;
        Money = 0;
        TimeInSeconds = 0;
    }
}
