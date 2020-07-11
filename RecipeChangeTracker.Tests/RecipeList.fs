module RecipeChangeTracker.Tests.RecipeTree

open NUnit.Framework
open RecipeChangeTracker

let ingredients = [ Ingredient.WithoutUnit { Quantity = Ingredient.Quantity.Integer(1); Name = "pineapple" } ]
let steps = [ "Peel"; "Eat" ]
let timeToCook = System.TimeSpan.FromMinutes 20.0
let testRecipe = Recipe.create "Test Recipe" ingredients steps timeToCook
let updatedRecipe = { testRecipe with Name = testRecipe.Name + "!" }

[<TestFixture>]
type Latest () =

    [<Test>]
    member this. ``Latest returns None when tree is empty``() =
        let tree = RecipeList.create None

        let latest = RecipeList.latest tree

        Assert.That(latest, Is.EqualTo(None))

    [<Test>]
    member this.``Latest returns only node when tree has one node`` () =
        let tree = RecipeList.create (Some testRecipe)

        let latest = RecipeList.latest tree

        Assert.That(latest, Is.EqualTo(Some testRecipe))

    [<Test>]
    member this.``Latest returns latest node when node has history`` () =
        let tree = RecipeList.create (Some testRecipe)
        let updatedTree = RecipeList.update tree updatedRecipe

        let latest = RecipeList.latest updatedTree

        Assert.That(latest, Is.EqualTo(Some updatedRecipe))

[<TestFixture>]
type Update () =
   
    let emptyTree = RecipeList.create None
    let updatedEmptyTree = RecipeList.update emptyTree testRecipe

    let existingTree = RecipeList.create (Some testRecipe)
    let existingNode = existingTree.Head;
    let updatedExistingTree = RecipeList.update existingTree updatedRecipe

    [<Test>]
    member this.``Update empty tree sets Head to recipe`` () =
        Assert.That(updatedEmptyTree.Head.Recipe, Is.EqualTo(Some testRecipe))

    [<Test>]
    member this.``Update empty tree leaves PreviousId as None`` () =
        Assert.That(updatedEmptyTree.Head.PreviousId, Is.EqualTo(None))

    [<Test>]
    member this.``Update existing recipe sets Head to new recipe state`` () =
        Assert.That(updatedExistingTree.Head.Recipe, Is.EqualTo(Some updatedRecipe))

    [<Test>]
    member this.``Update existing recipe sets PreviousId to old recipe`` () =
        Assert.That(updatedExistingTree.Head.PreviousId, Is.EqualTo(Some existingNode.Id))
