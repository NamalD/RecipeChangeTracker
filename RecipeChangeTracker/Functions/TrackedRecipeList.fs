namespace RecipeChangeTracker.Functions

open RecipeChangeTracker.Types
open System

module TrackedRecipeList =

    let create recipe =
        [ { Recipe = recipe
            Id = Guid.NewGuid()
            PreviousId = None } ]

    let latest (tree:TrackedRecipeList) = tree.Head.Recipe

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

    let private addToList recipe (list:TrackedRecipeList) =
        let previousId = Some list.Head.Id

        let newHead =
            { Recipe = recipe
              Id = Guid.NewGuid()
              PreviousId = previousId }

        newHead :: list

    let update newRecipe (list:TrackedRecipeList) =
        match list.IsEmpty with
        | true -> create newRecipe
        | false -> addToList newRecipe list

    let matchesName name list =
        let recipeName = (latest list).Name
        recipeName = name
