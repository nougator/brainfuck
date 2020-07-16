// Learn more about F# at http://fsharp.orgopen System
open System
open System

let loadFile (path: string) = IO.File.ReadAllText path

let mutable register: int[] = [|0; 0; 0; 0; 0; 0; 0; 0; 0; 0;|]
let mutable selectedRegister: int = 0;

let mutable step = 0
let mutable inLoop = false
let mutable loops: int[] = [|0|]
let mutable currentLoop = 0

let printInt (num: int) = 
    sprintf "%c" (Convert.ToChar(num))


let checkChar (character: char) = 
    match character with
        | '+' ->
            register.[selectedRegister] <- register.[selectedRegister] + 1
        | '-' ->
            register.[selectedRegister] <- register.[selectedRegister] - 1
        | '<' ->
            if selectedRegister <= 0 then selectedRegister <- 9
            else selectedRegister <- selectedRegister - 1
        | '>' ->
            if selectedRegister <= 9 then selectedRegister <- 0
            else selectedRegister <- selectedRegister + 1
        | '.' ->
            if register.[selectedRegister] < 0 then register.[selectedRegister] <- 0
            printf "%c" (Convert.ToChar(register.[selectedRegister])) 
        | ',' ->
            ()
        | '[' ->
            inLoop <- true
            loops <- Array.append loops [|step|]
            currentLoop <- currentLoop + 1
            ()
        | ']' ->
            (if inLoop && register.[selectedRegister] > 0 then step <- loops.[currentLoop]
            else if register.[selectedRegister] <= 0 then inLoop <- false)
        | _ ->
            ()


[<EntryPoint>]
let main argv =
    (if argv.Length = 0 then
        printfn "Usage: brainfuck filePath"
        Environment.Exit 1
    )
    let mutable file = loadFile argv.[0]
    file <- file.Replace("\n", "")
    (while step < (String.length file) do
        if register.[selectedRegister] < 0 then register.[selectedRegister] <- 0
        checkChar file.[step]
        step <- step + 1
    )
    0