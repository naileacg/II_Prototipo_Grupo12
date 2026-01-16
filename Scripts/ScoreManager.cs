using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int CurrentScore { get; private set; }

    public UnityEvent<int> onScoreChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
        onScoreChanged?.Invoke(CurrentScore);
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        onScoreChanged?.Invoke(CurrentScore);
    }
}