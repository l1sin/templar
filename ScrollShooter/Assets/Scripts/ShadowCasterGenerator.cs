using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class ShadowCasterGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;  
    [SerializeField] private GameObject _shadowCasterParentObject;
    [SerializeField] private bool _selfShadows = true;

    [SerializeField] private List<GameObject> _shadowCasterList;
    [SerializeField] private List<GameObject> _figures;
    [SerializeField] public static List<Vector3Int> TilePos = new List<Vector3Int>();

    public void AddShadowCaster()
    {
        RemoveShadowCaster();
        TilePos = GetAllTilesPos(_tilemap);
        TilePos = RemoveNullTiles(TilePos);
        SeparateShadowCastersIntoGroups(TilePos);
    }

    public void RemoveShadowCaster()
    {
        foreach (GameObject gameObject in _shadowCasterList)
        {
            DestroyImmediate(gameObject);
        }
        foreach (GameObject gameObject in _figures)
        {
            DestroyImmediate(gameObject);
        }
        _shadowCasterList.Clear();
        _figures.Clear();
        TilePos.Clear();
    }

    public void SeparateShadowCastersIntoGroups(List<Vector3Int> tilesToCheck)
    {
        if (tilesToCheck.Count != 0)
        {
            List<Vector3Int> checkedTiles = new List<Vector3Int>(tilesToCheck);
            GameObject figureGameObject = InstantiateFigure();
            List<Vector3Int> figure = new List<Vector3Int>()
        {
            checkedTiles[0]
        };

            CheckAllNeighbours(checkedTiles[0], checkedTiles, figure);
            for (int i = checkedTiles.Count - 1; i >= 0; i--)
            {
                if (figure.Contains(checkedTiles[i]))
                {
                    checkedTiles.Remove(checkedTiles[i]);
                }
            }
            foreach (Vector3Int tile in figure)
            {
                InstantiateShadowCaster(tile, figureGameObject.transform);
            }
            SeparateShadowCastersIntoGroups(checkedTiles);
        } 
    }

    private void CheckIfItIsNeighbour(List<Vector3Int> checkedTiles, Vector3Int checkedTile, List<Vector3Int> figure)
    {
        if (checkedTiles.Contains(checkedTile))
        {
            if (!figure.Contains(checkedTile))
            {
                figure.Add(checkedTile);
                CheckAllNeighbours(checkedTile, checkedTiles, figure);
            }
        }
    }

    private void CheckAllNeighbours(Vector3Int targetTile, List<Vector3Int> checkedTiles, List<Vector3Int> figure)
    {
        Vector3Int checkedTile = targetTile + new Vector3Int(1, 0, 0);
        CheckIfItIsNeighbour(checkedTiles, checkedTile, figure);

        checkedTile = targetTile + new Vector3Int(0, 1, 0);
        CheckIfItIsNeighbour(checkedTiles, checkedTile, figure);

        checkedTile = targetTile + new Vector3Int(-1, 0, 0);
        CheckIfItIsNeighbour(checkedTiles, checkedTile, figure);

        checkedTile = targetTile + new Vector3Int(0, -1, 0);
        CheckIfItIsNeighbour(checkedTiles, checkedTile, figure);
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

    private void InstantiateShadowCaster(Vector3Int tile, Transform parent)
    {
        GameObject shadowCaster = new GameObject("ShadowCaster");
        shadowCaster.AddComponent<ShadowCaster2D>().selfShadows = _selfShadows;
        shadowCaster.transform.position = _tilemap.CellToWorld(tile) + new Vector3(0.5f, 0.5f, 0);
        shadowCaster.transform.SetParent(parent);
        _shadowCasterList.Add(shadowCaster);
    }

    private GameObject InstantiateFigure()
    {
        GameObject figure = new GameObject("figure");
        figure.AddComponent<CompositeShadowCaster2D>();
        figure.transform.SetParent(_shadowCasterParentObject.transform);
        _figures.Add(figure);
        return figure;
    }
}
