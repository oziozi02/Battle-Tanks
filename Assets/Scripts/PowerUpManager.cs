using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void ActivatePowerUp(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.Grenade:
                ActivateGrenade();
                break;
            case PowerUpType.Helmet:
                Debug.Log("Helmet activated");
                break;
            case PowerUpType.Shovel:
                Debug.Log("Shovel activated");
                break;
            case PowerUpType.Star:
                Debug.Log("Star activated");
                break;
            case PowerUpType.Tank:
                Debug.Log("Tank activated");
                break;
            case PowerUpType.Timer:
                ActivateTimer();
                break;
        }
    }

    void ActivateGrenade()
    {
        EnemyTank[] enemies = FindObjectsByType<EnemyTank>();
        foreach (EnemyTank enemy in enemies)
        {
            enemy.InstantKill();
        }
    }

    void ActivateTimer()
    {
        EnemyTank.FreezeAll(6f);
    }
}