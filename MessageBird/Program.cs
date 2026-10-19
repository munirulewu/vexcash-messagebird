/*
 * File name            : Program.cs
 * Author               : Munirul Islam
 * Date                 : 16 October 2026
 * Version              : 1.0
 *
 * Description          : Main class of SMS Alter
 *
 * Modification history :
 * Name                         Date                            Desc
 * 
 * 
 * Copyright (c) 2026: THIS IS WRITTEN BY MUNIRUL ISLAM  FOR VEXCASH TEST PROJECT
 */

using MessageBird.DatabaseContext;
using MessageBird.DA;
using MessageBird.Services;
using MessageBird.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

// Configure Serilog before starting the application.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
  
    .WriteTo.File(
        "logs/messagebird-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30)
    .CreateLogger();

try {

    Log.Information("MessageBird SMS application starting");

    //Configaration Application Appsettings file
    var configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

    Log.Information("Application configuration loaded");


    // Read the BaseUrl
    string baseUrl = configuration["APIsettings:baseURL"]
        ?? throw new InvalidOperationException(
            "APIsettings:BaseUrl is missing from appsettings.json.");
    //read message end point
    string endpoint = configuration["APIsettings:messagesEndpoint"]
        ?? throw new InvalidOperationException(
            "BirdApi:MessagesEndpoint is missing from appsettings.json.");
    //Creating API Uri
    string apiUrl = $"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";
    //read message end point
    string senderId = configuration["APIsettings:senderId"]
        ?? throw new InvalidOperationException(
            "BirdApi:MessagesEndpoint is missing from appsettings.json.");
    //Read APi Key
    string apiKey = configuration["APIsettings:apiKey"]
        ?? throw new InvalidOperationException(
            "BirdApi:MessagesEndpoint is missing from appsettings.json.");

    var services = new ServiceCollection();

    services.AddSingleton<IConfiguration>(configuration);

    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(
            configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<MessadeDA>();
    services.AddScoped<Utility>();
    services.AddScoped<TemplateDataAccess>();
    services.AddScoped<CustomerDataAccess>();

    using var serviceProvider = services.BuildServiceProvider();

    // Resolve the data-access class through DI.
    using var scope = serviceProvider.CreateScope();

    var oMessageDA = scope.ServiceProvider.GetRequiredService<MessadeDA>();
    var oUtility = scope.ServiceProvider.GetRequiredService<Utility>();

    Log.Information("Database services initialized");

    //generate Menu 
    await oUtility.generateMenu(apiUrl, senderId, apiKey);
    Log.Information("menu loaded successfully");
}
catch (Exception ex) {

    Log.Error(
                ex,
                "Error while loading main program");

    Console.WriteLine(
        "An error occurred. Check the application log.");

}



