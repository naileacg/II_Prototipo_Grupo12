using UnityEngine;

/// <summary>
/// Handles reward logic for a cube when it is killed,
/// including score, death effects and notifying listeners.
/// </summary>
public class CubeReward : MonoBehaviour
{
    /// <summary>
    /// Number of points awarded when this cube is killed,
    /// regardless of the cause of death.
    /// </summary>
    public int pointsOnKill = 10;

    /// <summary>
    /// Delay in seconds before destroying the cube after it is killed.
    /// </summary>
    public float destroyDelay = 0.2f;

    public float actualHeatlth = 100f;

    /// <summary>
    /// Optional prefab for a visual death effect instantiated
    /// when the cube is killed by the player.
    /// </summary>
    public GameObject deathEffectPrefab;

    /// <summary>
    /// Position used to hide the cube before destruction,
    /// keeping it out of view and raycasts.
    /// </summary>
    private Vector3 hiddenPosition = new Vector3(0, -100f, 0);

    /// <summary>
    /// Kills the cube due to an automatic cause (e.g. reaching a central point),
    /// without spawning a visual effect.
    /// </summary>
    public void KillAuto()
    {
        KillCommon(spawnEffect: false);
    }

    /// <summary>
    /// Kills the cube due to player action, spawning a visual effect
    /// in addition to the common kill behavior.
    /// </summary>
    public void KillByPlayer()
    {
        KillCommon(spawnEffect: true);
    }

    /// <summary>
    /// Shared kill logic used by both automatic and player-caused deaths:
    /// awards points, notifies listeners and schedules destruction.
    /// </summary>
    /// <param name="spawnEffect">
    /// If true, instantiates the configured death effect prefab at the cube's position.
    /// </param>
    private void KillCommon(bool spawnEffect)
    {
        // Always award points, regardless of the source of death.
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(pointsOnKill);

        var reporter = GetComponent<CubeDeathReporter>();
        if (reporter != null)
            reporter.ReportDeath();

        if (spawnEffect && deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // Hide the cube before actually destroying it.
        transform.position = hiddenPosition;

        // Destroy the cube after a small delay.
        Destroy(gameObject, destroyDelay);
    }
}