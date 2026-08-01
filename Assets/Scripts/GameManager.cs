using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI livesTextP1;
    public TextMeshProUGUI livesTextP2;
    public TextMeshProUGUI enemyCountText;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public TextMeshProUGUI gameOverReasonText;
    public GameObject retryButton;

    public GameObject tallyPanel;
    public GameObject tallyP2Section; // container for P2's column, hidden in 1P mode
    public TextMeshProUGUI[] tallyTextsP1; // Basic, Fast, Power, Armor, Total (5 elements)
    public TextMeshProUGUI[] tallyTextsP2;
    public TextMeshProUGUI tallyGrandTotalText;
    public GameObject gameCompletePanel;
    public TextMeshProUGUI gameCompleteScoreText;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI stageText;
    public GameObject pausePanel;

    public bool twoPlayerMode = false;
    private bool player1Defeated = false;
    private bool player2Defeated = false;
    private bool gameEnded = false;
    private bool isPaused = false;

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
        AudioManager.Instance.PlayMusic(AudioManager.Instance.gameplayMusic);
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
            Difficulty diff = (Difficulty)PlayerPrefs.GetInt("Difficulty", 0);
            if (diff == Difficulty.Hardcore)
            {
                GameOverHardcore();
            }
            else
            {
                GameOver("Out of lives!");
            }
        }
    }

    void GameOverHardcore()
    {
        if (gameEnded) return;
        gameEnded = true;
        ScoreManager.Instance.CheckAndSaveHighScore();
        gameOverPanel.SetActive(true);
        gameOverReasonText.text = "Run over! No retries in Hardcore.";
        gameOverScoreText.text = "Score: " + ScoreManager.Instance.GetCumulativeTotal();
        retryButton.SetActive(false); // hide retry, only main menu option remains
        AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOver);
        Time.timeScale = 0f;
    }

    public void GameOver(string reason)
    {
        if (gameEnded) return;
        gameEnded = true;
        ScoreManager.Instance.CheckAndSaveHighScore();
        gameOverPanel.SetActive(true);
        gameOverReasonText.text = reason;
        gameOverScoreText.text = "Score: " + ScoreManager.Instance.GetCumulativeTotal();
        retryButton.SetActive(true);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOver);
        Time.timeScale = 0f;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        ScoreManager.Instance.ResetCumulativeTotal();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
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
        AudioManager.Instance.PlaySFX(AudioManager.Instance.stageClear);
        yield return new WaitForSecondsRealtime(2f);
        winPanel.SetActive(false);

        ShowTally();
        yield return new WaitForSecondsRealtime(8f);
        tallyPanel.SetActive(false);

        gameEnded = false;
        StageManager.Instance.NextStage();
        ScoreManager.Instance.ResetTally();
    }

    void ShowTally()
    {
        tallyPanel.SetActive(true);
        PopulateTally(1, tallyTextsP1);

        if (twoPlayerMode)
        {
            tallyP2Section.SetActive(true);
            PopulateTally(2, tallyTextsP2);
        }
        else
        {
            tallyP2Section.SetActive(false);
        }
        tallyGrandTotalText.text = "Total Points: " + ScoreManager.Instance.GetCumulativeTotal();
    }

    void PopulateTally(int playerIndex, TextMeshProUGUI[] texts)
    {
        var types = new EnemyTank.TankType[] { EnemyTank.TankType.Basic, EnemyTank.TankType.Fast, EnemyTank.TankType.Power, EnemyTank.TankType.Armor };
        int[] points = { 100, 200, 300, 400 };

        for (int i = 0; i < types.Length; i++)
        {
            int count = ScoreManager.Instance.GetKillCount(playerIndex, types[i]);
            texts[i].text = types[i] + " x" + count + " = " + (count * points[i]);
        }

        texts[4].text = "Total: " + ScoreManager.Instance.GetScore(playerIndex);
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        ScoreManager.Instance.CheckAndSaveHighScore();
        gameOverPanel.SetActive(false);
        gameEnded = false;
        player1Defeated = false;
        player2Defeated = false;

        ScoreManager.Instance.ApplyRetryPenalty();
        ScoreManager.Instance.ResetTally();

        PlayerHealth[] allPlayers = FindObjectsByType<PlayerHealth>(FindObjectsInactive.Include);
        foreach (var p in allPlayers)
        {
            if (p.playerIndex == 2 && !twoPlayerMode) continue;
            p.ResetPlayer();
        }

        StageManager.Instance.RetryStage();
    }

    public void ShowGameComplete()
    {
        ScoreManager.Instance.CheckAndSaveHighScore();
        gameCompletePanel.SetActive(true);
        gameCompleteScoreText.text = "Final Score: " + ScoreManager.Instance.GetCumulativeTotal();
        AudioManager.Instance.PlayJingle(AudioManager.Instance.gameCompleteJingle);
    }

    public void GameCompleteReturnToMenu()
    {
        gameCompletePanel.SetActive(false);
        ScoreManager.Instance.ResetCumulativeTotal();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void UpdateStageUI(int stageNumber)
    {
        stageText.text = "Stage " + stageNumber;
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !gameEnded)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.pauseToggle);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PauseMainMenu()
    {
        Time.timeScale = 1f;
        ScoreManager.Instance.CheckAndSaveHighScore();
        ScoreManager.Instance.ResetCumulativeTotal();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}