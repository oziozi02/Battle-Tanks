using UnityEngine;
using UnityEditor;

public class LevelEditorWindow : EditorWindow
{
    private LevelData currentLevel;
    private TileType selectedTileType = TileType.Brick;
    private Vector2 scrollPos;
    private const int cellPixelSize = 20;

    private enum EditMode { Tiles, SpawnPoints }
    private EditMode currentMode = EditMode.Tiles;
    private int selectedSpawnIndex = 0;

    [MenuItem("Battle Tanks/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    void OnGUI()
    {
        currentLevel = (LevelData)EditorGUILayout.ObjectField("Level Data", currentLevel, typeof(LevelData), false);

        if (currentLevel == null)
        {
            EditorGUILayout.HelpBox("Assign a LevelData asset to start editing.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space();
        currentMode = (EditMode)EditorGUILayout.EnumPopup("Edit Mode", currentMode);

        if (currentMode == EditMode.Tiles)
        {
            selectedTileType = (TileType)EditorGUILayout.EnumPopup("Paint Tile Type", selectedTileType);
        }
        else if (currentMode == EditMode.SpawnPoints)
        {
            selectedSpawnIndex = EditorGUILayout.IntSlider("Spawn Point Index", selectedSpawnIndex, 0, 2);
        }

        EditorGUILayout.Space();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(500));

        for (int y = currentLevel.height - 2; y >= 1; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 1; x < currentLevel.width - 1; x++)
            {
                bool isEagleTile = (y == 1) && (x == 12 || x == 13);
                bool isEagleSurround = (y == 1 && (x == 11 || x == 14)) || (y == 2 && (x == 11 || x == 12 || x == 13 || x == 14));

                if (isEagleTile || isEagleSurround)
                {
                    GUILayout.Label(isEagleTile ? "E" : "B", GUILayout.Width(cellPixelSize), GUILayout.Height(cellPixelSize));
                    continue;
                }

                TileType tile = currentLevel.GetTile(x, y);
                Color color = GetColorForTile(tile);

                bool isSpawnPoint = false;
                int spawnIdx = -1;
                for (int i = 0; i < currentLevel.enemySpawnPoints.Length; i++)
                {
                    if (currentLevel.enemySpawnPoints[i].x == x && currentLevel.enemySpawnPoints[i].y == y)
                    {
                        isSpawnPoint = true;
                        spawnIdx = i;
                    }
                }

                GUI.backgroundColor = isSpawnPoint ? Color.magenta : color;
                string label = isSpawnPoint ? ("S" + spawnIdx) : "";

                if (GUILayout.Button(label, GUILayout.Width(cellPixelSize), GUILayout.Height(cellPixelSize)))
                {
                    Undo.RecordObject(currentLevel, "Edit Level");
                    if (currentMode == EditMode.Tiles)
                    {
                        currentLevel.SetTile(x, y, selectedTileType);
                    }
                    else if (currentMode == EditMode.SpawnPoints)
                    {
                        currentLevel.enemySpawnPoints[selectedSpawnIndex] = new Vector2Int(x, y);
                    }
                    EditorUtility.SetDirty(currentLevel);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
        GUI.backgroundColor = Color.white;

        EditorGUILayout.Space();
        if (GUILayout.Button("Clear Level"))
        {
            Undo.RecordObject(currentLevel, "Clear Level");
            for (int i = 0; i < currentLevel.tiles.Length; i++)
                currentLevel.tiles[i] = TileType.Empty;
            EditorUtility.SetDirty(currentLevel);
        }
    }

    Color GetColorForTile(TileType type)
    {
        switch (type)
        {
            case TileType.Brick: return new Color(0.749f, 0.361f, 0.227f);
            case TileType.Steel: return new Color(0.471f, 0.565f, 0.612f);
            case TileType.Water: return new Color(0.086f, 0.396f, 0.753f);
            case TileType.Trees: return new Color(0.180f, 0.490f, 0.196f);
            case TileType.Ice: return new Color(0.698f, 0.922f, 0.949f);
            default: return Color.black;
        }
    }
}