module Parse

open System
open FSharp.Text.Lexing
open Absyn
open System.Text.Json.Serialization

// Parse a string into an expression
let fromString (str: string) : expr =
    let lexbuf = LexBuffer<char>.FromString(str)
    try 
        Par.Main Lex.token lexbuf
    with 
    | e -> 
        let pos = lexbuf.EndPos
        failwithf "Parse error near line %d, column %d: %s" 
            pos.Line pos.Column e.Message
