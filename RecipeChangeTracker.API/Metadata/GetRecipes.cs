using Carter.OpenApi;
using RecipeChangeTracker.Models;
using System.Collections.Generic;

namespace RecipeChangeTracker.API.Metadata
{
    internal class GetRecipes : RouteMetaData
    {
        public override string Description => "Returns all recipes";

        public override RouteMetaDataResponse[] Responses { get; } =
        {
            new RouteMetaDataResponse
            {
                Code = 200,
                Description = $"A list of Recipes",
                Response = typeof(IEnumerable<Recipe>)
            }
        };
    }
}