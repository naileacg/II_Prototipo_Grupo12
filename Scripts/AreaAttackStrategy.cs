using UnityEngine;

/// <summary>
/// Strategy that performs an area-of-effect attack, damaging all valid
/// targets within a configurable radius around the plant's attack point.
/// </summary>
[CreateAssetMenu(fileName = "AreaStrategy", menuName = "Plants/Strategies/Area Attack")]
public class AreaAttackStrategy : PlantStrategy
{
    /// <summary>
    /// Radius of the area effect used to detect and damage nearby targets.
    /// </summary>
    [Header("Area Configuration")]
    public float effectRadius = 2.5f;

    /// <summary>
    /// Executes the area attack by querying nearby colliders and
    /// applying damage to all detected <see cref="IDamageable"/> targets.
    /// </summary>
    /// <param name="controller">The plant controller that owns this strategy.</param>
    /// <returns>
    /// True if at least one valid target was hit and damaged; otherwise, false.
    /// </returns>
    public override bool Execute(PlantController controller)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(
            controller.attackPoint.position,
            effectRadius,
            controller.hitBuffer,
            targetLayer
        );

        if (hitCount == 0) return false;

        bool appliedDamage = false;

        for (int i = 0; i < hitCount; i++)
        {
            Collider hit = controller.hitBuffer[i];

            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(controller.stats.damage);
                appliedDamage = true;
            }
        }

        if (appliedDamage && controller.particles != null)
        {
            controller.particles.Play();
        }

        return appliedDamage;
    }
}
