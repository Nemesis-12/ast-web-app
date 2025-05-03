module AST

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http
open System.Text.Json.Serialization
open Parse
open Absyn

[<CLIMutable>]
type ParseRequest =
    { [<JsonPropertyName("code")>]
      Code: string }

[<ApiController>]
[<Route("api/ast")>]
type ASTController() =
    inherit ControllerBase()

    [<HttpPost("parse")>]
    member _.Parse([<FromBody>] req: ParseRequest) =
        try
            let ast = Parse.fromString req.Code
            let jsonAst = Absyn.toJson ast
            ContentResult(
                Content = jsonAst,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            )
        with ex ->
            let errJson = sprintf """{"error":"%s"}"""
                            (ex.Message.Replace("\"","\\\""))
            ContentResult(
                Content = errJson,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status400BadRequest
            )
