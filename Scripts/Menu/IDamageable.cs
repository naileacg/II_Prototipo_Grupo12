/// <summary>
/// Interface for objects that can receive damage, allowing generic
/// damage systems to interact with any implementing component.
/// </summary>
public interface IDamageable 
{
    /// <summary>
    /// Applies damage to the implementing object.
    /// </summary>
    /// <param name="damage">Amount of damage to inflict.</param>
    void TakeDamage(float damage);
}