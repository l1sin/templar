using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    public static bool IsPaused;

    private void Awake()
    {
        SetSingleton();
        IsPaused = false;
        Time.timeScale = 1;
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        if (IsPaused) PauseOn();
        else PauseOff();
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
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
}
