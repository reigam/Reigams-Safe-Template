module Api.Passwords

open LiteDB
open Shared

type UserStorage (db : LiteDatabase) as this =
    let collection = "users"
    let users = db.GetCollection<User> collection

    do
        if not (db.CollectionExists collection) then
            this.AddUser(User.create "test" "test") |> ignore

    member _.AddUser (user: User) =
        if User.isValid user.UserName then
            users.Insert(user) |> ignore
            Ok ()
        else Error "Invalid user"

    member _.ValidateUser (user: User) =
        users.FindAll ()
        |> Seq.tryFind (fun x -> x.UserName = user.UserName)
        |> (fun x ->
            match x with
            | Some y ->
                if y.UserName = user.UserName && y.Passord = user.Passord then true
                else false
            |_ -> false)


    // TODO
    // member _.ChangePassword (user: User) =
    // member _.DeleteUser (user: User) =

let usersApi (storage: UserStorage) =
    { addUser =
        fun user -> async {
            match storage.AddUser user with
            | Ok () -> return user
            | Error e -> return failwith e
        }
      validateUser =
        fun user -> async {
            match storage.ValidateUser user with
            | true -> return true
            | false -> return false
        }
    }

// let todosApi (storage : TodosStorage) =
//     { getTodos = fun () -> async { return storage.GetTodos() }
//       addTodo =
//         fun todo -> async {
//             match storage.AddTodo todo with
//             | Ok () -> return todo
//             | Error e -> return failwith e
//         } }