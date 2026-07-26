using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI enemyCountText;

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
    }
}