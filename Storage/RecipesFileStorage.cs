namespace MealPlanner.CoreLibrary.Storage;
public class RecipesFileStorage : IRecipesStorage
{
    private readonly string _recipesPath = "Recipes.json";
    public RecipesFileStorage()
    {
        if (bb1.OS == bb1.EnumOS.Android)
        {
            _recipesPath = Path.Combine(ff1.GetApplicationDataForMobileDevices(), _recipesPath);
        }
        else if (bb1.OS == bb1.EnumOS.WindowsDT)
        {
            _recipesPath = Path.Combine(aa1.GetApplicationPath(), _recipesPath);
        }
        else
        {
            throw new CustomBasicException("FileStorage Implementation only supports Desktop, or Android");
        }
    }
    BasicList<RecipeModel> IRecipesStorage.GetRecipes()
    {
        if (ff1.FileExists(_recipesPath) == false)
        {
            return new();
        }
        return jj1.RetrieveSavedObject<BasicList<RecipeModel>>(_recipesPath);
    }
    void IRecipesStorage.SaveRecipes(BasicList<RecipeModel> recipes)
    {
        jj1.SaveObject(_recipesPath, recipes);
    }
}