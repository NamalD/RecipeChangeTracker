module Firestore

open Types
open RecipeChangeTracker.Firestore
open Microsoft.FSharp.Reflection

type FirestoreSubcommand =
    | Connect
    | Get of string list
    | Add of string list
    | InvalidArgs

let parseArgs (args:string list) onErrorCommand =
    match args.IsEmpty with
    | true -> onErrorCommand
    | false ->
        let subcommandCases = FSharpType.GetUnionCases typeof<FirestoreSubcommand>

        let possibleSubcommands =
            subcommandCases
            |> Array.map (fun c -> c.Name.ToLower())

        let matchingSubcommand =
            Array.tryFind (fun c -> c = args.Head) possibleSubcommands

        match matchingSubcommand with
        | None -> onErrorCommand
        | Some subcommandName ->
            let subcommandCaseInfo = Array.find (fun (c:UnionCaseInfo) -> c.Name.ToLower() = subcommandName) subcommandCases
            match subcommandCaseInfo.GetFields () with
            | [||] -> FSharpValue.MakeUnion (subcommandCaseInfo, [||]) :?> FirestoreSubcommand
            | _ ->
                let subcommandArgs = [| args.Tail |] |> Array.map (fun a -> a :> obj)
                FSharpValue.MakeUnion (subcommandCaseInfo, subcommandArgs) :?> FirestoreSubcommand
            
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
            | Success data -> Write <| sprintf "Got %A" (Seq.map (fun (u:Users.User) -> u.Name) data)
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
    let subcommand = parseArgs args InvalidArgs
    handleSubcommand subcommand
