using System.Collections.Generic;

public class KioskAppState
{
    public List<DishData> Dishes { get; }
    public List<DishData> Order { get; }

    public DishData SelectedDish { get; set; }
    public string CurrentCategory { get; set; }
    public string CurrentSectionFilter { get; set; }

    public KioskAppState(List<DishData> dishes)
    {
        Dishes = dishes;
        Order = new List<DishData>();
        CurrentCategory = "Recomendados";
        CurrentSectionFilter = "Recomendados";
    }

    public void ClearOrder()
    {
        Order.Clear();
    }

    public void ResetForNewOrder()
    {
        Order.Clear();
        SelectedDish = null;
        CurrentCategory = "Recomendados";
        CurrentSectionFilter = "Recomendados";
    }
}