using UnityEngine;

public class IceZone : MonoBehaviour
{
    public float speedMultiplier = 1.8f;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null) player.SetOnIce(true);

        EnemyTank enemy = other.GetComponent<EnemyTank>();
        if (enemy != null) enemy.SetOnIce(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null) player.SetOnIce(false);

        EnemyTank enemy = other.GetComponent<EnemyTank>();
        if (enemy != null) enemy.SetOnIce(false);
    }
}