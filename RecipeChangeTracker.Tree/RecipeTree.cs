using RecipeChangeTracker.Models;

namespace RecipeChangeTracker.Tree
{
    public class RecipeTree
    {
        public string RecipeName { get; }

        public RecipeNode Head { get; private set; }

        public RecipeTree(string recipeName)
        {
            RecipeName = recipeName;
            Head = null;
        }

        public RecipeTree(string recipeName, RecipeNode head)
        {
            RecipeName = recipeName;
            Head = head;
        }

        public void Update(Recipe newRecipe)
        {
            Head = new RecipeNode(newRecipe, Head);
        }
    }
}
