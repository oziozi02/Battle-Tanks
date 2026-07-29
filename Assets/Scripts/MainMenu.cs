using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Difficulty { Easy, Hard, Hardcore }

public class MainMenu : MonoBehaviour
{
    public GameObject playerCountPanel;
    public GameObject difficultyPanel;
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
    }

    public void SelectOnePlayer()
    {
        PlayerPrefs.SetInt("TwoPlayerMode", 0);
        ShowDifficultyPanel();
    }

    public void SelectTwoPlayer()
    {
        PlayerPrefs.SetInt("TwoPlayerMode", 1);
        ShowDifficultyPanel();
    }

    void ShowDifficultyPanel()
    {
        playerCountPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void StartEasy()
    {
        PlayerPrefs.SetInt("Difficulty", (int)Difficulty.Easy);
        SceneManager.LoadScene("GameScene");
    }

    public void StartHard()
    {
        PlayerPrefs.SetInt("Difficulty", (int)Difficulty.Hard);
        SceneManager.LoadScene("GameScene");
    }

    public void StartHardcore()
    {
        PlayerPrefs.SetInt("Difficulty", (int)Difficulty.Hardcore);
        SceneManager.LoadScene("GameScene");
    }

    public void GoBack()
    {
        difficultyPanel.SetActive(false);
        playerCountPanel.SetActive(true);
    }
}