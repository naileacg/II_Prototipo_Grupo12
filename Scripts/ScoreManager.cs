using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Global score manager that keeps track of the player's score
/// and notifies other systems when the score changes.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the score manager.
    /// Allows global access to the score.
    /// </summary>
    public static ScoreManager Instance { get; private set; }

    /// <summary>
    /// Current score value.
    /// Can only be modified through AddScore or ResetScore.
    /// </summary>
    public int CurrentScore { get; private set; }

    /// <summary>
    /// Event triggered whenever the score changes.
    /// Sends the new score value to listeners (UI, audio, etc.).
    /// </summary>
    public UnityEvent<int> onScoreChanged;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Adds points to the current score and
    /// notifies all subscribed listeners.
    /// </summary>
    /// <param name="amount">Amount of points to add (can be negative).</param>
    public void AddScore(int amount)
    {
        CurrentScore += amount;
        onScoreChanged?.Invoke(CurrentScore);
    }

    /// <summary>
    /// Resets the score back to zero and
    /// notifies all subscribed listeners.
    /// </summary>
    public void ResetScore()
    {
        CurrentScore = 0;
        onScoreChanged?.Invoke(CurrentScore);
    }
}
