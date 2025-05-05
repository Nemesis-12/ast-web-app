module Absyn

type expr = 
    | CstI of int                         // Integer constant
    | CstB of bool                        // Boolean constant
    | Var of string                       // Variable
    | Let of string * expr * expr         // Let binding
    | Prim of string * expr * expr        // Primitive operation
    | If of expr * expr * expr            // Conditional
    | Fun of string * expr                // Function declaration
    | App of expr * expr                  // Function application

let rec toJson (e: expr) =
    match e with
    | CstI i -> 
        sprintf """{"type": "CstI", "value": %d}""" i
    | CstB b -> 
        sprintf """{"type": "CstB", "value": %b}""" b
    | Var x -> 
        sprintf """{"type": "Var", "name": "%s"}""" x
    | Let(x, e1, e2) -> 
        sprintf """{"type": "Let", "name": "%s", "definition": %s, "body": %s}""" 
            x (toJson e1) (toJson e2)
    | Prim(op, e1, e2) -> 
        sprintf """{"type": "Prim", "operator": "%s", "left": %s, "right": %s}""" 
            op (toJson e1) (toJson e2)
    | If(e1, e2, e3) -> 
        sprintf """{"type": "If", "condition": %s, "thenExpr": %s, "elseExpr": %s}""" 
            (toJson e1) (toJson e2) (toJson e3)
    | Fun(x, e) -> 
        sprintf """{"type": "Fun", "param": "%s", "body": %s}""" 
            x (toJson e)
    | App(e1, e2) -> 
        sprintf """{"type": "", "function": %s, "argument": %s}""" 
            (toJson e1) (toJson e2)

let rec toBracketNotation (e: expr) =
    match e with
    | CstI i -> string i
    | Var x -> $"[Var {x}]"
    | Prim(op, e1, e2) -> $"[Prim {op} {toBracketNotation e1} {toBracketNotation e2}]"
    | Let(x, e1, e2) -> $"[Let [Var {x}] {toBracketNotation e1} {toBracketNotation e2}]"
    | If(e1, e2, e3) -> $"[If {toBracketNotation e1} {toBracketNotation e2} {toBracketNotation e3}]"
    | Fun(x, body) -> $"[Fun [Param {x}] {toBracketNotation body}]"
    | App(f, arg) -> $"[App {toBracketNotation f} {toBracketNotation arg}]"
