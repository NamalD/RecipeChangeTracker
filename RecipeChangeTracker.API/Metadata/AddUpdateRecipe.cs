using Carter.OpenApi;
using RecipeChangeTracker.Models;

namespace RecipeChangeTracker.API.Metadata
{
    internal class AddUpdateRecipe : RouteMetaData
    {
        public override string Description => "Add a new recipe or update an existing recipe";

        public override RouteMetaDataRequest[] Requests { get; } =
        {
            new RouteMetaDataRequest
            {
                Request = typeof(Recipe)
            }
        };

        public override RouteMetaDataResponse[] Responses { get; } =
        {
            new RouteMetaDataResponse
            {
                Code = 201,
                Description = "Created new recipe"
            },
            new RouteMetaDataResponse
            {
                Code = 200,
                Description = "Updated recipe"
            }
        };
    }
}
