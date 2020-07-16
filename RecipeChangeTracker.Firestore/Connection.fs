namespace RecipeChangeTracker.Firestore

open FsFirestore.Firestore
open System

type FirestoreResponse<'a> =
    | Success of 'a
    | Failure

module Connection =

    let connect () =
        Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS")
        |> connectToFirestore 

    let withConnection callback =
        match connect() with
        | true -> Success <| callback ()
        | false -> Failure
