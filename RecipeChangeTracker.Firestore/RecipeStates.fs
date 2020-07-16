namespace RecipeChangeTracker.Firestore

open Google.Cloud.Firestore
open FsFirestore.Types
open FsFirestore.Firestore

module RecipeStates =
    
    [<FirestoreData>]
    type RecipeState () =
        inherit FirestoreDocument()

        [<FirestoreProperty>]
        member val Name = "" with get, set

        [<FirestoreProperty>]
        member val Steps = [| "" |] with get, set

        [<FirestoreProperty>]
        member val Ingredients = [| dict<string, obj>[] |] with get, set

        [<FirestoreProperty>]
        member val CookTime = 0 with get, set

    let getAllLatest () =
        let getAllLatestWithConnection () =
            allDocuments<RecipeState> "recipe-state"

        Connection.withConnection <| getAllLatestWithConnection
