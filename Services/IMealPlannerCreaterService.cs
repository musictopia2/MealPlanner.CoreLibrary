namespace MealPlanner.CoreLibrary.Services;
public interface IMealPlannerCreaterService
{
    Task<DateTime> GetNextDateAsync(bool canHaveCurrent);
    Task<BasicList<SimpleFoodModel>> GetPossibleMealsForDayAsync(DateTime date);
    Task SaveMealsAsync(BasicList<MealPlannerCreaterResultModel> list);
}