module AllPages

open System.Net
open Elmish
open Fable.Remoting.Client
open Shared
open Fable.Core.JsInterop
open Feliz.Router

importAll "./css/tailwind.css"

module WebPages =
    type PageName = PageName of string
    let getPageName (PageName str) = str

    type Pages =
        { TemplatePage: PageName
          LoginPage: PageName
          StartPage: PageName
          TodoPage: PageName }

    let pages: Pages =
        { TemplatePage = PageName "Template Page"
          LoginPage = PageName "Login Page"
          StartPage = PageName "Start Page"
          TodoPage = PageName "Todo Page" }

let todosApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ITodosApi>

type GlobalModel =
    { CurrentUrl: string list
      TodosAccess: bool
      Todos: Todo list
      User: string }

// Integrating javaScript commands
open Fable.Core

/// implements javaScript scrollToTop function
[<Emit("window.scrollTo(0, 0)")>]
let scrollToTop: unit = jsNative

// type Msg =
//     | NavigateTo of WebPages.PageName
//     | BackToStart
//
// let update (msg: Msg) (model) (globalModel: GlobalModel) =
//     match msg with
//     | NavigateTo page ->
//         scrollToTop
//         let pageName = WebPages.getPageName page
//         model, globalModel, Cmd.navigate pageName
//     | BackToStart ->
//         scrollToTop
//         model, globalModel, Cmd.navigate ""

