using UnityEngine;

/// <summary>
/// Abstract base class for all plant attack/behavior strategies,
/// providing shared configuration such as target layer filtering.
/// </summary>
public abstract class PlantStrategy : ScriptableObject
{
    /// <summary>
    /// Layer mask used to filter which objects can be targeted or damaged.
    /// </summary>
    [Header("Base Settings")]
    public LayerMask targetLayer;

    /// <summary>
    /// Executes the strategy logic for the given plant controller,
    /// typically performing detection and applying damage or effects.
    /// </summary>
    /// <param name="controller">The plant controller executing this strategy.</param>
    /// <returns>
    /// True if the strategy successfully performed an action
    /// (for example, hit at least one valid target); otherwise, false.
    /// </returns>
    public abstract bool Execute(PlantController controller);
}
