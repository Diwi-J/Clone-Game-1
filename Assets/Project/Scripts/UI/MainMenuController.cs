using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // The name of the scene you want to load when clicking play
    [SerializeField] private string gameSceneName = "Booth";

    public void OnPlayButtonClicked()
    {
        // Loads the game scene
        Debug.Log("Loading Game Scene...");
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnQuitButtonClicked()
    {
        // Quits the application (Note: This only works in built games, not in the editor)
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
