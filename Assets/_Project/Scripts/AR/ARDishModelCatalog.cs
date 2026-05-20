using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ARDishModelCatalog",
    menuName = "Italiannis AR/AR Dish Model Catalog")]
public class ARDishModelCatalog : ScriptableObject
{
    [SerializeField] private ARDishModelEntry[] entries;

    public GameObject GetPrefabByDishId(string dishId)
    {
        if (string.IsNullOrWhiteSpace(dishId))
        {
            return null;
        }

        foreach (ARDishModelEntry entry in entries)
        {
            if (entry != null && entry.DishId == dishId)
            {
                return entry.Prefab;
            }
        }

        return null;
    }
}

[Serializable]
public class ARDishModelEntry
{
    [SerializeField] private string dishId;
    [SerializeField] private GameObject prefab;

    public string DishId => dishId;
    public GameObject Prefab => prefab;
}