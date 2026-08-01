using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int playerIndex = 1;
    public int lives = 3;
    private int startingLives;
    private Vector3 startingPosition;
    private bool isInvincible = false;

    void Awake()
    {
        startingPosition = transform.position;
    }

    void Start()
    {
        Difficulty diff = (Difficulty)PlayerPrefs.GetInt("Difficulty", 0);

        if (diff == Difficulty.Easy) lives = 3;
        else lives = 1;

        startingLives = lives;

        GameManager.Instance.UpdateLivesUI(playerIndex, lives);
    }

    public void TakeDamage()
    {
        if (isInvincible) return;

        lives--;
        GameManager.Instance.UpdateLivesUI(playerIndex, lives);

        if (lives <= 0)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.tankDestroyed);
            gameObject.SetActive(false);
            GameManager.Instance.OnPlayerDefeated(playerIndex);
        }
        else
        {
            transform.position = startingPosition;
        }
    }

    public void AddLife()
    {
        lives++;
        GameManager.Instance.UpdateLivesUI(playerIndex, lives);
    }

    public void ActivateInvincibility(float duration)
    {
        StartCoroutine(InvincibilityCoroutine(duration));
    }

    System.Collections.IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
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

    public void ResetPlayer()
    {
        lives = startingLives;
        gameObject.SetActive(true);
        transform.position = startingPosition;
        GameManager.Instance.UpdateLivesUI(playerIndex, lives);

        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.ResetStarLevel();
            pc.ResetShootState();
        }
    }

    public void RepositionToStart()
    {
        if (gameObject.activeSelf)
        {
            transform.position = startingPosition;
        }
    }
}