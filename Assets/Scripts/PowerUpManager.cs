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
                ActivateHelmet();
                break;
            case PowerUpType.Shovel:
                ActivateShovel();
                break;
            case PowerUpType.Star:
                ActivateStar();
                break;
            case PowerUpType.Tank:
                ActivateTank();
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

    void ActivateTank()
    {
        FindAnyObjectByType<PlayerHealth>().AddLife();
    }

    void ActivateHelmet()
    {
        FindAnyObjectByType<PlayerHealth>().ActivateInvincibility(8f);
    }

    void ActivateShovel()
    {
        FindAnyObjectByType<LevelLoader>().ActivateShovel(15f);
    }

    void ActivateStar()
    {
        FindAnyObjectByType<PlayerController>().UpgradeStar();
    }

    public GameObject[] powerUpPrefabs; // In inspector

    public void SpawnRandomPowerUp(Vector3 position)
    {
        if (powerUpPrefabs.Length == 0) return;
        GameObject prefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
        Instantiate(prefab, position, Quaternion.identity);
    }
}