using UnityEngine;

public class Eagle : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
            Debug.Log("GAME OVER - Eagle destroyed!");
            // We'll add proper game over logic here later
            Destroy(gameObject);
        }
    }
}