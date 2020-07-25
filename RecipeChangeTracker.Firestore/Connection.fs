namespace RecipeChangeTracker.Firestore

open FsFirestore.Firestore
open System

type FirestoreResponse<'a> =
    | Success of 'a
    | Failure

module Connection =

    let connect () =
        connectToFirestoreProject "recipechangetracker"

    let withConnection callback =
        connect () |> ignore
        callback ()
