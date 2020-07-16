module Firestore.Router

open CliTypes

type FirestoreSubcommand =
    | Connect
    | Get of string list
    | Add of string list
    | InvalidArgs

let handleSubcommand subcommand =
    match subcommand with
    | Connect -> Handlers.connect ()
    | Get getArgs -> Handlers.get getArgs
    | Add addArgs -> Handlers.add addArgs
    | InvalidArgs -> Write "Invalid firestore arguments"

let handle args =
    let subcommand = Parser.parseArgs args InvalidArgs
    handleSubcommand subcommand
