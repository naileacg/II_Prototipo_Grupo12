using UnityEngine;

/// <summary>
/// Damageable component for cube enemies, handling health, death logic
/// and delegating reward behavior to an attached <see cref="CubeReward"/>.
/// </summary>
public class CubeDamagable : MonoBehaviour, IDamageable
{
    /// <summary>
    /// Maximum health value for this cube.
    /// </summary>
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    /// <summary>
    /// Current health value of the cube.
    /// </summary>
    private float currentHealth;

    /// <summary>
    /// Cached reference to the reward handler used when the cube dies.
    /// </summary>
    private CubeReward cubeReward;

    /// <summary>
    /// Initializes health and caches the <see cref="CubeReward"/> component.
    /// </summary>
    private void Awake()
    {
        cubeReward = GetComponent<CubeReward>();
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Resets health when the cube becomes active again (e.g. from a pool).
    /// </summary>
    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Applies damage to the cube and triggers death when health reaches zero.
    /// </summary>
    /// <param name="damage">Amount of damage to subtract from current health.</param>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log($"[Cube] {name} recibió {damage} daño. Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handles cube death, ensuring it is processed only once and delegating
    /// to <see cref="CubeReward"/> if present, otherwise destroying the GameObject.
    /// </summary>
    private void Die()
    {
        // Prevent multiple death processing using a sentinel value.
        if (currentHealth <= -1000) return;
        currentHealth = -1000;

        if (cubeReward != null)
        {
            cubeReward.KillByPlayer();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}