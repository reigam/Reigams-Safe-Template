module Api.Passwords

open LiteDB
open Shared

// type TodosStorage (db : LiteDatabase) as this =
//     let collection = "todos"
//     let todos = db.GetCollection<Todo> collection
//
//     do
//         if not (db.CollectionExists collection) then
//             this.AddTodo(Todo.create "Create Your First TODO") |> ignore
//
//     member _.GetTodos () =
//         todos.FindAll() |> List.ofSeq |> List.sortBy (fun l -> l.TimeStamp)
//
//     member _.AddTodo (todo: Todo) =
//         if Todo.isValid todo.Description then
//             todos.Insert(todo) |> ignore
//             Ok ()
//         else Error "Invalid todo"
//
// let todosApi (storage : TodosStorage) =
//     { getTodos = fun () -> async { return storage.GetTodos() }
//       addTodo =
//         fun todo -> async {
//             match storage.AddTodo todo with
//             | Ok () -> return todo
//             | Error e -> return failwith e
//         } }