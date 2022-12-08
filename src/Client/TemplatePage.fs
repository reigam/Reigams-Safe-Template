/// You can use this page as a template for your own custom pages.
module TemplatePage

open AllPages

open Elmish
open Fable.Remoting.Client
open AllPages
open Fable.Core.JsInterop
open Feliz.Router
open Shared

importAll "./css/tailwind.css"

let thisPage: WebPages.PageName = WebPages.pages.TemplatePage

type Model = { SharedElements: SharedElements.Model }

type Msg =
    | SharedElementsMsg of SharedElements.Msg
    | NavigateTo of WebPages.PageName
    | BackToStart

let initModel: Model = { SharedElements = fst (SharedElements.init ()) }

let init () = initModel, Cmd.none

let update (msg: Msg) (model: Model) (globalModel: GlobalModel) =
    match msg with
    | SharedElementsMsg m ->
        let l, g, c = SharedElements.update m model.SharedElements globalModel
        { model with SharedElements = l }, g, (Cmd.map SharedElementsMsg c)
    | NavigateTo page ->
        let pageName = WebPages.getPageName page
        model, globalModel, Cmd.navigate (pageName)
    | BackToStart -> model, globalModel, Cmd.navigate ()

open Feliz
open Feliz.DaisyUI

let view (model: Model) (globalModel: GlobalModel) dispatch =
    Html.div [
        prop.children [
            SharedElements.globalNavbar model.SharedElements globalModel (SharedElementsMsg >> dispatch)
            Daisy.hero [
                prop.className "min-h-screen flex"
                prop.style [
                    style.backgroundImageUrl "https://unsplash.it/1200/900?random"
                ]
                prop.children [
                    Daisy.heroContent [
                        prop.className "container text-center text-neutral-content hero-overlay bg-opacity-60"
                        prop.children [
                            Html.div [
                                prop.className "max-w-md "
                                prop.children [
                                    Html.h1 [
                                        prop.className "mb-5 text-5xl font-bold"
                                        prop.text "Do It!"
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