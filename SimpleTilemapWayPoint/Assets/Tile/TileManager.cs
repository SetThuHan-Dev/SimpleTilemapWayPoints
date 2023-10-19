using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class TileManager : MonoBehaviour
{
    
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Transform SpawnParent;
    [SerializeField] private GameObject wayPointPrefab;
    [SerializeField] private List<Transform> WayPoints;

    #region Context Menu
#if UNITY_EDITOR
    [ContextMenu("CreateWayPoints")]
    public void GetWayPoints()
    {
        CountCells(true);
    }

    [ContextMenu("DeleteWayPoints")]
    public void RemoveWayPoints()
    {
        WayPoints.Clear();

        GameObject[] clones = GameObject.FindGameObjectsWithTag("WayPoint");

        foreach (GameObject clone in clones)
        {
            if (clone.name.Contains("(Clone)"))
            {
                DestroyImmediate(clone);
            }
        }
    }
#endif
    #endregion

    private void Start()
    {
        if (WayPoints == null)
        {
            CountCells(true);
        }
    }
    private int CountCells(bool countLeftToRight)
    {
        int count = -1;

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] tiles = tilemap.GetTilesBlock(bounds);

        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            bool shouldCountLeftToRight = (y % 2 == 0) ? countLeftToRight : !countLeftToRight;

            if (shouldCountLeftToRight)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    TileBase tile = tiles[(x - bounds.xMin) + (y - bounds.yMin) * bounds.size.x];
                    if (tile != null)
                    {
                        count++;

                        Vector3Int cellPosition = new(x, y, 0);
                        SpawnTextNumber(cellPosition, count);
                    }
                }
            }
            else
            {
                for (int x = bounds.xMax - 1; x >= bounds.xMin; x--)
                {
                    TileBase tile = tiles[(x - bounds.xMin) + (y - bounds.yMin) * bounds.size.x];
                    if (tile != null)
                    {
                        count++;

                        Vector3Int cellPosition = new(x, y, 0);
                        SpawnTextNumber(cellPosition, count);
                    }
                }
            }
        }

        return count;
    }

    private void SpawnTextNumber(Vector3Int position, int number)
    {
        Vector3 cellSize = tilemap.cellSize;

        // to spawn wayPoint at cell's top and textObj will be left in the center of cell
        Vector3 cellTop = new(0f, cellSize.y - 0.09f, 0f);

        // Get cell offset in Grid Selection properties, m13 is Y offset
        Vector3 cellOffset = new(0f, tilemap.GetTransformMatrix(position).m13, 0f);

        Vector3 cellCenter = tilemap.CellToWorld(position) + cellSize / 2f;

        // spawn at the center of cell
        GameObject prefab = Instantiate(wayPointPrefab, (cellCenter + cellOffset) + cellTop, Quaternion.identity);
        prefab.transform.parent = SpawnParent;

        WayPoints.Add(prefab.transform);

        // offset textObj and count
        Transform transform = prefab.transform.GetChild(0);
        transform.position = (cellCenter + cellOffset);
        transform.GetComponent<TextMeshPro>().text = number.ToString();

    }
}