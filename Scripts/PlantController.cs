using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the lifecycle and combat behavior of a plant unit,
/// including health, attack strategy, visuals, audio and events
/// for activation and deactivation.
/// </summary>
public class PlantController : MonoBehaviour, IDamageable
{
    /// <summary>
    /// Enables additional debug logging when true.
    /// </summary>
    [Header("Debug Settings")]
    public bool debugMode = true;

    /// <summary>
    /// Scriptable stats that define health, cooldowns, range and audio clips.
    /// </summary>
    [Header("Configuration")]
    public PlantStats stats;

    /// <summary>
    /// Strategy object that encapsulates the plant's attack logic.
    /// </summary>
    public PlantStrategy strategy;

    /// <summary>
    /// Default visual model used when the plant is idle or not attacking.
    /// </summary>
    [Header("3D Visuals")]
    public GameObject modelNormal;

    /// <summary>
    /// Alternate visual model used when the plant is attacking.
    /// </summary>
    public GameObject modelAttack;

    /// <summary>
    /// Optional particle system for visual feedback (e.g. attack or hit effects).
    /// </summary>
    public ParticleSystem particles;

    /// <summary>
    /// Transform from which attacks originate (e.g. the mouth or weapon tip).
    /// </summary>
    [Header("Physics")]
    public Transform attackPoint;

    /// <summary>
    /// Reusable collider buffer for overlap queries when detecting targets.
    /// </summary>
    public Collider[] hitBuffer = new Collider[10];

    /// <summary>
    /// AudioSource used to play attack and feedback sounds.
    /// </summary>
    [Header("Audio")]
    private AudioSource audioSource;

    /// <summary>
    /// Delegate type used for plant status change callbacks.
    /// </summary>
    /// <param name="plant">The plant whose status changed.</param>
    public delegate void PlantStatusHandler(PlantController plant);

    /// <summary>
    /// Event raised when a plant instance becomes active.
    /// </summary>
    public static event PlantStatusHandler OnPlantActivated;

    /// <summary>
    /// Event raised when a plant instance becomes inactive.
    /// </summary>
    public static event PlantStatusHandler OnPlantDeactivated;

    /// <summary>
    /// Current health value of the plant.
    /// </summary>
    private float currentHealth;

    /// <summary>
    /// Cached wait instruction for attack cooldown between successful attacks.
    /// </summary>
    private WaitForSeconds waitCooldown;

    /// <summary>
    /// Cached wait instruction for the search interval when looking for targets.
    /// </summary>
    private WaitForSeconds waitSearch;

    /// <summary>
    /// Cached wait instruction used for short visual flash timings.
    /// </summary>
    private WaitForSeconds waitVisualFlash;

    /// <summary>
    /// Interval in seconds between target search attempts when idle.
    /// </summary>
    private readonly float searchRate = 0.2f;

    /// <summary>
    /// Initializes cached data such as wait instructions and the AudioSource reference.
    /// </summary>
    private void Awake()
    {
        if (stats == null)
        {
            Debug.LogError($"[ERROR] Stats missing on object: {gameObject.name}");
            return;
        }

        if (debugMode) Debug.Log($"[Awake] Initializing memory for: {gameObject.name}");

        waitCooldown = new WaitForSeconds(stats.cooldown);
        waitSearch = new WaitForSeconds(searchRate);
        waitVisualFlash = new WaitForSeconds(0.2f);
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) 
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    /// <summary>
    /// Resets health and visuals when the plant is enabled and starts its combat routine.
    /// </summary>
    private void OnEnable()
    {
        if (stats != null)
        {
            if (debugMode) Debug.Log($"[OnEnable] Plant ACTIVATED: {gameObject.name}");

            currentHealth = stats.maxHealth;
            ResetVisuals();
            OnPlantActivated?.Invoke(this);
            StartCoroutine(CombatRoutine());
        }
    }

    /// <summary>
    /// Stops coroutines and raises the deactivation event when the plant is disabled.
    /// </summary>
    private void OnDisable()
    {
        if (debugMode) Debug.Log($"[OnDisable] Plant DEACTIVATED: {gameObject.name}");
        OnPlantDeactivated?.Invoke(this);
        StopAllCoroutines();
    }

