using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsMethods : MonoBehaviour
{
    public static void TogglePause()
    {
        PauseManager.Instance.TogglePause();
    }

    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public static void Quit()
    {
        Application.Quit();
    }

    public static void OpenMenu(GameObject menu)
    {
        UI.Instance.InstantiateMenu(menu);
    }

    public static void CloseLastMenu()
    {
        UI.Instance.CloseLastMenu();
    }

    public static void DestroyMenu(GameObject menu)
    {
        Destroy(menu);
    }
}
