using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int lives = 3;
    private int startingLives;
    private Vector3 startingPosition;
    private bool isInvincible = false;

    void Start()
    {
        startingLives = lives;
        startingPosition = transform.position;
    }

    public void TakeDamage()
    {
        if (isInvincible) return;

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

    public void AddLife()
    {
        lives++;
        GameManager.Instance.UpdateLivesUI(lives);
    }

    public void ActivateInvincibility(float duration)
    {
        StartCoroutine(InvincibilityCoroutine(duration));
    }

    IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;

        // Flashing visual effect
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float elapsed = 0f;
        while (elapsed < duration)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }
        sr.enabled = true;

        isInvincible = false;
    }
}