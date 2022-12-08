/// You can use this page as a Login for your own custom pages.
module LoginPage

open AllPages

open Elmish
open Fable.Remoting.Client
open AllPages
open Fable.Core.JsInterop
open Feliz.Router
open Shared

importAll "./css/tailwind.css"

let thisPage: WebPages.PageName = WebPages.pages.LoginPage

type Model = { SharedElements: SharedElements.Model
               Title: WebPages.PageName }

type Msg =
    | SharedElementsMsg of SharedElements.Msg
    | NavigateTo of WebPages.PageName
    | BackToStart
    | UserChanged of string
    | ChangeTodosAccess of bool

let initModel: Model = { SharedElements = fst (SharedElements.init ())
                         Title = thisPage }

let init () = initModel, Cmd.none

let update (msg: Msg) (model: Model) (globalModel: GlobalModel) =
    match msg with
    | SharedElementsMsg m ->
        let l, g, c = SharedElements.update m model.SharedElements globalModel
        { model with SharedElements = l }, g, (Cmd.map SharedElementsMsg c)
    | NavigateTo page ->
        scrollToTop
        let pageName = WebPages.getPageName page
        model, globalModel, Cmd.navigate (pageName)
    | BackToStart ->
        scrollToTop
        model, globalModel, Cmd.navigate ()
    | UserChanged user ->
        Browser.WebStorage.localStorage.setItem ("UserName", user)
        if (user = "user") then
            model , {globalModel with User = user}, Cmd.ofMsg (ChangeTodosAccess true)
        else
            model, {globalModel with User = user}, Cmd.ofMsg (ChangeTodosAccess false)
    | ChangeTodosAccess b ->
        if b then
            Browser.WebStorage.localStorage.setItem ("TodosAccess", "true")
        else
            Browser.WebStorage.localStorage.setItem ("TodosAccess", "false")
        model, {globalModel with TodosAccess = b }, Cmd.none

open Feliz
open Feliz.DaisyUI

let view (model: Model) (globalModel: GlobalModel) dispatch =
    Daisy.hero [
        prop.className "flex"
        prop.style [
            style.backgroundImageUrl "https://unsplash.it/1200/900?random"
        ]
        prop.children [
            SharedElements.globalNavbar model.SharedElements globalModel (SharedElementsMsg >> dispatch)
            Daisy.heroContent [
                prop.className "container min-h-screen text-center text-neutral-content hero-overlay bg-opacity-60"
                prop.children [
                    Html.div [
                        prop.className "max-w-md "
                        prop.children [
                            Html.h1 [
                                prop.className "mb-5 text-5xl font-bold"
                                prop.text "Log ME In"
                            ]
                            Daisy.card [
                                prop.className "mt-3"
                                card.bordered
                                prop.children [
                                    Daisy.formControl [
                                        Daisy.label [Daisy.labelText "Enter 'user' to get access to the Todos Page!"]
                                        Daisy.input [
                                            input.bordered
                                            prop.value globalModel.User
                                            prop.placeholder "Username"
                                            prop.onTextChange (fun s ->
                                                dispatch (UserChanged s))
                                                // if s = "user" then dispatch (ChangeTodosAccess true)
                                                // else dispatch (ChangeTodosAccess false))
                                        ]
                                    ]
                                    Daisy.button.button [
                                        button.primary
                                        prop.className "mt-3 hover:bg-sky-700"
                                        prop.text "Got to Todo Page"
                                        if not globalModel.TodosAccess then
                                            button.disabled
                                        prop.onClick (fun _ -> dispatch (NavigateTo WebPages.pages.TodoPage))
                                    ]
                                ]
                            ]

                        ]
                    ]
                ]
            ]
            SharedElements.globalFooter model.SharedElements globalModel (SharedElementsMsg >> dispatch)
        ]
    ]
