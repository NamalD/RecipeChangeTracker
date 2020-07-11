using RecipeChangeTracker.CSharp.Models;
using RecipeChangeTracker.RecipeStore.Core;
using RecipeChangeTracker.Tree;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeChangeTracker.RecipeStore.InMemory
{
    public class InMemoryRecipeStore : IRecipeStore
    {
        private IDictionary<string, RecipeTree> _recipeTrees;

        public InMemoryRecipeStore()
        {
            _recipeTrees = new Dictionary<string, RecipeTree>();
        }

        public void AddOrUpdate(Recipe recipe)
        {
            var treeExists = _recipeTrees.ContainsKey(recipe.Name);
            if (treeExists)
            {
                _recipeTrees[recipe.Name].Update(recipe);
            }
            else
            {
                _recipeTrees.Add(
                    recipe.Name, 
                    new RecipeTree(
                        recipe.Name, 
                        new RecipeNode(recipe)));
            }
        }

        public Task<IEnumerable<Recipe>> GetAllRecipesAsync()
        {
            var latestRecipes = _recipeTrees.Values.Select(rt => rt.Head.Recipe).AsEnumerable();
            return Task.FromResult(latestRecipes);
        }
    }
}
