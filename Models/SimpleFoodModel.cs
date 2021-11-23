namespace MealPlanner.CoreLibrary.Models;
public class SimpleFoodModel
{
    public int ID { get; set; }
    public string MainCourse { get; set; } = "";
    public DateTime? LastHave { get; set; } //has to keep the time part for now because of sql server problems.
    public EnumMeal Meal { get; set; }
}