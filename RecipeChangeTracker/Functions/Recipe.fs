namespace RecipeChangeTracker.Functions

open RecipeChangeTracker.Types
open System

module Recipe =

    let create name ingredients steps cooktime =
        { Name = name
          Ingredients = ingredients
          Steps = steps
          CookTime = cooktime }

    let createEmpty =
        create ""  [] [] TimeSpan.Zero
