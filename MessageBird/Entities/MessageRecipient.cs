/*
 * File name            : MessageRecipient.cs
 * Author               : Munirul Islam
 * Date                 : 18 October 2026
 * Version              : 1.0
 *
 * Description          : Entity class  for MessageRecipient
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
    public sealed class MessageRecipient
    {
        public int Id { get; set; }

        public string MessageId { get; set; } = "";

        public string Recipient { get; set; } = "";

        public string? Status { get; set; }

        public string? StatusDatetime { get; set; }

        public MessageResponse Message { get; set; } = null!;
    }
}
