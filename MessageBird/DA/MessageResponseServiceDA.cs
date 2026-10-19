/*
 * File name            : MessageResponseServiceDA.cs
 * Author               : Munirul Islam
 * Date                 : 18 October 2026
 * Version              : 1.0
 *
 * Description          : Implementation of  MessageResponseServiceDA
 *
 * Modification history :
 * Name                         Date                            Desc
 * 
 * 
 * Copyright (c) 2026: THIS IS WRITTEN BY MUNIRUL ISLAM  FOR VEXCASH TEST PROJECT
 */

using MessageBird.DatabaseContext;
using MessageBird.Entities;
using MessageBird.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBird.DA
{
    public class MessageResponseServiceDA
    {
        private readonly AppDbContext _db;
       
        public MessageResponseServiceDA(AppDbContext db)
        {
            _db = db;
        }

        public async Task SaveResponseAsync(  MessageResponseDTO dto,    CancellationToken cancellationToken = default)
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
                    .Include(x =>x.Recipients)
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
