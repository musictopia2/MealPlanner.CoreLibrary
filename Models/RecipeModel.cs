namespace MealPlanner.CoreLibrary.Models;
public class RecipeModel : IComparable<RecipeModel>, IMappable
{
    [Required]
    public string Name { get; set; } = "";
    [Required]
    public string Detail { get; set; } = "";
    int IComparable<RecipeModel>.CompareTo(RecipeModel? other)
    {
        return Name.CompareTo(other!.Name);
    }
}