namespace MealPlanner.CoreLibrary.Models;
public class MealPlannerCreaterResultModel : IComparable<MealPlannerCreaterResultModel>
{
    public DateTime WhatDate { get; set; }
    public string DisplayDate => WhatDate.ToLongDateString();
    public SimpleFoodModel Breakfast { get; set; } = new();
    public SimpleFoodModel Lunch { get; set; } = new();
    public SimpleFoodModel Dinner { get; set; } = new();
    int IComparable<MealPlannerCreaterResultModel>.CompareTo(MealPlannerCreaterResultModel? other)
    {
        return WhatDate.CompareTo(other!.WhatDate);
    }
}