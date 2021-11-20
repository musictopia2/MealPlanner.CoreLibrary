namespace MealPlanner.CoreLibrary.Models;
public class SimpleFoodModel
{
    public int ID { get; set; }
    public string MainCourse { get; set; } = "";
    public DateOnly? LastHave { get; set; }
    public EnumMeal Meal { get; set; }
}