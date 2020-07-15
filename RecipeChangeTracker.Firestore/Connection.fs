namespace RecipeChangeTracker.Firestore

open FsFirestore.Firestore

type FirestoreResponse<'a> =
    | Success of 'a
    | Failure

module Connection =

    let connect () =
        connectToFirestore "serviceAccountKey.json"

    let withConnection callback =
        match connect() with
        | true -> Success <| callback ()
        | false -> Failure
