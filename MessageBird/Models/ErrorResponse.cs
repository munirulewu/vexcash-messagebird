/*
 * File name            : BirdErrorResponse.cs
 * Author               : Munirul Islam
 * Date                 : 18 October 2026
 * Version              : 1.0
 *
 * Description          : Implementation for BirdErrorResponse  
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
using System.Text.Json.Serialization;

namespace MessageBird.Models
{
    public sealed class BirdErrorResponse
    {
        [JsonPropertyName("error")]
        public BirdError? Error { get; set; }
    }

    public sealed class BirdError
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("doc_url")]
        public string? DocUrl { get; set; }

        [JsonPropertyName("request_id")]
        public string? RequestId { get; set; }

        [JsonPropertyName("remediation")]
        public string? Remediation { get; set; }
    }
}
