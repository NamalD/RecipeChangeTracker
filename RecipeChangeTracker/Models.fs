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
        { Recipe: Recipe.T Option
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

    let getHeadId (tree: T) =
        match tree.Head.Recipe with
        | None -> None
        | Some _ -> Some tree.Head.Id

    let getRecipeName (tree: T) =
        match tree.Head.Recipe with
        | None -> None
        | Some recipe -> Some recipe.Name

    let update (tree: T) newRecipe =
        let previousId = getHeadId tree

        let newHead =
            { Recipe = Some newRecipe
              Id = Guid.NewGuid()
              PreviousId = previousId }

        newHead :: tree

    let matchesName name list =
        let recipeName = getRecipeName list
        recipeName = Some name

module RecipeStore =

    type T = { Recipes: RecipeList.T list }

    let create recipes = { Recipes = recipes }

    let createFromRecipe recipe =
        let recipeList = RecipeList.create (Some recipe)
        { Recipes = [ recipeList ] }

    let addRecipe recipe store =
        let recipeTree = RecipeList.create (Some recipe)

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
