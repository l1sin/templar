using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObject/TileData")]
public class TileData : ScriptableObject
{
    [SerializeField] public TileBase[] Tiles;
    [SerializeField] public bool IsBro;
    
}
