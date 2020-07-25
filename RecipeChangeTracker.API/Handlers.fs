module RecipeChangeTracker.API.Handlers

open Giraffe.Core
open RecipeChangeTracker.Firestore
open Giraffe

module Recipes =
    let getLatest = RecipeStates.getAllLatest () |> json

    let getLatestHander: HttpHandler = warbler (fun _ -> getLatest)
