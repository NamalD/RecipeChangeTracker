namespace RecipeChangeTracker.Firestore

open FsFirestore.Firestore
open FsFirestore.Types
open Google.Cloud.Firestore

module Users =

    [<FirestoreData>]
    type User () =
        inherit FirestoreDocument()

        [<FirestoreProperty>]
        member val Name = "" with get, set

    let getUsers () =
        let getUsersOnConnect () =
            allDocuments<User> "users"

        Connection.withConnection <| getUsersOnConnect 

    let addUser username =
        let addUserOnConnect () =
            let user = new User(Name = username)
            addDocument "users" (Some username) user

        Connection.withConnection <| addUserOnConnect
