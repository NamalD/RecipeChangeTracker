module RecipeChangeTracker.API.Handlers

open Giraffe.Core
open RecipeChangeTracker.Firestore
open Giraffe
open RecipeChangeTracker.API.Mappings

module Recipes =
    let getLatest =
        match RecipeStates.getAllLatest () with
        | Failure ->
            ServerErrors.internalError
            <| text "Could not connect to Firestore"
        | Success recipes ->
            mapRecipesResponse recipes
            |> json
            |> Successful.ok

    let getLatestHander: HttpHandler = warbler (fun _ -> getLatest)
