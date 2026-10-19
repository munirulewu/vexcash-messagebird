/*
 * File name            : TemplateDataAccess.cs
 * Author               : Munirul Islam
 * Date                 : 18 October 2026
 * Version              : 1.0
 *
 * Description          : Implementation of  TemplateDataAccess
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
using System.Text;

namespace MessageBird.DA
{
    public sealed class TemplateDataAccess:IMessageTemplate
    {
        private readonly AppDbContext _db;
        public TemplateDataAccess(AppDbContext db)
        {
            _db = db;
        }


        public async Task<MessageTemplate?> getTempplatebyId(int templateId)
        {
            if (templateId <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(templateId), "Template ID must be positive.");

            MessageTemplate? template = new MessageTemplate();
            try
            {

                Log.Information(
                       "Retrieving message template {TemplateId}.", templateId);

                template = await _db.messageTemplate
                   .AsNoTracking()
                   .FirstOrDefaultAsync(x => x.Id == templateId);

                if (template is null)
                {
                    Log.Warning(
                        "Message template {TemplateId} was not found.", templateId);

                    throw new KeyNotFoundException(
                        $"Message template {templateId} was not found.");
                }
                //return  template;
            }
            catch (Exception ex)
            {
                Log.Error(
                       ex,
                       "Failed to retrieve templete using template {TemplateId}.",
                       templateId);


            }
            return template;
        }
        public async Task PrintAllTemplateListAsync()
        {
            List<TemplateDTO> templates = await GetTemplatesAsync();

            if (templates.Count == 0)
            {
                Console.WriteLine("No templates found.");
                return;
            }

            Console.WriteLine("\n========== CUSTOMER LIST ==========");

            Console.WriteLine(
                $"{"Template Id",-25} {"Template Type",-30}");

            Console.WriteLine(new string('-', 55));

            foreach (var template in templates)
            {
                Console.WriteLine(
                    $"{template.Id,-25} " +
                    $"{template.MessageType,-30}");
            }

            Console.WriteLine(new string('-', 55));
            Console.WriteLine($"Total template: {templates.Count}");
        }
        public async Task<List<TemplateDTO>> GetTemplatesAsync()
        {
            try
            {
                Log.Information("Starting Template lookup.");

                var templates = await _db.messageTemplate
                    .AsNoTracking()
                    .Select(x => new TemplateDTO
                    {
                        Id = x.Id,
                        MessageType = x.MsgType
                    })
                    .ToListAsync();

                if (templates.Count == 0)
                {
                    Log.Warning("No Template were found.");
                    return new List<TemplateDTO>();
                }

                Log.Information(
                    "Template lookup completed successfully. Found {TemplateCount} customers.",
                    templates.Count);

                return templates;
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

    }
}
