namespace RecipeChangeTracker.Functions

open RecipeChangeTracker.Types

module TrackedRecipeList =

    let headId list =
        match list with
        | [] -> None
        | _ -> Some list.Head.Id

    let applyChange change list =
        let changeNode = headId list |> RecipeChange.toNode change
        changeNode :: list

    let foldChange list change =
        applyChange change list

    let create recipe =
        let changes = RecipeChange.ofRecipe recipe
        List.fold foldChange [] changes

    let getNodeWithId list id =
        List.tryPick (fun node -> if node.Id = id then Some node else None) list

    let getPreviousNode node list =
        match node.PreviousId with
        | None -> None
        | Some previousId -> getNodeWithId list previousId

    let rec getOrderedNodes (list:TrackedRecipe) =
        match list.Length with
        | 0 -> []
        | 1 -> [ list.Head ]
        | 2 ->
            let previous = getPreviousNode list.Head list
            match previous with
            | Some node -> node :: [ list.Head ]
            | None -> []
        | _ -> getOrderedNodes list.Tail @ [ list.Head ]

    let getOrderedChanges list =
        getOrderedNodes list
        |> List.map (fun node -> node.Change)

    let latest (list:TrackedRecipe) =
        getOrderedChanges list |> RecipeChange.toRecipe

    let rec getVersion node list =
        match getPreviousNode node list with
        | None -> 1
        | Some previous -> 1 + (getVersion previous list)
