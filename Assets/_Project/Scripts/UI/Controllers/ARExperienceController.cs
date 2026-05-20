using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Vuforia;

public class ARExperienceController : MonoBehaviour
{
    private const float RotationStepDegrees = 15.0f;
    private const float ScaleStep = 0.10f;
    private const float MinScale = 0.35f;
    private const float MaxScale = 1.60f;
    private const float DefaultScale = 1.0f;
    private const float PresentationScale = 1.35f;

    [Header("UI Document")]
    [SerializeField] private UIDocument uiDocument;

    [Header("AR Content")]
    [SerializeField] private Transform dishModelRoot;

    [Header("Dish Models")]
    [SerializeField] private ARDishModelCatalog dishModelCatalog;
    [SerializeField] private GameObject fallbackDishPrefab;

    [Header("Vuforia")]
    [SerializeField] private ObserverBehaviour imageTargetObserver;

    [Header("Style Sheets")]
    [SerializeField] private StyleSheet[] styleSheets;

    private VisualElement root;
    private VisualElement centerStatusPanel;
    private VisualElement arStatusDot;

    private ScrollView arDetailScroll;

    private Label dishNameLabel;
    private Label dishCategoryLabel;
    private Label trackingStatusLabel;
    private Label trackingHelperLabel;
    private Label arStatusValueLabel;
    private Label arDetailTitleLabel;
    private Label arDetailBodyLabel;
    private Label scaleValueLabel;
    private Label rotationValueLabel;
    private Label helpTextLabel;

    private Button rotateLeftButton;
    private Button rotateRightButton;
    private Button scaleDownButton;
    private Button scaleUpButton;
    private Button resetButton;
    private Button backMenuButton;
    private Button previousDishButton;
    private Button nextDishButton;
    private Button presentationScaleButton;
    private Button addOrderFromArButton;

    private Button arTabViewButton;
    private Button arTabIngredientsButton;
    private Button arTabNutritionButton;
    private Button arTabTipsButton;

    private GameObject currentDishModel;
    private List<DishData> arDishes = new List<DishData>();

    private int currentDishIndex = -1;
    private float currentRotationY;
    private float currentScale = DefaultScale;
    private bool presentationScaleEnabled;
    private bool targetTracked;

    private void OnEnable()
    {
        ResolveReferences();
        LoadStyleSheets();
        BindUI();
        ConfigureScroll();
        BindVuforiaObserver();
        BuildDishNavigationList();
        LoadSelectedDish();

        ShowViewInfo();
        SetModelVisible(false);
        SetTrackingState(false, "Busca el marcador de mesa", "Esperando marcador");
        ApplyTransform();
    }

    private void OnDisable()
    {
        if (imageTargetObserver != null)
        {
            imageTargetObserver.OnTargetStatusChanged -= OnTargetStatusChanged;
        }

        UnbindUI();
    }

    private void ResolveReferences()
    {
        if (uiDocument == null)
        {
            uiDocument = GetComponent<UIDocument>();
        }

        if (uiDocument == null)
        {
            Debug.LogError("[ARExperienceController] No se encontró UIDocument.");
            return;
        }

        root = uiDocument.rootVisualElement;
    }

    private void LoadStyleSheets()
    {
        if (root == null || styleSheets == null)
        {
            return;
        }

        foreach (StyleSheet styleSheet in styleSheets)
        {
            if (styleSheet == null)
            {
                continue;
            }

            if (!root.styleSheets.Contains(styleSheet))
            {
                root.styleSheets.Add(styleSheet);
            }
        }
    }

