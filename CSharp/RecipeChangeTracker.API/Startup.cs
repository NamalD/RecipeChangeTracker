using Carter;
using Carter.ModelBinding;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RecipeChangeTracker.RecipeStore.Core;
using RecipeChangeTracker.RecipeStore.InMemory;

namespace RecipeChangeTracker.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRecipeStore, InMemoryRecipeStore>();
            
            services.AddCarter(configurator: config => config.WithModelBinder<NewtonsoftJsonModelBinder>());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "openapi/ui";
                options.SwaggerEndpoint("/openapi", "Recipes Change Tracker");
            });

            app.UseEndpoints(builder => builder.MapCarter());
        }
    }
}
