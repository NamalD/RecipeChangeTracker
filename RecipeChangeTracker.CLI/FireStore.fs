module FireStore

open Types
open RecipeChangeTracker.Firestore

type FireStoreSubcommand =
    | Connect
    | Get of string list
    | Add of string list
    | InvalidArgs

let handleArgs (args:string list) =
    match args.IsEmpty with
    | true -> InvalidArgs
    | false ->
        match args.Head.ToLower() with
        | "connect" -> Connect
        | "get" -> Get args.Tail
        | "add" -> Add args.Tail
        | _ -> InvalidArgs

let handleConnect () =
    let didConnect = Connection.connect ()
    match didConnect with
    | true -> Write "Connected succesfully"
    | false -> Write "Failed to connect"

let handleGet (args:string list) =
    match args with
    | [] -> Write "Missing collection name"
    | _ ->
        match args.Head.ToLower() with
        | "users" ->
            let userCollection = Users.getUsers ()
            match userCollection with
            | Success data -> Write <| sprintf "Got %A" (Seq.map (fun (u:Users.User) -> u.AllFields) data)
            | Failure -> Write "Failed to get collection"
        | _ -> Write "Unsupported collection"

let handleAdd (args:string list) =
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

let handleSubcommand subcommand =
    match subcommand with
    | Connect -> handleConnect ()
    | Get getArgs -> handleGet getArgs
    | Add addArgs -> handleAdd addArgs
    | InvalidArgs -> Write "Invalid firestore arguments"

let handle args =
    let subcommand = handleArgs args

    handleSubcommand subcommand
