﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Settings
{
    public static class Connections
    {
        public static readonly BLL.Enums.DevServer Server = BLL.Enums.DevServer.Publish;
        public static string GetServerAddress()
        {
            switch (Server)
            {
                case Enums.DevServer.Local:
                    return "http://192.168.135.128:5000";
                case Enums.DevServer.Jood:
                    return "http://192.168.1.15:5000";
                case Enums.DevServer.Raghad:
                    return "http://192.168.8.200:5000";
                case Enums.DevServer.Saja:
                    return "http://192.168.1.250:5000";
                case Enums.DevServer.Heba:
                    return "http://192.168.1.250:5000";
                case Enums.DevServer.Publish:
                    return "https://bumpapp.azurewebsites.net";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
