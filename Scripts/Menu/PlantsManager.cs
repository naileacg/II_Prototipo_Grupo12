using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Central manager that tracks all plant instances in the scene,
/// maintaining separate lists for active and inactive plants and
/// providing utility methods to query and revive them.
/// </summary>
public class PlantsManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance used for global access to the plants manager.
    /// </summary>
    public static PlantsManager Instance { get; private set; }

    /// <summary>
    /// List of plants that are currently active in the scene.
    /// </summary>
    [Header("Monitor (Solo lectura)")]
    [SerializeField] private List<PlantController> activePlants = new List<PlantController>();

    /// <summary>
    /// List of plants that currently exist but are inactive (e.g. dead or disabled).
    /// </summary>
    [SerializeField] private List<PlantController> inactivePlants = new List<PlantController>();

    /// <summary>
    /// Initializes the singleton instance, populates plant lists
    /// and subscribes to plant activation/deactivation events.
    /// </summary>
    private void Awake()
    {
        // Basic singleton pattern for easy access from other scripts.
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Initialization: find ALL plants in the scene, including inactive ones.
        // FindObjectsInactive.Include is required to detect those that start disabled.
        var allPlants = FindObjectsByType<PlantController>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var plant in allPlants)
        {
            if (plant.gameObject.activeInHierarchy)
            {
                if (!activePlants.Contains(plant)) activePlants.Add(plant);
            }
            else
            {
                if (!inactivePlants.Contains(plant)) inactivePlants.Add(plant);
            }
        }

        // Subscribe to static events to keep the lists automatically updated.
        PlantController.OnPlantActivated += HandlePlantActivated;
        PlantController.OnPlantDeactivated += HandlePlantDeactivated;
    }

    /// <summary>
    /// Unsubscribes from static plant events when the manager is destroyed
    /// to avoid memory leaks and dangling references.
    /// </summary>
    private void OnDestroy()
    {
        PlantController.OnPlantActivated -= HandlePlantActivated;
        PlantController.OnPlantDeactivated -= HandlePlantDeactivated;
    }

    // ---------------------------------------------------------
    // EVENT HANDLERS (Automatic list management)
    // ---------------------------------------------------------

    /// <summary>
    /// Moves the plant from inactive to active list when it is activated.
    /// </summary>
    /// <param name="plant">The plant that became active.</param>
    private void HandlePlantActivated(PlantController plant)
    {
        if (inactivePlants.Contains(plant)) inactivePlants.Remove(plant);
        if (!activePlants.Contains(plant)) activePlants.Add(plant);
    }

    /// <summary>
    /// Moves the plant from active to inactive list when it is deactivated.
    /// </summary>
    /// <param name="plant">The plant that became inactive.</param>
    private void HandlePlantDeactivated(PlantController plant)
    {
        if (activePlants.Contains(plant)) activePlants.Remove(plant);
        if (!inactivePlants.Contains(plant)) inactivePlants.Add(plant);
    }

    // ---------------------------------------------------------
    // PUBLIC API (Functions to use in gameplay)
    // ---------------------------------------------------------

    /// <summary>
    /// Gets the list of currently active plants.
    /// </summary>
    /// <returns>List of active <see cref="PlantController"/> instances.</returns>
    public List<PlantController> GetActivePlants()
    {
        return activePlants;
    }

    /// <summary>
    /// Gets the list of currently inactive plants.
    /// </summary>
    /// <returns>List of inactive <see cref="PlantController"/> instances.</returns>
    public List<PlantController> GetInactivePlants()
    {
        return inactivePlants;
    }

    /// <summary>
    /// Calculates the total cost by summing the cost of all plants.
    /// </summary>
    /// <returns>Sum of the <c>cost</c> field from each plant's stats.</returns>
    public int GetTotalCost()
    {
        int total = 0;
        foreach (var plant in inactivePlants)
        {
            if (plant.stats != null)
            {
                total += plant.stats.cost;
            }
        }
        return total;
    }

    /// <summary>
    /// Activates a specific plant by enabling its GameObject.
    /// </summary>
    /// <param name="plant">The plant to activate.</param>
    public void ActivatePlant(PlantController plant)
    {
        if (plant == null) return;
        plant.gameObject.SetActive(true);
    }

    /// <summary>
    /// Attempts to revive a random plant from the inactive list.
    /// Logs a warning if no inactive plants are available.
    /// </summary>
    public void ActivateRandomInactivePlant()
    {
        if (inactivePlants.Count == 0)
        {
            Debug.LogWarning("PlantsManager: No quedan plantas inactivas para revivir.");
            return;
        }

        int index = Random.Range(0, inactivePlants.Count);
        ActivatePlant(inactivePlants[index]);
    }

    // ---------------------------------------------------------
    // DEBUG TOOLS
    // ---------------------------------------------------------

    /// <summary>
    /// Logs a detailed report of active and inactive plants to the console.
    /// Use the context menu on this component to execute it in the editor.
    /// </summary>
    [ContextMenu("DEBUG: Mostrar Nombres en Consola")]
    public void DebugLogAllPlantNames()
    {
        Debug.Log($"<color=cyan>--- REPORTE DE PLANTS MANAGER ---</color>");

        Debug.Log($"<color=green><b>PLANTAS ACTIVAS ({activePlants.Count}): Coste Total: {GetTotalCost()}</b></color>");
        if (activePlants.Count == 0) Debug.Log(" - Ninguna -");
        foreach (var plant in activePlants)
        {
            if (plant != null)
                Debug.Log($" -> [VIVA] {plant.name} (HP: {plant.stats.maxHealth})");
        }

        Debug.Log($"<color=red><b>PLANTAS INACTIVAS ({inactivePlants.Count}):</b></color>");
        if (inactivePlants.Count == 0) Debug.Log(" - Ninguna -");
        foreach (var plant in inactivePlants)
        {
            if (plant != null)
                Debug.Log($" -> [MUERTA] {plant.name}");
        }

        Debug.Log("-------------------------------------------");
    }
}
