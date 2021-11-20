namespace MealPlanner.CoreLibrary.Services;
public interface ICompleteDataService
{
    Task<bool> MealsExistAsync();
    Task<CompleteDataModel> GetCompleteDataAsync();
    Task SaveCompleteDataAsync(CompleteDataModel data);
    Task DeleteCompleteDataAsync();
}