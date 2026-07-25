using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int lives = 3;

    public void TakeDamage()
    {
        lives--;
        Debug.Log("Lives remaining: " + lives);

        if (lives <= 0)
        {
            Debug.Log("Game Over!");
            gameObject.SetActive(false);
        }
        else
        {
            // Reset position for now
            transform.position = Vector3.zero;
        }
    }
}
