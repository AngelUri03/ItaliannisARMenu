using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class RecommendationsViewController
{
    private readonly VisualElement root;
    private readonly KioskAppState appState;

    private readonly Action onBackMenu;
    private readonly Action<DishData> onOpenDishDetail;

    private readonly List<Button> recommendationFilterButtons = new List<Button>();

    private VisualElement recommendationList;
    private Label recommendationTitleLabel;
    private Label recommendationDescriptionLabel;

    public RecommendationsViewController(
        VisualElement root,
        KioskAppState appState,
        Action onBackMenu,
        Action<DishData> onOpenDishDetail)
    {
        this.root = root;
        this.appState = appState;
        this.onBackMenu = onBackMenu;
        this.onOpenDishDetail = onOpenDishDetail;
    }

    public void Bind()
    {
        recommendationList = root.Q<VisualElement>("recommendation-list");
        recommendationTitleLabel = root.Q<Label>("recommendation-title-label");
        recommendationDescriptionLabel = root.Q<Label>("recommendation-description-label");

        Button classicButton = root.Q<Button>("recommend-classic-button");
        Button shareButton = root.Q<Button>("recommend-share-button");
        Button pastaButton = root.Q<Button>("recommend-pasta-button");
        Button dessertButton = root.Q<Button>("recommend-dessert-button");
        Button arButton = root.Q<Button>("recommend-ar-button");
        Button backMenuButton = root.Q<Button>("recommendations-back-menu-button");

        recommendationFilterButtons.Clear();

        RegisterRecommendationFilter(classicButton, "classic");
        RegisterRecommendationFilter(shareButton, "share");
        RegisterRecommendationFilter(pastaButton, "pasta");
        RegisterRecommendationFilter(dessertButton, "dessert");
        RegisterRecommendationFilter(arButton, "ar");

        if (backMenuButton != null)
        {
            backMenuButton.clicked += onBackMenu;
        }
    }

    public void Initialize(string initialGroup)
    {
        ShowRecommendationGroup(initialGroup);
        MarkInitialFilter(initialGroup);
    }

    private void RegisterRecommendationFilter(Button button, string group)
    {
        if (button == null)
        {
            return;
        }

        recommendationFilterButtons.Add(button);

        button.clicked += () =>
        {
            MarkSelectedRecommendationFilter(button);
            ShowRecommendationGroup(group);
        };
    }

    private void MarkInitialFilter(string group)
    {
        foreach (Button button in recommendationFilterButtons)
        {
            button.RemoveFromClassList("selected-filter");
        }

        string expectedText = GetFilterButtonText(group);

        Button selectedButton = recommendationFilterButtons.FirstOrDefault(button => button.text == expectedText);

        if (selectedButton != null)
        {
            selectedButton.AddToClassList("selected-filter");
        }
    }

    private string GetFilterButtonText(string group)
    {
        switch (group)
        {
            case "share":
                return "Para compartir";

            case "pasta":
                return "Pasta";

            case "dessert":
                return "Postre";

            case "ar":
                return "Ver solo AR";

            case "classic":
            default:
                return "Algo clásico";
        }
    }

    private void MarkSelectedRecommendationFilter(Button selectedButton)
    {
        foreach (Button button in recommendationFilterButtons)
        {
            button.RemoveFromClassList("selected-filter");
        }

        selectedButton.AddToClassList("selected-filter");
    }

    private void ShowRecommendationGroup(string group)
    {
        if (recommendationList == null)
        {
            Debug.LogWarning("[RecommendationsViewController] recommendation-list es null.");
            return;
        }

        List<DishData> recommendedDishes = GetRecommendedDishesByGroup(group);

        UpdateRecommendationHeader(group);

        recommendationList.Clear();

        foreach (DishData dish in recommendedDishes)
        {
            recommendationList.Add(CreateRecommendationCard(dish));
        }
    }

    private List<DishData> GetRecommendedDishesByGroup(string group)
    {
        switch (group)
        {
            case "share":
                return appState.Dishes
                    .Where(dish => dish.IsShareable || HasTag(dish, "Compartir") || dish.Category == "Abbondanza")
                    .ToList();

            case "pasta":
                return appState.Dishes
                    .Where(dish => dish.Category == "Pasta" || dish.Category == "Pasta Ripiena")
                    .ToList();

            case "dessert":
                return appState.Dishes
                    .Where(dish => dish.Category == "Postres")
                    .ToList();

            case "ar":
                return appState.Dishes
                    .Where(dish => dish.HasAR)
                    .ToList();

            case "classic":
            default:
                return appState.Dishes
                    .Where(dish => dish.Recommended)
                    .ToList();
        }
    }

    private bool HasTag(DishData dish, string tag)
    {
        if (dish == null || dish.Tags == null)
        {
            return false;
        }

        return dish.Tags.Any(dishTag => dishTag == tag);
    }

    private void UpdateRecommendationHeader(string group)
    {
        if (recommendationTitleLabel == null || recommendationDescriptionLabel == null)
        {
            return;
        }

        switch (group)
        {
            case "share":
                recommendationTitleLabel.text = "Para compartir";
                recommendationDescriptionLabel.text = "Platillos pensados para mesa, porciones grandes o experiencia familiar.";
                break;

            case "pasta":
                recommendationTitleLabel.text = "Pastas recomendadas";
                recommendationDescriptionLabel.text = "Pastas largas, rellenas, cremosas, horneadas o clásicas italianas.";
                break;

            case "dessert":
                recommendationTitleLabel.text = "Postres destacados";
                recommendationDescriptionLabel.text = "Opciones dulces para cerrar la experiencia de mesa.";
                break;

            case "ar":
                recommendationTitleLabel.text = "Platillos con RA";
                recommendationDescriptionLabel.text = "Platillos disponibles para visualizar en Realidad Aumentada.";
                break;

            case "classic":
            default:
                recommendationTitleLabel.text = "Algo clásico";
                recommendationDescriptionLabel.text = "Platillos recomendados por su popularidad, presentación o valor para demo.";
                break;
        }
    }

    private VisualElement CreateRecommendationCard(DishData dish)
    {
        VisualElement card = new VisualElement();
        card.AddToClassList("recommendation-card");

        VisualElement top = new VisualElement();
        top.AddToClassList("recommendation-card-top");

        Label icon = new Label(dish.Icon);
        icon.AddToClassList("recommendation-icon");

        Label badge = new Label(dish.HasAR ? "AR" : "Menú");
        badge.AddToClassList("recommendation-badge");

        top.Add(icon);
        top.Add(badge);

        Label name = new Label(dish.Name);
        name.AddToClassList("recommendation-name");

        Label description = new Label(dish.Description);
        description.AddToClassList("recommendation-description");

        Label meta = new Label($"{dish.Category} · {dish.Portion}");
        meta.AddToClassList("recommendation-meta");

        Button actionButton = new Button
        {
            text = "Ver detalle"
        };

        actionButton.AddToClassList("recommendation-card-action");

        actionButton.clicked += () =>
        {
            OpenDishFromRecommendation(dish);
        };

        card.Add(top);
        card.Add(name);
        card.Add(description);
        card.Add(meta);
        card.Add(actionButton);

        return card;
    }

    private void OpenDishFromRecommendation(DishData dish)
    {
        appState.SelectedDish = dish;
        appState.CurrentCategory = dish.Category;

        Debug.Log($"[RecommendationsViewController] Platillo recomendado seleccionado: {dish.Name}");

        onOpenDishDetail?.Invoke(dish);
    }
}