using System;
using UnityEngine.UIElements;

public class ConfirmationViewController
{
    private readonly VisualElement root;
    private readonly KioskAppState appState;

    private readonly Action onNewOrder;
    private readonly Action onBackMenu;
    private readonly Action onBackStart;

    private Label confirmationProductCountLabel;
    private Label confirmationMessageLabel;

    public ConfirmationViewController(
        VisualElement root,
        KioskAppState appState,
        Action onNewOrder,
        Action onBackMenu,
        Action onBackStart)
    {
        this.root = root;
        this.appState = appState;
        this.onNewOrder = onNewOrder;
        this.onBackMenu = onBackMenu;
        this.onBackStart = onBackStart;
    }

    public void Bind()
    {
        confirmationProductCountLabel = root.Q<Label>("confirmation-product-count-label");
        confirmationMessageLabel = root.Q<Label>("confirmation-message-label");

        Button newOrderButton = root.Q<Button>("new-order-button");
        Button backMenuButton = root.Q<Button>("back-menu-button");
        Button backStartButton = root.Q<Button>("back-start-button");

        if (newOrderButton != null)
        {
            newOrderButton.clicked += onNewOrder;
        }

        if (backMenuButton != null)
        {
            backMenuButton.clicked += onBackMenu;
        }

        if (backStartButton != null)
        {
            backStartButton.clicked += onBackStart;
        }
    }

    public void Initialize()
    {
        int productCount = appState.Order.Count;

        if (confirmationProductCountLabel != null)
        {
            confirmationProductCountLabel.text = productCount.ToString();
        }

        if (confirmationMessageLabel != null)
        {
            confirmationMessageLabel.text = $"Tu orden simulada con {productCount} producto(s) fue registrada correctamente en este prototipo.";
        }
    }
}