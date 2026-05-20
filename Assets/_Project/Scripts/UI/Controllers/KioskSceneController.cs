using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class KioskSceneController : MonoBehaviour
{
    [Header("UI Document")]
    [SerializeField] private UIDocument uiDocument;

    [Header("Views")]
    [SerializeField] private VisualTreeAsset welcomeViewAsset;
    [SerializeField] private VisualTreeAsset menuHomeViewAsset;
    [SerializeField] private VisualTreeAsset orderViewAsset;
    [SerializeField] private VisualTreeAsset confirmationViewAsset;
    [SerializeField] private VisualTreeAsset recommendationsViewAsset;

    [Header("Style Sheets")]
    [SerializeField] private StyleSheet[] styleSheets;

    private VisualElement root;
    private VisualElement screenHost;

    private KioskAppState appState;

    private void OnEnable()
    {
        ResolveReferences();
        LoadStyleSheets();
        CreateState();

        if (RuntimeAppState.Instance != null && RuntimeAppState.Instance.ConsumeReturnToMenuAfterAR())
        {
            RestoreStateFromRuntime();
            ShowMenuHomeView(appState.SelectedDish);
            return;
        }

        ShowWelcomeView();
    }

    private void RestoreStateFromRuntime()
    {
        if (RuntimeAppState.Instance == null || appState == null)
        {
            return;
        }

        RuntimeAppState runtime = RuntimeAppState.Instance;

        appState.Order.Clear();

        foreach (string dishId in runtime.CurrentOrderDishIds)
        {
            DishData dish = appState.Dishes.FirstOrDefault(item => item.Id == dishId);

            if (dish != null)
            {
                appState.Order.Add(dish);
            }
        }

        if (!string.IsNullOrWhiteSpace(runtime.SavedMenuSectionFilter))
        {
            appState.CurrentSectionFilter = runtime.SavedMenuSectionFilter;
        }

        if (!string.IsNullOrWhiteSpace(runtime.SavedMenuCategory))
        {
            appState.CurrentCategory = runtime.SavedMenuCategory;
        }

        if (!string.IsNullOrWhiteSpace(runtime.SavedMenuSelectedDishId))
        {
            appState.SelectedDish = appState.Dishes.FirstOrDefault(dish => dish.Id == runtime.SavedMenuSelectedDishId);
        }

        Debug.Log($"[KioskSceneController] Estado restaurado. Orden={appState.Order.Count}, Filtro={appState.CurrentSectionFilter}, Categoria={appState.CurrentCategory}");
    }

    private void ResolveReferences()
    {
        if (uiDocument == null)
        {
            uiDocument = FindFirstObjectByType<UIDocument>();
        }

        if (uiDocument == null)
        {
            Debug.LogError("[KioskSceneController] No se encontró ningún UIDocument en la escena.");
            return;
        }

        root = uiDocument.rootVisualElement;

        if (root == null)
        {
            Debug.LogError("[KioskSceneController] El rootVisualElement del UIDocument es null.");
            return;
        }

        screenHost = root.Q<VisualElement>("screen-host");

        if (screenHost == null)
        {
            Debug.LogError("[KioskSceneController] No se encontró el VisualElement con name='screen-host'. Revisa AppRoot.uxml.");
        }
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

    private void CreateState()
    {
        appState = new KioskAppState(MenuRepository.GetDishes());
    }

    private VisualElement LoadView(VisualTreeAsset viewAsset, string viewName)
    {
        if (screenHost == null)
        {
            Debug.LogError($"[KioskSceneController] No se puede mostrar {viewName} porque screenHost es null.");
            return null;
        }

        screenHost.Clear();

        if (viewAsset == null)
        {
            Debug.LogError($"[KioskSceneController] No se asignó {viewName}.uxml en el Inspector.");
            return null;
        }

        VisualElement view = viewAsset.CloneTree();
        view.AddToClassList("screen");

        screenHost.Add(view);

        return view;
    }

    public void ShowWelcomeView()
    {
        VisualElement view = LoadView(welcomeViewAsset, "WelcomeView");

        if (view == null)
        {
            return;
        }

        WelcomeViewController controller = new WelcomeViewController(
            view,
            ShowMenuHomeView
        );

        controller.Bind();
    }

    public void ShowMenuHomeView()
    {
        ShowMenuHomeView(null);
    }

    public void ShowMenuHomeView(DishData dishToSelect)
    {
        VisualElement view = LoadView(menuHomeViewAsset, "MenuHomeView");

        if (view == null)
        {
            return;
        }

        MenuHomeViewController controller = new MenuHomeViewController(
            view,
            appState,
            ShowWelcomeView,
            ShowOrderView,
            () => ShowRecommendationsView("classic"),
            () => ShowRecommendationsView("ar"),
            OnViewARClicked
        );

        controller.Bind();
        controller.Initialize(dishToSelect);
    }

    public void ShowOrderView()
    {
        VisualElement view = LoadView(orderViewAsset, "OrderView");

        if (view == null)
        {
            return;
        }

        OrderViewController controller = new OrderViewController(
            view,
            appState,
            ShowMenuHomeView,
            ShowConfirmationView
        );

        controller.Bind();
        controller.Initialize();
    }

    public void ShowConfirmationView()
    {
        VisualElement view = LoadView(confirmationViewAsset, "ConfirmationView");

        if (view == null)
        {
            return;
        }

        ConfirmationViewController controller = new ConfirmationViewController(
            view,
            appState,
            StartNewOrder,
            ShowMenuHomeView,
            ShowWelcomeView
        );

        controller.Bind();
        controller.Initialize();
    }

    public void ShowRecommendationsView(string initialGroup)
    {
        VisualElement view = LoadView(recommendationsViewAsset, "RecommendationsView");

        if (view == null)
        {
            return;
        }

        RecommendationsViewController controller = new RecommendationsViewController(
            view,
            appState,
            ShowMenuHomeView,
            ShowMenuHomeView
        );

        controller.Bind();
        controller.Initialize(initialGroup);
    }

    private void StartNewOrder()
    {
        appState.ResetForNewOrder();

        if (RuntimeAppState.Instance != null)
        {
            RuntimeAppState.Instance.ClearMenuSnapshot();
            RuntimeAppState.Instance.ClearSelectedDish();
        }

        ShowMenuHomeView();
    }

    private void OnViewARClicked(DishData dish)
    {
        if (dish == null)
        {
            Debug.LogWarning("[KioskSceneController] No hay platillo seleccionado para RA.");
            return;
        }

        if (!dish.HasAR)
        {
            Debug.LogWarning($"[KioskSceneController] El platillo {dish.Name} no tiene RA disponible.");
            return;
        }

        if (RuntimeAppState.Instance == null)
        {
            Debug.LogError("[KioskSceneController] No existe RuntimeAppState en la escena.");
            return;
        }

        RuntimeAppState.Instance.SetSelectedDish(dish);
        RuntimeAppState.Instance.SaveMenuState(appState);
        RuntimeAppState.Instance.MarkReturnToMenuAfterAR();

        Debug.Log($"[KioskSceneController] Cargando escena AR para: {dish.Name}");

        SceneManager.LoadScene(AppSceneNames.ARExperience);
    }

}