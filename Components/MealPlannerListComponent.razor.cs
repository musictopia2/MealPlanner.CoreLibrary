namespace MealPlanner.CoreLibrary.Components;
public partial class MealPlannerListComponent
{
    [Parameter]
    public BasicList<MealPlannerCreaterResultModel> ItemList { get; set; } = new();
    private readonly BasicList<string> _headers = new()
    {
        "Date",
        "Breakfast",
        "Lunch",
        "Dinner"
    };
}