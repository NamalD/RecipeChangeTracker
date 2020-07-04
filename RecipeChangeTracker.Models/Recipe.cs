using System;
using System.Collections.Generic;

namespace RecipeChangeTracker.Models
{
    public class Recipe
    {
        public string Name { get; }

        public IEnumerable<Ingredient> Ingredients { get; }

        public IList<string> Steps { get; }

        public TimeSpan CookTime { get; }

        public Recipe(string name, TimeSpan cookTime, IEnumerable<Ingredient> ingredients, IList<string> steps)
        {
            Name = name;
            Ingredients = ingredients;
            Steps = steps;
            CookTime = cookTime;
        }
    }
}
