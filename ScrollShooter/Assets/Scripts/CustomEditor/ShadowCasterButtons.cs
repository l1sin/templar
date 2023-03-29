#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShadowCasterGenerator))]

public class ShadowCasterButtons : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ShadowCasterGenerator shadowCasterGenerator = (ShadowCasterGenerator)target;
        if (GUILayout.Button("Add/Refresh Shadow Caster"))
        {
            shadowCasterGenerator.AddShadowCaster();
        }

        if (GUILayout.Button("Remove Shadow Caster"))
        {
            shadowCasterGenerator.RemoveShadowCaster();
        }
    }
}
#endif
