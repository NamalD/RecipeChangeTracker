using Carter;
using Carter.ModelBinding;
using Carter.Response;
using Microsoft.AspNetCore.Http;
using RecipeChangeTracker.API.Metadata;
using RecipeChangeTracker.Models;
using RecipeChangeTracker.RecipeStore.Core;

namespace RecipeChangeTracker.API.Modules
{
    public class RecipeModule : CarterModule
    {
        private readonly IRecipeStore _recipeStore;

        public RecipeModule(IRecipeStore recipeStore)
        {
            _recipeStore = recipeStore;

            // Get all recipes
            Get<GetRecipes>("/recipes", async (req, res) =>
            {
                var recipes = await _recipeStore.GetAllRecipesAsync();
                await res.AsJson(recipes);
            });

            // Get single recipe
            Get<GetRecipe>("/recipes/{name}", async (req, res) => await res.WriteAsync("Get single recipe!"));

            // Get history of recipe
            Get<GetRecipeHistory>("/recipes/{name}/history", async (req, res) => await res.WriteAsync("Get history of recipe!"));

            // Add or Update recipe
            Put<AddUpdateRecipe>("/recipes", async (req, res) =>
            {
                var recipe = await req.Bind<Recipe>();
                _recipeStore.AddOrUpdate(recipe);

                await res.WriteAsync("OK");
            });
        }
    }
}
