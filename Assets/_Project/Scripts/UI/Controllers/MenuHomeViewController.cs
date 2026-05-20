using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuHomeViewController
{
    private const string FilterRecommended = "Recomendados";
    private const string FilterAll = "Todo";
    private const string FilterFood = "Comidas";
    private const string FilterBreakfast = "Desayunos";
    private const string FilterDesserts = "Postres";
    private const string FilterDrinks = "Bebidas";
    private const string FilterKids = "Infantil";

    private readonly VisualElement root;
    private readonly KioskAppState appState;
    private readonly Action onBackWelcome;
    private readonly Action onOpenOrder;
    private readonly Action<DishData> onViewAR;

    private readonly List<Button> sectionFilterButtons = new List<Button>();
    private readonly List<VisualElement> dishCards = new List<VisualElement>();

    private VisualElement dishList;

    private Label currentCategoryLabel;
    private Label currentFilterDescriptionLabel;
    private Label orderCountLabel;
    private Label menuArStatusLabel;

    private Label detailArBadgeLabel;
    private Label detailNameLabel;
    private Label detailCategoryLabel;
    private Label detailDescriptionLabel;
    private Label detailPortionLabel;
    private Label detailCaloriesLabel;

    private TextField dishSearchField;

    private Button viewARButton;
    private Button addOrderButton;
    private Button openDetailModalButton;
    private Button navOrderButton;

    private Button filterRecommendedButton;
    private Button filterAllButton;
    private Button filterFoodButton;
    private Button filterBreakfastButton;
    private Button filterDessertsButton;
    private Button filterDrinksButton;
    private Button filterKidsButton;

    private VisualElement menuToast;
    private Label menuToastTitleLabel;
    private Label menuToastMessageLabel;

    private VisualElement dishDetailModal;
    private Label modalDishNameLabel;
    private Label modalDishMetaLabel;
    private Label modalDishArLabel;
    private Label modalSectionLabel;
    private Label modalSubcategoryLabel;
    private Label modalPortionLabel;
    private Label modalCaloriesLabel;
    private Label modalTagsLabel;
    private Label modalDetailTitleLabel;
    private Label modalDetailBodyLabel;
    private VisualElement dishModalTextCard;

    private Button closeDishModalButton;
    private Button modalCloseButton;
    private Button modalViewArButton;
    private Button modalAddOrderButton;
    private Button modalTabSummaryButton;
    private Button modalTabIngredientsButton;
    private Button modalTabNutritionButton;
    private Button modalTabAllergensButton;
    private Button modalTabARButton;
    private Image modalDishImage;

    private ScrollView dishModalScroll;
    private ScrollView dishScroll;
    private VisualElement dishModelViewerPanel;
    private Label modelViewerTitleLabel;
    private Label modelViewerSubtitleLabel;
    private Image dishModelRenderImage;
    private DishModelPreviewRenderer modelPreviewRenderer;

    private Image detailDishImage;

    public MenuHomeViewController(
        VisualElement root,
        KioskAppState appState,
        Action onBackWelcome,
        Action onOpenOrder,
        Action onOpenRecommendations,
        Action onOpenArCatalog,
        Action<DishData> onViewAR)
    {
        this.root = root;
        this.appState = appState;
        this.onBackWelcome = onBackWelcome;
        this.onOpenOrder = onOpenOrder;
        this.onViewAR = onViewAR;
    }

    public void Bind()
    {
        BindMainElements();
        BindTopFilters();
        BindMainActions();
        BindDishDetailModal();
    }

    public void Initialize(DishData dishToSelect)
    {
        DishData targetDish = dishToSelect ?? appState.SelectedDish;

        if (targetDish != null)
        {
            string targetFilter = GetSectionFilterForDish(targetDish);
            ShowSectionFilter(targetFilter, false, false);
            SelectDish(targetDish);
            MarkDishCardById(targetDish.Id);
        }
        else
        {
            string currentFilter = GetSafeFilter(appState.CurrentSectionFilter);
            ShowSectionFilter(currentFilter, true, false);
        }

        UpdateOrderCount();
    }

    private void BindMainElements()
    {
        dishList = root.Q<VisualElement>("dish-list");
        dishScroll = root.Q<ScrollView>("dish-scroll");

        currentCategoryLabel = root.Q<Label>("current-category-label");
        currentFilterDescriptionLabel = root.Q<Label>("current-filter-description-label");
        orderCountLabel = root.Q<Label>("order-count-label");
        menuArStatusLabel = root.Q<Label>("menu-ar-status-label");

        detailDishImage = root.Q<Image>("detail-dish-image");
        detailArBadgeLabel = root.Q<Label>("detail-ar-badge-label");
        detailNameLabel = root.Q<Label>("detail-name-label");
        detailCategoryLabel = root.Q<Label>("detail-category-label");
        detailDescriptionLabel = root.Q<Label>("detail-description-label");
        detailPortionLabel = root.Q<Label>("detail-portion-label");
        detailCaloriesLabel = root.Q<Label>("detail-calories-label");

        dishSearchField = root.Q<TextField>("dish-search-field");

        viewARButton = root.Q<Button>("view-ar-button");
        addOrderButton = root.Q<Button>("add-order-button");
        openDetailModalButton = root.Q<Button>("open-detail-modal-button");
        navOrderButton = root.Q<Button>("nav-order-button");

        filterRecommendedButton = root.Q<Button>("filter-recommended-button");
        filterAllButton = root.Q<Button>("filter-all-button");
        filterFoodButton = root.Q<Button>("filter-food-button");
        filterBreakfastButton = root.Q<Button>("filter-breakfast-button");
        filterDessertsButton = root.Q<Button>("filter-desserts-button");
        filterDrinksButton = root.Q<Button>("filter-drinks-button");
        filterKidsButton = root.Q<Button>("filter-kids-button");

        menuToast = root.Q<VisualElement>("menu-toast");
        menuToastTitleLabel = root.Q<Label>("menu-toast-title-label");
        menuToastMessageLabel = root.Q<Label>("menu-toast-message-label");
    }

    private void BindTopFilters()
    {
        sectionFilterButtons.Clear();

        RegisterSectionFilter(filterRecommendedButton, FilterRecommended);
        RegisterSectionFilter(filterAllButton, FilterAll);
        RegisterSectionFilter(filterFoodButton, FilterFood);
        RegisterSectionFilter(filterBreakfastButton, FilterBreakfast);
        RegisterSectionFilter(filterDessertsButton, FilterDesserts);
        RegisterSectionFilter(filterDrinksButton, FilterDrinks);
        RegisterSectionFilter(filterKidsButton, FilterKids);
    }

    private void RegisterSectionFilter(Button button, string filter)
    {
        if (button == null)
        {
            return;
        }

        button.userData = filter;
        sectionFilterButtons.Add(button);

        button.clicked += () =>
        {
            ShowSectionFilter(filter, true, true);
        };
    }

    private void BindMainActions()
    {
        Button bottomHomeButton = root.Q<Button>("bottom-home-button");
        Button backWelcomeButton = root.Q<Button>("back-welcome-button");

        if (bottomHomeButton != null)
        {
            bottomHomeButton.clicked += onBackWelcome;
        }

        if (backWelcomeButton != null)
        {
            backWelcomeButton.clicked += onBackWelcome;
        }

        if (navOrderButton != null)
        {
            navOrderButton.clicked += onOpenOrder;
        }

        if (dishSearchField != null)
        {
            dishSearchField.RegisterValueChangedCallback(evt =>
            {
                RefreshDishCards(false);
                MarkSelectedDishIfVisible();
            });
        }

        if (viewARButton != null)
        {
            viewARButton.clicked += () =>
            {
                onViewAR?.Invoke(appState.SelectedDish);
            };
        }

        if (addOrderButton != null)
        {
            addOrderButton.clicked += AddSelectedDishToOrder;
        }

        if (openDetailModalButton != null)
        {
            openDetailModalButton.clicked += OpenDishDetailModal;
        }

        if (dishScroll != null)
        {
            dishScroll.mode = ScrollViewMode.Vertical;
            dishScroll.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            dishScroll.verticalScrollerVisibility = ScrollerVisibility.Auto;
            dishScroll.touchScrollBehavior = ScrollView.TouchScrollBehavior.Clamped;
        }
    }

    private void ShowSectionFilter(string filter, bool selectFirstDish, bool clearSearch)
    {
        string safeFilter = GetSafeFilter(filter);

        appState.CurrentSectionFilter = safeFilter;
        appState.CurrentCategory = safeFilter;

        if (clearSearch && dishSearchField != null)
        {
            dishSearchField.SetValueWithoutNotify(string.Empty);
        }

        MarkSelectedSectionFilter(safeFilter);
        UpdateFilterDescription();

        if (currentCategoryLabel != null)
        {
            currentCategoryLabel.text = safeFilter;
        }

        RefreshDishCards(selectFirstDish);
    }

    private void RefreshDishCards(bool selectFirstDish)
    {
        string searchText = dishSearchField != null
            ? dishSearchField.value
            : string.Empty;

        List<DishData> filteredDishes = GetFilteredDishes(appState.CurrentSectionFilter, searchText);
        BuildDishCards(filteredDishes);

        if (selectFirstDish)
        {
            SelectFirstVisibleDish();
        }
    }

    private void MarkSelectedSectionFilter(string filter)
    {
        foreach (Button button in sectionFilterButtons)
        {
            button.RemoveFromClassList("section-filter-selected");

            if (button.userData is string buttonFilter && buttonFilter == filter)
            {
                button.AddToClassList("section-filter-selected");
            }
        }
    }

    private void UpdateFilterDescription()
    {
        if (currentFilterDescriptionLabel == null)
        {
            return;
        }

        switch (appState.CurrentSectionFilter)
        {
            case FilterRecommended:
                currentFilterDescriptionLabel.text = "Platillos destacados para una demo rápida, visual y atractiva.";
                break;

            case FilterFood:
                currentFilterDescriptionLabel.text = "Entradas, ensaladas, pizzas, pastas, especialidades y platillos para compartir.";
                break;

            case FilterBreakfast:
                currentFilterDescriptionLabel.text = "Opciones matutinas con huevos, omelettes y platillos calientes.";
                break;

            case FilterDesserts:
                currentFilterDescriptionLabel.text = "Dulces italianos, tartas y postres para cerrar la experiencia.";
                break;

            case FilterDrinks:
                currentFilterDescriptionLabel.text = "Bebidas, sodas italianas, cocteles y vinos disponibles en el menú.";
                break;

            case FilterKids:
                currentFilterDescriptionLabel.text = "Opciones Bambino con porciones pensadas para niños.";
                break;

            case FilterAll:
            default:
                currentFilterDescriptionLabel.text = "Explora el menú completo con modelos 3D y experiencia RA para cada producto.";
                break;
        }
    }

    private string GetSafeFilter(string filter)
    {
        switch (filter)
        {
            case FilterRecommended:
            case FilterAll:
            case FilterFood:
            case FilterBreakfast:
            case FilterDesserts:
            case FilterDrinks:
            case FilterKids:
                return filter;

            default:
                return FilterRecommended;
        }
    }

    private string GetSectionFilterForDish(DishData dish)
    {
        if (dish == null)
        {
            return FilterRecommended;
        }

        switch (dish.MenuSection)
        {
            case "Comidas":
                return FilterFood;

            case "Desayunos":
                return FilterBreakfast;

            case "Postres":
                return FilterDesserts;

            case "Bebidas":
            case "Vinos":
                return FilterDrinks;

            case "Infantil":
                return FilterKids;

            default:
                return FilterAll;
        }
    }

    private List<DishData> GetFilteredDishes(string filter, string searchText)
    {
        IEnumerable<DishData> result = appState.Dishes;

        switch (GetSafeFilter(filter))
        {
            case FilterRecommended:
                result = result.Where(dish => dish.Recommended);
                break;

            case FilterFood:
                result = result.Where(dish => dish.MenuSection == "Comidas");
                break;

            case FilterBreakfast:
                result = result.Where(dish => dish.MenuSection == "Desayunos");
                break;

            case FilterDesserts:
                result = result.Where(dish => dish.MenuSection == "Postres");
                break;

            case FilterDrinks:
                result = result.Where(dish => dish.MenuSection == "Bebidas" || dish.MenuSection == "Vinos");
                break;

            case FilterKids:
                result = result.Where(dish => dish.MenuSection == "Infantil" || dish.IsKidsMenu);
                break;

            case FilterAll:
            default:
                result = appState.Dishes;
                break;
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            string normalizedSearch = searchText.Trim().ToLowerInvariant();

            result = result.Where(dish =>
                ContainsNormalized(dish.Name, normalizedSearch) ||
                ContainsNormalized(dish.Description, normalizedSearch) ||
                ContainsNormalized(dish.Category, normalizedSearch) ||
                ContainsNormalized(dish.SubCategory, normalizedSearch) ||
                ContainsNormalized(dish.MenuSection, normalizedSearch) ||
                ContainsAnyNormalized(dish.Ingredients, normalizedSearch) ||
                ContainsAnyNormalized(dish.Tags, normalizedSearch) ||
                ContainsAnyNormalized(dish.Allergens, normalizedSearch));
        }

        return result.ToList();
    }

    private bool ContainsNormalized(string value, string search)
    {
        return !string.IsNullOrWhiteSpace(value) &&
               value.ToLowerInvariant().Contains(search);
    }

    private bool ContainsAnyNormalized(IEnumerable<string> values, string search)
    {
        if (values == null)
        {
            return false;
        }

        return values.Any(value => ContainsNormalized(value, search));
    }

    private void BuildDishCards(List<DishData> filteredDishes)
    {
        if (dishList == null)
        {
            Debug.LogWarning("[MenuHomeViewController] dish-list es null.");
            return;
        }

        dishList.Clear();
        dishCards.Clear();

        if (filteredDishes == null || filteredDishes.Count == 0)
        {
            dishList.Add(CreateEmptyDishListCard());
            return;
        }

        foreach (DishData dish in filteredDishes)
        {
            VisualElement card = CreateDishCard(dish);
            dishList.Add(card);
            dishCards.Add(card);
        }
    }

    private VisualElement CreateEmptyDishListCard()
    {
        VisualElement card = new VisualElement();
        card.AddToClassList("dish-empty-card");

        Label title = new Label("Sin resultados");
        title.AddToClassList("dish-empty-title");

        Label text = new Label("No encontramos platillos con el filtro o búsqueda actual. Prueba cambiando la sección o limpiando el buscador.");
        text.AddToClassList("dish-empty-text");

        card.Add(title);
        card.Add(text);

        return card;
    }

    private VisualElement CreateDishCard(DishData dish)
    {
        VisualElement card = new VisualElement();
        card.AddToClassList("dish-card");
        card.userData = dish;

        VisualElement top = new VisualElement();
        top.AddToClassList("dish-card-top");

        Label modelBadge = new Label("MODELO 3D");
        modelBadge.AddToClassList("dish-card-model-badge");

        Label categoryBadge = new Label(dish.Category);
        categoryBadge.AddToClassList("dish-card-category-badge");
        categoryBadge.AddToClassList(GetCategoryBadgeClass(dish));

        top.Add(modelBadge);
        top.Add(categoryBadge);

        Label name = new Label(dish.Name);
        name.AddToClassList("dish-card-name");

        Label description = new Label(GetShortDescription(dish.Description, 120));
        description.AddToClassList("dish-card-description");

        VisualElement metaRow = new VisualElement();
        metaRow.AddToClassList("dish-card-meta-row");

        Label portionChip = CreateDishMetaChip(GetCompactPortionText(dish), "dish-card-meta-chip-wide");
        Label caloriesChip = CreateDishMetaChip(GetCompactCaloriesText(dish), "dish-card-meta-chip-medium");
        Label arChip = CreateDishMetaChip("RA lista", "dish-card-meta-chip-ar");

        metaRow.Add(portionChip);
        metaRow.Add(caloriesChip);
        metaRow.Add(arChip);

        card.Add(top);
        card.Add(name);
        card.Add(description);
        card.Add(metaRow);

        card.RegisterCallback<ClickEvent>(_ =>
        {
            SelectDish(dish);
            MarkSelectedCard(card);
        });

        return card;
    }

    private Label CreateDishMetaChip(string text, string extraClass)
    {
        Label chip = new Label(string.IsNullOrWhiteSpace(text) ? "-" : text);

        chip.AddToClassList("dish-card-meta-chip");

        if (!string.IsNullOrWhiteSpace(extraClass))
        {
            chip.AddToClassList(extraClass);
        }

        return chip;
    }

    private string GetCompactPortionText(DishData dish)
    {
        if (dish == null || string.IsNullOrWhiteSpace(dish.Portion))
        {
            return "-";
        }

        string portion = dish.Portion
            .Replace("Botella", "Bot.")
            .Replace("botella", "Bot.")
            .Replace("Copa", "Copa")
            .Replace("Mililitros", "ml")
            .Replace("mililitros", "ml")
            .Replace("ML", "ml");

        portion = portion.Replace(" / ", "/");

        return GetShortMetaText(portion, 20);
    }

    private string GetCompactCaloriesText(DishData dish)
    {
        if (dish == null || string.IsNullOrWhiteSpace(dish.Calories))
        {
            return "-";
        }

        string calories = dish.Calories
            .Replace("Variable / consultar menú", "Variable")
            .Replace("variable / consultar menú", "Variable")
            .Replace("Consultar menú", "Variable")
            .Replace("consultar menú", "Variable")
            .Replace("kilocalorías", "kcal")
            .Replace("calorías", "kcal");

        return GetShortMetaText(calories, 13);
    }

    private string GetShortMetaText(string text, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return "-";
        }

        string cleanText = text.Trim();

        if (cleanText.Length <= maxLength)
        {
            return cleanText;
        }

        return cleanText.Substring(0, maxLength).TrimEnd() + "...";
    }

    private string GetCategoryBadgeClass(DishData dish)
    {
        if (dish == null)
        {
            return "dish-category-default";
        }

        switch (dish.Category)
        {
            case "Pizzas":
                return "dish-category-pizza";

            case "Pasta":
            case "Pasta Ripiena":
                return "dish-category-pasta";

            case "Della Cucina":
                return "dish-category-cucina";

            case "Abbondanza":
                return "dish-category-abbondanza";

            case "Postres":
                return "dish-category-postre";

            case "Bebidas":
                return "dish-category-bebida";

            case "Vinos":
                return "dish-category-vino";

            case "Desayunos":
                return "dish-category-desayuno";

            case "Infantil":
                return "dish-category-infantil";

            case "Antipasti":
            case "Zuppe":
            case "Insalate":
                return "dish-category-entrada";

            default:
                return "dish-category-default";
        }
    }

    private string GetShortDescription(string text, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return "Sin descripción disponible.";
        }

        if (text.Length <= maxLength)
        {
            return text;
        }

        return text.Substring(0, maxLength).TrimEnd() + "...";
    }

    private void SelectFirstVisibleDish()
    {
        if (dishCards.Count == 0)
        {
            ClearDetailPanel();
            return;
        }

        if (dishCards[0].userData is DishData dish)
        {
            SelectDish(dish);
            MarkSelectedCard(dishCards[0]);
        }
    }

    private void MarkSelectedDishIfVisible()
    {
        if (appState.SelectedDish == null)
        {
            return;
        }

        MarkDishCardById(appState.SelectedDish.Id);
    }

    private void MarkDishCardById(string dishId)
    {
        foreach (VisualElement card in dishCards)
        {
            card.RemoveFromClassList("dish-card-selected");

            if (card.userData is DishData dish && dish.Id == dishId)
            {
                card.AddToClassList("dish-card-selected");
            }
        }
    }

    private void MarkSelectedCard(VisualElement selectedCard)
    {
        foreach (VisualElement card in dishCards)
        {
            card.RemoveFromClassList("dish-card-selected");
        }

        selectedCard.AddToClassList("dish-card-selected");
    }

    private void SelectDish(DishData dish)
    {
        if (dish == null)
        {
            return;
        }

        appState.SelectedDish = dish;

        SetDetailDishImage(dish);

        if (detailArBadgeLabel != null)
        {
            detailArBadgeLabel.text = "Modelo 3D";
        }

        if (detailNameLabel != null)
        {
            detailNameLabel.text = dish.Name;
        }

        if (detailCategoryLabel != null)
        {
            detailCategoryLabel.text = dish.Category;
        }

        if (detailDescriptionLabel != null)
        {
            detailDescriptionLabel.text = BuildMenuDetailDescription(dish);
        }

        if (detailPortionLabel != null)
        {
            detailPortionLabel.text = dish.Portion;
        }

        if (detailCaloriesLabel != null)
        {
            detailCaloriesLabel.text = dish.Calories;
        }

        if (viewARButton != null)
        {
            viewARButton.SetEnabled(true);
        }

        if (menuArStatusLabel != null)
        {
            menuArStatusLabel.text = "Modelos 3D listos";
        }
    }

    private void SetDetailDishImage(DishData dish)
    {
        if (detailDishImage == null)
        {
            return;
        }

        if (dish == null || string.IsNullOrWhiteSpace(dish.Id))
        {
            detailDishImage.image = null;
            detailDishImage.AddToClassList("hidden");
            return;
        }

        Texture2D dishTexture = Resources.Load<Texture2D>($"DishImages/{dish.Id}");

        if (dishTexture == null)
        {
            Debug.LogWarning($"[MenuHomeViewController] No se encontró imagen para DishId={dish.Id}. Ruta esperada: Resources/DishImages/{dish.Id}.png");

            detailDishImage.image = null;
            detailDishImage.AddToClassList("hidden");
            return;
        }

        detailDishImage.image = dishTexture;
        detailDishImage.scaleMode = ScaleMode.ScaleToFit;
        detailDishImage.RemoveFromClassList("hidden");
    }

    private void ClearDetailPanel()
    {
        appState.SelectedDish = null;

        if (detailArBadgeLabel != null)
        {
            detailArBadgeLabel.text = "Modelo 3D";
        }

        if (detailNameLabel != null)
        {
            detailNameLabel.text = "Sin resultados";
        }

        if (detailCategoryLabel != null)
        {
            detailCategoryLabel.text = "Cambia el filtro";
        }

        if (detailDescriptionLabel != null)
        {
            detailDescriptionLabel.text = "No hay platillos visibles con la búsqueda actual.";
        }

        if (detailPortionLabel != null)
        {
            detailPortionLabel.text = "-";
        }

        if (detailCaloriesLabel != null)
        {
            detailCaloriesLabel.text = "-";
        }

        if (viewARButton != null)
        {
            viewARButton.SetEnabled(false);
        }
    }

    private string BuildMenuDetailDescription(DishData dish)
    {
        if (dish == null)
        {
            return "Selecciona un platillo para ver su información.";
        }

        return $"{dish.Description}\n\n" +
               $"Modelo 3D disponible\n" +
               $"Realidad Aumentada sobre marcador de mesa\n" +
               $"{dish.Portion} · {dish.Calories}\n" +
               $"{dish.SubCategory}";
    }

    private void AddSelectedDishToOrder()
    {
        if (appState.SelectedDish == null)
        {
            Debug.LogWarning("[MenuHomeViewController] No hay platillo seleccionado.");
            return;
        }

        appState.Order.Add(appState.SelectedDish);
        UpdateOrderCount();

        ShowMenuToast(
            "Platillo agregado",
            $"{appState.SelectedDish.Name} se agregó a tu orden. Total: {appState.Order.Count} producto(s)."
        );

        Debug.Log($"[MenuHomeViewController] Agregado a orden: {appState.SelectedDish.Name}");
    }

    private void UpdateOrderCount()
    {
        int totalProducts = appState.Order.Count;

        if (orderCountLabel != null)
        {
            orderCountLabel.text = $"Orden: {totalProducts}";
        }

        if (navOrderButton != null)
        {
            navOrderButton.text = totalProducts > 0
                ? $"Mi orden ({totalProducts})"
                : "Mi orden";
        }
    }

    private void ShowMenuToast(string title, string message)
    {
        if (menuToast == null)
        {
            return;
        }

        if (menuToastTitleLabel != null)
        {
            menuToastTitleLabel.text = title;
        }

        if (menuToastMessageLabel != null)
        {
            menuToastMessageLabel.text = message;
        }

        menuToast.RemoveFromClassList("hidden");

        menuToast.schedule.Execute(() =>
        {
            HideMenuToast();
        }).StartingIn(2200);
    }

    private void HideMenuToast()
    {
        if (menuToast != null)
        {
            menuToast.AddToClassList("hidden");
        }
    }

    private void BindDishDetailModal()
    {
        dishDetailModal = root.Q<VisualElement>("dish-detail-modal");

        modalDishNameLabel = root.Q<Label>("modal-dish-name-label");
        modalDishMetaLabel = root.Q<Label>("modal-dish-meta-label");
        modalDishArLabel = root.Q<Label>("modal-dish-ar-label");
        modalSectionLabel = root.Q<Label>("modal-section-label");
        modalSubcategoryLabel = root.Q<Label>("modal-subcategory-label");
        modalPortionLabel = root.Q<Label>("modal-portion-label");
        modalCaloriesLabel = root.Q<Label>("modal-calories-label");
        modalTagsLabel = root.Q<Label>("modal-tags-label");
        modalDetailTitleLabel = root.Q<Label>("modal-detail-title-label");
        modalDetailBodyLabel = root.Q<Label>("modal-detail-body-label");
        modalDishImage = root.Q<Image>("modal-dish-image");

        closeDishModalButton = root.Q<Button>("close-dish-modal-button");
        modalCloseButton = root.Q<Button>("modal-close-button");
        modalViewArButton = root.Q<Button>("modal-view-ar-button");
        modalAddOrderButton = root.Q<Button>("modal-add-order-button");

        modalTabSummaryButton = root.Q<Button>("modal-tab-summary-button");
        modalTabIngredientsButton = root.Q<Button>("modal-tab-ingredients-button");
        modalTabNutritionButton = root.Q<Button>("modal-tab-nutrition-button");
        modalTabAllergensButton = root.Q<Button>("modal-tab-allergens-button");
        modalTabARButton = root.Q<Button>("modal-tab-ar-button");

        dishModalScroll = root.Q<ScrollView>("dish-modal-scroll");
        dishModelViewerPanel = root.Q<VisualElement>("dish-model-viewer-panel");
        dishModalTextCard = root.Q<VisualElement>("dish-modal-text-card");
        modelViewerTitleLabel = root.Q<Label>("model-viewer-title-label");
        modelViewerSubtitleLabel = root.Q<Label>("model-viewer-subtitle-label");
        dishModelRenderImage = root.Q<Image>("dish-model-render-image");
        modelPreviewRenderer = UnityEngine.Object.FindFirstObjectByType<DishModelPreviewRenderer>();

        BindDishModalButtons();
        BindDishModalTabs();
    }

    private void BindDishModalButtons()
    {
        if (closeDishModalButton != null)
        {
            closeDishModalButton.clicked += CloseDishDetailModal;
        }

        if (modalCloseButton != null)
        {
            modalCloseButton.clicked += CloseDishDetailModal;
        }

        if (modalViewArButton != null)
        {
            modalViewArButton.clicked += () =>
            {
                if (appState.SelectedDish != null)
                {
                    onViewAR?.Invoke(appState.SelectedDish);
                }
            };
        }

        if (modalAddOrderButton != null)
        {
            modalAddOrderButton.clicked += AddSelectedDishToOrder;
        }
    }

    private void BindDishModalTabs()
    {
        if (modalTabSummaryButton != null)
        {
            modalTabSummaryButton.clicked += ShowModalSummaryTab;
        }

        if (modalTabIngredientsButton != null)
        {
            modalTabIngredientsButton.clicked += ShowModalIngredientsTab;
        }

        if (modalTabNutritionButton != null)
        {
            modalTabNutritionButton.clicked += ShowModalNutritionTab;
        }

        if (modalTabAllergensButton != null)
        {
            modalTabAllergensButton.clicked += ShowModalAllergensTab;
        }

        if (modalTabARButton != null)
        {
            modalTabARButton.clicked += ShowModalARTab;
        }
    }

    private void OpenDishDetailModal()
    {
        if (appState.SelectedDish == null)
        {
            Debug.LogWarning("[MenuHomeViewController] No hay platillo seleccionado para mostrar detalle.");
            return;
        }

        PopulateDishDetailModal(appState.SelectedDish);
        ShowModalSummaryTab();

        if (dishDetailModal != null)
        {
            dishDetailModal.RemoveFromClassList("hidden");
        }
    }

    private void CloseDishDetailModal()
    {
        if (dishDetailModal != null)
        {
            dishDetailModal.AddToClassList("hidden");
        }
    }

    private void PopulateDishDetailModal(DishData dish)
    {
        if (dish == null)
        {
            return;
        }

        SetModalDishImage(dish);

        if (modalDishNameLabel != null)
        {
            modalDishNameLabel.text = dish.Name;
        }

        if (modalDishMetaLabel != null)
        {
            modalDishMetaLabel.text = $"{dish.MenuSection} · {dish.Category} · {dish.Portion} · {dish.Calories}";
        }

        if (modalDishArLabel != null)
        {
            modalDishArLabel.text = "Modelo 3D listo";
        }

        if (modalSectionLabel != null)
        {
            modalSectionLabel.text = dish.MenuSection;
        }

        if (modalSubcategoryLabel != null)
        {
            modalSubcategoryLabel.text = dish.SubCategory;
        }

        if (modalPortionLabel != null)
        {
            modalPortionLabel.text = dish.Portion;
        }

        if (modalCaloriesLabel != null)
        {
            modalCaloriesLabel.text = dish.Calories;
        }

        if (modalTagsLabel != null)
        {
            modalTagsLabel.text = dish.GetTagsText();
        }

        if (modalViewArButton != null)
        {
            modalViewArButton.SetEnabled(true);
        }
    }

    private void SetModalDishImage(DishData dish)
    {
        if (modalDishImage == null)
        {
            return;
        }

        if (dish == null || string.IsNullOrWhiteSpace(dish.Id))
        {
            modalDishImage.image = null;
            modalDishImage.AddToClassList("hidden");
            return;
        }

        Texture2D dishTexture = Resources.Load<Texture2D>($"DishImages/{dish.Id}");

        if (dishTexture == null)
        {
            Debug.LogWarning($"[MenuHomeViewController] No se encontró imagen para modal. DishId={dish.Id}. Ruta esperada: Resources/DishImages/{dish.Id}.png");

            modalDishImage.image = null;
            modalDishImage.AddToClassList("hidden");
            return;
        }

        modalDishImage.image = dishTexture;
        modalDishImage.scaleMode = ScaleMode.ScaleToFit;
        modalDishImage.RemoveFromClassList("hidden");
    }

    private void ShowTextModalContent()
    {
        if (dishModalTextCard != null)
        {
            dishModalTextCard.RemoveFromClassList("hidden");
        }

        if (dishModelViewerPanel != null)
        {
            dishModelViewerPanel.AddToClassList("hidden");
        }
    }

    private void ShowModalSummaryTab()
    {
        SelectModalTab(modalTabSummaryButton);
        ShowTextModalContent();

        DishData dish = appState.SelectedDish;

        if (dish == null)
        {
            return;
        }

        SetModalDetailText(
            "Resumen del platillo",
            $"{dish.Description}\n\n" +
            $"{dish.Story}\n\n" +
            $"Temperatura sugerida: {dish.Temperature}\n" +
            $"Nivel de picante: {dish.SpiceLevel}\n" +
            $"Vegetariano: {(dish.IsVegetarianFriendly ? "Sí" : "No")}\n" +
            $"Para compartir: {(dish.IsShareable ? "Sí" : "No")}\n" +
            $"Menú infantil: {(dish.IsKidsMenu ? "Sí" : "No")}\n" +
            $"Contiene alcohol: {(dish.IsAlcoholic ? "Sí" : "No")}"
        );
    }

    private void ShowModalIngredientsTab()
    {
        SelectModalTab(modalTabIngredientsButton);
        ShowTextModalContent();

        DishData dish = appState.SelectedDish;

        if (dish == null)
        {
            return;
        }

        SetModalDetailText(
            "Ingredientes y preparación",
            $"Ingredientes:\n{dish.GetIngredientsText()}\n\n" +
            $"Preparación:\n{dish.GetPreparationText()}\n\n" +
            $"Maridaje sugerido:\n{dish.GetPairingsText()}"
        );
    }

    private void ShowModalNutritionTab()
    {
        SelectModalTab(modalTabNutritionButton);
        ShowTextModalContent();

        DishData dish = appState.SelectedDish;

        if (dish == null)
        {
            return;
        }

        SetModalDetailText(
            "Información nutrimental",
            $"{dish.GetNutritionText()}\n\n" +
            $"Calidad del dato base: {dish.BaseDataQuality}\n" +
            $"Calidad del dato nutrimental: {dish.NutritionDataQuality}"
        );
    }

    private void ShowModalAllergensTab()
    {
        SelectModalTab(modalTabAllergensButton);
        ShowTextModalContent();

        DishData dish = appState.SelectedDish;

        if (dish == null)
        {
            return;
        }

        SetModalDetailText(
            "Alérgenos y advertencias",
            $"Alérgenos:\n{dish.GetAllergensText()}\n\n" +
            $"Advertencias:\n{dish.GetWarningsText()}"
        );
    }

    private void ShowModalARTab()
    {
        SelectModalTab(modalTabARButton);

        DishData dish = appState.SelectedDish;

        if (dish == null)
        {
            return;
        }

        if (dishModalTextCard != null)
        {
            dishModalTextCard.AddToClassList("hidden");
        }

        if (dishModelViewerPanel != null)
        {
            dishModelViewerPanel.RemoveFromClassList("hidden");
        }

        if (modelViewerTitleLabel != null)
        {
            modelViewerTitleLabel.text = dish.Name;
        }

        if (modelViewerSubtitleLabel != null)
        {
            modelViewerSubtitleLabel.text = $"{dish.Category} · Modelo 3D disponible";
        }

        if (modelPreviewRenderer == null)
        {
            modelPreviewRenderer = UnityEngine.Object.FindFirstObjectByType<DishModelPreviewRenderer>();
        }

        if (modelPreviewRenderer != null)
        {
            modelPreviewRenderer.ShowDishModel(dish.Id);
            ApplyPreviewRenderTexture();
        }
        else
        {
            Debug.LogWarning("[MenuHomeViewController] No se encontró DishModelPreviewRenderer en la escena.");
        }
    }

    private void ApplyPreviewRenderTexture()
    {
        if (dishModelRenderImage == null)
        {
            return;
        }

        if (modelPreviewRenderer == null || modelPreviewRenderer.PreviewRenderTexture == null)
        {
            return;
        }

        dishModelRenderImage.image = modelPreviewRenderer.PreviewRenderTexture;
        dishModelRenderImage.scaleMode = ScaleMode.ScaleToFit;
    }

    private void SetModalDetailText(string title, string body)
    {
        if (modalDetailTitleLabel != null)
        {
            modalDetailTitleLabel.text = title;
        }

        if (modalDetailBodyLabel != null)
        {
            modalDetailBodyLabel.text = body;
        }
    }

    private void SelectModalTab(Button selectedButton)
    {
        RemoveModalTabSelection(modalTabSummaryButton);
        RemoveModalTabSelection(modalTabIngredientsButton);
        RemoveModalTabSelection(modalTabNutritionButton);
        RemoveModalTabSelection(modalTabAllergensButton);
        RemoveModalTabSelection(modalTabARButton);

        if (selectedButton != null)
        {
            selectedButton.AddToClassList("dish-modal-tab-selected");
        }
    }

    private void RemoveModalTabSelection(Button button)
    {
        if (button != null)
        {
            button.RemoveFromClassList("dish-modal-tab-selected");
        }
    }

}
