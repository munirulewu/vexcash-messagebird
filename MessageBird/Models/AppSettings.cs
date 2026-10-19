/*
 * File name            : AppSettings.cs
 * Author               : Munirul Islam
 * Date                 : 18 October 2026
 * Version              : 1.0
 *
 * Description          : Implementation for AppSettings to read AppSettings config file
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

    public sealed class AppSettings
    {
        public List<string> Exclude { get; set; } = [];

        public ConnectionStringsSettings ConnectionStrings { get; set; } = new();

        public ApiSettings APIsettings { get; set; } = new();
    }

    public sealed class ConnectionStringsSettings
    {
        public string DefaultConnection { get; set; } = "";
    }

    public sealed class ApiSettings
    {
        public string BaseURL { get; set; } = "";

        public string API_KEY { get; set; } = "";

        public string SENDER_ID { get; set; } = "";

        public string SENDER_PHONE { get; set; } = "";
    }
}
