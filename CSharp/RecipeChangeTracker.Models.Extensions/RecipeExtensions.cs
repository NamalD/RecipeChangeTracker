using RecipeChangeTracker.CSharp.Models;
using System;
using System.Collections.Generic;

namespace RecipeChangeTracker.Models
{
    public static class RecipeExtensions
    {
        public static Recipe With(
            this Recipe baseRecipe,
            TimeSpan? cookTime = null,
            IEnumerable<Ingredient> ingredients = null,
            IList<string> steps = null) =>
            new Recipe(
                baseRecipe.Name,
                cookTime ?? baseRecipe.CookTime,
                ingredients ?? baseRecipe.Ingredients,
                steps ?? baseRecipe.Steps
                );
    }
}
