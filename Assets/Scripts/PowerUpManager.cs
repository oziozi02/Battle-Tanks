using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void ActivatePowerUp(PowerUpType type, PlayerController pc, PlayerHealth ph)
    {
        switch (type)
        {
            case PowerUpType.Grenade:
                ActivateGrenade();
                break;
            case PowerUpType.Helmet:
                ph.ActivateInvincibility(8f);
                break;
            case PowerUpType.Shovel:
                ActivateShovel();
                break;
            case PowerUpType.Star:
                pc.UpgradeStar();
                break;
            case PowerUpType.Tank:
                ph.AddLife();
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

    void ActivateShovel()
    {
        FindAnyObjectByType<LevelLoader>().ActivateShovel(15f);
    }

    public GameObject[] powerUpPrefabs; // In inspector

    public void SpawnRandomPowerUp(Vector3 position)
    {
        if (powerUpPrefabs.Length == 0) return;
        GameObject prefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
        Instantiate(prefab, position, Quaternion.identity);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.powerUpAppear);
    }
}