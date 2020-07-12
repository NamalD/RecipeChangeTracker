module RecipeChangeTracker.Tests.RecipeStoreTests

open NUnit.Framework
open RecipeChangeTracker

let ingredients = [ Ingredient.Unitless { Quantity = Ingredient.Quantity.Integer(1); Name = "pineapple" } ]
let steps = [ "Peel"; "Eat" ]
let timeToCook = System.TimeSpan.FromMinutes 20.0
let testRecipe = Recipe.create "Test Recipe" ingredients steps timeToCook

[<TestFixture>]
type AddRecipe () =

    [<Test>]
    member this.``Add Recipe Updates Stored Recipes``() =
        let store = RecipeStore.create None

        let expectedRecipeTree = Some testRecipe

        let updatedRecipeStore = RecipeStore.addRecipe testRecipe store
        let latestRecipe = RecipeList.latest updatedRecipeStore.Recipes.Value.Head

        Assert.That(latestRecipe, Is.EqualTo(expectedRecipeTree));

[<TestFixture>]
type DeleteRecipe () =
    
    [<Test>]
    member this.``Delete non-existent recipe does nothing when store is empty`` () =
        let store = RecipeStore.create None
        
        let updatedStore = RecipeStore.deleteRecipe store "blah"

        Assert.That(updatedStore, Is.EqualTo(store))

    [<Test>]
    member this.``Delete non-existent recipe does nothing when a recipe exists`` () =
        let store = RecipeStore.createFromRecipe testRecipe

        let updatedStore = RecipeStore.deleteRecipe store "foo"

        Assert.That(updatedStore, Is.EqualTo(store))

    [<Test>]
    member this.``Delete existing recipe removes recipe`` () =
        let expectedRecipe = { testRecipe with Name = "Another Recipe" }
        let expectedRecipes = [ Some expectedRecipe; ]
        let store = 
            RecipeStore.create None
            |> RecipeStore.addRecipe testRecipe
            |> RecipeStore.addRecipe { testRecipe with Name = "Another Recipe" }

        let updatedRecipes =
            RecipeStore.deleteRecipe store testRecipe.Name
            |> RecipeStore.getLatestRecipes

        Assert.That(updatedRecipes, Is.EqualTo(expectedRecipes))
