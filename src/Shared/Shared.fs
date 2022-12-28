namespace Shared

open System

(*Todo*)
[<CLIMutable>]
type Todo = { Id: Guid; TimeStamp: DateTime; Description: string }

module Todo =
    let isValid (description: string) =
        String.IsNullOrWhiteSpace description |> not

    let create (description: string) =
        { Id = Guid.NewGuid()
          TimeStamp = DateTime.Now
          Description = description }

type ITodosApi =
    { getTodos: unit -> Async<Todo list>
      addTodo: Todo -> Async<Todo> }

(*User*)
type Password = Password of string
module Password =
    let set (s) = Password s

[<CLIMutable>]
type User = { Id: Guid; UserName: string; Passord: Password }

module User =
    let isValid (userName: string) =
        String.IsNullOrWhiteSpace userName |> not

    let create (userName: string) (userPassword: string) =
        { Id = Guid.NewGuid()
          UserName = userName
          Passord = Password.set userPassword }

type IUsersApi =
    { addUser: User -> Async<User>
      validateUser: User -> Async<bool> }

(*Route*)
module Route =
    let builder typeName methodName =
        sprintf $"/api/{typeName}/{methodName}"