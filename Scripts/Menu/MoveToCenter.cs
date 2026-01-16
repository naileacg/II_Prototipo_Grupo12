using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Moves the cube towards a target central point with an optional
/// lateral wobble effect and triggers events on arrival.
/// Supports an optional intermediate waypoint before the final target.
/// </summary>
public class MoveToCenter : MonoBehaviour
{
    /// <summary>
    /// Forward movement speed in units per second.
    /// </summary>
    public float speed = 3f;
    public float damage = 10f;

    /// <summary>
    /// Distance threshold used to consider that the cube
    /// has reached its target.
    /// </summary>
    public float arriveDistance = 0.5f;

    [Header("Wobble")]
    /// <summary>
    /// Maximum lateral offset amplitude of the wobble effect.
    /// </summary>
    public float wobbleAmplitude = 0.5f;

    /// <summary>
    /// Frequency of the wobble oscillation in radians per second.
    /// </summary>
    public float wobbleFrequency = 3f;

    /// <summary>
    /// Phase offset used to desynchronize the wobble between cubes.
    /// </summary>
    public float wobblePhaseOffset = 0f;

    /// <summary>
    /// Event invoked when the cube reaches its final target.
    /// </summary>
    public UnityEvent onArrived;

    /// <summary>
    /// Current movement target of this cube.
    /// </summary>
    private Transform target;

    /// <summary>
    /// Optional intermediate waypoint this cube moves to first.
    /// </summary>
    private Transform waypoint;

    /// <summary>
    /// Final central point target this cube moves to after the waypoint.
    /// </summary>
    private Transform finalTarget;

    /// <summary>
    /// Whether the cube is currently moving towards the waypoint.
    /// When false, it is moving towards the final target.
    /// </summary>
    private bool goingToWaypoint = false;

    /// <summary>
    /// Initial offset from the target, used to randomize wobble behavior.
    /// </summary>
    private Vector3 initialOffset;

    /// <summary>
    /// Initializes this cube with its final target only (no waypoint).
    /// </summary>
    /// <param name="center">Target transform to move towards.</param>
    public void Init(Transform center)
    {
        waypoint = null;
        finalTarget = center;
        goingToWaypoint = false;

        SetTarget(center);

        // Random phase so cubes do not wobble in sync.
        wobblePhaseOffset = Random.Range(0f, Mathf.PI * 2f);
        initialOffset = transform.position - center.position;
    }

    /// <summary>
    /// Initializes this cube with an intermediate waypoint and a final target.
    /// The cube will move first to the waypoint, then to the final target.
    /// </summary>
    /// <param name="midPoint">Intermediate waypoint to pass through.</param>
    /// <param name="center">Final central point target.</param>
    public void Init(Transform midPoint, Transform center)
    {
        waypoint = midPoint;
        finalTarget = center;
        goingToWaypoint = true;

        SetTarget(waypoint);

        // Random phase so cubes do not wobble in sync.
        wobblePhaseOffset = Random.Range(0f, Mathf.PI * 2f);
        initialOffset = transform.position - waypoint.position;
    }

    /// <summary>
    /// Sets a new movement target for this cube and realigns its rotation.
    /// </summary>
    /// <param name="newTarget">New target transform.</param>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        if (target == null) return;

        // Recalculate initial rotation towards the new target.
        Vector3 toTarget = (target.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(toTarget);
    }

    /// <summary>
    /// Attempts to find a new closest central point using the Spawner
    /// when the current final target is no longer valid.
    /// </summary>
    private void TryRetarget()
    {
        // If we were going to a waypoint and it disappears, skip to final.
        if (goingToWaypoint && finalTarget != null)
        {
            goingToWaypoint = false;
            SetTarget(finalTarget);
            return;
        }

        // Look for a spawner in the scene.
        var spawner = Object.FindAnyObjectByType<Spawner>();
        if (spawner == null) return;

        Transform newTarget = spawner.GetClosestActivePoint(transform.position);
        finalTarget = newTarget;
        goingToWaypoint = false;
        SetTarget(newTarget);
    }

    private void Update()
    {
        // If the current target has been destroyed, try to find another one.
        if (target == null)
        {
            TryRetarget();
            if (target == null) return;
        }

        // 1) Base movement towards the target.
        Vector3 toTarget = (target.position - transform.position).normalized;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        // 2) Smooth rotation towards the target.
        Quaternion targetRotation = Quaternion.LookRotation(toTarget);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * speed * 5f
        );

        // 3) Lateral direction for the wobble (perpendicular to forward).
        Vector3 lateral = Vector3.Cross(toTarget, Vector3.up).normalized;

        // 4) Sinusoidal lateral offset.
        float wobble = Mathf.Sin(Time.time * wobbleFrequency + wobblePhaseOffset) * wobbleAmplitude;
        transform.position += lateral * wobble * Time.deltaTime;

        // 5) Arrival check.
        if (Vector3.Distance(transform.position, target.position) <= arriveDistance)
        {
            // Si estábamos yendo al waypoint, cambiamos al destino final y no destruimos aún.
            if (goingToWaypoint)
            {
                goingToWaypoint = false;
                SetTarget(Spawner.GetRandomActivePoint());
                return;
            }

            // Hemos llegado al destino final.
            onArrived?.Invoke();

            // Notify the central point that it has been hit.
            var central = target.GetComponent<PlantController>();
            if (central != null)
                central.TakeDamage(damage);

            // Automatic cube death (no player kill).
            var reward = GetComponent<CubeReward>();
            if (reward != null)
                reward.KillAuto();
            else
                Destroy(gameObject, 0.2f);
        }
    }
}