using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles basic menu actions for both the Main Menu and the Game Over menu.
/// Allows the player to start the game or exit the application.
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// Starts or restarts the main game.
    /// Resets the time scale and loads the main gameplay scene.
    /// </summary>
    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainGame"); 
    }

    /// <summary>
    /// Exits the game application.
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }
}
