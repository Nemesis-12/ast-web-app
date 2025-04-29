module Startup

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

type Startup(configuration: IConfiguration) =
    member _.Configuration = configuration

    member _.ConfigureServices(services: IServiceCollection) =
        services.AddControllers() |> ignore
        services.AddCors(fun options ->
            options.AddPolicy("AllowAll", fun builder ->
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader() |> ignore
            )
        ) |> ignore

    member _.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if env.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore

        app.UseHttpsRedirection() |> ignore
        app.UseDefaultFiles() |> ignore
        app.UseStaticFiles() |> ignore
        app.UseRouting() |> ignore
        app.UseCors("AllowAll") |> ignore

        app.UseEndpoints(fun endpoints ->
            endpoints.MapControllers() |> ignore
            endpoints.MapFallbackToFile("index.html") |> ignore
        ) |> ignore
