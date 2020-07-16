open System
open CliTypes

let toCommand (value:string) =
    match value.ToLower() with
    | "firestore" -> FireStore
    | "q" | "quit" | "exit" -> Quit
    | _ -> Invalid

let parse (input:string) =
    let split = input.Split ' ' |> Array.toList
    let command = toCommand split.Head
    let args = split.Tail

    (command, args)

let getCommand () =
    let input = Console.ReadLine ()
    parse input

let handle command args =
    match command with
    | GetCommand -> GetNextCommand
    | Quit -> QuitApplication
    | Invalid -> Write "Invalid command"
    | FireStore -> Firestore.Router.handle args

let parseHandle () =
    let (command, args) = getCommand ()

    let action = handle command args 

    match action with
    | Write text ->
        printfn "%s" text
        GetNextCommand
    | _ -> action 

[<EntryPoint>]
let main argv =
    let mutable acceptCommands = true

    while acceptCommands do
        let next = parseHandle ()

        match next with
        | QuitApplication -> acceptCommands <- false
        | _ -> ()

    0 
