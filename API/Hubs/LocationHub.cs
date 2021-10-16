using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Hubs
{
    public class LocationHub : Hub
    {
        public async Task CheckDangers(double Latitude, double Longitude)
        {

        }
    }
}
