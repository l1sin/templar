using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Camera _camera;

    [SerializeField] private List<TileData> _tileDatas;

    [SerializeField] private Dictionary<TileBase, TileData> _dataFromTiles;

    private void Awake()
    {
        _dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (TileData tileData in _tileDatas)
        {
            foreach (TileBase tile in tileData.Tiles)
            {
                _dataFromTiles.Add(tile, tileData);
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePos = _tilemap.WorldToCell(mousePos);
            if (_tilemap.GetTile(tilePos) != null)
            {
                TileBase tile = _tilemap.GetTile(tilePos);
                bool tileInfo = _dataFromTiles[tile].IsBro;
                Debug.Log(tile.name + " is at position " + tilePos + " and bool IsBro = " + tileInfo);
            }          
        }
    }
}