    private void BindUI()
    {
        if (root == null)
        {
            return;
        }

        centerStatusPanel = root.Q<VisualElement>("ar-center-status");
        arStatusDot = root.Q<VisualElement>("ar-status-dot");

        dishNameLabel = root.Q<Label>("ar-dish-name-label");
        dishCategoryLabel = root.Q<Label>("ar-dish-category-label");
        trackingStatusLabel = root.Q<Label>("tracking-status-label");
        trackingHelperLabel = root.Q<Label>("tracking-helper-label");
        arStatusValueLabel = root.Q<Label>("ar-status-value-label");
        arDetailTitleLabel = root.Q<Label>("ar-detail-title-label");
        arDetailBodyLabel = root.Q<Label>("ar-detail-body-label");
        scaleValueLabel = root.Q<Label>("ar-scale-value-label");
        rotationValueLabel = root.Q<Label>("ar-rotation-value-label");
        helpTextLabel = root.Q<Label>("ar-help-text-label");

        arDetailScroll = root.Q<ScrollView>("ar-detail-scroll");

        rotateLeftButton = root.Q<Button>("rotate-left-button");
        rotateRightButton = root.Q<Button>("rotate-right-button");
        scaleDownButton = root.Q<Button>("scale-down-button");
        scaleUpButton = root.Q<Button>("scale-up-button");
        resetButton = root.Q<Button>("reset-ar-button");
        backMenuButton = root.Q<Button>("back-menu-button");
        previousDishButton = root.Q<Button>("previous-dish-button");
        nextDishButton = root.Q<Button>("next-dish-button");
        presentationScaleButton = root.Q<Button>("presentation-scale-button");
        addOrderFromArButton = root.Q<Button>("add-order-from-ar-button");

        arTabViewButton = root.Q<Button>("ar-tab-view-button");
        arTabIngredientsButton = root.Q<Button>("ar-tab-ingredients-button");
        arTabNutritionButton = root.Q<Button>("ar-tab-nutrition-button");
        arTabTipsButton = root.Q<Button>("ar-tab-tips-button");

        if (arTabViewButton != null)
        {
            arTabViewButton.clicked += ShowViewInfo;
        }

        if (arTabIngredientsButton != null)
        {
            arTabIngredientsButton.clicked += ShowIngredientsInfo;
        }

        if (arTabNutritionButton != null)
        {
            arTabNutritionButton.clicked += ShowNutritionInfo;
        }

        if (arTabTipsButton != null)
        {
            arTabTipsButton.clicked += ShowTipsInfo;
        }

        if (rotateLeftButton != null)
        {
            rotateLeftButton.clicked += RotateLeft;
        }

        if (rotateRightButton != null)
        {
            rotateRightButton.clicked += RotateRight;
        }

        if (scaleDownButton != null)
        {
            scaleDownButton.clicked += ScaleDown;
        }

        if (scaleUpButton != null)
        {
            scaleUpButton.clicked += ScaleUp;
        }

        if (resetButton != null)
        {
            resetButton.clicked += ResetView;
        }

        if (presentationScaleButton != null)
        {
            presentationScaleButton.clicked += TogglePresentationScale;
        }

        if (previousDishButton != null)
        {
            previousDishButton.clicked += ShowPreviousDish;
        }

        if (nextDishButton != null)
        {
            nextDishButton.clicked += ShowNextDish;
        }

        if (addOrderFromArButton != null)
        {
            addOrderFromArButton.clicked += AddSelectedDishToOrderFromAR;
        }

        if (backMenuButton != null)
        {
            backMenuButton.clicked += BackToMenu;
        }
    }

