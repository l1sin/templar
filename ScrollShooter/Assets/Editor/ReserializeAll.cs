using UnityEditor;
using UnityEngine;

public class ReserializeAll : MonoBehaviour
{
    [MenuItem("MyMenu/Reserialize")]
    public static void Reserialize()
    {
        AssetDatabase.ForceReserializeAssets();
    }
}
