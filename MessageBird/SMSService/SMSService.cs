/*
 * File name            : BirdSMSService.cs
 * Author               : Munirul Islam
 * Date                 : 16 October 2026
 * Version              : 1.0
 *
 * Description          : interface for BirdSMSService for sending sms
 *
 * Modification history :
 * Name                         Date                            Desc
 * 
 * 
 * Copyright (c) 2026: THIS IS WRITTEN BY MUNIRUL ISLAM  FOR VEXCASH TEST PROJECT
 */

using MessageBird.Entities;
using MessageBird.Interfaces;
using MessageBird.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Serilog;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MessageBird.SMSService
{
    public class BirdSMSService: IBirdSendSMSClient
    {

        public BirdSMSService() { }
        
        public async Task<string> sendSMS(customerInfo customer, 
            string message, string apiUri,string senderId, string apiKey,
            CancellationToken cancellationToken = default
            )
        {
            // 1. Validate inputs
            ArgumentNullException.ThrowIfNull(customer);

            ArgumentException.ThrowIfNullOrWhiteSpace(
                customer.PhoneNumber, nameof(customer.PhoneNumber));

            ArgumentException.ThrowIfNullOrWhiteSpace(
                message, nameof(message));

            ArgumentException.ThrowIfNullOrWhiteSpace(
                apiUri, nameof(apiUri));

            ArgumentException.ThrowIfNullOrWhiteSpace(
                senderId, nameof(senderId));

            ArgumentException.ThrowIfNullOrWhiteSpace(
                apiKey, nameof(apiKey));

            try {


                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    Log.Information("BIRD_API_KEY is not loaded");
                    throw new InvalidOperationException(
                        "BIRD_API_KEY environment variable is missing.");
                }

                using var client = new HttpClient();

                client.BaseAddress = new Uri(apiUri);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiKey);

                var payload = new
                {
                    to = customer.PhoneNumber, 
                    from = senderId, 
                    text = message,
                    category = "transactional"
                };

                var json = JsonSerializer.Serialize(payload);
                Log.Information("payload is created {json}", json);
                using var content = new StringContent(
                    json, Encoding.UTF8, "application/json");

                using var response = await client.PostAsync(
                    apiUri, content);

                var responseBody = await response.Content.ReadAsStringAsync();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"HTTP Status: {(int)response.StatusCode}");
                Console.ResetColor();
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    Console.WriteLine("Message is send");
                }
                else
                {
                    
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Message is not send");
                    Console.ResetColor();
                }
                Log.Information("Response of payload {responseBody}", responseBody);

                //get the static response from and store it in db

                return responseBody;

            } catch (Exception ex) {

                Log.Error(
                      ex,
                      "Failed to send message due to exception {ex}",
                      ex);

                throw;
            }
        }

        internal void sendSMS()
        {
            throw new NotImplementedException();
        }


        

    }
}
