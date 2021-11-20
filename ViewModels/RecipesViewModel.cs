namespace MealPlanner.CoreLibrary.ViewModels;

//hint:
//in november 2021, rethink the recipes part.  because we have the mobile versions but also other versions.
//refer to contact manager processes.
//after november 2021, still not sure.  later can rethink.
public class RecipesViewModel
{
    private readonly IRecipesStorage _storage;
    private readonly IRecipeService _service;
    private readonly IToast _toast;
    public BasicList<RecipeModel> Recipes { private set; get; }
    public RecipeModel? RecipeChosen { get; private set; }
    public string Status { get; set; } = "";
    public Action? StateChanged { get; set; }
    public bool ShowBack => RecipeChosen is not null;
    public string Title => RecipeChosen is not null ? RecipeChosen.Name : "Recipes List";
    public RecipesViewModel(IRecipesStorage storage,
        IRecipeService service,
        IToast toast
        )
    {
        _storage = storage;
        _service = service;
        _toast = toast;
        Recipes = _storage.GetRecipes();
    }
    public async Task DownloadRecipesAsync()
    {
        if (RecipeChosen is not null)
        {
            throw new CustomBasicException("There was a recipe chosen.  Therefore, should not have allowed downloading recipes");
        }
        Status = "Downloading Recipes";
        StateChanged?.Invoke();
        await Task.Delay(10);
        var list = await _service.GetRecipesAsync();
        if (list.Count == 0)
        {
            _toast.ShowWarningToast("There was no recipes");
            Status = "";
            return;
        }
        Recipes = list;
        _storage.SaveRecipes(list);
        Status = "";
    }
    public void GoBack()
    {
        RecipeChosen = null;
    }
    public void ChooseRecipe(RecipeModel recipe)
    {
        RecipeChosen = recipe;
    }
}