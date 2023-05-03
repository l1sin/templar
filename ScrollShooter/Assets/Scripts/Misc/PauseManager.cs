using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    public static bool IsPaused;
    private bool IsMainMenu;

    private void Awake()
    {
        SetSingleton();
        IsPaused = false;
        Time.timeScale = 1;
    }

    public void TogglePause()
    {
        if (!IsMainMenu && !Player.Instance.GameOver && !Player.Instance.LevelComplete)
        {
            IsPaused = !IsPaused;
            if (IsPaused) PauseOn();
            else PauseOff();
        }
    }

    private void PauseOn()
    {
        Time.timeScale = 0;
        UI.Instance.InstantiateMenu(UI.Instance.PauseMenu);
    }
    private void PauseOff()
    {
        Time.timeScale = 1;
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
        if (scene.buildIndex == 0)
        {
            IsMainMenu = true;
        }
        else
        {
            IsMainMenu = false;
        }
    }
}
