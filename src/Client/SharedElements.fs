module SharedElements

open AllPages

open Elmish

open Fable.Core.JsInterop
open Feliz.Router


importAll "./css/tailwind.css"

type Model = { Title: string }

type Msg =
    | NavigateTo of WebPages.PageName
    | Logout
    | BackToStart
    | ChangeTodosAccess of bool

let initModel: Model = { Title = "SAFE4 Template" }

let init () = initModel, Cmd.none

let update (msg: Msg) (model: Model) (globalModel: GlobalModel) =
    match msg with
    | NavigateTo page ->
        scrollToTop
        let pageName = WebPages.getPageName page
        model, globalModel, Cmd.navigate pageName
    | Logout ->
        Browser.WebStorage.localStorage.setItem ("UserName", "")
        Browser.WebStorage.localStorage.setItem ("TodosAccess", "false")
        model, {globalModel with
                    User = ""
                    TodosAccess = false}, Cmd.ofMsg (NavigateTo WebPages.pages.StartPage)
    | BackToStart ->
        scrollToTop
        model, globalModel, Cmd.navigate ()
    | ChangeTodosAccess b ->
        if b then
            Browser.WebStorage.localStorage.setItem ("TodosAccess", "true")
        else
            Browser.WebStorage.localStorage.setItem ("TodosAccess", "false")
        model, {globalModel with TodosAccess = b }, Cmd.none

open Feliz
open Feliz.DaisyUI

let globalNavbar (model: Model) (globalModel: GlobalModel) dispatch =
    Daisy.navbar [
//        theme.cyberpunk // <-- this is the theme
        prop.className "mb-2 shadow-lg text-primary rounded-box backdrop-blur-sm first:bg-accent first:opacity-80 hover:opacity-100"
        prop.children [
            Daisy.navbarStart [
                Daisy.dropdown [
                    dropdown.hover
                    prop.children [
                        Daisy.button.button [
                            button.square
                            button.ghost
                            prop.children [
                                Html.i [ prop.className "fas fa-bars" ]
                            ]
                        ]
                        Daisy.dropdownContent [
                            prop.className "p-2 shadow menu bg-base-100 rounded-box w-52 fixed left-0"
                            prop.tabIndex 0
                            prop.children [
                                Html.li [Html.a [prop.text "Item 1"]]
                                Html.li [Html.a [prop.text "Item 2"]]
                                Html.li [Html.a [prop.text "Item 3"]]
                            ]
                        ]
                    ]
                ]
            ]
            Daisy.navbarCenter [
                prop.children [ Html.span model.Title ]
            ]
            Daisy.navbarEnd [
                Daisy.dropdown [
                    dropdown.hover
                    prop.children [
                        Daisy.button.button [
                            button.square
                            button.ghost
                            prop.children [
                                Html.i [
                                    prop.className "fas fa-ellipsis-h"
                                ]
                            ]
                        ]
                        Daisy.dropdownContent [
                            prop.className "p-2 shadow menu bg-base-100 rounded-box w-52 fixed right-0"
                            prop.tabIndex 0
                            prop.children [
                                if globalModel.TodosAccess then
                                    Html.li [Html.a [
                                        prop.text "Log out"
                                        prop.onClick (fun _ -> dispatch Logout)
                                        //TODO Reset UserName
                                        //TODO Make Global Messages
                                    ]]
                                else
                                    Html.li [Html.a [
                                        prop.text "Log in"
                                        prop.onClick (fun _ -> dispatch (NavigateTo WebPages.pages.LoginPage))
                                    ]]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]

let globalFooter (model: Model) (globalModel: GlobalModel) dispatch =
    Daisy.footer [
        prop.className "p-10 bg-primary text-neutral-content"
        prop.children [
            Html.div [
                Daisy.footerTitle "Services"
                Daisy.link [
                    link.hover
                    prop.text "Branding"
                ]
                Daisy.link [
                    link.hover
                    prop.text "Design"
                ]
                Daisy.link [
                    link.hover
                    prop.text "Marketing"
                ]
                Daisy.link [
                    link.hover
                    prop.text "Advertisement"
                ]
            ]
            Html.div [
                Daisy.footerTitle "Company"
                Daisy.link [
                    link.hover
                    prop.text "About us"
                ]
                Daisy.link [
                    link.hover
                    prop.text "Contact"
                ]
                Daisy.link [
                    link.hover
                    prop.text "Jobs"
                ]
                Daisy.link [
                    link.hover
                    prop.text "Press kit"
                ]
            ]
            Html.div [
                Daisy.footerTitle "Legal"
                Daisy.link [
                    link.hover
                    prop.text "Legal"
                ]
                Daisy.link [
                    link.hover
                    prop.text "Terms of use"
                ]
                Daisy.link [
                    link.hover
                    prop.text "Privacy policy"
                ]
                Daisy.link [
                    link.hover
                    prop.text "Cookie policy"
                ]
            ]
        ]
    ]

let globalBackButton (model: Model) (globalModel: GlobalModel) dispatch (buttonText: string) =
    Daisy.button.button [
        button.primary
        prop.className "hover:bg-sky-700"
        prop.text buttonText
        prop.onClick (fun _ -> dispatch BackToStart)
    ]