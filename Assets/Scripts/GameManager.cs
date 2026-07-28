using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI livesTextP1;
    public TextMeshProUGUI livesTextP2;
    public TextMeshProUGUI enemyCountText;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public TextMeshProUGUI gameOverReasonText;

    public bool twoPlayerMode = false;
    private bool player1Defeated = false;
    private bool player2Defeated = false;
    private bool gameEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        twoPlayerMode = PlayerPrefs.GetInt("TwoPlayerMode", 0) == 1;

        GameObject player2 = GameObject.Find("PlayerTank2");
        if (player2 != null)
        {
            player2.SetActive(twoPlayerMode);
        }

        if (livesTextP2 != null)
        {
            livesTextP2.gameObject.SetActive(twoPlayerMode);
        }
    }

    public void UpdateLivesUI(int playerIndex, int lives)
    {
        if (playerIndex == 1 && livesTextP1 != null)
            livesTextP1.text = "P1 Lives: " + lives;
        else if (playerIndex == 2 && livesTextP2 != null)
            livesTextP2.text = "P2 Lives: " + lives;
    }

    public void UpdateEnemyCountUI(int remaining)
    {
        enemyCountText.text = "Enemies: " + remaining;

        if (remaining <= 0 && !gameEnded)
        {
            WinGame();
        }
    }

    public void OnPlayerDefeated(int playerIndex)
    {
        if (playerIndex == 1) player1Defeated = true;
        if (playerIndex == 2) player2Defeated = true;

        bool shouldEndGame = twoPlayerMode
            ? (player1Defeated && player2Defeated)
            : player1Defeated;

        if (shouldEndGame)
        {
            GameOver("Out of lives!");
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

    IEnumerator WinSequence()
    {
        winPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        winPanel.SetActive(false);
        gameEnded = false;
        StageManager.Instance.NextStage();
    }

    public void Retry()
    {
        gameOverPanel.SetActive(false);
        gameEnded = false;
        player1Defeated = false;
        player2Defeated = false;

        PlayerHealth[] allPlayers = FindObjectsByType<PlayerHealth>(FindObjectsInactive.Include);
        foreach (var p in allPlayers)
        {
            if (p.playerIndex == 2 && !twoPlayerMode) continue;
            p.ResetPlayer();
        }

        StageManager.Instance.RetryStage();
    }
}