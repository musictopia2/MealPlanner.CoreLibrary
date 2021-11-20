namespace MealPlanner.CoreLibrary.Storage;
public interface IRecipesStorage
{
    BasicList<RecipeModel> GetRecipes();
    void SaveRecipes(BasicList<RecipeModel> recipes);
}