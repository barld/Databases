// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

open System

// Define your library scripting code here

type AirportSize = Small | Medium | Large
type City = {Name:string}
type Airport = {Name:string;Capicity:int;Size:AirportSize}
type AirtportStateInCity = {Airport:Airport;City:City}
type Terminal = {Code:string;Open:bool}
type GateState = Boarding | Closed
type Gate = {Terminal:Terminal;State:GateState}
type Company = {Name:string}
type Flight = {TimeHours:int;TimeMinutes:int}

let rnd = new System.Random()

let getState () =
    if rnd.Next(0,2) = 1 then
        "Boarding"
    else
        "Closed"


let createGate (belongsTo:string) number =
    sprintf "(%s_%i:Gate{number:%i}), (%s_%i)-[:Is]->(%s)" belongsTo number number belongsTo number (getState())
    + (sprintf ",(%s_%i)-[:BlongsTo]->(%s)" belongsTo number belongsTo)
createGate "AMS_B" 2

let createTerminal airportCode code =
    let name = sprintf "%s_%c" airportCode code
    sprintf "(%s:Terminal{code:\"%c\", open:%b})" name code (rnd.Next(0,10)<7)
    + (sprintf ", (%s)-[:Includes]->(%s)" airportCode name)
    + ", "
    + ([1..rnd.Next(3,6)] |> List.map (createGate name) |> String.concat "," )

createTerminal "AMS" 'A'

let Airports = ["AMS"; "VCE"; "LHR"; "SEN"; "FCO"; "BER"; "HAM"]

Airports 
|> List.map (fun a -> a,['A'..('A' |> int |> (+) (rnd.Next(4,7)) |> char)])
|> List.map (fun (a, tcodes) -> (tcodes |> List.map (createTerminal a)) |> String.concat ",")
|> String.concat ","
|> printfn "%s"


