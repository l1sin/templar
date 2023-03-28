using UnityEngine;

public class SetCursor : MonoBehaviour
{
    [SerializeField] private Texture2D _crosshair;

    private void Awake()
    {
        Vector2 hotspot = new Vector2(_crosshair.width * 0.5f, _crosshair.height * 0.5f);
        Cursor.SetCursor(_crosshair, hotspot, CursorMode.Auto);
    }
}
