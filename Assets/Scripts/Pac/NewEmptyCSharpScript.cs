using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDebug : MonoBehaviour
{
    public Tilemap tilemap;
    void Start()
    {
        BoundsInt bounds = tilemap.cellBounds;
        int count = 0;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                if (tilemap.HasTile(new Vector3Int(x, y, 0)))
                    count++;
            }
        }
        Debug.Log("瓦片数量: " + count);
    }
}