namespace MealPlanner.CoreLibrary.ViewModels;
public class ShellViewModel(ICompleteDataService completeDataService,
    IFinalResultsService finalResultsService,
    IMealService mealService,
    IMealPlannerCreaterService createrService,
    IMessageBox message,
    IExit exit
        )
{
    private EnumMeal _currentMeal;
    private CompleteDataModel? _currentData;
    public BasicList<MealPlannerCreaterResultModel> FinalList { get; private set; } = new();
    public string ProgressData { get; set; } = "";
    public bool CanDeleteMeal { get; private set; }
    public bool AlsoCurrentDay { get; set; }
    public string EnteredSoFar { get; set; } = "";
    public string CurrentDisplayDate { get; set; } = "";
    public string CurrentFood { get; private set; } = "";
    public string GetListText => _currentData is null ? "Meal List" : $"Meal List For {_currentData.CurrentDate.GetLongDate()} For {_currentMeal}";
    public BasicList<SimpleFoodModel> CurrentList { get; private set; } = new();
    public bool CanSaveToNetwork { get; private set; }
    public Action? StateHasChanged { get; set; }
    public Action? FocusFirst { get; set; }
    public async Task SaveToNetworkAsync()
    {
        ProgressData = "Saving Meals";
        StateHasChanged?.Invoke();
        await createrService.SaveMealsAsync(FinalList);
        ProgressData = "Deleting Temporary Files";
        StateHasChanged?.Invoke();
        await DeleteFilesAsync();
        await message.ShowMessageAsync("Meals Saved");
        exit.ExitApp();
    }
    private async Task DeleteFilesAsync()
    {
        await completeDataService.DeleteCompleteDataAsync();
        await finalResultsService.DeleteFinalResultsAsync();
        await mealService.DeleteMealAsync();
        CanSaveToNetwork = false;
        CanDeleteMeal = false;
        _currentData = new();
        _currentMeal = EnumMeal.Breakfast;
        FinalList.Clear();
        ClearItems();
    }
    private void ClearItems()
    {
        CurrentFood = "";
        CurrentDisplayDate = "";
    }
    private async Task SaveMealAsync() => await SaveMealAsync(true, null, "");
    public async Task SkipMealAsync()
    {
        await SaveMealAsync();
    }
    public async Task SaveNonSkippedMealAsync(SimpleFoodModel? food = null)
    {
        if (EnteredSoFar == "" || EnteredSoFar == "N/A")
        {
            if (EnteredSoFar == "N/A")
            {
                EnteredSoFar = "";
                return;
            }
            await RepeatSingleMealAsync();
            return;
        }
        if (food == null)
        {
            await SaveMealAsync(false, null, EnteredSoFar);

        }
        else
        {
            await SaveMealAsync(false, food, "");
        }
    }
    public async Task StartOverAsync()
    {
        ProgressData = "Deleting Files To Start Over";
        StateHasChanged?.Invoke();
        await DeleteFilesAsync();
        _needsInit = true;
        await InitAsync();
    }
    private void UpdateCombo()
    {
        if (_currentMeal == EnumMeal.Breakfast)
        {
            CurrentList = _currentData!.CompleteList.Where(xx => xx.Meal == EnumMeal.Breakfast).ToBasicList();
        }
        else
        {
            CurrentList = _currentData!.CompleteList.Where(xx => xx.Meal != EnumMeal.Breakfast).ToBasicList();
        }
    }
    public async Task DeleteLastMealAsync()
    {
        if (FinalList.Count == 0)
        {
            throw new CustomBasicException("Should not have allowed a meal to be deleted because there was none");
        }
        MealPlannerCreaterResultModel item;
        if (_currentMeal != EnumMeal.Breakfast)
        {
            item = FinalList.Last();
            if (_currentMeal == EnumMeal.Lunch)
            {
                FinalList.RemoveSpecificItem(item);
                _currentMeal = EnumMeal.Breakfast;
                await SaveSectionAsync();
                UpdateCombo();
                FocusFirst?.Invoke();
                return;
            }
            if (_currentMeal == EnumMeal.Dinner)
            {
                item.Lunch.MainCourse = "";
                item.Lunch.ID = 0;
                _currentMeal = EnumMeal.Lunch;
                await SaveSectionAsync();
                FocusFirst?.Invoke();
                return;
            }
            throw new CustomBasicException("Not Sure");
        }
        CanSaveToNetwork = false;
        item = FinalList.OrderByDescending(items => items.WhatDate).Take(1).Single();
        _currentData!.CurrentDate = _currentData.CurrentDate.AddDays(-1); //to back track.
        item.Dinner.MainCourse = "";
        item.Dinner.ID = 0;
        _currentMeal = EnumMeal.Dinner;
        await SaveSectionAsync();
        UpdateCombo();
        FocusFirst?.Invoke();
    }
    private async Task SaveSectionAsync()
    {
        await mealService.SaveCurrentMealAsync(_currentMeal);
        await finalResultsService.SaveFinalResultsAsync(FinalList);
    }
    private async Task SaveMealAsync(bool skippedMeal, SimpleFoodModel? payLoad, string foodName, MealPlannerCreaterResultModel? previousItem = null, bool hasMore = false)
    {
        CanSaveToNetwork = false;
        MealPlannerCreaterResultModel result;
        bool wasBreakfast = _currentMeal == EnumMeal.Breakfast;
        if (_currentMeal == EnumMeal.Breakfast)
        {
            result = new();
            result.WhatDate = _currentData!.CurrentDate;
            result.Breakfast.LastHave = _currentData.CurrentDate.ToDateTime();
            if (previousItem == null)
            {
                if (skippedMeal)
                {
                    result.Breakfast.MainCourse = "N/A";
                }
                else if (payLoad != null)
                {
                    result.Breakfast.MainCourse = payLoad.MainCourse;
                    result.Breakfast.ID = payLoad.ID;
                }
                else
                {
                    result.Breakfast.MainCourse = foodName;
                }
            }
            else
            {
                result.Breakfast.MainCourse = previousItem.Breakfast.MainCourse;
                result.Breakfast.ID = previousItem.Breakfast.ID;
            }
            FinalList.Add(result);
            _currentMeal = EnumMeal.Lunch;
            if (hasMore == false)
            {
                await SaveSectionAsync();
            }
        }
        else
        {
            result = FinalList.Last();
        }
        if (_currentMeal == EnumMeal.Lunch && wasBreakfast == false)
        {
            result.Lunch.LastHave = _currentData!.CurrentDate.ToDateTime();
            if (previousItem == null)
            {
                if (skippedMeal)
                {
                    result.Lunch.MainCourse = "N/A";
                }
                else if (payLoad != null)
                {
                    result.Lunch.MainCourse = payLoad.MainCourse;
                    result.Lunch.ID = payLoad.ID;
                }
                else
                {
                    result.Lunch.MainCourse = foodName;
                }
            }
            else
            {
                result.Lunch.MainCourse = previousItem.Lunch.MainCourse;
                result.Lunch.ID = previousItem.Lunch.ID;
            }
            _currentMeal = EnumMeal.Dinner;
            if (hasMore == false)
            {
                await SaveSectionAsync();
            }
        }
        else if (_currentMeal == EnumMeal.Dinner)
        {
            result.Lunch.LastHave = _currentData!.CurrentDate.ToDateTime();
            if (previousItem == null)
            {
                if (skippedMeal)
                {
                    result.Dinner.MainCourse = "N/A";
                }
                else if (payLoad != null)
                {
                    result.Dinner.MainCourse = payLoad.MainCourse;
                    result.Dinner.ID = payLoad.ID;
                }
                else
                {
                    result.Dinner.MainCourse = foodName;
                }
            }
            else
            {
                result.Dinner.MainCourse = previousItem.Dinner.MainCourse;
                result.Dinner.ID = previousItem.Dinner.ID;
            }
            _currentData.CurrentDate = _currentData.CurrentDate.AddDays(1);
            await PrivateGetNextMealsAsync(_currentData.CurrentDate);
            await finalResultsService.SaveFinalResultsAsync(FinalList);
        }
        EnteredSoFar = "";
        if (hasMore == false)
        {
            UpdateCombo();
            ClearItems();
            if (_currentMeal == EnumMeal.Breakfast)
            {
                CanSaveToNetwork = true;
            }
            FocusFirst?.Invoke();
        }
        CanDeleteMeal = true;
    }
    private async Task RepeatSingleMealAsync()
    {
        if (FinalList.Count == 0)
        {
            return;
        }
        var lastItem = FinalList.Last();
        if (FinalList.Count == 1 && _currentMeal == EnumMeal.Lunch)
        {
            return;
        }
        if (FinalList.Count == 1 && _currentMeal == EnumMeal.Dinner)
        {
            await SaveMealAsync(false, null, "", lastItem);
            return;
        }
        if (_currentMeal == EnumMeal.Breakfast)
        {
            await SaveMealAsync(false, null, "", lastItem);
            return;
        }
        lastItem = FinalList.OrderByDescending(items => items.WhatDate).Skip(1).Take(1).Single();
        await SaveMealAsync(false, null, "", lastItem);
    }
    public void UpdateFoodSelected()
    {
        if (EnteredSoFar == "")
        {
            CurrentDisplayDate = "";
            CurrentFood = "";
            return;
        }
        var thisItem = _currentData!.CompleteList.SingleOrDefault(items => items.MainCourse == EnteredSoFar);
        if (thisItem == null)
        {
            CurrentDisplayDate = "";
            CurrentFood = "";
            return;
        }
        if (thisItem.LastHave.HasValue == false)
        {
            CurrentDisplayDate = "Start";
        }
        else
        {
            CurrentDisplayDate = thisItem.LastHave!.Value.ToString("M/d/yyyy");
        }
        CurrentFood = thisItem.MainCourse;
    }
    private async Task PrivateGetNextMealsAsync(DateOnly? nextDate = null)
    {
        DateOnly currentDate;
        if (nextDate.HasValue)
        {
            currentDate = nextDate.Value;
        }
        else
        {
            ProgressData = "Getting Date";
            StateHasChanged?.Invoke();
            await Task.Delay(200);
            currentDate = await createrService.GetNextDateAsync(AlsoCurrentDay);
        }
        _currentData = new();
        _currentData.CompleteList = await createrService.GetPossibleMealsForDayAsync(currentDate);
        _currentData.CurrentDate = currentDate;
        _currentMeal = EnumMeal.Breakfast;
        await mealService.SaveCurrentMealAsync(_currentMeal);
        ProgressData = "Saving Meals For The Day";
        StateHasChanged?.Invoke();
        await completeDataService.SaveCompleteDataAsync(_currentData);
        ProgressData = "";
    }
    private bool _needsInit = true;
    public async Task InitAsync()
    {
        if (_needsInit == false)
        {
            return;
        }
        if (await completeDataService.MealsExistAsync() == false)
        {
            CanDeleteMeal = false;
            await PrivateGetNextMealsAsync();
        }
        else
        {
            _currentData = await completeDataService.GetCompleteDataAsync();
            _currentMeal = await mealService.GetCurrentMealAsync();
        }
        if (await finalResultsService.HasFinalResultsAsync() == false)
        {
            FinalList.Clear();
            CanDeleteMeal = false;
            await finalResultsService.SaveFinalResultsAsync(FinalList);
        }
        else
        {
            FinalList = await finalResultsService.GetFinalResultsAsync();
            if (_currentMeal == EnumMeal.Breakfast && FinalList.Count > 0)
            {
                CanSaveToNetwork = true;
            }
        }
        ClearItems();
        UpdateCombo();
        EnteredSoFar = "";
        CanDeleteMeal = FinalList.Count > 0;
        _needsInit = false;
        FocusFirst?.Invoke();
    }
    public bool CanRepeatDay => _currentMeal == EnumMeal.Breakfast && FinalList.Count > 0;
    public async Task RepeatDayAsync()
    {
        var previousItem = FinalList.Last();
        await 3.TimesAsync(async x =>
        {
            bool rets = x < 3;
            await SaveMealAsync(false, null, "", previousItem, rets);
        });
    }
}