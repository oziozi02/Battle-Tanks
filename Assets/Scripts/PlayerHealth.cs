using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int lives = 3;

    public void TakeDamage()
    {
        lives--;
        GameManager.Instance.UpdateLivesUI(lives);

        if (lives <= 0)
        {
            Debug.Log("Game Over!");
            gameObject.SetActive(false);
            GameManager.Instance.GameOver("Out of lives!");
        }
        else
        {
            // Reset position for now
            transform.position = Vector3.zero;
        }
    }
}
