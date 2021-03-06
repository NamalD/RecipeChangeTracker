module CliTypes

type Command =
    | GetCommand
    | FireStore
    | Invalid
    | Quit

type Action =
    | GetNextCommand
    | QuitApplication
    | Write of string
