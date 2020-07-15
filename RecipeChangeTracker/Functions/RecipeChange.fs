namespace RecipeChangeTracker.Functions

open RecipeChangeTracker.Types
open System

module RecipeChange =

    let ofRecipe recipe =
        [ NameChange recipe.Name;
          IngredientsChange recipe.Ingredients;
          StepsChange recipe.Steps;
          CookTimeChange recipe.CookTime
        ]

    let applyChangeToRecipe (recipe:Recipe) change =
        match change with
        | NameChange name -> { recipe with Name = name }
        | IngredientsChange ingredients -> { recipe with Ingredients = ingredients }
        | StepsChange steps -> { recipe with Steps = steps }
        | CookTimeChange time -> { recipe with CookTime = time }

    let toRecipe changes =
        List.fold applyChangeToRecipe Recipe.createEmpty changes

    let toNode change previousId =
        { Change = change
          Id = Guid.NewGuid()
          PreviousId = previousId
        }