    private void UnbindUI()
    {
        if (arTabViewButton != null)
        {
            arTabViewButton.clicked -= ShowViewInfo;
        }

        if (arTabIngredientsButton != null)
        {
            arTabIngredientsButton.clicked -= ShowIngredientsInfo;
        }

        if (arTabNutritionButton != null)
        {
            arTabNutritionButton.clicked -= ShowNutritionInfo;
        }

        if (arTabTipsButton != null)
        {
            arTabTipsButton.clicked -= ShowTipsInfo;
        }

        if (rotateLeftButton != null)
        {
            rotateLeftButton.clicked -= RotateLeft;
        }

        if (rotateRightButton != null)
        {
            rotateRightButton.clicked -= RotateRight;
        }

        if (scaleDownButton != null)
        {
            scaleDownButton.clicked -= ScaleDown;
        }

        if (scaleUpButton != null)
        {
            scaleUpButton.clicked -= ScaleUp;
        }

        if (resetButton != null)
        {
            resetButton.clicked -= ResetView;
        }

        if (presentationScaleButton != null)
        {
            presentationScaleButton.clicked -= TogglePresentationScale;
        }

        if (previousDishButton != null)
        {
            previousDishButton.clicked -= ShowPreviousDish;
        }

        if (nextDishButton != null)
        {
            nextDishButton.clicked -= ShowNextDish;
        }

        if (addOrderFromArButton != null)
        {
            addOrderFromArButton.clicked -= AddSelectedDishToOrderFromAR;
        }

        if (backMenuButton != null)
        {
            backMenuButton.clicked -= BackToMenu;
        }
    }

    private void ConfigureScroll()
    {
        if (arDetailScroll == null)
        {
            return;
        }

        arDetailScroll.mode = ScrollViewMode.Vertical;
        arDetailScroll.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        arDetailScroll.verticalScrollerVisibility = ScrollerVisibility.Auto;
        arDetailScroll.touchScrollBehavior = ScrollView.TouchScrollBehavior.Clamped;
    }

    private void BindVuforiaObserver()
    {
        if (imageTargetObserver == null)
        {
            imageTargetObserver = FindFirstObjectByType<ObserverBehaviour>();
        }

        if (imageTargetObserver == null)
        {
            Debug.LogWarning("[ARExperienceController] No se encontró ObserverBehaviour del ImageTarget.");
            return;
        }

        imageTargetObserver.OnTargetStatusChanged -= OnTargetStatusChanged;
        imageTargetObserver.OnTargetStatusChanged += OnTargetStatusChanged;

        Debug.Log($"[ARExperienceController] Observer conectado: {imageTargetObserver.TargetName}");
    }

    private void BuildDishNavigationList()
    {
        arDishes = MenuRepository.GetDishes()
            .Where(dish => dish != null && dish.HasAR)
            .ToList();

        if (RuntimeAppState.Instance == null || !RuntimeAppState.Instance.HasSelectedDish())
        {
            currentDishIndex = arDishes.Count > 0 ? 0 : -1;
            return;
        }

        currentDishIndex = arDishes.FindIndex(dish => dish.Id == RuntimeAppState.Instance.SelectedDishId);

        if (currentDishIndex < 0 && arDishes.Count > 0)
        {
            currentDishIndex = 0;
        }
    }

    private void LoadSelectedDish()
    {
        if (RuntimeAppState.Instance == null || !RuntimeAppState.Instance.HasSelectedDish())
        {
            Debug.LogWarning("[ARExperienceController] No hay platillo seleccionado. Regresando al menú.");
            SceneManager.LoadScene(AppSceneNames.MenuKiosk);
            return;
        }

        RefreshSelectedDishVisuals(true, true);
    }

    private void RefreshSelectedDishVisuals(bool respawnModel, bool resetView)
    {
        if (RuntimeAppState.Instance == null || !RuntimeAppState.Instance.HasSelectedDish())
        {
            return;
        }

        if (dishNameLabel != null)
        {
            dishNameLabel.text = RuntimeAppState.Instance.SelectedDishName;
        }

        if (dishCategoryLabel != null)
        {
            dishCategoryLabel.text = $"{RuntimeAppState.Instance.SelectedDishCategory} · {RuntimeAppState.Instance.SelectedDishPortion} · {RuntimeAppState.Instance.SelectedDishCalories}";
        }

        if (respawnModel)
        {
            SpawnSelectedDishModel();
            SetModelVisible(targetTracked);
        }

        if (resetView)
        {
            ResetView();
        }

        ShowViewInfo();
        UpdateNavigationButtons();

        Debug.Log($"[ARExperienceController] Platillo cargado: {RuntimeAppState.Instance.SelectedDishName}");
    }

