using System;
using UnityEngine;

/// <summary>
/// Reports a cube's death to any subscribed listeners,
/// such as the round or wave manager.
/// </summary>
public class CubeDeathReporter : MonoBehaviour
{
    /// <summary>
    /// Event invoked when this cube is killed.
    /// </summary>
    public Action onKilled;

    /// <summary>
    /// Notifies all subscribers that this cube has died.
    /// Typically called just before the cube is destroyed.
    /// </summary>
    public void ReportDeath()
    {
        onKilled?.Invoke();
    }
}