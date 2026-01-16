using UnityEngine;
using Google.XR.Cardboard;

/// <summary>
/// Allows a cube to be collected by gazing at it with the Cardboard reticle
/// for a configurable amount of time, triggering its death and rewards.
/// </summary>
public class Collectible : MonoBehaviour
{
    /// <summary>
    /// Time in seconds the player must keep gazing at the object
    /// before it is collected.
    /// </summary>
    public float gazeTime = 2.0f;

    /// <summary>
    /// Delay in seconds before destroying the object when no CubeReward
    /// component is present.
    /// </summary>
    public float destroyDelay = 0.5f;

    /// <summary>
    /// Accumulated gaze time while the reticle is over this object.
    /// </summary>
    private float gazeTimer = 0f;

    /// <summary>
    /// Indicates whether the object is currently being gazed at.
    /// </summary>
    private bool isGazed = false;

    /// <summary>
    /// Indicates whether this collectible has already been collected.
    /// </summary>
    private bool collected = false;

    /// <summary>
    /// Called by CardboardReticlePointer when the reticle starts
    /// hovering over this object.
    /// </summary>
    public void OnPointerEnter()
    {
        if (collected) return;

        isGazed = true;
        gazeTimer = 0f;
    }

    /// <summary>
    /// Called by CardboardReticlePointer when the reticle stops
    /// hovering over this object.
    /// </summary>
    public void OnPointerExit()
    {
        if (collected) return;

        isGazed = false;
        gazeTimer = 0f;
    }

    private void Update()
    {
        if (!isGazed || collected) return;

        gazeTimer += Time.deltaTime;
        if (gazeTimer >= gazeTime)
        {
            Collect();
        }
    }

    /// <summary>
    /// Handles the collection logic once the required gaze time is reached:
    /// flags the object as collected and delegates the actual death behavior
    /// to <see cref="CubeReward"/> if available.
    /// </summary>
    private void Collect()
    {
        collected = true;
        isGazed = false;

        var reward = GetComponent<CubeReward>();
        if (reward != null)
            reward.KillByPlayer();
        else
            Destroy(gameObject, destroyDelay);
    }
}