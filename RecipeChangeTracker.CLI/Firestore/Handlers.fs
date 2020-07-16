module Handlers

open RecipeChangeTracker.Firestore
open CliTypes

let connect () =
    let didConnect = Connection.connect ()
    match didConnect with
    | true -> Write "Connected succesfully"
    | false -> Write "Failed to connect"

let handleFirestoreResponse map response =
    match response with
    | Success data -> Write <| sprintf "%A" (map data)
    | Failure -> Write "Failed to get collection"

let getUsers () =
    let map (users:Users.User seq) =
        Seq.map (fun (u:Users.User) -> u.Name) users

    Users.getUsers ()
    |> handleFirestoreResponse map

let getRecipeStates () =
    let mapping (recipes:RecipeStates.RecipeState seq) =
        Seq.map (fun (s:RecipeStates.RecipeState) -> s.AllFields) recipes

    RecipeStates.getAllLatest ()
    |> handleFirestoreResponse mapping

let get (args:string list) =
    match args with
    | [] -> Write "Missing collection name"
    | _ ->
        match args.Head.ToLower() with
        | "users" -> getUsers ()
        | "recipe-state" -> getRecipeStates ()
        | _ -> Write "Unsupported collection"

let add (args:string list) =
    match args with
    | [] -> Write "Missing collection name"
    | _ ->
        match args.Head.ToLower() with
        | "users" ->
            match args.Tail with
            | [] -> Write "Missing username"
            | _ ->
                let username = args.Tail.Head
                match Users.addUser username with
                | Success ref -> Write <| sprintf "Added user %s" ref.Id
                | Failure -> Write "Failed to add user"
        | _ -> Write "Unsupported collection"
