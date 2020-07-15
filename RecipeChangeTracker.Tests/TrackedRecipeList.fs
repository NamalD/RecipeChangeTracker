namespace TrackedRecipeList

open NUnit.Framework
open FsCheck.NUnit
open RecipeChangeTracker.Types
open RecipeChangeTracker.Functions
open FsCheck

module TestData =
    let ingredients =
        [ Ingredient.Unitless
            { Quantity = Quantity.Integer(1)
              Name = "pineapple" } ]

    let steps = [ "Peel"; "Eat" ]
    let timeToCook = System.TimeSpan.FromMinutes 20.0

    let testRecipe =
        Recipe.create "Test Recipe" ingredients steps timeToCook

    let updatedName = testRecipe.Name + "!"
    let changeName = NameChange updatedName

    let basicList = TrackedRecipeList.applyChange (NameChange "My Recipe") []

[<TestFixture>]
type ``Get Ordered Changes`` () =

    [<Property>]
    let ``Returns empty when list is empty`` () =
        let list = []

        let changes = TrackedRecipeList.getOrderedChanges list

        changes = []

    [<Property>]
    let ``Returns single change when list has one change`` rootChange =
        let list = TrackedRecipeList.applyChange rootChange []

        let changes = TrackedRecipeList.getOrderedChanges list

        changes = [ rootChange ]

    [<Property>]
    let ``Returns changes in order of insertion`` changes =
        let list = List.fold TrackedRecipeList.foldChange [] changes

        let orderedChanges = TrackedRecipeList.getOrderedChanges list

        orderedChanges = changes

    [<Test>]
    member this.``Returns single change when list has one example change`` () =
        let rootChange = IngredientsChange [WithUnit ({ Name = ""; Quantity = Decimal 1.0m }, null)]
        let list = TrackedRecipeList.applyChange rootChange []

        let changes = TrackedRecipeList.getOrderedChanges list

        let c = rootChange = changes.Head
        let cc = List.compareWith (fun left right -> if left = right then 0 else -1) [rootChange] changes

        Assert.That(changes.Head, Is.EqualTo(rootChange))

[<TestFixture>]
type Latest() =

    [<Test>]
    member this.``Latest returns only node when list has one node``() =
        let list = TrackedRecipeList.create TestData.testRecipe

        let latest = TrackedRecipeList.latest list

        Assert.That(latest, Is.EqualTo(TestData.testRecipe))

    [<Test>]
    member this.``Latest returns recipe with changes when node has history``() =
        let expectedName = "Updated Recipe"
        let expectedIngredients = [ Unitless { Name = "Avocado"; Quantity = Integer 4 } ]
        let updatedList =
            TrackedRecipeList.create TestData.testRecipe
            |> TrackedRecipeList.applyChange (NameChange expectedName) 
            |> TrackedRecipeList.applyChange (IngredientsChange expectedIngredients)

        let expected = { TestData.testRecipe with Name = expectedName; Ingredients = expectedIngredients }

        let latest = TrackedRecipeList.latest updatedList

        Assert.That(latest, Is.EqualTo(expected))

[<TestFixture>]
type ApplyChange() =

    let existingList = TrackedRecipeList.create TestData.testRecipe
    let existingNode = existingList.Head

    let updatedList = TrackedRecipeList.applyChange TestData.changeName existingList

    [<Test>]
    member this.``Apply changes to existing recipe sets Head to new recipe state``() =
        Assert.That(updatedList.Head.Change, Is.EqualTo(TestData.changeName))

    [<Test>]
    member this.``Update existing recipe sets PreviousId to old recipe``() =
        Assert.That(updatedList.Head.PreviousId, Is.EqualTo(Some existingNode.Id))

[<TestFixture>]
type GetPreviousNode() =

    [<Test>]
    member this.``Get previous when previous is none``() =
        let previous = TrackedRecipeList.getPreviousNode TestData.basicList.Head TestData.basicList

        Assert.That(previous, Is.EqualTo(None))

    [<Test>]
    member this.``Get previous when previous exists``() =
        let list = TrackedRecipeList.create TestData.testRecipe
        let expectedNode = list.Head
        let updatedTree = TrackedRecipeList.applyChange TestData.changeName list

        let previous = TrackedRecipeList.getPreviousNode updatedTree.Head updatedTree 

        Assert.That(previous, Is.EqualTo(Some expectedNode))

[<TestFixture>]
type GetVersion() =

    [<Property>]
    let ``Version is 1 when there is no history`` change =
        let list = TrackedRecipeList.applyChange change []
        let version = TrackedRecipeList.getVersion list.Head list

        version = 1

    [<Property>]
    let ``Version of latest is equal to iterations in history`` (NonEmptyArray changes) =
        let recipeList = Array.fold TrackedRecipeList.foldChange [] changes

        let version = TrackedRecipeList.getVersion recipeList.Head recipeList

        version = changes.Length
