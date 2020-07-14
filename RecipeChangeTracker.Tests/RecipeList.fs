module RecipeChangeTracker.Tests.RecipeTree

open NUnit.Framework
open FsCheck.NUnit
open RecipeChangeTracker
open FsCheck

let ingredients =
    [ Ingredient.Unitless
        { Quantity = Ingredient.Quantity.Integer(1)
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
        let tree = RecipeList.create testRecipe

        let latest = RecipeList.latest tree

        Assert.That(latest, Is.EqualTo(testRecipe))

    [<Test>]
    member this.``Latest returns latest node when node has history``() =
        let tree = RecipeList.create testRecipe
        let updatedTree = RecipeList.update updatedRecipe tree 

        let latest = RecipeList.latest updatedTree

        Assert.That(latest, Is.EqualTo(updatedRecipe))

[<TestFixture>]
type Update() =

    let existingTree = RecipeList.create testRecipe
    let existingNode = existingTree.Head

    let updatedExistingTree =
        RecipeList.update updatedRecipe existingTree 

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
        let tree = RecipeList.create testRecipe

        let previous =
            RecipeList.getPreviousNode tree tree.Head

        Assert.That(previous, Is.EqualTo(None))

    [<Test>]
    member this.``Get previous when previous exists``() =
        let tree = RecipeList.create testRecipe
        let expectedNode = tree.Head
        let updatedTree = RecipeList.update updatedRecipe tree

        let previous =
            RecipeList.getPreviousNode updatedTree updatedTree.Head

        Assert.That(previous, Is.EqualTo(Some expectedNode))

[<TestFixture>]
type GetVersion() =

    [<Property>]
    let ``Version is 1 when there is no history`` recipe =
        let recipeList = RecipeList.create recipe

        let version = RecipeList.getVersion recipeList.Head recipeList

        version = 1

    [<Property>]
    let ``Version of latest is equal to iterations in history`` (NonEmptyArray recipes) =
        let recipeList = Array.fold (fun acc recipe -> RecipeList.update recipe acc) [] recipes

        let version = RecipeList.getVersion recipeList.Head recipeList

        version = recipes.Length
