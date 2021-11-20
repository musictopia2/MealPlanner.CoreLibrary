namespace MealPlanner.CoreLibrary.Services;
public interface IFinalResultsService
{
    Task<bool> HasFinalResultsAsync();
    Task<BasicList<MealPlannerCreaterResultModel>> GetFinalResultsAsync();
    Task SaveFinalResultsAsync(BasicList<MealPlannerCreaterResultModel> results);
    Task DeleteFinalResultsAsync();
}