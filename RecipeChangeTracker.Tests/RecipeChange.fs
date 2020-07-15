namespace Equality

open NUnit.Framework
open FsCheck
open FsCheck.NUnit
open RecipeChangeTracker.Types

[<TestFixture>]
type Quanitity () =

    [<Property>]
    let ``Same integers`` i =
        let first = Integer i
        let second = Integer i

        first = second

    let genTuple map arb =
        arb 
        |> Arb.toGen
        |> Gen.map map 
        |> Arb.fromGen

    let mapDifferentInts i =
        (i, i + 1)

    let mapDifferentDecimals f =
        (f, f + 1.0m)

    [<Property>]
    let ``Different integers`` () =
        Arb.Default.Int32()
        |> genTuple mapDifferentInts
        |> Prop.forAll <| fun (i, j) ->
            let first = Integer i
            let second = Integer j

            first <> second

    [<Property>]
    let ``Same decimals`` f =
        let first = Decimal f
        let second = Decimal f
        first = second

    [<Property>]
    let ``Different decimals`` () =
        Arb.Default.Decimal()
        |> genTuple mapDifferentDecimals
        |> Prop.forAll <| fun (i, j) ->
            let first = Decimal i
            let second = Decimal j

            first <> second

[<TestFixture>]
type RecipeChange () =

    [<Property>]
    let ``Compare name change`` name =
        let first = NameChange name
        let second = NameChange name

        first = second

    [<Property>]
    let ``Compare equal ingredient changes`` ingredients =
        let first = IngredientsChange ingredients
        let second = IngredientsChange ingredients

        first = second

    [<Property>]
    let ``Compare different ingredient changes`` (NonEmptyArray ingredients) =
        let ingredientList = Array.toList ingredients
        let badIngredient = Unitless { Name = "Bad Ingredient"; Quantity = Integer 99 }
        let first = IngredientsChange ingredientList
        let second = IngredientsChange (badIngredient :: ingredientList.Tail)

        first <> second
