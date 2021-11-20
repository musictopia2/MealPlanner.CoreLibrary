namespace MealPlanner.CoreLibrary.Services;
public interface IRecipeService
{
    Task<BasicList<RecipeModel>> GetRecipesAsync();
    Task AddNewRecipeAsync(RecipeModel recipe);
    Task UpdateRecipeAsync(RecipeModel recipe);
}