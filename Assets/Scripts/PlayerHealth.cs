using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int lives = 3;
    private int startingLives;
    private Vector3 startingPosition;

    void Start()
    {
        startingLives = lives;
        startingPosition = transform.position;
    }

    public void TakeDamage()
    {
        lives--;
        GameManager.Instance.UpdateLivesUI(lives);

        if (lives <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.GameOver("Out of lives!");
        }
        else
        {
            transform.position = startingPosition;
        }
    }

    public void ResetPlayer()
    {
        lives = startingLives;
        gameObject.SetActive(true);
        transform.position = startingPosition;
        GameManager.Instance.UpdateLivesUI(lives);
    }
}