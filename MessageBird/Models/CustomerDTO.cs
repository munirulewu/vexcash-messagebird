/*
 * File name            : CustomerDTO.cs
 * Author               : Munirul Islam
 * Date                 : 18 October 2026
 * Version              : 1.0
 *
 * Description          : Implementation for CustomerDTO 
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
    public sealed class CustomerDTO
    {
        public string? CustomerCreditNumber { get; set; }
        public string? CustomerName { get; set; }
    }
}
