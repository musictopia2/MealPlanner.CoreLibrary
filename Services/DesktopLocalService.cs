using fs1 = CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.FileHelpers;
namespace MealPlanner.CoreLibrary.Services;
public class DesktopLocalService : ICompleteDataService, IFinalResultsService, IMealService
{
    private readonly string _completePath = "completelist.json";
    private readonly string _mealPath = "currentmeal.json";
    private readonly string _finalPath = "finallist.json";
    public DesktopLocalService()
    {
        _completePath = $"{aa1.GetApplicationPath()}/{_completePath}";
        _mealPath = $"{aa1.GetApplicationPath()}/{_mealPath}";
        _finalPath = $"{aa1.GetApplicationPath()}/{_finalPath}";
    }
    Task ICompleteDataService.DeleteCompleteDataAsync()
    {
        return ff1.DeleteFileAsync(_completePath);
    }
    Task IFinalResultsService.DeleteFinalResultsAsync()
    {
        return ff1.DeleteFileAsync(_finalPath);
    }
    Task IMealService.DeleteMealAsync()
    {
        return ff1.DeleteFileAsync(_mealPath);
    }
    Task<CompleteDataModel> ICompleteDataService.GetCompleteDataAsync()
    {
        return fs1.RetrieveSavedObjectAsync<CompleteDataModel>(_completePath);
    }
    Task<EnumMeal> IMealService.GetCurrentMealAsync()
    {
        return fs1.RetrieveSavedObjectAsync<EnumMeal>(_mealPath);
    }
    Task<BasicList<MealPlannerCreaterResultModel>> IFinalResultsService.GetFinalResultsAsync()
    {
        return fs1.RetrieveSavedObjectAsync<BasicList<MealPlannerCreaterResultModel>>(_finalPath);
    }
    Task<bool> IFinalResultsService.HasFinalResultsAsync()
    {
        return Task.FromResult(ff1.FileExists(_finalPath));
    }
    Task<bool> ICompleteDataService.MealsExistAsync()
    {
        return Task.FromResult(ff1.FileExists(_completePath));
    }
    Task ICompleteDataService.SaveCompleteDataAsync(CompleteDataModel data)
    {
        return fs1.SaveObjectAsync(_completePath, data);
    }
    Task IMealService.SaveCurrentMealAsync(EnumMeal meal)
    {
        return fs1.SaveObjectAsync(_mealPath, meal);
    }
    Task IFinalResultsService.SaveFinalResultsAsync(BasicList<MealPlannerCreaterResultModel> results)
    {
        return fs1.SaveObjectAsync(_finalPath, results);
    }
}