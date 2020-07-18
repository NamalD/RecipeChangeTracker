module RecipeChangeTracker.API.Mappings

open RecipeChangeTracker.API.Models
open RecipeChangeTracker.Firestore.RecipeStates

let mapRecipeResponse (recipe:RecipeState) : RecipeResponse =
    { Id = recipe.Id;
      Name = recipe.Name;
      Steps = recipe.Steps;
      Ingredients = recipe.Ingredients;
      CookTime = recipe.CookTime }

let mapRecipesResponse (recipes:RecipeState seq) : RecipeResponse seq =
    Seq.map mapRecipeResponse recipes

