using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); // adjust to match your actual gameplay scene name
    }

    public void OpenConstructor()
    {
        Debug.Log("Constructor mode not yet implemented");
    }
}