namespace RecipeChangeTracker.Functions

open RecipeChangeTracker.Types

module Recipe =

    let create name ingredients steps cooktime =
        { Name = name
          Ingredients = ingredients
          Steps = steps
          CookTime = cooktime }
