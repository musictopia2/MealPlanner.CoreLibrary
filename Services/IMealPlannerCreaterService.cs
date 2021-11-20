namespace MealPlanner.CoreLibrary.Services;
public interface IMealPlannerCreaterService
{
    Task<DateOnly> GetNextDateAsync(bool canHaveCurrent);
    Task<BasicList<SimpleFoodModel>> GetPossibleMealsForDayAsync(DateOnly date);
    Task SaveMealsAsync(BasicList<MealPlannerCreaterResultModel> list);
}