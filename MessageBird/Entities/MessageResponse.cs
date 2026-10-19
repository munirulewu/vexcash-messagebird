/*
 * File name            : MessageResponse.cs
 * Author               : Munirul Islam
 * Date                 : 18 October 2026
 * Version              : 1.0
 *
 * Description          : Entity class  for MessageResponse
 *
 * Modification history :
 * Name                         Date                            Desc
 * 
 * 
 * Copyright (c) 2026: THIS IS WRITTEN BY MUNIRUL ISLAM  FOR VEXCASH TEST PROJECT
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBird.Entities
{
    public sealed class MessageResponse
    {
        public string Id { get; set; } = "";

        public string? Href { get; set; }
        public string? Direction { get; set; }
        public string? Type { get; set; }
        public string? Originator { get; set; }
        public string? Body { get; set; }
        public string? Reference { get; set; }
        public int? Validity { get; set; }
        public string? Gateway { get; set; }

        public string? TypeDetailsJson { get; set; }

        public string? Datacoding { get; set; }
        public int? Mclass { get; set; }
        public string? ScheduledDatetime { get; set; }
        public string? CreatedDatetime { get; set; }

        public int TotalCount { get; set; }
        public int TotalSentCount { get; set; }
        public int TotalDeliveredCount { get; set; }
        public int TotalDeliveryFailedCount { get; set; }

        public List<MessageRecipient> Recipients { get; set; } = new();
    }
}
