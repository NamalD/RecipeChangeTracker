namespace RecipeStore

open NUnit.Framework
open RecipeChangeTracker.Types
open RecipeChangeTracker.Functions

module TestData = 
    let ingredients =
        [ Ingredient.Unitless
            { Quantity = Quantity.Integer(1)
              Name = "pineapple" } ]

    let steps = [ "Peel"; "Eat" ]
    let timeToCook = System.TimeSpan.FromMinutes 20.0

    let testRecipe =
        Recipe.create "Test Recipe" ingredients steps timeToCook

[<TestFixture>]
type AddRecipe() =

    [<Test>]
    member this.``Add Recipe Updates Stored Recipes``() =
        let store = RecipeStore.create []

        let updatedRecipeStore = RecipeStore.addRecipe TestData.testRecipe store

        let latestRecipe = TrackedRecipeList.latest updatedRecipeStore.Recipes.Head

        Assert.That(latestRecipe, Is.EqualTo(TestData.testRecipe))

[<TestFixture>]
type DeleteRecipe() =

    [<Test>]
    member this.``Delete non-existent recipe does nothing when store is empty``() =
        let store = RecipeStore.create []

        let updatedStore = RecipeStore.deleteRecipe store "blah"

        Assert.That(updatedStore, Is.EqualTo(store))

    [<Test>]
    member this.``Delete non-existent recipe does nothing when a recipe exists``() =
        let store = RecipeStore.createFromRecipe TestData.testRecipe

        let updatedStore = RecipeStore.deleteRecipe store "foo"

        Assert.That(updatedStore, Is.EqualTo(store))

    [<Test>]
    member this.``Delete existing recipe removes recipe``() =
        let expectedRecipe =
            { TestData.testRecipe with
                  Name = "Another Recipe" }

        let expectedRecipes = [ expectedRecipe ]

        let store =
            RecipeStore.create []
            |> RecipeStore.addRecipe TestData.testRecipe
            |> RecipeStore.addRecipe
                { TestData.testRecipe with
                      Name = "Another Recipe" }

        let updatedRecipes =
            RecipeStore.deleteRecipe store TestData.testRecipe.Name
            |> RecipeStore.getLatestRecipes

        Assert.That(updatedRecipes, Is.EqualTo(expectedRecipes))
