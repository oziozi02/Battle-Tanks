using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelLoader : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap brickTilemap;
    public Tilemap steelTilemap;
    public Tilemap waterTilemap;
    public Tilemap treesTilemap;
    public Tilemap iceTilemap;

    public TileBase steelTile;
    public TileBase brickTile;
    public TileBase waterTile;
    public TileBase treesTile;
    public TileBase iceTile;

    public GameObject eaglePrefab;
    public Transform[] spawnPointTransforms; // 3 spawn point empty GameObjects

    private GameObject currentEagle;

    public void LoadLevel(LevelData level)
    {
        ClearAllTilemaps();

        // Convert tile coordinates to world/tilemap cell coordinates
        // Assuming the level grid is centered like our current map (-13 to 13)
        int offsetX = -level.width / 2;
        int offsetY = -level.height / 2;

        for (int y = 0; y < level.height; y++)
        {
            for (int x = 0; x < level.width; x++)
            {
                TileType type = level.GetTile(x, y);
                Vector3Int pos = new Vector3Int(x + offsetX, y + offsetY, 0);

                switch (type)
                {
                    case TileType.Brick:
                        brickTilemap.SetTile(pos, brickTile);
                        break;
                    case TileType.Steel:
                        steelTilemap.SetTile(pos, steelTile);
                        break;
                    case TileType.Water:
                        waterTilemap.SetTile(pos, waterTile);
                        break;
                    case TileType.Trees:
                        treesTilemap.SetTile(pos, treesTile);
                        break;
                    case TileType.Ice:
                        iceTilemap.SetTile(pos, iceTile);
                        break;
                }
            }
        }

        // Position spawn points
        for (int i = 0; i < spawnPointTransforms.Length && i < level.enemySpawnPoints.Length; i++)
        {
            Vector2Int spawnGridPos = level.enemySpawnPoints[i];
            spawnPointTransforms[i].position = new Vector3(spawnGridPos.x + offsetX, spawnGridPos.y + offsetY, 0);
        }

        // Place eagle
        if (currentEagle != null) Destroy(currentEagle);
        Vector3 eagleWorldPos = new Vector3(level.eaglePosition.x + offsetX, level.eaglePosition.y + offsetY, 0);
        currentEagle = Instantiate(eaglePrefab, eagleWorldPos, Quaternion.identity);
    }

    void ClearAllTilemaps()
    {
        brickTilemap.CompressBounds();
        steelTilemap.CompressBounds();
        waterTilemap.CompressBounds();
        treesTilemap.CompressBounds();
        iceTilemap.CompressBounds();

        brickTilemap.ClearAllTiles();
        // Don't clear steelTilemap fully if it holds the permanent border - we'll handle that separately
        waterTilemap.ClearAllTiles();
        treesTilemap.ClearAllTiles();
        iceTilemap.ClearAllTiles();
    }
}