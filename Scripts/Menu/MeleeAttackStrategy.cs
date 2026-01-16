using UnityEngine;

/// <summary>
/// Strategy that performs a close-range melee attack using an oriented hitbox,
/// applying damage to all <see cref="IDamageable"/> targets within the box.
/// </summary>
[CreateAssetMenu(fileName = "MeleeStrategy", menuName = "Plants/Strategies/Melee Attack")]
public class MeleeAttackStrategy : PlantStrategy
{
    /// <summary>
    /// Half-size of the melee hitbox used for the OverlapBox query,
    /// defined in local space relative to the attack point.
    /// </summary>
    [Header("Hitbox Configuration")]
    public Vector3 hitBoxSize = new Vector3(0.8f, 1f, 1.5f);

    /// <summary>
    /// Executes the melee attack by casting an oriented box from the plant's
    /// attack point and damaging every hit <see cref="IDamageable"/> target.
    /// </summary>
    /// <param name="controller">The plant controller that owns this strategy.</param>
    /// <returns>
    /// True if at least one valid target was hit and damaged; otherwise, false.
    /// </returns>
    public override bool Execute(PlantController controller)
    {
        int hitCount = Physics.OverlapBoxNonAlloc(
            controller.attackPoint.position,
            hitBoxSize,
            controller.hitBuffer,
            controller.attackPoint.rotation,
            targetLayer
        );

        if (controller.debugMode)
        {
            Debug.Log($"<color=cyan>[FÍSICA] La caja de {controller.name} ha tocado {hitCount} cosas.</color>");
        }

        if (hitCount == 0) return false;

        /// <summary>
        /// Tracks whether at least one target received damage during this attack.
        /// </summary>
        bool appliedDamage = false;

        for (int i = 0; i < hitCount; i++)
        {
            Collider hit = controller.hitBuffer[i];
            if (hit == null) continue;

            if (controller.debugMode)
            {
                Debug.Log($"   -> He tocado: {hit.name} (Tiene Script de Daño: {hit.GetComponent<IDamageable>() != null})");
            }

            IDamageable damageable = hit.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(controller.stats.damage);
                appliedDamage = true;
            }
        }

        return appliedDamage;
    }
}