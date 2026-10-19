
# MessageBird SMS Integration — .NET 10

A .NET 10 console application for retrieving customer information from a database, generating personalized SMS messages from database-driven templates, and integrating with the Bird (formerly MessageBird) SMS API.

## Overview

This project demonstrates how to build a database-driven SMS messaging application using C#, .NET 10, Entity Framework Core, SQLite, and Serilog.

The application allows a user to search for a customer using a credit number, retrieve available message templates, generate a personalized message, and prepare it for delivery through the Bird SMS API.


## Technology Stack

 Technology 
========================
| C# 
|.NET 10 
| Entity Framework Core 
| SQLite - Local relational database 
| Bird SMS API - SMS message submission 

## Prerequisites

- .NET 10 SDK
- A Bird account
- A Bird API key with the required permissions
- SQLite database and connection configuration
- A permitted test phone number for SMS testing
-
Install any missing packages required by the project:

## Configuration
Update the following information from the appsettings 
"apiKey": "your api keu",
"senderId": "your authorized sender"

After changing the appsettings file please keep the file the following locations. 

appsettings folder location \vexCash\MessageBird\bin\Debug\net10.0

database folder location \vexCash\MessageBird\bin\Debug\net10.0

log file location: \vexCash\MessageBird\bin\Debug\logs

## Documentation

Quickstart:https://developers.messagebird.com/quickstarts/sms-overview/

API Reference:https://developers.messagebird.com/api/sms-messaging/“

## How to run
1. application is compled. ensure that you have visual studion 2026 is installed in your working station. 
2. folder location \vexCash\MessageBird\bin\Debug\net10.0\messagebird.exe application file
 

