using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("References")]
    public GameObject prefab;
    public Transform[] spawnPoints;
    public Transform[] midPoints;

    [Header("Settings")]
    /// <summary>
    /// Filtro de capa: Selecciona "Plants" aquÃ­ en el inspector.
    /// Solo las plantas en esta capa serÃ¡n objetivos.
    /// </summary>
    public LayerMask plantLayer;

    [SerializeField]
    private static List<PlantController> activePoints = new List<PlantController>();

    public event Action OnGameOver;

    private void Awake()
    {
        // 1. LIMPIEZA INICIAL
        activePoints.Clear();
        PlantController.OnPlantActivated += HandlePlantActivated;
        PlantController.OnPlantDeactivated += HandlePlantDeactivated;

        // 2. BARRIDO INICIAL DE LA ESCENA
        // Buscamos todos los PlantController que ya existen en la escena al dar Play.
        PlantController[] existingPlants = FindObjectsByType<PlantController>(FindObjectsSortMode.None);

        foreach (var plant in existingPlants)
        {
            // Solo las registramos si estÃ¡n en la capa correcta ("Plants")
            if (IsOnTargetLayer(plant.gameObject))
            {
                RegisterPlant(plant);
            }
        }
    }

    private void OnDisable()
    {
        PlantController.OnPlantActivated -= HandlePlantActivated;
        PlantController.OnPlantDeactivated -= HandlePlantDeactivated;
    }

    // -------------------------------------------------------------------
    // GESTIÓN DE EVENTOS Y FILTRO DE CAPAS
    // -------------------------------------------------------------------

    private void HandlePlantActivated(PlantController plant)
    {
        if (IsOnTargetLayer(plant.gameObject))
        {
            RegisterPlant(plant);
        }
    }

    private void HandlePlantDeactivated(PlantController plant)
    {
        UnregisterPlant(plant);
    }

    private void RegisterPlant(PlantController plant)
    {
        if (!activePoints.Contains(plant))
        {
            activePoints.Add(plant);
        }
    }

    private void UnregisterPlant(PlantController plant)
    {
        if (activePoints.Contains(plant))
        {
            activePoints.Remove(plant);
        }

        if (activePoints.Count == 0)
        {
            OnGameOver?.Invoke();
        }
    }

    /// <summary>
    /// Comprueba si el GameObject pertenece a la LayerMask definida.
    /// </summary>
    private bool IsOnTargetLayer(GameObject obj)
    {
        // OperaciÃ³n bitwise para comprobar si la layer del objeto estÃ¡ dentro de la mÃ¡scara
        return (plantLayer.value & (1 << obj.layer)) > 0;
    }

    // -------------------------------------------------------------------
    // LOGICA DE SPAWN (Sin cambios)
    // -------------------------------------------------------------------

    public GameObject SpawnOne()
    {
        if (spawnPoints.Length == 0 || activePoints.Count == 0) return null;

        var spawn = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        var obj = Instantiate(prefab, spawn.position, spawn.rotation);

        var mover = obj.GetComponent<MoveToCenter>();
        if (mover != null)
        {
            Transform finalTarget = GetClosestActivePoint(spawn.position);
            Transform mid = null;

            if (midPoints != null && midPoints.Length > 0)
            {
                mid = midPoints[UnityEngine.Random.Range(0, midPoints.Length)];
            }

            if (mid != null) mover.Init(mid, finalTarget);
            else mover.Init(finalTarget);
        }

        return obj;
    }

    public static Transform GetRandomActivePoint(int index = 100)
    {
        if (activePoints.Count == 0) return null;
        if (index < 0) index = 0;
        if (index >= activePoints.Count) index = activePoints.Count - 1;
        int randomIndex = UnityEngine.Random.Range(0, index);
        return activePoints[randomIndex].transform;
    }

    public Transform GetClosestActivePoint(Vector3 fromPos)
    {
        Transform closest = null;
        float bestDistSqr = float.MaxValue;

        foreach (var p in activePoints)
        {
            if (p == null) continue;

            float d = (p.transform.position - fromPos).sqrMagnitude;
            if (d < bestDistSqr)
            {
                bestDistSqr = d;
                closest = p.transform;
            }
        }
        return closest;
    }


}