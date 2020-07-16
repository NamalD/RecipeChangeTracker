module Parser

open Microsoft.FSharp.Reflection

let parseArgs<'command> (args:string list) onErrorCommand =
    match args.IsEmpty with
    | true -> onErrorCommand
    | false ->
        let commandCases = FSharpType.GetUnionCases typeof<'command>

        let possibleSubcommands =
            commandCases
            |> Array.map (fun c -> c.Name.ToLower())

        let matchingSubcommand =
            Array.tryFind (fun c -> c = args.Head) possibleSubcommands

        match matchingSubcommand with
        | None -> onErrorCommand
        | Some commandName ->
            let commandCaseInfo = Array.find (fun (c:UnionCaseInfo) -> c.Name.ToLower() = commandName) commandCases
            match commandCaseInfo.GetFields () with
            | [||] -> FSharpValue.MakeUnion (commandCaseInfo, [||]) :?> 'command
            | _ ->
                let subcommandArgs = [| args.Tail |] |> Array.map (fun a -> a :> obj)
                FSharpValue.MakeUnion (commandCaseInfo, subcommandArgs) :?> 'command
            
