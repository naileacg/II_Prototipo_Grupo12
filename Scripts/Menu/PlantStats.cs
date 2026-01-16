using UnityEngine;

/// <summary>
/// ScriptableObject that defines configurable stats and metadata
/// for a plant, including cost, health, combat values, icon and audio.
/// </summary>
[CreateAssetMenu(fileName = "NewPlantStats", menuName = "Plants/Stats")]
public class PlantStats : ScriptableObject
{
    /// <summary>
    /// Display name of the plant used in UI and debugging.
    /// </summary>
    [Header("General Info")]
    public string plantName;

    /// <summary>
    /// Resource cost required to place or purchase this plant.
    /// </summary>
    public int cost = 50;

    /// <summary>
    /// Icon sprite used for UI representations of the plant.
    /// </summary>
    public Sprite icon;

    /// <summary>
    /// Maximum health value the plant can have when fully alive.
    /// </summary>
    [Header("Survival")]
    public float maxHealth = 100f;

    /// <summary>
    /// Audio clip played when the plant performs an attack.
    /// </summary>
    [Header("Audio")]
    public AudioClip attackSound;

    /// <summary>
    /// Amount of damage the plant deals per hit or per second,
    /// depending on how the strategy interprets this value.
    /// </summary>
    [Header("Combat & Balance")]
    [Tooltip("Damage dealt per hit OR per second")]
    public float damage = 10f;

    /// <summary>
    /// Time in seconds between consecutive actions or attacks.
    /// </summary>
    [Tooltip("Time in seconds between actions")]
    public float cooldown = 1f;

    /// <summary>
    /// Effective attack range or detection radius for targeting.
    /// </summary>
    [Tooltip("Attack range or detection radius")]
    public float range = 1.5f;
}