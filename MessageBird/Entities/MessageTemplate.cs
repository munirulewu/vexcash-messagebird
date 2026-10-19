/*
 * File name            : MessageTemplate.cs
 * Author               : Munirul Islam
 * Date                 : 18 October 2026
 * Version              : 1.0
 *
 * Description          : Entity class  for MessageTemplate
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
    public sealed class MessageTemplate
    {
        public int Id { get; set; }

        public string? MsgType { get; set; }

        public string? TmsgText { get; set; }
    }
}
