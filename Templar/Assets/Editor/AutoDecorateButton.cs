#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AutoDecorate))]

public class AutoDecorateButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AutoDecorate autoDecorate = (AutoDecorate)target;
        if (GUILayout.Button("Generate"))
        {
            autoDecorate.PlaceRandomTiles();
        }
        if (GUILayout.Button("Generate Specific"))
        {
            autoDecorate.PlaceRandomTilesSpecific();
        }
        if (GUILayout.Button("Generate On Top"))
        {
            autoDecorate.PlaceRandomTilesOnTop();
        }
        if (GUILayout.Button("Destroy"))
        {
            autoDecorate.DestroyTiles();
        }
    }
}
#endif
