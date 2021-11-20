namespace MealPlanner.CoreLibrary.Storage;
public class RecipesFileStorage : IRecipesStorage
{
    private readonly string _recipesPath = "Recipes.json";
    public RecipesFileStorage()
    {
        if (bb.OS == bb.EnumOS.Android)
        {
            _recipesPath = Path.Combine(ff.GetApplicationDataForMobileDevices(), _recipesPath);
        }
        else if (bb.OS == bb.EnumOS.WindowsDT)
        {
            _recipesPath = Path.Combine(aa.GetApplicationPath(), _recipesPath);
        }
        else
        {
            throw new CustomBasicException("FileStorage Implementation only supports Desktop, or Android");
        }
    }
    BasicList<RecipeModel> IRecipesStorage.GetRecipes()
    {
        if (ff.FileExists(_recipesPath) == false)
        {
            return new();
        }
        return jj.RetrieveSavedObject<BasicList<RecipeModel>>(_recipesPath);
    }
    void IRecipesStorage.SaveRecipes(BasicList<RecipeModel> recipes)
    {
        jj.SaveObject(_recipesPath, recipes);
    }
}