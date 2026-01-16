using UnityEngine;

/// <summary>
/// Simple helper component that announces itself in the console on startup,
/// providing information about available debug controls for the plants system.
/// </summary>
public class PlantsManagerTester : MonoBehaviour
{
    /// <summary>
    /// Logs basic usage instructions for testing the PlantsManager
    /// when the scene starts.
    /// </summary>
    void Start()
    {
        Debug.Log("<color=yellow>[TESTER] PlantsManagerTester listo.</color>");
        Debug.Log("Controles: [I] Información, [R] Revivir Random, [D] Debug Completo");
    }
}
