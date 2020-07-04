using RecipeChangeTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeChangeTracker.RecipeStore.Core
{
    public interface IRecipeStore
    {
        public Task<IEnumerable<Recipe>> GetAllRecipesAsync();

        void AddOrUpdate(Recipe recipe);
    }
}
