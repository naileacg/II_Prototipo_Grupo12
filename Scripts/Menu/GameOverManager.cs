using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the Game Over state of the game.
/// Listens for the game over event and loads the Game Over scene when triggered.
/// </summary>
public class GameOverManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the spawner that notifies when the player dies.
    /// </summary>
    public Spawner spawner;

    /// <summary>
    /// Reference to the player's camera transform.
    /// (Not used here, but kept for consistency or future use.)
    /// </summary>
    public Transform cameraTransform;

    /// <summary>
    /// Subscribes to the Game Over event when the object is enabled.
    /// </summary>
    private void OnEnable()
    {
        if (spawner != null)
            spawner.OnGameOver += Dead;
    }

    /// <summary>
    /// Unsubscribes from the Game Over event when the object is disabled.
    /// </summary>
    private void OnDisable()
    {
        if (spawner != null)
            spawner.OnGameOver -= Dead;
    }

    /// <summary>
    /// Called when the player dies.
    /// Restores normal time scale and loads the Game Over scene.
    /// </summary>
    public void Dead()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameOver");
    }
}
