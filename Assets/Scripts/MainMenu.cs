using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayOnePlayer()
    {
        PlayerPrefs.SetInt("TwoPlayerMode", 0);
        SceneManager.LoadScene("GameScene");
    }

    public void PlayTwoPlayer()
    {
        PlayerPrefs.SetInt("TwoPlayerMode", 1);
        SceneManager.LoadScene("GameScene");
    }

    public void OpenConstructor()
    {
        Debug.Log("Constructor mode not yet implemented");
    }
}