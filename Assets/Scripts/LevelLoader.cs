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
    public Transform[] spawnPointTransforms;

    private GameObject currentEagle;

    // Adjust these two to match your actual border layout
    private readonly Vector2Int fixedEaglePosition = new Vector2Int(12, 1);
    private readonly Vector2Int[] eagleSurroundOffsets = new Vector2Int[]
    {
        new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1),
        new Vector2Int(-1, 0), new Vector2Int(2, 0)
    };

    public void LoadLevel(LevelData level)
    {
        ClearAllTilemaps();

        int offsetX = -level.width / 2;
        int offsetY = -level.height / 2;

        for (int y = 0; y < level.height; y++)
        {
            for (int x = 0; x < level.width; x++)
            {
                TileType type = level.GetTile(x, y);
                Vector3Int pos = new Vector3Int(x + offsetX, y + offsetY, 0);

                // Skip eagle footprint when painting level tiles
                if (y + offsetY == fixedEaglePosition.y && (x + offsetX == fixedEaglePosition.x || x + offsetX == fixedEaglePosition.x + 1))
                {
                    continue;
                }

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

        PaintEagleSurround(offsetX, offsetY);

        for (int i = 0; i < spawnPointTransforms.Length && i < level.enemySpawnPoints.Length; i++)
        {
            Vector2Int spawnGridPos = level.enemySpawnPoints[i];
            spawnPointTransforms[i].position = new Vector3(spawnGridPos.x + offsetX, spawnGridPos.y + offsetY, 0);
        }

        if (currentEagle != null) Destroy(currentEagle);
        Vector3 eagleWorldPos = new Vector3(fixedEaglePosition.x + offsetX + 1f, fixedEaglePosition.y + offsetY + 0.5f, 0);
        currentEagle = Instantiate(eaglePrefab, eagleWorldPos, Quaternion.identity);

        FindAnyObjectByType<EnemySpawner>().SetSpawnPoints(spawnPointTransforms);
    }

    void PaintEagleSurround(int offsetX, int offsetY)
    {
        Vector3Int eagleTile1 = new Vector3Int(fixedEaglePosition.x + offsetX, fixedEaglePosition.y + offsetY, 0);
        Vector3Int eagleTile2 = new Vector3Int(fixedEaglePosition.x + 1 + offsetX, fixedEaglePosition.y + offsetY, 0);

        // Clear eagle footprint from every tilemap
        ClearPositionFromAllTilemaps(eagleTile1);
        ClearPositionFromAllTilemaps(eagleTile2);

        foreach (var offset in eagleSurroundOffsets)
        {
            Vector3Int pos = new Vector3Int(fixedEaglePosition.x + offset.x + offsetX, fixedEaglePosition.y + offset.y + offsetY, 0);
            ClearPositionFromAllTilemaps(pos);
            brickTilemap.SetTile(pos, brickTile);
        }
    }

    void ClearPositionFromAllTilemaps(Vector3Int pos)
    {
        brickTilemap.SetTile(pos, null);
        steelTilemap.SetTile(pos, null);
        waterTilemap.SetTile(pos, null);
        treesTilemap.SetTile(pos, null);
        iceTilemap.SetTile(pos, null);
    }

    void ClearAllTilemaps()
    {
        brickTilemap.ClearAllTiles();
        steelTilemap.ClearAllTiles();
        waterTilemap.ClearAllTiles();
        treesTilemap.ClearAllTiles();
        iceTilemap.ClearAllTiles();
    }
}