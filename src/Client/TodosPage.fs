module TodosPage

open AllPages

open Elmish
open Fable.Core.JsInterop
open Shared
open Feliz.Router


importAll "./css/tailwind.css"

let thisPage: WebPages.PageName = WebPages.pages.TodoPage

type Model =
    { SharedElements: SharedElements.Model
      Title: WebPages.PageName
      Input: string }

type Msg =
    | SharedElementsMsg of SharedElements.Msg
    | GotTodos of Todo list
    | SetInput of string
    | AddTodo
    | AddedTodo of Todo
    | GetTodos

let initModel: Model =
    { SharedElements = fst (SharedElements.init ())
      Title = thisPage
      Input = "" }

let init () =
    let cmd = Cmd.OfAsync.perform todosApi.getTodos () GotTodos
    initModel, Cmd.none

let update (msg: Msg) (model: Model) (globalModel: GlobalModel) =
    match msg with
    | SharedElementsMsg m ->
        let l, g, c = SharedElements.update m model.SharedElements globalModel
        { model with SharedElements = l }, g, (Cmd.map SharedElementsMsg c)
    | GotTodos todos -> model, { globalModel with Todos = todos }, Cmd.none
    | SetInput value -> { model with Input = value }, globalModel, Cmd.none
    | AddTodo ->
        let todo = Todo.create model.Input
        let cmd = Cmd.OfAsync.perform todosApi.addTodo todo AddedTodo
        { model with Input = "" }, globalModel, cmd
    | AddedTodo todo -> model, { globalModel with Todos = globalModel.Todos @ [ todo ] }, Cmd.none
    | GetTodos ->
        let cmd = Cmd.OfAsync.perform todosApi.getTodos () GotTodos
        model, globalModel, cmd

open Feliz
open Feliz.DaisyUI

let view (model: Model) (globalModel: GlobalModel) dispatch =
    Daisy.hero [
        prop.className " flex"
        prop.style [
            style.backgroundImageUrl "https://unsplash.it/1200/900?random"
        ]
        prop.children [
            SharedElements.globalNavbar model.SharedElements globalModel (SharedElementsMsg >> dispatch)
            Daisy.heroContent [
                prop.className "container min-h-screen text-center text-neutral-content hero-overlay bg-opacity-40"
                prop.children [
                    Html.div [
                        prop.className "max-w-md "
                        prop.children [
                            Html.h1 [
                                prop.className "mb-5 text-5xl font-bold"
                                prop.text "ToDos"
                            ]
                            Daisy.card [
                                prop.className "shadow-lg"
                                prop.children [
                                    Daisy.cardBody [

                                        Daisy.cardTitle "My TODO List"
                                        Html.ol [
                                            for todo in globalModel.Todos do
                                                //TODO Add Button To Delete the specific Todo
                                                //TODO Add Pop Up Text for TimeStamp
                                                Html.li [ prop.text $"{todo.Description}" ]
                                        ]
                                        Daisy.formControl [
                                            Html.div [
                                                prop.className "relative"
                                                prop.children [
                                                    Daisy.input [
                                                        input.bordered
                                                        input.primary
                                                        prop.value model.Input
                                                        prop.placeholder "New Todo Here"
                                                        prop.onChange (fun x -> SetInput x |> dispatch)
                                                    ]
                                                    Daisy.button.button [
                                                        button.primary
                                                        prop.className "absolute top-0 right-0 rounded-l-none"
                                                        prop.text "Go"
                                                        prop.onClick (fun _ -> AddTodo |> dispatch)
                                                    ]
                                                ]
                                            ]
                                        ]
                                    ]
                                ]
                            ]
                            Html.p "Try the button below, to go back to the start Page"
                            SharedElements.globalBackButton model.SharedElements globalModel (SharedElementsMsg >> dispatch) "Back to start"
                            //TODO: Better Composition for shared Elements (no longer call them global
                        ]
                    ]
                ]
            ]
            SharedElements.globalFooter model.SharedElements globalModel (SharedElementsMsg >> dispatch)
        ]
    ]