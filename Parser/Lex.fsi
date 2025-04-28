module Lex

open Absyn
open Par
open FSharp.Text.Lexing/// Rule token
val token: lexbuf: LexBuffer<char> -> token
