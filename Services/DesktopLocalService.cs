using fs = CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.FileHelpers;
namespace MealPlanner.CoreLibrary.Services;
public class DesktopLocalService : ICompleteDataService, IFinalResultsService, IMealService
{
    private string _completePath = "completelist.json";
    private string _mealPath = "currentmeal.json";
    private string _finalPath = "finallist.json";
    public DesktopLocalService()
    {
        _completePath = $"{aa.GetApplicationPath()}/{_completePath}";
        _mealPath = $"{aa.GetApplicationPath()}/{_mealPath}";
        _finalPath = $"{aa.GetApplicationPath()}/{_finalPath}";
    }
    Task ICompleteDataService.DeleteCompleteDataAsync()
    {
        return ff.DeleteFileAsync(_completePath);
    }
    Task IFinalResultsService.DeleteFinalResultsAsync()
    {
        return ff.DeleteFileAsync(_finalPath);
    }
    Task IMealService.DeleteMealAsync()
    {
        return ff.DeleteFileAsync(_mealPath);
    }
    Task<CompleteDataModel> ICompleteDataService.GetCompleteDataAsync()
    {
        return fs.RetrieveSavedObjectAsync<CompleteDataModel>(_completePath);
    }
    Task<EnumMeal> IMealService.GetCurrentMealAsync()
    {
        return fs.RetrieveSavedObjectAsync<EnumMeal>(_mealPath);
    }
    Task<BasicList<MealPlannerCreaterResultModel>> IFinalResultsService.GetFinalResultsAsync()
    {
        return fs.RetrieveSavedObjectAsync<BasicList<MealPlannerCreaterResultModel>>(_finalPath);
    }
    Task<bool> IFinalResultsService.HasFinalResultsAsync()
    {
        return Task.FromResult(ff.FileExists(_finalPath));
    }
    Task<bool> ICompleteDataService.MealsExistAsync()
    {
        return Task.FromResult(ff.FileExists(_completePath));
    }
    Task ICompleteDataService.SaveCompleteDataAsync(CompleteDataModel data)
    {
        return fs.SaveObjectAsync(_completePath, data);
    }
    Task IMealService.SaveCurrentMealAsync(EnumMeal meal)
    {
        return fs.SaveObjectAsync(_mealPath, meal);
    }
    Task IFinalResultsService.SaveFinalResultsAsync(BasicList<MealPlannerCreaterResultModel> results)
    {
        return fs.SaveObjectAsync(_finalPath, results);
    }
}