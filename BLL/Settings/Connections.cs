using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Settings
{
    public static class Connections
    {
        public static readonly BLL.Enums.DevServer Server = BLL.Enums.DevServer.Local;
        public static string GetServerAddress()
        {
            switch (Server)
            {
                case Enums.DevServer.Local:
                    return "http://192.168.0.100:52600";
                case Enums.DevServer.Jood:
                    return "http://192.168.1.15:52600";
                case Enums.DevServer.Raghad:
                    return "http://192.168.8.200:52600";
                case Enums.DevServer.Saja:
                    return "http://192.168.1.250:52600";
                case Enums.DevServer.Heba:
                    return "http://192.168.1.254:52600";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
