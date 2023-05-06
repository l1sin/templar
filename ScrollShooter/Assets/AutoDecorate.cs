using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class AutoDecorate : MonoBehaviour
{
    [SerializeField] private Tilemap _targetTilemap;
    [SerializeField] private Tilemap _thisTilemap;
    [SerializeField] public static List<Vector3Int> TilePos = new List<Vector3Int>();
    [SerializeField] private TileBase[] _tiles;
    [SerializeField] private float _tileChance;
    [SerializeField] private Sprite _groundTile;

    private List<Vector3Int> GetAllTilesPos(Tilemap tilemap)
    {
        List<Vector3Int> newList = new List<Vector3Int>();

        tilemap.CompressBounds();


        for (int i = tilemap.origin.z; i <= tilemap.origin.z + tilemap.cellBounds.size.z - 1; i++)
        {
            for (int j = tilemap.origin.y; j <= tilemap.origin.y + tilemap.cellBounds.size.y - 1; j++)
            {
                for (int k = tilemap.origin.x; k <= tilemap.origin.x + tilemap.cellBounds.size.x - 1; k++)
                {
                    newList.Add(new Vector3Int(k, j, i));
                }
            }
        }
        return newList;
    }

    private List<Vector3Int> RemoveNullTiles(List<Vector3Int> list)
    {

        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (_targetTilemap.GetTile(list[i]) == null)
            {
                list.Remove(list[i]);
            }
        }
        return list;
    }

    public void PlaceRandomTiles()
    {
        var allTiles = GetAllTilesPos(_targetTilemap);
        TilePos = RemoveNullTiles(allTiles);
        foreach (Vector3Int tile in TilePos)
        {
            if (Random.Range(0f, 1f) < _tileChance && _targetTilemap.GetSprite(tile) == _groundTile)
            {
                _thisTilemap.SetTile(tile, _tiles[Random.Range(0, _tiles.Length)]);
            }
        }
    }

    public void DestroyTiles()
    {
        _thisTilemap.ClearAllTiles();
    }
}