    /// <summary>
    /// Main combat loop that periodically executes the configured strategy,
    /// handles attack cooldowns, and falls back to search delays.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    private IEnumerator CombatRoutine()
    {
        // Small random offset to desynchronize multiple plants.
        yield return new WaitForSeconds(Random.Range(0f, 0.2f));

        while (true)
        {
            if (strategy != null)
            {
                bool hasAttacked = strategy.Execute(this);

                if (hasAttacked)
                {
                    if (debugMode) Debug.Log($"<color=green>[Combat] {gameObject.name} ATTACKED successfully!</color>");

                    if (stats.attackSound != null && audioSource != null)
                    {
                        audioSource.pitch = Random.Range(0.9f, 1.1f);
                        audioSource.PlayOneShot(stats.attackSound);
                    }
                    StartCoroutine(VisualFlashRoutine());

                    yield return waitCooldown;
                }
                else
                {
                    yield return waitSearch;
                }
            }
            else
            {
                if (debugMode) Debug.LogWarning($"[Warning] {gameObject.name} has NO STRATEGY assigned!");
                yield return null;
            }
        }
    }

    /// <summary>
    /// Performs a short squash-and-stretch style visual effect and
    /// briefly swaps to the attack model before resetting visuals.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    private IEnumerator VisualFlashRoutine()
    {
        if (modelNormal == null) yield break;

        float shrinkDuration = 0.05f;
        float growDuration = 0.05f;

        Vector3 originalScale = modelNormal.transform.localScale;
        Vector3 tinyScale = originalScale * 0.7f;

        GameObject currentModel = GetActiveModel();
        yield return StartCoroutine(AnimateScale(currentModel.transform, originalScale, tinyScale, shrinkDuration));

        GameObject newModel = SwitchToAttackModel();

        newModel.transform.localScale = tinyScale;

        yield return StartCoroutine(AnimateScale(newModel.transform, tinyScale, originalScale, growDuration));

        yield return new WaitForSeconds(0.1f);

        ResetVisuals();

        modelNormal.transform.localScale = originalScale;
    }

    /// <summary>
    /// Returns the currently active visual model (normal or attack).
    /// </summary>
    /// <returns>The active model GameObject.</returns>
    private GameObject GetActiveModel()
    {
        if (modelAttack != null && modelAttack.activeSelf) return modelAttack;
        return modelNormal;
    }

    /// <summary>
    /// Switches from the normal model to the attack model if available,
    /// and returns the new active model.
    /// </summary>
    /// <returns>The model used during the attack phase.</returns>
    private GameObject SwitchToAttackModel()
    {
        modelNormal.SetActive(false);

        if (modelAttack != null)
        {
            modelAttack.SetActive(true);
            return modelAttack;
        }

        modelNormal.SetActive(true);
        return modelNormal;
    }

    /// <summary>
    /// Animates a scale transition on the given transform over a specified duration.
    /// </summary>
    /// <param name="target">Transform to scale.</param>
    /// <param name="start">Initial local scale.</param>
    /// <param name="end">Final local scale.</param>
    /// <param name="duration">Duration of the animation in seconds.</param>
    /// <returns>Coroutine enumerator.</returns>
    private IEnumerator AnimateScale(Transform target, Vector3 start, Vector3 end, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            target.localScale = Vector3.Lerp(start, end, progress);
            yield return null;
        }
        target.localScale = end;
    }

    /// <summary>
    /// Restores the default visual state, enabling the normal model
    /// and disabling the attack model.
    /// </summary>
    private void ResetVisuals()
    {
        if (modelNormal != null) modelNormal.SetActive(true);
        if (modelAttack != null) modelAttack.SetActive(false);
    }

    /// <summary>
    /// Applies damage to the plant and checks for death when health reaches zero.
    /// </summary>
    /// <param name="amount">Amount of damage to subtract from current health.</param>
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (debugMode) Debug.Log($"<color=red>[Damage] {gameObject.name} took {amount} dmg. HP: {currentHealth}</color>");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handles plant death logic, including logging and disabling the GameObject
    /// so that it can be returned to a pool if needed.
    /// </summary>
    private void Die()
    {
        if (debugMode) Debug.Log($"<color=red><b>[DEATH] {gameObject.name} has died. Returning to pool.</b></color>");
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Draws a gizmo in the editor to visualize the plant's attack range.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (stats != null && attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, stats.range);
        }
    }
}