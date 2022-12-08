module StartPage

open AllPages

open Elmish
open Fable.Core.JsInterop
open Feliz.Router

importAll "./css/tailwind.css"

let thisPage: WebPages.PageName = WebPages.pages.StartPage

type Model =
    { SharedElements: SharedElements.Model
      Title: WebPages.PageName }

type Msg =
    | SharedElementsMsg of SharedElements.Msg
    | NavigateTo of WebPages.PageName

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
        model, globalModel, Cmd.navigate pageName
(*  // Router.navigate with query string parameters
    | NavigateToId userId -> model, Cmd.navigate("users", [ "id", userId ])  *)

open Feliz
open Feliz.DaisyUI

let view (model: Model) (globalModel: GlobalModel) dispatch =

    let continueButton =
        Daisy.button.button [
            button.primary
            prop.className "mt-3 hover:bg-sky-700"
            if globalModel.TodosAccess then
                prop.text "Got to Todo Page"
                prop.onClick (fun _ -> dispatch (NavigateTo WebPages.pages.TodoPage))
            else
                prop.text "Got to Login Page"
                prop.onClick (fun _ -> dispatch (NavigateTo WebPages.pages.LoginPage))
        ]

    let welcomeText =
        Html.h1 [
            prop.className "mb-5 text-5xl font-bold"
            prop.text "Welcome to the start page of this app"
        ]

    let startHero =
        Daisy.hero [
            prop.className "min-h-screen flex "
            prop.style [
                style.backgroundImageUrl "https://picsum.photos/id/1005/1600/1400"
            ]
            prop.children [
                SharedElements.globalNavbar model.SharedElements globalModel (SharedElementsMsg >> dispatch)
                Daisy.heroContent [
                    prop.className "container text-center text-neutral-content hero-overlay "
                    prop.children [
                        Html.div [
                            prop.className "max-w-md "
                            prop.children [
                                welcomeText
                                continueButton
                            ]
                        ]
                    ]
                ]
            ]
        ]

    Html.div [
        prop.children [
            startHero
            SharedElements.globalFooter model.SharedElements globalModel (SharedElementsMsg >> dispatch)
        ]
    ]

