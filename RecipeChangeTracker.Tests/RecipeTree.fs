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
        let tree = RecipeTree.create None

        let latest = RecipeTree.latest tree

        Assert.That(latest, Is.EqualTo(None))

    [<Test>]
    member this.``Latest returns only node when tree has one node`` () =
        let tree = RecipeTree.create (Some testRecipe)

        let latest = RecipeTree.latest tree

        Assert.That(latest, Is.EqualTo(Some testRecipe))

    [<Test>]
    member this.``Latest returns latest node when node has history`` () =
        let tree = RecipeTree.create (Some testRecipe)
        let updatedTree = RecipeTree.update tree updatedRecipe

        let latest = RecipeTree.latest updatedTree

        Assert.That(latest, Is.EqualTo(Some updatedRecipe))

[<TestFixture>]
type Update () =
   
    let emptyTree = RecipeTree.create None
    let updatedEmptyTree = RecipeTree.update emptyTree testRecipe

    let existingTree = RecipeTree.create (Some testRecipe)
    let updatedExistingTree = RecipeTree.update existingTree updatedRecipe

    [<Test>]
    member this.``Update empty tree sets Head to recipe`` () =
        Assert.That(updatedEmptyTree.Head.Recipe, Is.EqualTo(Some testRecipe))

    [<Test>]
    member this.``Update empty tree leaves PreviousState as None`` () =
        Assert.That(updatedEmptyTree.Head.PreviousState, Is.EqualTo(None))

    [<Test>]
    member this.``Update existing recipe sets Head to new recipe state`` () =
        Assert.That(updatedExistingTree.Head.Recipe, Is.EqualTo(Some updatedRecipe))

    [<Test>]
    member this.``Update existing recipe sets PreviousState to old recipe`` () =
        Assert.That(updatedExistingTree.Head.PreviousState.Value.Recipe, Is.EqualTo(Some testRecipe))
