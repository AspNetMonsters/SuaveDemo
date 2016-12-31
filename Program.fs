open Suave
open Suave.Filters
open Chiron.Operators
open Suave.Operators
open Suave.Successful
open Suave.Headers
open System.Net
open Suave.Utils
open Chiron
open Writers


let home = OK "Hello home"

type account = 
    { number: string;
      balance: decimal;
      name: string}
    static member ToJson(x:account) = 
        Json.write "number" x.number
       *> Json.write "balance" x.balance
       *> Json.write "name" x.name


let accountInfo = 
    let acc = { 
        number= "904324-1";
        balance= 33.87m;
        name="Chequing 1"
    } 
    
    let b = acc |> Json.serialize |> Json.format 
    OK (b) >=> setMimeType "application/json; charset=utf-8"

[<EntryPoint>]
let main argv = 
    let app = 
        choose 
            [ GET >=> choose
                [ path "/home" >=> home
                  path "/account" >=> accountInfo ]
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

