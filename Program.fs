module Program

open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Http
open AST            // contains [<CLIMutable>] ParseRequest
open Parse          // Parse.fromString
open Absyn          // Absyn.toJson

[<EntryPoint>]
let main args =
  let builder = WebApplication.CreateBuilder(args)
  builder.Services.AddCors(fun o ->
    o.AddPolicy("AllowAll", fun p -> p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader() |> ignore)
  ) |> ignore

  let app = builder.Build()

  app.UseDefaultFiles() |> ignore
  app.UseStaticFiles() |> ignore

  app.MapPost(
    "/api/ast/parse",
    Func<ParseRequest, IResult>(fun req ->
      try
        let ast = Parse.fromString req.Code
        let formattedAst = ast.ToString().Replace("\\n", "\n").Replace("\\t", "\t")
        Results.Text(formattedAst)

      with ex ->
        Results.Text(sprintf """{"error":"%s"}""" (ex.Message.Replace("\"","\\\"")))
    )
  ) |> ignore

  app.Run()
  0
