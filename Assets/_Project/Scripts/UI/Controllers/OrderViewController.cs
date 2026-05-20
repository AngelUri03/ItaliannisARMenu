using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class OrderViewController
{
    private readonly VisualElement root;
    private readonly KioskAppState appState;
    private readonly Action onContinueMenu;
    private readonly Action onConfirmOrder;

    private VisualElement orderList;
    private Label orderTotalCountLabel;
    private Label summaryProductCountLabel;
    private ScrollView orderScroll;

    public OrderViewController(
        VisualElement root,
        KioskAppState appState,
        Action onContinueMenu,
        Action onConfirmOrder)
    {
        this.root = root;
        this.appState = appState;
        this.onContinueMenu = onContinueMenu;
        this.onConfirmOrder = onConfirmOrder;
    }

    public void Bind()
    {
        orderList = root.Q<VisualElement>("order-list");
        orderTotalCountLabel = root.Q<Label>("order-total-count-label");
        summaryProductCountLabel = root.Q<Label>("summary-product-count-label");

        orderScroll = root.Q<ScrollView>("order-scroll");
        ConfigureOrderScroll();

        Button continueMenuButton = root.Q<Button>("continue-menu-button");
        Button clearOrderButton = root.Q<Button>("clear-order-button");
        Button confirmOrderButton = root.Q<Button>("confirm-order-button");

        if (continueMenuButton != null)
        {
            continueMenuButton.clicked += onContinueMenu;
        }

        if (clearOrderButton != null)
        {
            clearOrderButton.clicked += ClearOrder;
        }

        if (confirmOrderButton != null)
        {
            confirmOrderButton.clicked += ConfirmOrder;
        }
    }

    private void ConfigureOrderScroll()
    {
        if (orderScroll == null)
        {
            return;
        }

        orderScroll.mode = ScrollViewMode.Vertical;
        orderScroll.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        orderScroll.verticalScrollerVisibility = ScrollerVisibility.Auto;
        orderScroll.touchScrollBehavior = ScrollView.TouchScrollBehavior.Clamped;
    }

    public void Initialize()
    {
        BuildOrderList();
        UpdateOrderSummary();
    }

    private void BuildOrderList()
    {
        if (orderList == null)
        {
            Debug.LogWarning("[OrderViewController] order-list es null.");
            return;
        }

        orderList.Clear();

        if (appState.Order.Count == 0)
        {
            orderList.Add(CreateEmptyOrderCard());
            return;
        }

        List<OrderGroup> groupedOrder = GetGroupedOrder();

        foreach (OrderGroup group in groupedOrder)
        {
            orderList.Add(CreateOrderItemCard(group));
        }
    }

    private VisualElement CreateEmptyOrderCard()
    {
        VisualElement emptyCard = new VisualElement();
        emptyCard.AddToClassList("order-empty-card");

        Label emptyTitle = new Label("Tu orden está vacía");
        emptyTitle.AddToClassList("order-empty-title");

        Label emptyText = new Label("Regresa al menú para seleccionar platillos y agregarlos a la orden simulada de la mesa.");
        emptyText.AddToClassList("order-empty-text");

        emptyCard.Add(emptyTitle);
        emptyCard.Add(emptyText);

        return emptyCard;
    }

    private List<OrderGroup> GetGroupedOrder()
    {
        return appState.Order
            .Where(dish => dish != null)
            .GroupBy(dish => dish.Id)
            .Select(group => new OrderGroup(group.First(), group.Count()))
            .ToList();
    }

    private VisualElement CreateOrderItemCard(OrderGroup orderGroup)
    {
        DishData dish = orderGroup.Dish;

        VisualElement card = new VisualElement();
        card.AddToClassList("order-item-card");

        Image dishImage = new Image();
        dishImage.AddToClassList("order-item-image");
        SetOrderItemImage(dishImage, dish.Id);

        VisualElement info = new VisualElement();
        info.AddToClassList("order-item-info");

        Label name = new Label(dish.Name);
        name.AddToClassList("order-item-name");

        Label meta = new Label(BuildOrderItemMeta(dish));
        meta.AddToClassList("order-item-meta");

        VisualElement badgeRow = new VisualElement();
        badgeRow.AddToClassList("order-item-badge-row");

        if (dish.HasAR)
        {
            Label arBadge = new Label("AR");
            arBadge.AddToClassList("order-item-badge");
            badgeRow.Add(arBadge);
        }

        if (dish.IsShareable)
        {
            Label shareBadge = new Label("Compartir");
            shareBadge.AddToClassList("order-item-badge");
            badgeRow.Add(shareBadge);
        }

        if (dish.IsKidsMenu)
        {
            Label kidsBadge = new Label("Infantil");
            kidsBadge.AddToClassList("order-item-badge");
            badgeRow.Add(kidsBadge);
        }

        info.Add(name);
        info.Add(meta);
        info.Add(badgeRow);

        VisualElement controls = CreateQuantityControls(orderGroup);

        card.Add(dishImage);
        card.Add(info);
        card.Add(controls);

        return card;
    }

    private void SetOrderItemImage(Image targetImage, string dishId)
    {
        if (targetImage == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(dishId))
        {
            targetImage.image = null;
            targetImage.AddToClassList("hidden");
            return;
        }

        Texture2D texture = Resources.Load<Texture2D>($"DishImages/{dishId}");

        if (texture == null)
        {
            Debug.LogWarning($"[OrderViewController] No se encontró imagen para DishId={dishId}. Ruta esperada: Resources/DishImages/{dishId}.png");
            targetImage.image = null;
            targetImage.AddToClassList("hidden");
            return;
        }

        targetImage.image = texture;
        targetImage.scaleMode = ScaleMode.ScaleToFit;
        targetImage.RemoveFromClassList("hidden");
    }

    private VisualElement CreateQuantityControls(OrderGroup orderGroup)
    {
        VisualElement controls = new VisualElement();
        controls.AddToClassList("order-quantity-panel");

        VisualElement quantityRow = new VisualElement();
        quantityRow.AddToClassList("order-quantity-row");

        Button decreaseButton = new Button
        {
            text = "−"
        };
        decreaseButton.AddToClassList("order-quantity-button");

        Label quantityLabel = new Label(orderGroup.Quantity.ToString());
        quantityLabel.AddToClassList("order-quantity-label");

        Button increaseButton = new Button
        {
            text = "+"
        };
        increaseButton.AddToClassList("order-quantity-button");

        Button removeButton = new Button
        {
            text = "Eliminar"
        };
        removeButton.AddToClassList("order-remove-button");

        decreaseButton.clicked += () =>
        {
            RemoveOneDish(orderGroup.Dish);
        };

        increaseButton.clicked += () =>
        {
            AddOneDish(orderGroup.Dish);
        };

        removeButton.clicked += () =>
        {
            RemoveAllDish(orderGroup.Dish);
        };

        quantityRow.Add(decreaseButton);
        quantityRow.Add(quantityLabel);
        quantityRow.Add(increaseButton);

        controls.Add(quantityRow);
        controls.Add(removeButton);

        return controls;
    }

    private string BuildOrderItemMeta(DishData dish)
    {
        if (dish == null)
        {
            return "Producto sin información.";
        }

        string arStatus = dish.HasAR ? "AR disponible" : "Solo menú";
        string shareStatus = dish.IsShareable ? "Para compartir" : "Individual";

        return $"{dish.Category} · {dish.SubCategory} · {dish.Portion} · {dish.Calories} · {arStatus} · {shareStatus}";
    }

    private void AddOneDish(DishData dish)
    {
        if (dish == null)
        {
            return;
        }

        appState.Order.Add(dish);
        BuildOrderList();
        UpdateOrderSummary();
    }

    private void RemoveOneDish(DishData dish)
    {
        if (dish == null)
        {
            return;
        }

        DishData match = appState.Order.FirstOrDefault(item => item != null && item.Id == dish.Id);

        if (match != null)
        {
            appState.Order.Remove(match);
        }

        BuildOrderList();
        UpdateOrderSummary();
    }

    private void RemoveAllDish(DishData dish)
    {
        if (dish == null)
        {
            return;
        }

        appState.Order.RemoveAll(item => item != null && item.Id == dish.Id);

        BuildOrderList();
        UpdateOrderSummary();
    }

    private void UpdateOrderSummary()
    {
        int totalProducts = appState.Order.Count;
        int uniqueProducts = appState.Order
            .Where(dish => dish != null)
            .Select(dish => dish.Id)
            .Distinct()
            .Count();

        if (orderTotalCountLabel != null)
        {
            orderTotalCountLabel.text = $"Productos: {totalProducts}";
        }

        if (summaryProductCountLabel != null)
        {
            summaryProductCountLabel.text = $"{totalProducts} total / {uniqueProducts} únicos";
        }
    }

    private void ClearOrder()
    {
        appState.ClearOrder();
        BuildOrderList();
        UpdateOrderSummary();

        Debug.Log("[OrderViewController] Orden vaciada.");
    }

    private void ConfirmOrder()
    {
        if (appState.Order.Count == 0)
        {
            Debug.LogWarning("[OrderViewController] No se puede confirmar una orden vacía.");
            return;
        }

        Debug.Log($"[OrderViewController] Orden simulada confirmada con {appState.Order.Count} productos.");

        onConfirmOrder?.Invoke();
    }

    private class OrderGroup
    {
        public DishData Dish { get; }
        public int Quantity { get; }

        public OrderGroup(DishData dish, int quantity)
        {
            Dish = dish;
            Quantity = quantity;
        }
    }
}