module RecipeChangeTracker.Tests.RecipeTree

open NUnit.Framework
open FsCheck.NUnit
open RecipeChangeTracker.Types
open RecipeChangeTracker.Functions
open FsCheck

let ingredients =
    [ Ingredient.Unitless
        { Quantity = Quantity.Integer(1)
          Name = "pineapple" } ]

let steps = [ "Peel"; "Eat" ]
let timeToCook = System.TimeSpan.FromMinutes 20.0

let testRecipe =
    Recipe.create "Test Recipe" ingredients steps timeToCook

let updatedRecipe =
    { testRecipe with
          Name = testRecipe.Name + "!" }

[<TestFixture>]
type Latest() =

    [<Test>]
    member this.``Latest returns only node when tree has one node``() =
        let tree = TrackedRecipeList.create testRecipe

        let latest = TrackedRecipeList.latest tree

        Assert.That(latest, Is.EqualTo(testRecipe))

    [<Test>]
    member this.``Latest returns latest node when node has history``() =
        let tree = TrackedRecipeList.create testRecipe
        let updatedTree = TrackedRecipeList.update updatedRecipe tree 

        let latest = TrackedRecipeList.latest updatedTree

        Assert.That(latest, Is.EqualTo(updatedRecipe))

[<TestFixture>]
type Update() =

    let existingTree = TrackedRecipeList.create testRecipe
    let existingNode = existingTree.Head

    let updatedExistingTree =
        TrackedRecipeList.update updatedRecipe existingTree 

    [<Test>]
    member this.``Update existing recipe sets Head to new recipe state``() =
        Assert.That(updatedExistingTree.Head.Recipe, Is.EqualTo(updatedRecipe))

    [<Test>]
    member this.``Update existing recipe sets PreviousId to old recipe``() =
        Assert.That(updatedExistingTree.Head.PreviousId, Is.EqualTo(Some existingNode.Id))

[<TestFixture>]
type GetPreviousNode() =

    [<Test>]
    member this.``Get previous when previous is none``() =
        let tree = TrackedRecipeList.create testRecipe

        let previous =
            TrackedRecipeList.getPreviousNode tree tree.Head

        Assert.That(previous, Is.EqualTo(None))

    [<Test>]
    member this.``Get previous when previous exists``() =
        let tree = TrackedRecipeList.create testRecipe
        let expectedNode = tree.Head
        let updatedTree = TrackedRecipeList.update updatedRecipe tree

        let previous =
            TrackedRecipeList.getPreviousNode updatedTree updatedTree.Head

        Assert.That(previous, Is.EqualTo(Some expectedNode))

[<TestFixture>]
type GetVersion() =

    [<Property>]
    let ``Version is 1 when there is no history`` recipe =
        let recipeList = TrackedRecipeList.create recipe

        let version = TrackedRecipeList.getVersion recipeList.Head recipeList

        version = 1

    [<Property>]
    let ``Version of latest is equal to iterations in history`` (NonEmptyArray recipes) =
        let recipeList = Array.fold (fun acc recipe -> TrackedRecipeList.update recipe acc) [] recipes

        let version = TrackedRecipeList.getVersion recipeList.Head recipeList

        version = recipes.Length
