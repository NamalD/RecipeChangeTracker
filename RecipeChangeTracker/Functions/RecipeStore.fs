namespace RecipeChangeTracker.Functions

open RecipeChangeTracker.Types

module RecipeStore =

    let create recipes = { Recipes = recipes }

    let createFromRecipe recipe =
        let recipeList = TrackedRecipeList.create recipe
        { Recipes = [ recipeList ] }

    let addRecipe recipe store =
        let recipeTree = TrackedRecipeList.create recipe

        let updatedRecipes = recipeTree :: store.Recipes

        { store with Recipes = updatedRecipes }

    let getLatestRecipes store = List.map TrackedRecipeList.latest store.Recipes

    let excludingRecipe recipeToExclude recipes =
        List.where (TrackedRecipeList.matchesName recipeToExclude >> not) recipes

    let deleteRecipe store recipeToDelete =
        let recipesWithoutDeleted =
            excludingRecipe recipeToDelete store.Recipes

        { store with
              Recipes = recipesWithoutDeleted }
