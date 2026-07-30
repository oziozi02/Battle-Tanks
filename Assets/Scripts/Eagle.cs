using UnityEngine;

public class Eagle : MonoBehaviour
{
    public Sprite destroyedSprite;

    void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null && destroyedSprite != null)
            {
                sr.sprite = destroyedSprite;
            }

            // Disable collider so it can't be "hit" again, but leave the object visible
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            GameManager.Instance.GameOver("Base destroyed!");

            // Destroy after a short delay so the rubble sprite is visible briefly
            Destroy(gameObject, 1.5f);
        }
    }
}