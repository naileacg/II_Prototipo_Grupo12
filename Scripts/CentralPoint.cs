using System;
using UnityEngine;

/// <summary>
/// Represents a central target point that can be hit by cubes.
/// After receiving a certain number of hits it is removed from the game,
/// notifying any listeners through the <see cref="onDestroyed"/> event.
/// </summary>
public class CentralPoint : MonoBehaviour
{
    /// <summary>
    /// Maximum number of hits this point can receive before being destroyed.
    /// </summary>
    public int maxHits = 3;

    /// <summary>
    /// Current number of hits this point has received.
    /// </summary>
    private int currentHits = 0;

    /// <summary>
    /// Event invoked when this central point is destroyed.
    /// The spawner subscribes to this event to update its list of active targets.
    /// </summary>
    public Action<CentralPoint> onDestroyed;

    /// <summary>
    /// Position used to hide the central point before destroying it,
    /// keeping it out of view and raycasts.
    /// </summary>
    private Vector3 hiddenPosition = new Vector3(0, -100f, 0);

    /// <summary>
    /// Registers a hit from a cube. When the number of hits reaches
    /// <see cref="maxHits"/>, the point is hidden, the destruction event
    /// is raised and the object is destroyed.
    /// </summary>
    public void HitByCube()
    {
        currentHits++;

        if (currentHits >= maxHits)
        {
            onDestroyed?.Invoke(this);

            // Move the central point out of view / raycasts before destroying it.
            transform.position = hiddenPosition;

            Destroy(gameObject);
        }
    }
}