    private void UpdateNavigationButtons()
    {
        bool canNavigate = arDishes != null && arDishes.Count > 1;

        SetButtonEnabled(previousDishButton, canNavigate);
        SetButtonEnabled(nextDishButton, canNavigate);
    }

    private void SpawnSelectedDishModel()
    {
        if (dishModelRoot == null)
        {
            Debug.LogWarning("[ARExperienceController] dishModelRoot no está asignado.");
            return;
        }

        ClearCurrentDishModel();

        GameObject prefabToSpawn = null;

        if (RuntimeAppState.Instance != null && RuntimeAppState.Instance.HasSelectedDish())
        {
            string dishId = RuntimeAppState.Instance.SelectedDishId;

            if (dishModelCatalog != null)
            {
                prefabToSpawn = dishModelCatalog.GetPrefabByDishId(dishId);
            }
        }

        if (prefabToSpawn == null)
        {
            prefabToSpawn = fallbackDishPrefab;
        }

        if (prefabToSpawn == null)
        {
            Debug.LogWarning("[ARExperienceController] No hay prefab asignado para el platillo ni fallback.");
            return;
        }

        currentDishModel = Instantiate(prefabToSpawn, dishModelRoot);

        currentDishModel.transform.localPosition = Vector3.zero;
        currentDishModel.transform.localRotation = Quaternion.identity;
        currentDishModel.transform.localScale = Vector3.one;

        Debug.Log($"[ARExperienceController] Modelo instanciado: {prefabToSpawn.name}");
    }

