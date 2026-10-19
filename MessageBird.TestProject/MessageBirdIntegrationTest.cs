using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Xunit;
using Microsoft.Extensions.Configuration;

namespace MessageBird.TestProject
{

    public class MessageBirdIntegrationTest
    {
       
        private static readonly JsonSerializerOptions JsonOptions =
            new(JsonSerializerDefaults.Web);

        private readonly IConfiguration _configuration;

        public MessageBirdIntegrationTest()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

        }


        [Fact]
        public void ReadBirdSettings_FromAppSettings_ReturnsValues()
        {
            string? baseUrl = _configuration["Bird:BaseUrl"];
            string? endpoint = _configuration["Bird:SmsEndpoint"];
            string? senderId = _configuration["Bird:SenderId"];
            string? recipient = _configuration["Bird:Recipient"];
            string? apiKey = _configuration["Bird:ApiKey"];
            
            Assert.False(string.IsNullOrWhiteSpace(baseUrl));
            Assert.False(string.IsNullOrWhiteSpace(endpoint));
            Assert.False(string.IsNullOrWhiteSpace(senderId));
            Assert.False(string.IsNullOrWhiteSpace(recipient));
            Assert.False(string.IsNullOrWhiteSpace(apiKey));
            // Use these values in your SMS tests.
        }
        
        [Fact]
        public async Task SendSms_WithAuthorizedSender_ReturnsAcceptedResponse()
        {
            string? baseUrl = _configuration["Bird:BaseUrl"];
            string? endpoint = _configuration["Bird:SmsEndpoint"];
            string? senderId = _configuration["Bird:SenderId"];
            string? recipient = _configuration["Bird:Recipient"];
            string? apiKey = _configuration["Bird:ApiKey"];

            Assert.False(string.IsNullOrWhiteSpace(baseUrl));
            Assert.False(string.IsNullOrWhiteSpace(endpoint));
            Assert.False(string.IsNullOrWhiteSpace(senderId));
            Assert.False(string.IsNullOrWhiteSpace(recipient));
            Assert.False(string.IsNullOrWhiteSpace(apiKey));

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var payload = new
            {
                to = recipient,
                from = senderId,
                text = "This is a test message",
                category = "marketing"
            };

            string json = JsonSerializer.Serialize(payload, JsonOptions);

            using var content = new StringContent(
                json, Encoding.UTF8, "application/json");

            string apiUrl = $"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";
            using var response = await client.PostAsync(apiUrl, content);

            string responseBody =
                await response.Content.ReadAsStringAsync();

            Assert.True(
                response.StatusCode == HttpStatusCode.PaymentRequired,
                $"Expected 402 PaymentRequired {(int)response.StatusCode}. " +
                $"Response: {responseBody}");

            using var document = JsonDocument.Parse(responseBody);
            var root = document.RootElement;

            Assert.True(root.TryGetProperty("id", out var id));
            Assert.False(string.IsNullOrWhiteSpace(id.GetString()));

            Assert.True(root.TryGetProperty("status", out var status));
            Assert.Equal("Payment Required", status.GetString());

          
        }
        

        [Fact]
        public async Task SendSms_WithInvalidSender_ReturnsValidationError()
        {
            // Arrange: Read configuration
            string? baseUrl = _configuration["Bird:BaseUrl"];
            string? endpoint = _configuration["Bird:SmsEndpoint"];
            string? senderId = _configuration["Bird:SenderId"];
            string? recipient = _configuration["Bird:Recipient"];
            string? apiKey = _configuration["Bird:ApiKey"];

            // Validate required configuration
            Assert.False( string.IsNullOrWhiteSpace(baseUrl),"Bird:BaseUrl is missing.");
            Assert.False( string.IsNullOrWhiteSpace(endpoint),"Bird:SmsEndpoint is missing."); 
            Assert.False( string.IsNullOrWhiteSpace(senderId),"Bird:SenderId is missing.");

            Assert.False( string.IsNullOrWhiteSpace(recipient),"Bird:Recipient is missing.");
            Assert.False(string.IsNullOrWhiteSpace(apiKey), "Bird:ApiKey is missing.");

            // Build the API URL
            string apiUrl =
                $"{baseUrl!.TrimEnd('/')}/{endpoint!.TrimStart('/')}";

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            // Build the SMS request
            var payload = new
            {
                to = recipient,
                from = senderId,
                text = "This is a test message",
                category = "marketing"
            };

            string json = JsonSerializer.Serialize(payload, JsonOptions);

            using var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            // Act: Send the request to Bird
            using var response = await client.PostAsync(apiUrl, content);

            string responseBody =
                await response.Content.ReadAsStringAsync();

            // Assert: Verify HTTP status
            Assert.True(
                response.StatusCode == HttpStatusCode.UnprocessableEntity,
                $"Expected HTTP 422 but received {(int)response.StatusCode}. " +
                $"Response body: {responseBody}");

            // Assert: Parse the error response
            using var document = JsonDocument.Parse(responseBody);

            Assert.True(
                document.RootElement.TryGetProperty("error", out var error),
                $"Response does not contain an 'error' property. " +
                $"Response body: {responseBody}");

            // Assert: Verify the expected Bird error
            Assert.Equal(
                "E12056",
                error.GetProperty("code").GetString());

            Assert.Equal(
                "SMSSenderNotConfigured",
                error.GetProperty("name").GetString());

            Assert.Equal(
                "validation_error",
                error.GetProperty("type").GetString());

            Assert.Equal(
                "This sender is not one of your configured SMS senders.",
                error.GetProperty("message").GetString());
        }



    }

}

