using System.Collections.Generic;
using UnityEngine;

public class RuntimeAppState : MonoBehaviour
{
    public static RuntimeAppState Instance { get; private set; }

    public string SelectedDishId { get; private set; }
    public string SelectedDishName { get; private set; }
    public string SelectedDishMenuSection { get; private set; }
    public string SelectedDishCategory { get; private set; }
    public string SelectedDishSubCategory { get; private set; }
    public string SelectedDishIcon { get; private set; }
    public string SelectedDishDescription { get; private set; }
    public string SelectedDishStory { get; private set; }

    public string SelectedDishPortion { get; private set; }
    public string SelectedDishCalories { get; private set; }

    public bool SelectedDishHasAR { get; private set; }
    public bool SelectedDishRecommended { get; private set; }
    public bool SelectedDishIsAlcoholic { get; private set; }
    public bool SelectedDishIsKidsMenu { get; private set; }
    public bool SelectedDishIsShareable { get; private set; }
    public bool SelectedDishIsVegetarianFriendly { get; private set; }

    public string SelectedDishIngredientsText { get; private set; }
    public string SelectedDishPreparationText { get; private set; }
    public string SelectedDishNutritionText { get; private set; }
    public string SelectedDishWarningsText { get; private set; }
    public string SelectedDishAllergensText { get; private set; }
    public string SelectedDishTagsText { get; private set; }
    public string SelectedDishPairingsText { get; private set; }

    public string SelectedDishImageResourcePath { get; private set; }
    public string SelectedDishModelPrefabPath { get; private set; }
    public string SelectedDishTripoPrompt { get; private set; }

    public string SavedMenuSelectedDishId { get; private set; }
    public string SavedMenuCategory { get; private set; }
    public string SavedMenuSectionFilter { get; private set; }

    public bool ShouldReturnToMenuAfterAR { get; private set; }

    public List<string> CurrentOrderDishIds { get; } = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSelectedDish(DishData dish)
    {
        if (dish == null)
        {
            Debug.LogWarning("[RuntimeAppState] No se puede guardar un platillo null.");
            return;
        }

        SelectedDishId = dish.Id;
        SelectedDishName = dish.Name;
        SelectedDishMenuSection = dish.MenuSection;
        SelectedDishCategory = dish.Category;
        SelectedDishSubCategory = dish.SubCategory;
        SelectedDishIcon = dish.Icon;
        SelectedDishDescription = dish.Description;
        SelectedDishStory = dish.Story;

        SelectedDishPortion = dish.Portion;
        SelectedDishCalories = dish.Calories;

        SelectedDishHasAR = dish.HasAR;
        SelectedDishRecommended = dish.Recommended;
        SelectedDishIsAlcoholic = dish.IsAlcoholic;
        SelectedDishIsKidsMenu = dish.IsKidsMenu;
        SelectedDishIsShareable = dish.IsShareable;
        SelectedDishIsVegetarianFriendly = dish.IsVegetarianFriendly;

        SelectedDishIngredientsText = dish.GetIngredientsText();
        SelectedDishPreparationText = dish.GetPreparationText();
        SelectedDishNutritionText = dish.GetNutritionText();
        SelectedDishWarningsText = dish.GetWarningsText();
        SelectedDishAllergensText = dish.GetAllergensText();
        SelectedDishTagsText = dish.GetTagsText();
        SelectedDishPairingsText = dish.GetPairingsText();

        SelectedDishImageResourcePath = dish.ImageResourcePath;
        SelectedDishModelPrefabPath = dish.ModelPrefabPath;
        SelectedDishTripoPrompt = dish.TripoPrompt;

        Debug.Log($"[RuntimeAppState] Platillo seleccionado para RA: {SelectedDishName}");
    }

    public void SaveMenuState(KioskAppState appState)
    {
        if (appState == null)
        {
            return;
        }

        SavedMenuSelectedDishId = appState.SelectedDish != null
            ? appState.SelectedDish.Id
            : null;

        SavedMenuCategory = appState.CurrentCategory;
        SavedMenuSectionFilter = appState.CurrentSectionFilter;

        SetOrder(appState.Order);

        Debug.Log("[RuntimeAppState] Estado del menú guardado antes de entrar a RA.");
    }

    public void SetOrder(List<DishData> order)
    {
        CurrentOrderDishIds.Clear();

        if (order == null)
        {
            return;
        }

        foreach (DishData dish in order)
        {
            if (dish != null)
            {
                CurrentOrderDishIds.Add(dish.Id);
            }
        }
    }

    public void AddToOrder(DishData dish)
    {
        if (dish == null)
        {
            Debug.LogWarning("[RuntimeAppState] No se puede agregar un platillo null a la orden.");
            return;
        }

        CurrentOrderDishIds.Add(dish.Id);
        Debug.Log($"[RuntimeAppState] Platillo agregado a orden temporal: {dish.Name}. Total={CurrentOrderDishIds.Count}");
    }

    public void AddDishToOrder(DishData dish)
    {
        AddToOrder(dish);
    }

    public int GetCurrentOrderCount()
    {
        return CurrentOrderDishIds.Count;
    }

    public void MarkReturnToMenuAfterAR()
    {
        if (HasSelectedDish())
        {
            SavedMenuSelectedDishId = SelectedDishId;
        }

        ShouldReturnToMenuAfterAR = true;
    }

    public bool ConsumeReturnToMenuAfterAR()
    {
        if (!ShouldReturnToMenuAfterAR)
        {
            return false;
        }

        ShouldReturnToMenuAfterAR = false;
        return true;
    }

    public bool HasSelectedDish()
    {
        return !string.IsNullOrWhiteSpace(SelectedDishId);
    }

    public void ClearSelectedDish()
    {
        SelectedDishId = null;
        SelectedDishName = null;
        SelectedDishMenuSection = null;
        SelectedDishCategory = null;
        SelectedDishSubCategory = null;
        SelectedDishIcon = null;
        SelectedDishDescription = null;
        SelectedDishStory = null;

        SelectedDishPortion = null;
        SelectedDishCalories = null;

        SelectedDishHasAR = false;
        SelectedDishRecommended = false;
        SelectedDishIsAlcoholic = false;
        SelectedDishIsKidsMenu = false;
        SelectedDishIsShareable = false;
        SelectedDishIsVegetarianFriendly = false;

        SelectedDishIngredientsText = null;
        SelectedDishPreparationText = null;
        SelectedDishNutritionText = null;
        SelectedDishWarningsText = null;
        SelectedDishAllergensText = null;
        SelectedDishTagsText = null;
        SelectedDishPairingsText = null;

        SelectedDishImageResourcePath = null;
        SelectedDishModelPrefabPath = null;
        SelectedDishTripoPrompt = null;
    }

    public void ClearMenuSnapshot()
    {
        SavedMenuSelectedDishId = null;
        SavedMenuCategory = null;
        SavedMenuSectionFilter = null;
        CurrentOrderDishIds.Clear();
        ShouldReturnToMenuAfterAR = false;
    }
}
