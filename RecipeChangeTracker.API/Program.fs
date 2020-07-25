module RecipeChangeTracker.API.App

open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open RecipeChangeTracker.API.Handlers

let webApp =
    choose
        [ GET
          >=> choose [ route "/recipes" >=> Recipes.getLatestHander ]
          setStatusCode 404 >=> text "Not Found" ]

let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse
    >=> setStatusCode 500
    >=> text ex.Message

let configureCors (builder: CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader()
    |> ignore

let configureApp (app: IApplicationBuilder) =
    let env =
        app.ApplicationServices.GetService<IWebHostEnvironment>()

    (match env.EnvironmentName with
     | "Development" -> app.UseDeveloperExceptionPage()
     | _ -> app.UseGiraffeErrorHandler(errorHandler)).UseHttpsRedirection().UseCors(configureCors).UseStaticFiles()
        .UseGiraffe(webApp)

let configureServices (services: IServiceCollection) =
    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder: ILoggingBuilder) =
    builder.AddConsole().AddDebug() |> ignore

[<EntryPoint>]
let main _ =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot = Path.Combine(contentRoot, "WebRoot")

    let portEnvVar =
        Environment.GetEnvironmentVariable("PORT")

    let port =
        if isNull portEnvVar then "5000" else portEnvVar

    let url = sprintf "%s:%s" "http://0.0.0.0" port

    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHostBuilder ->
        webHostBuilder.UseContentRoot(contentRoot).UseWebRoot(webRoot).UseUrls(url)
                      .Configure(Action<IApplicationBuilder> configureApp).ConfigureServices(configureServices)
                      .ConfigureLogging(configureLogging)
        |> ignore).Build().Run()
    0
