module Index

open Elmish
open Feliz.Router
open AllPages
open Shared

type Model =
    { Global: GlobalModel
      LoginPage:LoginPage.Model
      StartPage: StartPage.Model
      TodoPage: TodosPage.Model }

type Msg =
    | LoginPageMsg of LoginPage.Msg
    | StartPageMsg of StartPage.Msg
    | TodoPageMsg of TodosPage.Msg
    | UrlChanged of string list
    | GetTodos
    | GotTodos of Todo list

let initModel: Model =
    { Global =
        { CurrentUrl = Router.currentUrl ()
          TodosAccess =
              (Browser.WebStorage.localStorage.getItem "TodosAccess" = "true")
          Todos = []
          User = Browser.WebStorage.localStorage.getItem "UserName" }
      LoginPage = fst (LoginPage.init ())
      StartPage = fst (StartPage.init ())
      TodoPage = fst (TodosPage.init ()) }
    //TODO: maybe init globalModel? no more need for newGlobal in Update

let init () : Model * Cmd<Msg> =
    let cmd = Cmd.OfAsync.perform todosApi.getTodos () GotTodos
    let model = initModel
    model, cmd

let update (msg: Msg) (model: Model) =
    match msg with
    | LoginPageMsg m ->
        let l, g, c = LoginPage.update m model.LoginPage model.Global
        { model with LoginPage = l; Global = g }, (Cmd.map LoginPageMsg c)
    | StartPageMsg m ->
        let l, g, c = StartPage.update m model.StartPage model.Global
        { model with StartPage = l; Global = g }, (Cmd.map StartPageMsg c)
    | TodoPageMsg m ->
        let l, g, c = TodosPage.update m model.TodoPage model.Global
        { model with TodoPage = l; Global = g }, (Cmd.map TodoPageMsg c)
    | UrlChanged segments ->
        let newGlobal: GlobalModel =
            { CurrentUrl = segments
              TodosAccess = model.Global.TodosAccess
              Todos = model.Global.Todos
              User = Browser.WebStorage.localStorage.getItem "UserName" }
        { model with Global = newGlobal }, Cmd.none
    | GetTodos ->
        let cmd = Cmd.OfAsync.perform todosApi.getTodos () GotTodos
        model, cmd
    | GotTodos todos ->
        let newGlobal: GlobalModel =
            { CurrentUrl = model.Global.CurrentUrl
              TodosAccess = model.Global.TodosAccess
              Todos = todos
              User = Browser.WebStorage.localStorage.getItem "UserName" }
        { model with Global = newGlobal }, Cmd.none

open Feliz

let view (model: Model) (dispatch: Msg -> unit) =
    let currentPage =
        match model.Global.CurrentUrl with
        | [] -> StartPage.view model.StartPage model.Global (StartPageMsg >> dispatch)
        | [ "Start Page" ] -> StartPage.view model.StartPage model.Global (StartPageMsg >> dispatch)
        | [ "Login Page" ] ->  LoginPage.view model.LoginPage model.Global (LoginPageMsg >> dispatch)
        | [ "Todo Page" ] ->
            if model.Global.TodosAccess then
                TodosPage.view model.TodoPage model.Global (TodoPageMsg >> dispatch)
            else Html.h1 "Not Logged in"
            //else LoginPage.view model.LoginPage model.Global (LoginPageMsg >> dispatch)
        //        | [ "users"; Route.Query [ "id", Route.Int userId ] ] -> //// KEEP FOR FUTURE Queries
        //            Html.h1 (sprintf "Showing user %d" userId)
        | _ -> Html.h1 "Something went wrong. Page not found"

    React.router [
        router.onUrlChanged (UrlChanged >> dispatch)
        router.children currentPage
    ]