module RecipeChangeTracker.Tests.RecipeStoreTests

open NUnit.Framework
open RecipeChangeTracker

[<TestFixture>]
type RecipeStore () =
    
    let ingredients = [ Ingredient.WithoutUnit { Quantity = Ingredient.Quantity.Integer(1); Name = "pineapple" } ]
    let steps = [ "Peel"; "Eat" ]
    let timeToCook = System.TimeSpan.FromMinutes 20.0
    let testRecipe = Recipe.create "Test Recipe" ingredients steps timeToCook

    [<Test>]
    member this.``Add Recipe Updates Stored Recipes``() =
        let newRecipe = testRecipe 
        let store = RecipeStore.create None

        let expectedRecipeTree = Some newRecipe

        let updatedRecipeStore = RecipeStore.addRecipe store newRecipe
        let latestRecipe = RecipeTree.latest updatedRecipeStore.Recipes.Value.Head

        Assert.That(latestRecipe, Is.EqualTo(expectedRecipeTree));
