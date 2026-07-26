using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public LevelData[] stages;
    public LevelLoader levelLoader;
    public EnemySpawner enemySpawner;

    private int currentStageIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadStage(currentStageIndex);
    }

    public void LoadStage(int index)
    {
        currentStageIndex = index;
        levelLoader.LoadLevel(stages[index]);
        enemySpawner.ResetSpawner();
    }

    public void NextStage()
    {
        currentStageIndex++;
        if (currentStageIndex >= stages.Length)
        {
            currentStageIndex = 0; // loop back, or handle "game complete" separately
        }
        LoadStage(currentStageIndex);
    }

    public void RetryStage()
    {
        LoadStage(currentStageIndex);
    }
}