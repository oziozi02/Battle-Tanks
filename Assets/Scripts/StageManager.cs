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
        EnemyTank.ResetFreezeState();
        levelLoader.LoadLevel(stages[index]);
        enemySpawner.ResetSpawner();
        GameManager.Instance.UpdateStageUI(index + 1);

        PlayerHealth[] allPlayers = FindObjectsByType<PlayerHealth>(FindObjectsInactive.Include);
        foreach (var p in allPlayers)
        {
            p.RepositionToStart();
        }
    }

    public void NextStage()
    {
        currentStageIndex++;
        if (currentStageIndex >= stages.Length)
        {
            GameManager.Instance.ShowGameComplete();
            return;
        }
        LoadStage(currentStageIndex);
    }

    public void RetryStage()
    {
        LoadStage(currentStageIndex);
    }
}