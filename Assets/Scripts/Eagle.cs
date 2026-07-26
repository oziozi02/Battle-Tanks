using UnityEngine;

public class Eagle : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
            GameManager.Instance.GameOver("Base destroyed!");
            Destroy(gameObject);
        }
    }
}