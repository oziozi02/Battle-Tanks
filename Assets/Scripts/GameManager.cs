using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI enemyCountText;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public TextMeshProUGUI gameOverReasonText;

    private bool gameEnded = false;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateLivesUI(int lives)
    {
        livesText.text = "Lives: " + lives;
    }

    public void UpdateEnemyCountUI(int remaining)
    {
        enemyCountText.text = "Enemies: " + remaining;

        if (remaining <= 0 && !gameEnded)
        {
            WinGame();
        }
    }

    public void GameOver(string reason)
    {
        if (gameEnded) return;
        gameEnded = true;

        gameOverPanel.SetActive(true);
        gameOverReasonText.text = reason;
    }

    void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;
        StartCoroutine(WinSequence());
    }

    public void Retry()
    {
        gameOverPanel.SetActive(false);
        gameEnded = false;
        PlayerHealth ph = FindAnyObjectByType<PlayerHealth>(FindObjectsInactive.Include);
        if (ph != null) ph.ResetPlayer();
        StageManager.Instance.RetryStage();
    }

    IEnumerator WinSequence()
    {
        winPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        winPanel.SetActive(false);
        gameEnded = false;
        StageManager.Instance.NextStage();
    }
}