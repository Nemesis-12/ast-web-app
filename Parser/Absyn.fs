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
        sprintf """{"type": "App", "function": %s, "argument": %s}""" 
            (toJson e1) (toJson e2)
