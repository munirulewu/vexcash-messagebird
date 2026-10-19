/*
 * File name            : IAPIResponse.cs
 * Author               : Munirul Islam
 * Date                 : 16 October 2026
 * Version              : 1.0
 *
 * Description          : interface for IAPIResponse
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
    public interface IAPIResponse
    {
        Task<int> SaveAsync(
            int? customerId,
            string? responseMsg,
            CancellationToken cancellationToken = default);
        Task SaveSucessResponseAsync(MessageResponseDTO dto, CancellationToken cancellationToken = default);
    }
}