    private void ClearCurrentDishModel()
    {
        if (currentDishModel != null)
        {
            Destroy(currentDishModel);
            currentDishModel = null;
        }

        if (dishModelRoot == null)
        {
            return;
        }

        for (int i = dishModelRoot.childCount - 1; i >= 0; i--)
        {
            Transform child = dishModelRoot.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        bool isTracked = targetStatus.Status == Status.TRACKED ||
                         targetStatus.Status == Status.EXTENDED_TRACKED ||
                         targetStatus.Status == Status.LIMITED;

        if (isTracked)
        {
            targetTracked = true;

            SetModelVisible(true);
            HideCenterStatus();
            SetTrackingState(true, "Marcador detectado", "Modelo activo");

            Debug.Log($"[ARExperienceController] Target detectado: {targetStatus.Status}");
            return;
        }

        targetTracked = false;

        SetModelVisible(false);
        ShowCenterStatus();
        SetTrackingState(false, "Busca el marcador de mesa", "Esperando marcador");

        Debug.Log($"[ARExperienceController] Target perdido: {targetStatus.Status}");
    }

    private void ShowViewInfo()
    {
        SelectTab(arTabViewButton);

        string description = RuntimeAppState.Instance != null
            ? RuntimeAppState.Instance.SelectedDishDescription
            : "Gira, escala y acomoda el modelo 3D sobre el marcador de mesa.";

        string story = RuntimeAppState.Instance != null
            ? RuntimeAppState.Instance.SelectedDishStory
            : string.Empty;

        SetDetailInfo(
            "Vista del platillo",
            $"{description}\n\n{story}\n\n" +
            "Funciones disponibles:\n" +
            "• Cambia de platillo con Anterior o Siguiente sin salir de RA.\n" +
            "• Gira el platillo para revisarlo desde distintos ángulos.\n" +
            "• Ajusta el tamaño para comparar la porción sobre la mesa.\n" +
            "• Usa Vista grande para una presentación más llamativa.\n" +
            "• Agrega el platillo a la orden cuando estés listo."
        );

        SetHelpText("Cambia de platillo, ajusta la vista y agrégalo a tu orden directamente desde RA.");
    }

    private void ShowIngredientsInfo()
    {
        SelectTab(arTabIngredientsButton);

        SetDetailInfo(
            "Ingredientes y preparación",
            $"Ingredientes:\n{GetIngredientsText()}\n\nPreparación:\n{GetPreparationText()}"
        );

        SetHelpText("Consulta ingredientes antes de confirmar si tienes alguna preferencia alimentaria.");
    }

    private void ShowNutritionInfo()
    {
        SelectTab(arTabNutritionButton);

        SetDetailInfo(
            "Nutrición, alérgenos y advertencias",
            $"Información nutrimental:\n{GetNutritionText()}\n\nAlérgenos:\n{GetAllergensText()}\n\nAdvertencias:\n{GetWarningsText()}\n\nMaridaje sugerido:\n{GetPairingsText()}"
        );

        SetHelpText("Revisa calorías, alérgenos y advertencias antes de ordenar.");
    }

    private void ShowTipsInfo()
    {
        SelectTab(arTabTipsButton);

        SetDetailInfo(
            "Consejos de uso",
            "Para una mejor experiencia:\n\n" +
            "• Mantén el marcador completo dentro de la cámara.\n" +
            "• Evita sombras fuertes sobre el marcador impreso.\n" +
            "• Mantén la tablet estable mientras cargas el modelo.\n" +
            "• Usa Reset si el modelo se ve muy grande o rotado.\n" +
            "• Usa Vista grande solo para presentación, no como referencia exacta de porción.\n" +
            "• Puedes avanzar o retroceder entre platillos sin volver al menú."
        );

        SetHelpText("Buena luz y marcador visible ayudan a que el modelo se mantenga estable.");
    }

    private void SetDetailInfo(string title, string body)
    {
        if (arDetailTitleLabel != null)
        {
            arDetailTitleLabel.text = title;
        }

        if (arDetailBodyLabel != null)
        {
            arDetailBodyLabel.text = body;
        }

        if (arDetailScroll != null)
        {
            arDetailScroll.scrollOffset = Vector2.zero;
        }
    }

    private void SelectTab(Button selectedButton)
    {
        RemoveTabSelection(arTabViewButton);
        RemoveTabSelection(arTabIngredientsButton);
        RemoveTabSelection(arTabNutritionButton);
        RemoveTabSelection(arTabTipsButton);

        if (selectedButton != null)
        {
            selectedButton.AddToClassList("ar-tab-selected");
        }
    }

    private void RemoveTabSelection(Button button)
    {
        if (button != null)
        {
            button.RemoveFromClassList("ar-tab-selected");
        }
    }

    private string GetIngredientsText()
    {
        if (RuntimeAppState.Instance == null)
        {
            return "Ingredientes no disponibles.";
        }

        return RuntimeAppState.Instance.SelectedDishIngredientsText;
    }

    private string GetPreparationText()
    {
        if (RuntimeAppState.Instance == null)
        {
            return "Preparación no disponible.";
        }

        return RuntimeAppState.Instance.SelectedDishPreparationText;
    }

    private string GetNutritionText()
    {
        if (RuntimeAppState.Instance == null)
        {
            return "Información nutrimental no disponible.";
        }

        return RuntimeAppState.Instance.SelectedDishNutritionText;
    }

    private string GetWarningsText()
    {
        if (RuntimeAppState.Instance == null)
        {
            return "Advertencias no disponibles.";
        }

        return RuntimeAppState.Instance.SelectedDishWarningsText;
    }

    private string GetAllergensText()
    {
        if (RuntimeAppState.Instance == null)
        {
            return "Alérgenos no disponibles.";
        }

        return RuntimeAppState.Instance.SelectedDishAllergensText;
    }

    private string GetPairingsText()
    {
        if (RuntimeAppState.Instance == null)
        {
            return "Maridaje no disponible.";
        }

        return RuntimeAppState.Instance.SelectedDishPairingsText;
    }

    private void ShowPreviousDish()
    {
        NavigateDish(-1);
    }

    private void ShowNextDish()
    {
        NavigateDish(1);
    }

    private void NavigateDish(int direction)
    {
        if (arDishes == null || arDishes.Count == 0 || RuntimeAppState.Instance == null)
        {
            Debug.LogWarning("[ARExperienceController] No hay platillos disponibles para navegar en RA.");
            return;
        }

        if (currentDishIndex < 0)
        {
            currentDishIndex = 0;
        }

        currentDishIndex = (currentDishIndex + direction + arDishes.Count) % arDishes.Count;
        DishData dish = arDishes[currentDishIndex];

        RuntimeAppState.Instance.SetSelectedDish(dish);
        RefreshSelectedDishVisuals(true, true);
        SetHelpText($"Ahora viendo: {dish.Name}.");
    }

    private void RotateLeft()
    {
        currentRotationY -= RotationStepDegrees;
        NormalizeRotation();
        ApplyTransform();
    }

    private void RotateRight()
    {
        currentRotationY += RotationStepDegrees;
        NormalizeRotation();
        ApplyTransform();
    }

    private void ScaleDown()
    {
        presentationScaleEnabled = false;
        currentScale = Mathf.Max(MinScale, currentScale - ScaleStep);
        ApplyTransform();
    }

    private void ScaleUp()
    {
        presentationScaleEnabled = false;
        currentScale = Mathf.Min(MaxScale, currentScale + ScaleStep);
        ApplyTransform();
    }

    private void TogglePresentationScale()
    {
        presentationScaleEnabled = !presentationScaleEnabled;
        currentScale = presentationScaleEnabled ? PresentationScale : DefaultScale;

        if (presentationScaleButton != null)
        {
            presentationScaleButton.text = presentationScaleEnabled ? "Vista real" : "Vista grande";
        }

        ApplyTransform();
    }

    private void ResetView()
    {
        currentRotationY = 0.0f;
        currentScale = DefaultScale;
        presentationScaleEnabled = false;

        if (dishModelRoot != null)
        {
            dishModelRoot.localPosition = Vector3.zero;
        }

        if (currentDishModel != null)
        {
            currentDishModel.transform.localPosition = Vector3.zero;
            currentDishModel.transform.localRotation = Quaternion.identity;
            currentDishModel.transform.localScale = Vector3.one;
        }

        if (presentationScaleButton != null)
        {
            presentationScaleButton.text = "Vista grande";
        }

        ApplyTransform();
        SetHelpText("Vista reiniciada. Mantén el marcador visible para ver el platillo.");
    }

    private void ApplyTransform()
    {
        if (dishModelRoot == null)
        {
            Debug.LogWarning("[ARExperienceController] dishModelRoot no está asignado.");
            return;
        }

        dishModelRoot.localRotation = Quaternion.Euler(0.0f, currentRotationY, 0.0f);
        dishModelRoot.localScale = Vector3.one * currentScale;

        UpdateMetrics();
    }

    private void NormalizeRotation()
    {
        currentRotationY %= 360.0f;

        if (currentRotationY < 0.0f)
        {
            currentRotationY += 360.0f;
        }
    }

    private void UpdateMetrics()
    {
        if (scaleValueLabel != null)
        {
            scaleValueLabel.text = $"{Mathf.RoundToInt(currentScale * 100.0f)}%";
        }

        if (rotationValueLabel != null)
        {
            rotationValueLabel.text = $"{Mathf.RoundToInt(currentRotationY)}°";
        }
    }

    private void SetModelVisible(bool visible)
    {
        if (dishModelRoot == null)
        {
            return;
        }

        dishModelRoot.gameObject.SetActive(visible);
    }

    private void ShowCenterStatus()
    {
        if (centerStatusPanel != null)
        {
            centerStatusPanel.style.display = DisplayStyle.Flex;
        }
    }

    private void HideCenterStatus()
    {
        if (centerStatusPanel != null)
        {
            centerStatusPanel.style.display = DisplayStyle.None;
        }
    }

    private void SetTrackingState(bool active, string centerMessage, string statusMessage)
    {
        if (trackingStatusLabel != null)
        {
            trackingStatusLabel.text = centerMessage;
        }

        if (trackingHelperLabel != null)
        {
            trackingHelperLabel.text = active
                ? "Puedes girar, escalar o agregar el platillo a la orden."
                : "Apunta la cámara hacia el marcador impreso.";
        }

        if (arStatusValueLabel != null)
        {
            arStatusValueLabel.text = statusMessage;
        }

        UpdateStatusDot(active);
        SetControlsEnabled(active);
    }

    private void UpdateStatusDot(bool active)
    {
        if (arStatusDot == null)
        {
            return;
        }

        arStatusDot.RemoveFromClassList("ar-status-dot-waiting");
        arStatusDot.RemoveFromClassList("ar-status-dot-active");
        arStatusDot.RemoveFromClassList("ar-status-dot-lost");

        arStatusDot.AddToClassList(active ? "ar-status-dot-active" : "ar-status-dot-waiting");
    }

    private void SetControlsEnabled(bool enabled)
    {
        SetButtonEnabled(rotateLeftButton, enabled);
        SetButtonEnabled(rotateRightButton, enabled);
        SetButtonEnabled(scaleDownButton, enabled);
        SetButtonEnabled(scaleUpButton, enabled);
        SetButtonEnabled(resetButton, enabled);
        SetButtonEnabled(presentationScaleButton, enabled);
        SetButtonEnabled(addOrderFromArButton, RuntimeAppState.Instance != null && RuntimeAppState.Instance.HasSelectedDish());
    }

    private void SetButtonEnabled(Button button, bool enabled)
    {
        if (button != null)
        {
            button.SetEnabled(enabled);
        }
    }

    private void SetHelpText(string message)
    {
        if (helpTextLabel != null)
        {
            helpTextLabel.text = message;
        }
    }

    private void AddSelectedDishToOrderFromAR()
    {
        if (RuntimeAppState.Instance == null || !RuntimeAppState.Instance.HasSelectedDish())
        {
            Debug.LogWarning("[ARExperienceController] No hay platillo seleccionado para agregar a la orden.");
            SetHelpText("Selecciona un platillo antes de agregarlo a la orden.");
            return;
        }

        DishData selectedDish = MenuRepository.FindById(RuntimeAppState.Instance.SelectedDishId);

        if (selectedDish == null)
        {
            Debug.LogWarning($"[ARExperienceController] No se encontró DishData para agregar a orden. DishId={RuntimeAppState.Instance.SelectedDishId}");
            SetHelpText("No se encontró la información del platillo seleccionado.");
            return;
        }

        RuntimeAppState.Instance.AddToOrder(selectedDish);
        MarkAddOrderSuccess(selectedDish);
    }

    private void MarkAddOrderSuccess(DishData selectedDish)
    {
        SetHelpText($"{selectedDish.Name} agregado a tu orden. Total: {RuntimeAppState.Instance.GetCurrentOrderCount()} producto(s).");

        if (addOrderFromArButton != null)
        {
            addOrderFromArButton.text = "Agregado ✓";
            addOrderFromArButton.AddToClassList("ar-add-order-button-added");
            StartCoroutine(RestoreAddOrderButton());
        }

        Debug.Log($"[ARExperienceController] Agregado a orden desde RA: {selectedDish.Name}");
    }

    private System.Collections.IEnumerator RestoreAddOrderButton()
    {
        yield return new WaitForSeconds(1.2f);

        if (addOrderFromArButton != null)
        {
            addOrderFromArButton.text = "Agregar a orden";
            addOrderFromArButton.RemoveFromClassList("ar-add-order-button-added");
        }
    }

    private void BackToMenu()
    {
        if (RuntimeAppState.Instance != null)
        {
            RuntimeAppState.Instance.MarkReturnToMenuAfterAR();
        }

        SceneManager.LoadScene(AppSceneNames.MenuKiosk);
    }
}
