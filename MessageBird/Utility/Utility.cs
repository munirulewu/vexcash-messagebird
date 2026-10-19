/*
 * File name            : Utility.cs
 * Author               : Munirul Islam
 * Date                 : 18 October 2026
 * Version              : 1.0
 *
 * Description          : Implementation for Utility 
 *
 * Modification history :
 * Name                         Date                            Desc
 * 
 * 
 * Copyright (c) 2026: THIS IS WRITTEN BY MUNIRUL ISLAM  FOR VEXCASH TEST PROJECT
 */

using MessageBird.DA;
using MessageBird.Entities;
using MessageBird.Models;
using MessageBird.Services;
using MessageBird.SMSService;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MessageBird.SMSService;
namespace MessageBird.Utility
{
    public sealed class Utility
    {


        private readonly MessadeDA _messageDA;
        private readonly TemplateDataAccess _templateDataAccess;
        private readonly CustomerDataAccess _customerDataAccess;
        public Utility(MessadeDA messageDA, TemplateDataAccess templateDataAccess, CustomerDataAccess customerDataAccess)
        {
            _messageDA = messageDA
           ?? throw new ArgumentNullException(nameof(messageDA));

            _templateDataAccess = templateDataAccess
          ?? throw new ArgumentNullException(nameof(templateDataAccess));

            _customerDataAccess = customerDataAccess
          ?? throw new ArgumentNullException(nameof(customerDataAccess));

        }


        public async Task generateMenu(string apiUrl, string senderId, string apiKey)
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();

                Console.WriteLine("==================================");
                Console.WriteLine("      VexCash SMS Management");
                Console.WriteLine("==================================");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" Before Sendig SMS please have a look on existing customer and template id by choosing option 1 and 2");
                Console.WriteLine("=========================================================================================");
                Console.ResetColor();
                Console.WriteLine("1. View All Template IDs");
                Console.WriteLine("2. View All Customer Information");
                Console.WriteLine("3. Send SMS");
                Console.WriteLine("4. Help");
                Console.WriteLine("0. Exit");
                Console.WriteLine("==================================");
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ViewAllTemplateIds();
                        break;

                    case "2":
                        await ViewAllCustomerInformation();
                        break;
                    case "3":
                        await SendSMSToCustomer(apiUrl, senderId, apiKey);
                        break;
                    case "4":
                        DisplaySmsHelp();
                        break;
                    case "0":
                        isRunning = false;
                        Console.WriteLine("Application closed.");
                        break;

