using UnityEngine;

/// <summary>
/// Manages enemy rounds, including spawn pacing, progression,
/// and delays between consecutive rounds.
/// </summary>
public class RoundManager : MonoBehaviour
{
    [Header("References")]
    /// <summary>
    /// Spawner responsible for instantiating cube enemies.
    /// </summary>
    public Spawner spawner;

    [Header("Rounds")]
    /// <summary>
    /// Time in seconds between individual spawns within a round.
    /// </summary>
    public float spawnInterval = 2f;

    /// <summary>
    /// Number of cubes spawned in the first round.
    /// </summary>
    public int baseCubesPerRound = 5;

    /// <summary>
    /// Multiplier used to scale the number of cubes per round,
    /// applied as <c>base * multiplier^(round - 1)</c>.
    /// </summary>
    public float cubesPerRoundMultiplier = 1.3f;

    [Header("Round delay")]
    /// <summary>
    /// Delay in seconds between the end of a round and the start of the next one.
    /// </summary>
    public float roundDelay = 3f;

    /// <summary>
    /// Indicates whether the manager is currently waiting before starting the next round.
    /// </summary>
    private bool waitingNextRound = false;

    /// <summary>
    /// Countdown timer used for the delay between rounds.
    /// </summary>
    private float roundDelayTimer = 0f;

    /// <summary>
    /// Current round index. Starts at 0 (no active round yet).
    /// </summary>
    private int currentRound = 0;

    /// <summary>
    /// Total number of cubes to spawn in the current round.
    /// </summary>
    private int cubesToSpawnThisRound;

    /// <summary>
    /// Number of cubes already spawned in the current round.
    /// </summary>
    private int cubesSpawnedThisRound;

    /// <summary>
    /// Number of cubes currently alive in the scene.
    /// </summary>
    private int cubesAlive;

    /// <summary>
    /// Internal timer used to control spawn interval within a round.
    /// </summary>
    private float timer;

    /// <summary>
    /// Indicates whether a round is currently active.
    /// </summary>
    private bool roundActive = false;

    private void Start()
    {
        if (spawner == null)
            spawner = Object.FindFirstObjectByType<Spawner>();

        // Start first round automatically.
        StartNextRound();
    }

    private void Update()
    {
        if (spawner == null) return;

        if (roundActive)
        {
            // Spawn cubes until the round quota is reached.
            if (cubesSpawnedThisRound < cubesToSpawnThisRound)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    SpawnOne();
                    timer = spawnInterval;
                }
            }

            // When the round is finished, start the delay countdown.
            if (cubesAlive <= 0 && cubesSpawnedThisRound >= cubesToSpawnThisRound)
            {
                roundActive = false;
                waitingNextRound = true;
                roundDelayTimer = roundDelay;
                Debug.Log($"Round {currentRound} finished. Next round in {roundDelay} seconds.");
            }
        }
        else if (waitingNextRound)
        {
            roundDelayTimer -= Time.deltaTime;
            if (roundDelayTimer <= 0f)
            {
                waitingNextRound = false;
                StartNextRound();
            }
        }
    }

    // ---------- Public API for other systems ----------

    /// <summary>
    /// Starts the next round if no round is currently active.
    /// Can be called externally (e.g. from a shop or UI) when
    /// automatic progression is disabled.
    /// </summary>
    public void StartNextRound()
    {
        if (roundActive) return;

        currentRound++;
        StartRoundInternal(currentRound);
    }

    /// <summary>
    /// Gets the current round number.
    /// </summary>
    public int CurrentRound => currentRound;

    /// <summary>
    /// Gets the number of enemies currently alive.
    /// </summary>
    public int EnemiesAlive => cubesAlive;

    /// <summary>
    /// Gets the total number of enemies that will be spawned this round.
    /// </summary>
    public int EnemiesThisRound => cubesToSpawnThisRound;

    /// <summary>
    /// Indicates whether a round is currently running.
    /// </summary>
    public bool RoundActive => roundActive;

    // ---------- Internal logic ----------

    /// <summary>
    /// Configures and starts a specific round index.
    /// </summary>
    /// <param name="newRound">Round number to start.</param>
    private void StartRoundInternal(int newRound)
    {
        currentRound = newRound;

        cubesToSpawnThisRound = Mathf.RoundToInt(
            baseCubesPerRound * Mathf.Pow(cubesPerRoundMultiplier, currentRound - 1)
        );
        cubesSpawnedThisRound = 0;
        cubesAlive = 0;
        timer = 0f;
        roundActive = true;

        Debug.Log($"Starting round {currentRound} with {cubesToSpawnThisRound} cubes.");
    }

    /// <summary>
    /// Requests the spawner to instantiate a single cube and
    /// subscribes to its death event.
    /// </summary>
    private void SpawnOne()
    {
        var obj = spawner.SpawnOne();
        if (obj == null) return;

        var reporter = obj.GetComponent<CubeDeathReporter>();
        if (reporter != null)
            reporter.onKilled += OnCubeKilled;

        cubesSpawnedThisRound++;
        cubesAlive++;
    }

    /// <summary>
    /// Called when a cube reports its death, decreasing the alive counter.
    /// </summary>
    private void OnCubeKilled()
    {
        cubesAlive--;
    }
}

//Posible uso desde UI o tienda:
// using UnityEngine;

// public class ShopController : MonoBehaviour
// {
//     public RoundManager roundManager;

//     void Start()
//     {
//         if (roundManager == null)
//             roundManager = Object.FindFirstObjectByType<RoundManager>();
//     }

//     public void OnStartRoundButtonPressed()
//     {
//         roundManager.StartNextRound();
//     }
// }