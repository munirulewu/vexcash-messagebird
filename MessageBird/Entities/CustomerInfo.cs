/*
 * File name            : customerInfo.cs
 * Author               : Munirul Islam
 * Date                 : 18 October 2026
 * Version              : 1.0
 *
 * Description          : Entity class  for customerInfo
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

    public sealed class customerInfo
    {
        public int Id { get; set; }

        public string? PhoneNumber { get; set; }

        public string? CustName { get; set; }

        public string? CreditNumber { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? DueDate { get; set; }

        public string? IsSend { get; set; }
    }
}
