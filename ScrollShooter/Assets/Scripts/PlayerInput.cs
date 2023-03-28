using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static bool Jump;
    public static float Movement;

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Jump = false;
    }

    private void GetInput()
    {
        if (Input.GetButtonDown(GlobalStrings.JumpInput))
        {
            Jump = true;
        }
        Movement = Input.GetAxisRaw(GlobalStrings.MoveInput);
    }
}
