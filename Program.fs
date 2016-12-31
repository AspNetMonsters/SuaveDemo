open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open System.Net


let home = OK "Hello home"

[<EntryPoint>]
let main argv = 
    let app = 
        choose 
            [ GET >=> choose
                [ path "/home" >=> home
                  path "/about" >=> OK "All about" ]
              POST >=> choose
                [
                    path "/home" >=> OK "Hello post to home"
                    path "/about" >=> OK "Hello post to about"
                ]
            ]
    let config = { defaultConfig with 
                    bindings =
                                [ HttpBinding.mk HTTP IPAddress.Loopback 9000us] }
    startWebServer config app
    0 // return an integer exit code

