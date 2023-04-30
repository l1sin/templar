using System.Collections.Generic;
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
        UI.Instance.InstantiatePauseMenu();
        Time.timeScale = 0;
    }
    private void PauseOff()
    {
        UI.Instance.ClosePauseMenu();
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
