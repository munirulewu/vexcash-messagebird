/*
 * File name            : ApiResponse.cs
 * Author               : Munirul Islam
 * Date                 : 18 October 2026
 * Version              : 1.0
 *
 * Description          : Implementation for ApiResponse for transfering data from one layer to another
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
    public class ApiResponse
    {
        public int Id;
        public string? ResponseMsg { get; set; } = string.Empty;
        public int? CustomerId { get; set; } 
        public DateTime? EntryDate { get; set; }
    }
}
