#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ParticlesFollow))]

public class ParticlesFollowButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ParticlesFollow particlesFollow = (ParticlesFollow)target;
        if (GUILayout.Button("Generate ParticleSystems"))
        {
            particlesFollow.CreateParticleSystems();
        }

        if (GUILayout.Button("Clear"))
        {
            particlesFollow.Clear();
        }
    }
}
#endif
