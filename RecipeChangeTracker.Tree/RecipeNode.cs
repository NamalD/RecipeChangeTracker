using RecipeChangeTracker.Models;

namespace RecipeChangeTracker.Tree
{
    public class RecipeNode
    {
        public Recipe Recipe { get; }

        public RecipeNode PreviousState { get; }

        public RecipeNode(Recipe recipe)
        {
            Recipe = recipe;
        }

        public RecipeNode(Recipe recipe, RecipeNode previousState)
        {
            Recipe = recipe;
            PreviousState = previousState;
        }
    }
}