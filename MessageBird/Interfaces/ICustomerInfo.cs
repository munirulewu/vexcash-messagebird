/*
 * File name            : ICustomerInfo.cs
 * Author               : Munirul Islam
 * Date                 : 18 October 2026
 * Version              : 1.0
 *
 * Description          : interface for ICustomerInfo
 *
 * Modification history :
 * Name                         Date                            Desc
 * 
 * 
 * Copyright (c) 2026: THIS IS WRITTEN BY MUNIRUL ISLAM  FOR VEXCASH TEST PROJECT
 */

using MessageBird.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBird.Interfaces
{
    public interface ICustomerInfo
    {
        Task PrintCustomerListAsync();
        Task<List<CustomerDTO>> GetCustomersAsync();
    }
}
