namespace Shared

open System

[<CLIMutable>]
type Todo = { Id: Guid; TimeStamp: DateTime; Description: string }

module Todo =
    let isValid (description: string) =
        String.IsNullOrWhiteSpace description |> not

    let create (description: string) =
        { Id = Guid.NewGuid()
          TimeStamp = DateTime.Now
          Description = description }

module Route =
    let builder typeName methodName =
        sprintf $"/api/{typeName}/{methodName}"

type ITodosApi =
    { getTodos: unit -> Async<Todo list>
      addTodo: Todo -> Async<Todo> }