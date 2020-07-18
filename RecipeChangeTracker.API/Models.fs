module RecipeChangeTracker.API.Models

open System.Collections.Generic

type RecipeResponse = {
    Id: string
    Name: string
    Steps: string array
    Ingredients: IDictionary<string, obj> array
    CookTime: int
}
