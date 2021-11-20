namespace MealPlanner.CoreLibrary.Services;
public interface IMealService
{
    Task<EnumMeal> GetCurrentMealAsync();
    Task SaveCurrentMealAsync(EnumMeal meal);
    Task DeleteMealAsync();
}