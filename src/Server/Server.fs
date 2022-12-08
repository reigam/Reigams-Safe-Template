module Server

open Giraffe
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn

open Shared
open Database
open Api.Todos

let webApp =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue (database "Todo" |> TodosStorage |> todosApi)
    |> Remoting.buildHttpHandler

let app =
    application {
        // url "http://*:8085"
        use_router (choose [ webApp ]) // ready for new webApps [ webApp; webApp2; ... ]
        memory_cache
        use_static "public"
        use_gzip
    }

[<EntryPoint>]
let main _ =
    run app
    0