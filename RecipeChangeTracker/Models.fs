namespace RecipeChangeTracker

open System

module Ingredient =

    type Quantity =
        | Integer of int
        | Float of float

    type BasicIngredient = { Name: string; Quantity: Quantity }

    type T =
        | WithUnit of Ingredient: BasicIngredient * Quantity: string
        | Unitless of Ingredient: BasicIngredient

module Recipe =

    type T =
        { Name: string
          Ingredients: Ingredient.T list
          Steps: string list
          CookTime: TimeSpan }

    let create name ingredients steps cooktime =
        { Name = name
          Ingredients = ingredients
          Steps = steps
          CookTime = cooktime }

module RecipeList =

    type Node =
        { Recipe: Recipe.T
          Id: Guid
          PreviousId: Guid Option }

    type T = Node list

    let create recipe =
        [ { Recipe = recipe
            Id = Guid.NewGuid()
            PreviousId = None } ]

    let latest (tree: T) = tree.Head.Recipe

    let getNodeWithId tree id =
        List.tryPick (fun node -> if node.Id = id then Some node else None) tree

    let getPreviousNode tree node =
        match node.PreviousId with
        | None -> None
        | Some previousId -> getNodeWithId tree previousId

    let rec getVersion node list =
        match getPreviousNode list node with
        | None -> 1
        | Some previous -> 1 + (getVersion previous list)

    let private addToList recipe (list:T) =
        let previousId = Some list.Head.Id

        let newHead =
            { Recipe = recipe
              Id = Guid.NewGuid()
              PreviousId = previousId }

        newHead :: list

    let update newRecipe (list: T) =
        match list.IsEmpty with
        | true -> create newRecipe
        | false -> addToList newRecipe list

    let matchesName name list =
        let recipeName = (latest list).Name
        recipeName = name

module RecipeStore =

    type T = { Recipes: RecipeList.T list }

    let create recipes = { Recipes = recipes }

    let createFromRecipe recipe =
        let recipeList = RecipeList.create recipe
        { Recipes = [ recipeList ] }

    let addRecipe recipe store =
        let recipeTree = RecipeList.create recipe

        let updatedRecipes = recipeTree :: store.Recipes

        { store with Recipes = updatedRecipes }

    let getLatestRecipes store = List.map RecipeList.latest store.Recipes

    let excludingRecipe recipeToExclude recipes =
        List.where (RecipeList.matchesName recipeToExclude >> not) recipes

    let deleteRecipe store recipeToDelete =
        let recipesWithoutDeleted =
            excludingRecipe recipeToDelete store.Recipes

        { store with
              Recipes = recipesWithoutDeleted }
