using UnityEngine;

public enum TileType
{
    Empty,
    Brick,
    Steel,
    Water,
    Trees,
    Ice
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Battle Tanks/Level Data")]
public class LevelData : ScriptableObject
{
    public int width = 26;
    public int height = 26;

    // Flattened grid - index = y * width + x
    public TileType[] tiles;

    public Vector2Int[] enemySpawnPoints = new Vector2Int[3];
    public Vector2Int eaglePosition;

    public TileType GetTile(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return TileType.Empty;
        return tiles[y * width + x];
    }

    public void SetTile(int x, int y, TileType type)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return;
        tiles[y * width + x] = type;
    }

    void OnEnable()
    {
        if (tiles == null || tiles.Length != width * height)
        {
            tiles = new TileType[width * height];
        }
    }
}