                    default:
                        Console.WriteLine(
                            "Invalid choice. Please select 1, 2,3,4 or 0.");
                        break;
                }

                if (isRunning)
                {
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(intercept: true);
                }
            }
        }

        private async Task SendSMSToCustomer(string apiUrl, string senderId,string apiKey)
        {
            Console.WriteLine("Enter credit number of customer:");
            string ? creditNumber = Console.ReadLine();
            string? message = string.Empty;
            try {

                if (creditNumber.Trim() != null)
                {
                    customerInfo? oCustomer = await _messageDA.GetCustomerByCreditNumberAsync(creditNumber);
                    if (oCustomer != null)
                    {
                        Log.Information("Customer lookup successful");
                        Console.WriteLine("Enter a Template Id:");
                        string? templeteId = Console.ReadLine();
                        Log.Information("selected Template id {templateId}", templeteId);
                        MessageTemplate? oTemplate = await _templateDataAccess.getTempplatebyId(Convert.ToInt16(templeteId));
                        if (oTemplate is not null)
                            message = await _messageDA.GenerateMessageAsync(oTemplate, oCustomer);

                        string responseBody = string.Empty;
                        if (oTemplate is not null)
                        {
                            BirdSMSService smsService = new BirdSMSService();
                            responseBody = await smsService.sendSMS(oCustomer, message, apiUrl, senderId, apiKey);
                            FormatErrorOutputt(responseBody);
                            // write response to database
                            await _messageDA.SaveAsync(oCustomer.Id, responseBody);
                            Log.Information("sms response is saved successfully in database");
                        }
                    }
                    else
                    {
                        Log.Warning("Customer lookup returned no result");
                        Console.WriteLine("Customer not found.");

                        
                    }


                }


            }
            catch (Exception ex)
            { Console.WriteLine(ex.ToString()); }
        }

        private void FormatErrorOutputt(string responseBody)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            BirdErrorResponse? result =
                JsonSerializer.Deserialize<BirdErrorResponse>(responseBody, options);

            if (result?.Error is not null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error Type: {result.Error.Type}");
                Console.WriteLine($"Error Name: {result.Error.Name}");
                Console.WriteLine($"Message: {result.Error.Message}");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Remediation: {result.Error.Remediation}");
                Console.ResetColor();

            }
            else
            {
                Console.WriteLine("Unable to parse the Bird API error response.");
            }
        }
        private async Task ViewAllTemplateIds()
        {
            Console.Clear();
            Console.WriteLine("===== All Template IDs =====");

            await _templateDataAccess.PrintAllTemplateListAsync();

           
        }

        private async Task ViewAllCustomerInformation()
        {
            Console.Clear();

            Console.WriteLine("===== All Customer Information =====");
            await _customerDataAccess.PrintCustomerListAsync();
           
        }


        private static string getjsonString()
        {
            string json = """
            {
              "id": "e8077d803532c0b5937c639b60216938",
              "href": "https://rest.messagebird.com/messages/e8077d803532c0b5937c639b60216938",
              "direction": "mt",
              "type": "sms",
              "originator": "YourName",
              "body": "This is a test message",
              "reference": null,
              "validity": null,
              "gateway": null,
              "typeDetails": {},
              "datacoding": "plain",
              "mclass": 1,
              "scheduledDatetime": null,
              "createdDatetime": "2016-05-03T14:26:57+00:00",
              "recipients": {
                "totalCount": 1,
                "totalSentCount": 1,
                "totalDeliveredCount": 0,
                "totalDeliveryFailedCount": 0,
                "items": [
                  {
                    "recipient": "31612345678",
                    "status": "sent",
                    "statusDatetime": "2016-05-03T14:26:57+00:00"
                  }
                ]
              }
            }
        """;
            return json;
        }
        public static async Task<MessageResponseDTO> gerStaticResponse()
        {


            string json = getjsonString();

            MessageResponseDTO? oMessageResponse = JsonSerializer.Deserialize<MessageResponseDTO>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return oMessageResponse ?? throw new JsonException(
             "Could not deserialize the JSON into MessageResponseDTO.");
        }

        public static void DisplaySmsHelp()
        {
            Console.Clear();

            // Title
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==============================================");
            Console.WriteLine("             SMS SENDING - HELP");
            Console.WriteLine("==============================================");
            Console.ResetColor();

            Console.WriteLine();

            // Important information
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("IMPORTANT: Check your Bird wallet balance!");
            Console.ResetColor();

            Console.WriteLine();

            // Instructions
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Steps to Send SMS:");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  1. Log in to your Bird organization account.");
            Console.WriteLine("  2. Top up your wallet with sufficient funds.");
            Console.WriteLine("  3. Configure your SMS sender correctly.");
            Console.WriteLine("  4. Return here and send your SMS.");
            Console.ResetColor();

            Console.WriteLine();

            // Warning
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("WARNING:");
            Console.WriteLine("SMS sending will fail if your wallet balance");
            Console.WriteLine("is insufficient. Top up before retrying.");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("==============================================");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ResetColor();

            Console.ReadKey(true);
        }

        public async void sendMessageWithDynamicOTP()
        {
            Console.WriteLine("=== Bird OTP SMS Console ===");
            //Bearer bk_eu1_5K0ZgBxFepm8PV90CdDNFG53tMOC2";
            // Read the API key from an environment variable.
            string? apiKey = Environment.GetEnvironmentVariable("BIRD_API_KEY");

            Log.Information("BIRD API KEY is read from environment variable");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.WriteLine(
                    "Error: BIRD_API_KEY environment variable is missing.");
                return;
            }

            Console.Write("Recipient phone number (+4915252066529): ");
            string recipient = "+4915252066529";// Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(recipient))
            {
                Console.WriteLine("Recipient phone number is required.");
                return;
            }

            // Generate a six-digit OTP for demonstration.
            string otp = Random.Shared.Next(100000, 1000000).ToString();

            var payload = new
            {
                to = recipient,
                template = new
                {
                    slug = "bird_otp_verification",
                    parameters = new
                    {
                        code = otp
                    }
                }
            };

            string json = JsonSerializer.Serialize(payload);

            using var http = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://eu1.platform.bird.com/v1/sms/messages");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            try
            {
                using HttpResponseMessage response =
                    await http.SendAsync(request);

                string responseBody =
                    await response.Content.ReadAsStringAsync();

                Console.WriteLine();
                Console.WriteLine($"HTTP Status: {(int)response.StatusCode}");
                Console.WriteLine($"Response: {responseBody}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(
                        "Bird accepted the OTP SMS for processing.");
                }
                else
                {
                    Console.WriteLine(
                        "SMS request failed. Check the response above.");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("The request timed out.");
            }
        }
    }
}
