using UnityEngine;

public enum PowerUpType
{
    Grenade,
    Helmet,
    Shovel,
    Star,
    Tank,
    Timer
}

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PowerUpManager.Instance.ActivatePowerUp(type);
            PlayerController pc = other.GetComponent<PlayerController>();
            int idx = pc != null ? pc.playerIndex : 1;
            ScoreManager.Instance.RegisterPowerUp(idx, transform.position);
            Destroy(gameObject);
        }
    }
}