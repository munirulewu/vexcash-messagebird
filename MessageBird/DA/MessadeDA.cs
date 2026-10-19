/*
 * File name            : MessadeDA.cs
 * Author               : Munirul Islam
 * Date                 : 18 October 2026
 * Version              : 1.0
 *
 * Description          : Implementation of  MessadeDA
 *
 * Modification history :
 * Name                         Date                            Desc
 * 
 * 
 * Copyright (c) 2026: THIS IS WRITTEN BY MUNIRUL ISLAM  FOR VEXCASH TEST PROJECT
 */

using MessageBird.DatabaseContext;
using MessageBird.Entities;
using MessageBird.Interfaces;
using MessageBird.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
namespace MessageBird.Services
{
    public sealed class MessadeDA: IAPIResponse
    {
        private readonly AppDbContext _db;
        public MessadeDA() { }
        public MessadeDA(AppDbContext db)
        {
            _db = db;
        }

        // SELECT: Find a customer by credit number.
        /// <summary>
        /// Get a customer by credit number
        /// </summary>
        /// <param name="creditNumber"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<customerInfo?> GetCustomerByCreditNumberAsync(
            string creditNumber)
        {
            if (string.IsNullOrWhiteSpace(creditNumber))
            {
                Log.Warning(
                    "Customer lookup failed because credit number was empty.");

                throw new ArgumentException(
                    "Credit number cannot be empty.",
                    nameof(creditNumber));
            }

            try
            {
                Log.Information("Starting customer lookup.");

                var customer = await _db.Customer_info
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        x => x.CreditNumber == creditNumber);

                if (customer is null)
                {
                    Log.Warning("Customer lookup returned no result.");
                    return null;
                }

                Log.Information("Customer lookup completed successfully.");

                return customer;
            }
            catch (OperationCanceledException)
            {
                Log.Warning("Customer lookup was canceled.");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(
                    ex,
                    "An error occurred while retrieving customer information.");

                throw;
            }
        }


        public async Task<string> GenerateMessageAsync(
            MessageTemplate templateMessage,
            customerInfo customer)
        {
            ArgumentNullException.ThrowIfNull(customer);

            try
            {
                Log.Information(
                    " Message template {TemplateId}.", templateMessage.Id);

               

                if (templateMessage is null)
                {
                    Log.Warning(
                        "Message template {TemplateId} was not found.", templateMessage);

                    throw new KeyNotFoundException(
                        $"Message template {templateMessage} was not found.");
                }

                
                string ?message = templateMessage.TmsgText;

                if (string.IsNullOrWhiteSpace(message))
                {
                    Log.Warning(
                        "Message template {TemplateId} has empty content.", templateMessage);

                    throw new InvalidOperationException(
                        $"Message template {templateMessage} has no content.");
                }

                // Replace placeholders using values from Customer_info.
                message = message
                    .Replace("{Name}", customer.CustName ?? string.Empty,
                        StringComparison.OrdinalIgnoreCase)
                    .Replace("{Amount}",
                        customer.Amount.ToString(),
                        StringComparison.OrdinalIgnoreCase)
                    .Replace("{CreditNumber}", customer.CreditNumber ?? string.Empty,
                        StringComparison.OrdinalIgnoreCase)
                    .Replace("{DueDate}",
                        customer.DueDate.ToString(),
                        StringComparison.OrdinalIgnoreCase);

                Log.Information(
                    "Message generated successfully using template {TemplateId}.",
                    templateMessage.Id);

                return message;
            }
            catch (Exception ex)
            {
                Log.Error(
                    ex,
                    "Failed to generate message using template {TemplateId}.",
                    templateMessage);

                throw;
            }
        }


        /// <summary>
        /// purpose of the function is to receive template id and customer information. based on the template id
        /// template information will be picked from database and customize by the customer information
        /// </summary>
        /// <param name="templateId">Template Id</param>
        /// <param name="customer">Customer Information</param>
        /// <returns>string message</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>

        public async Task<string> GenerateMessageAsync(
            int templateId,
            customerInfo customer)
        {
            ArgumentNullException.ThrowIfNull(customer);

            if (templateId <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(templateId), "Template ID must be positive.");

            try
            {
                Log.Information(
                    "Retrieving message template {TemplateId}.", templateId);

                var template = await _db.messageTemplate
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == templateId);

                if (template is null)
                {
                    Log.Warning(
                        "Message template {TemplateId} was not found.", templateId);

                    throw new KeyNotFoundException(
                        $"Message template {templateId} was not found.");
                }

                // Change TemplateText to the actual text property in your entity.
                string ?message = template.TmsgText;

                if (string.IsNullOrWhiteSpace(message))
                {
                    Log.Warning(
                        "Message template {TemplateId} has empty content.", templateId);

                    throw new InvalidOperationException(
                        $"Message template {templateId} has no content.");
                }

                // Replace placeholders using values from Customer_info.
                message = message
                    .Replace("{Name}", customer.CustName ?? string.Empty,
                        StringComparison.OrdinalIgnoreCase)
                    .Replace("{Amount}",
                        customer.Amount.ToString(),
                        StringComparison.OrdinalIgnoreCase)
                    .Replace("{CreditNumber}", customer.CreditNumber ?? string.Empty,
                        StringComparison.OrdinalIgnoreCase)
                    .Replace("{DueDate}",
                        customer.DueDate.ToString(),
                        StringComparison.OrdinalIgnoreCase);

                Log.Information(
                    "Message generated successfully using template {TemplateId}.",
                    templateId);

                return message;
            }
            catch (Exception ex)
            {
                Log.Error(
                    ex,
                    "Failed to generate message using template {TemplateId}.",
                    templateId);

                throw;
            }
        }
        public async Task<int> SaveAsync(
            int? customerId,
            string? responseMsg,
            CancellationToken cancellationToken = default)
        {
            // Validate customer ID
            if (customerId is null)
            {
                throw new ArgumentNullException(
                    nameof(customerId),
                    "Customer ID cannot be null.");
            }

            if (customerId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(customerId),
                    "Customer ID must be greater than zero.");
            }

            // Validate response message
            if (string.IsNullOrWhiteSpace(responseMsg))
            {
                throw new ArgumentException(
                    "Response message cannot be null, empty, or whitespace.",
                    nameof(responseMsg));
            }

            // Validate maximum message length
            if (responseMsg.Length > 2000)
            {
                throw new ArgumentException(
                    "Response message cannot exceed 2000 characters.",
                    nameof(responseMsg));
            }

            // Create entity
            var apiResponse = new ApiResponse
            {
                CustomerId = customerId,
                ResponseMsg = responseMsg.Trim()
            };

            // Save to database
            _db.ApiResponse.Add(apiResponse);

            await _db.SaveChangesAsync(cancellationToken);

            // Return the generated primary key
            return apiResponse.Id;
        }

        public async Task SaveSucessResponseAsync(MessageResponseDTO dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (string.IsNullOrWhiteSpace(dto.Id))
            {
                throw new ArgumentException(
                    "The Bird message ID is required.",
                    nameof(dto));
            }

            try
            {
                var message = await _db.SmsMessage
                    .Include(x => x.Recipients)
                    .SingleOrDefaultAsync(
                        x => x.Id == dto.Id,
                        cancellationToken);

                var isNew = message is null;

                if (isNew)
                {
                    message = new MessageResponse
                    {
                        Id = dto.Id
                    };

                    _db.SmsMessage.Add(message);
                }

                // Map message properties
                message!.Href = dto.Href;
                message.Direction = dto.Direction;
                message.Type = dto.Type;
                message.Originator = dto.Originator;
                message.Body = dto.Body;
                message.Reference = dto.Reference;
                message.Validity = dto.Validity;
                message.Gateway = dto.Gateway;

                message.TypeDetailsJson =
                    dto.TypeDetails.HasValue
                        ? dto.TypeDetails.Value.GetRawText()
                        : null;

                message.Datacoding = dto.Datacoding;
                message.Mclass = dto.Mclass;
                message.ScheduledDatetime = dto.ScheduledDatetime;
                message.CreatedDatetime = dto.CreatedDatetime;

                // Map recipient summary counts
                if (dto.Recipients is not null)
                {
                    message.TotalCount = dto.Recipients.TotalCount;
                    message.TotalSentCount =
                        dto.Recipients.TotalSentCount;
                    message.TotalDeliveredCount =
                        dto.Recipients.TotalDeliveredCount;
                    message.TotalDeliveryFailedCount =
                        dto.Recipients.TotalDeliveryFailedCount;

                    foreach (var item in dto.Recipients.Items)
                    {
                        if (string.IsNullOrWhiteSpace(item.Recipient))
                        {
                            Log.Information(
                                "Skipping an empty recipient for Bird message {MessageId}.",
                                dto.Id);

                            continue;
                        }

                        var recipient = message.Recipients
                            .SingleOrDefault(x =>
                                x.Recipient == item.Recipient);

                        if (recipient is null)
                        {
                            recipient = new MessageRecipient
                            {
                                MessageId = dto.Id,
                                Recipient = item.Recipient
                            };

                            message.Recipients.Add(recipient);
                        }

                        recipient.Status = item.Status;
                        recipient.StatusDatetime = item.StatusDatetime;
                    }
                }

                await _db.SaveChangesAsync(cancellationToken);

                Log.Information(
                    "Bird message {MessageId} {Action} successfully. Recipient count: {RecipientCount}.",
                    dto.Id,
                    isNew ? "inserted" : "updated",
                    message.Recipients.Count);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(
                     ex,
                     "Database error while saving Bird message {MessageId}.",
                     dto.Id);

                throw;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                Log.Error(
                    ex,
                    "Unexpected error while saving Bird message {MessageId}.",
                    dto.Id);

                throw;
            }
        }

        
    }
}
