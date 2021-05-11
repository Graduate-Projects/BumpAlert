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
                    return "http://192.168.0.199:52600";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
