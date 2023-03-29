using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShadowCasterGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;  
    [SerializeField] private GameObject _shadowCasterPrefab;
    [SerializeField] private GameObject _shadowCasterParentObject;

    [HideInInspector] private List<GameObject> _shadowCasterList;
    [HideInInspector] private List<Vector3Int> _tilePos = new List<Vector3Int>();

    public void AddShadowCaster()
    {
        RemoveShadowCaster();
        _tilePos = GetAllTilesPos(_tilemap);
        _tilePos = RemoveNullTiles(_tilePos);

        foreach (Vector3Int tile in _tilePos)
        {
            var newShadowCaster = Instantiate(_shadowCasterPrefab, _tilemap.CellToWorld(tile) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            newShadowCaster.transform.SetParent(_shadowCasterParentObject.transform);
            _shadowCasterList.Add(newShadowCaster);
        }
    }

    public void RemoveShadowCaster()
    {
        foreach (GameObject shadowCaster in _shadowCasterList)
        {
            DestroyImmediate(shadowCaster);
        }
        _shadowCasterList.Clear();
    }

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
        foreach (Vector3Int tile in list)
        {
            if (_tilemap.GetTile(tile) == null)
            {
                list.Remove(tile);
                RemoveNullTiles(list);
                return list;
            }
        }
        return list;
    }
}
