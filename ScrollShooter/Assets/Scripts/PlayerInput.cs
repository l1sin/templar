using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    public static bool Jump;
    public static float Movement;
    public static bool LShift;
    public static bool Mouse0;
    public static bool Mouse1;
    public static bool Mouse2;

    private void Update()
    {
        GetPauseInput();
        if (PauseManager.IsPaused) return;
        GetPlayerMovementInput();
        GetMouseInput();
    }

    private void FixedUpdate()
    {
        Jump = false;
    }

    // Set execution order later than other scripts.
    private void GetPlayerMovementInput()
    {
        if (Input.GetButtonDown(GlobalStrings.JumpInput))
        {
            Jump = true;
        }
        Movement = Input.GetAxisRaw(GlobalStrings.MoveInput);
        LShift = Input.GetKey(KeyCode.LeftShift);
    }
    private void GetPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UI.Instance.MenuQueue.Count == 0)
            {
                PauseManager.Instance.TogglePause();
                if (SceneManager.GetActiveScene().buildIndex != 0 && !Player.Instance.GameOver && !Player.Instance.LevelComplete)
                {
                    UI.Instance.InstantiateMenu(UI.Instance.PauseMenu);
                    UI.Instance.SetSystemCursor();
                } 
            }
            else
            {
                UI.Instance.CloseLastMenu();
            }
        }
    }

    private void GetMouseInput()
    {
        Mouse0 = Input.GetMouseButton(0);
        Mouse1 = Input.GetMouseButton(1);
        Mouse2 = Input.GetMouseButton(2);
    }
}
