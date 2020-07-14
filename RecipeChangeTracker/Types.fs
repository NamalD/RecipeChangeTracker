namespace RecipeChangeTracker.Types

open System


type Quantity =
    | Integer of int
    | Float of float

type BasicIngredient = { Name: string; Quantity: Quantity }

type Ingredient =
    | WithUnit of Ingredient: BasicIngredient * Quantity: string
    | Unitless of Ingredient: BasicIngredient

type Recipe =
    { Name: string
      Ingredients: Ingredient list
      Steps: string list
      CookTime: TimeSpan }

type Node =
    { Recipe: Recipe
      Id: Guid
      PreviousId: Guid Option }

type TrackedRecipeList = Node list

type RecipeStore = { Recipes: TrackedRecipeList list }
