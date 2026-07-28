using UnityEngine;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private Dictionary<EnemyTank.TankType, int> killsP1 = new Dictionary<EnemyTank.TankType, int>();
    private Dictionary<EnemyTank.TankType, int> killsP2 = new Dictionary<EnemyTank.TankType, int>();
    private int scoreP1 = 0;
    private int scoreP2 = 0;

    void Awake()
    {
        Instance = this;
        ResetTally();
    }

    public void ResetTally()
    {
        killsP1.Clear();
        killsP2.Clear();
        foreach (EnemyTank.TankType type in System.Enum.GetValues(typeof(EnemyTank.TankType)))
        {
            killsP1[type] = 0;
            killsP2[type] = 0;
        }
        scoreP1 = 0;
        scoreP2 = 0;
    }

    public void RegisterKill(EnemyTank.TankType type, int playerIndex)
    {
        int points = GetPointsForType(type);
        if (playerIndex == 1)
        {
            killsP1[type]++;
            scoreP1 += points;
        }
        else
        {
            killsP2[type]++;
            scoreP2 += points;
        }
    }

    public void RegisterPowerUp(int playerIndex)
    {
        if (playerIndex == 1) scoreP1 += 500;
        else scoreP2 += 500;
    }

    int GetPointsForType(EnemyTank.TankType type)
    {
        switch (type)
        {
            case EnemyTank.TankType.Basic: return 100;
            case EnemyTank.TankType.Fast: return 200;
            case EnemyTank.TankType.Power: return 300;
            case EnemyTank.TankType.Armor: return 400;
            default: return 0;
        }
    }

    public int GetKillCount(int playerIndex, EnemyTank.TankType type)
    {
        return playerIndex == 1 ? killsP1[type] : killsP2[type];
    }

    public int GetScore(int playerIndex)
    {
        return playerIndex == 1 ? scoreP1 : scoreP2;
    }
}