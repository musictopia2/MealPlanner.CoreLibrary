
namespace MealPlanner.CoreLibrary.Models;
public class SpecialInstructionModel : IMappable
{
    public int ID { get; set; }
    [Required]
    public string FoodName { get; set; } = "";
    [Required]
    public string Instructions { get; set; } = "";
    [Range(1, int.MaxValue)]
    public int FirstTime { get; set; }
    public int? SecondTime { get; set; }
    public int? ThirdTime { get; set; }
}