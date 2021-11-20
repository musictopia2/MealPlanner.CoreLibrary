namespace MealPlanner.CoreLibrary.Models;
public class CompleteDataModel
{
    public DateOnly CurrentDate { get; set; } //time does not matter in this case.
    public BasicList<SimpleFoodModel> CompleteList { get; set; } = new();
}