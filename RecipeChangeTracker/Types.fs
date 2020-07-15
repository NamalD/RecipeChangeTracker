namespace RecipeChangeTracker.Types

open System

[<CustomEquality>]
[<NoComparison>]
type Quantity =
    | Integer of int
    | Decimal of decimal

    with
        override this.Equals(other) =
            match other with
            | :? Quantity as quantity ->
                match (this, quantity) with
                | (Integer t, Integer o) -> t = o
                | (Decimal t, Decimal o) -> t = o
                | (Integer t, Decimal o) -> decimal t = o
                | (Decimal t, Integer o) -> t = decimal o
            | _ -> false

        override this.GetHashCode() =
            match this with
            | Integer i -> hash(i)
            | Decimal f -> hash(f)

[<CustomEquality>]
[<NoComparison>]
type BasicIngredient = { Name: string; Quantity: Quantity }
    with
        override this.Equals(other) =
            match other with
            | :? BasicIngredient as otherIngredient ->
                otherIngredient.Name = this.Name
                && otherIngredient.Quantity = this.Quantity
            | _ -> false

        override this.GetHashCode() = hash(this.Name, this.Quantity)

[<CustomEquality>]
[<NoComparison>]
type Ingredient =
    | WithUnit of Ingredient: BasicIngredient * Unit: string
    | Unitless of Ingredient: BasicIngredient

    with
        override this.Equals(other) =
            match other with
            | :? Ingredient as i ->
                match (i, this) with
                | (WithUnit (i, q), WithUnit(ti, tq)) -> i = ti && q = tq
                | ((Unitless i), (Unitless ti)) -> i = ti
                | _ -> false
            | _ -> false

        override this.GetHashCode() =
            match this with
            | WithUnit (i, q) -> hash(i, q)
            | Unitless i -> hash(i)

type Recipe =
    { Name: string
      Ingredients: Ingredient list
      Steps: string list
      CookTime: TimeSpan }

type RecipeChange =
    | NameChange of string
    | IngredientsChange of Ingredient list
    | StepsChange of string list
    | CookTimeChange of TimeSpan

type RecipeChangeNode =
    { Change: RecipeChange
      Id: Guid
      PreviousId: Guid Option }

type TrackedRecipeList = RecipeChangeNode list

type RecipeStore = { Recipes: TrackedRecipeList list }
