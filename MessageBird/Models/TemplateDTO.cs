/*
 * File name            : TemplateDTO.cs
 * Author               : Munirul Islam
 * Date                 : 18 October 2026
 * Version              : 1.0
 *
 * Description          : Implementation for TemplateDTO 
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

namespace MessageBird.Models
{

    public sealed class TemplateDTO
    {
        public int? Id { get; set; }
        public string? MessageType { get; set; }
        public string? TemplateMessage { get; set; }
    }
}
