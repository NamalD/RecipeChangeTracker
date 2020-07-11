namespace RecipeChangeTracker

open System

// Add recipe

// Delete recipe

// Update recipe

// Get latest recipe

// Get historical recipe

// Undo/redo to state 
module Ingredient =
   
    type Quantity =
        | Integer of int
        | Float of float

    type BasicIngredient = {
        Name: string 
        Quantity: Quantity
    }
    
    type T =
        | WithUnit of Ingredient: BasicIngredient * Quantity: string 
        | WithoutUnit of Ingredient: BasicIngredient 

module Recipe =
    
    type T = {
        Name: string
        Ingredients: Ingredient.T list
        Steps: string list
        CookTime: TimeSpan
    }

    let create name ingredients steps cooktime =
        { Name = name; Ingredients = ingredients; Steps = steps; CookTime = cooktime }

module RecipeTree =
    
    type Node = {
        Recipe: Recipe.T Option
        PreviousState: Node Option
    }

    type T = {
        Head: Node
    }

    let create recipe =
        { Head = { Recipe = recipe; PreviousState = None } }

    let latest tree =
        tree.Head.Recipe

    let update tree newRecipe =
        let newHistory =
            match tree.Head.Recipe with
            | None -> None
            | Some _ -> Some tree.Head 

        let newHead = { Recipe = Some newRecipe; PreviousState = newHistory }
        { Head = newHead }

module RecipeStore =

    type T = {
        Recipes: RecipeTree.T list Option
    }

    let create recipes =
        { Recipes = recipes }

    let addRecipe store recipe =
        let recipeTree = RecipeTree.create (Some recipe)

        let updatedRecipes = 
            match store.Recipes with
            | None -> Some [ recipeTree ]
            | Some rt -> Some (recipeTree :: rt)

        { store with Recipes = updatedRecipes }

