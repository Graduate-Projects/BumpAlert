using API.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Hubs
{
    public class LocationHub : Hub
    {
        private readonly APIContext _context;
        public LocationHub(APIContext context)
        {
            _context = context;
        }

        public async Task CheckDangers(double Latitude, double Longitude)
        {
            var UserPosition = new Location(Latitude, Longitude);
            var ListOfDangers = _context.Dangers.ToList();
            var DangerClosets = ListOfDangers.OrderByDescending(danger => Location.CalculateDistance(UserPosition, new Location(danger.Latitude, danger.Longitude))).Take(1);
            foreach (var Danger in DangerClosets)
            {
                var DangerLocation = new Location(Danger.Latitude, Danger.Longitude);
                switch (Danger.DangerType)
                {
                    case BLL.Enums.DangerType.RADAR:
                        if (Location.CalculateDistance(UserPosition, DangerLocation) <= 0.500) //0.5 Km = 500m
                            await Clients.Caller.SendAsync("DetectDanger", Danger.ID, Danger.DangerType);
                        break;
                    case BLL.Enums.DangerType.BUMP:
                    case BLL.Enums.DangerType.PIT:
                        if (Location.CalculateDistance(UserPosition, DangerLocation) <= 0.050) //0.05 Km = 50 m
                            await Clients.Caller.SendAsync("DetectDanger", Danger.ID, Danger.DangerType);
                        break;
                }
            }
        }
        public class Location
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public Location(double Latitude, double Longitude)
            {
                this.Latitude = Latitude;
                this.Longitude = Longitude;
            }
            public static double CalculateDistance(Location StartLocation, Location EndLocation)
            {
                var DeltaLongitude = EndLocation.Longitude - StartLocation.Longitude;
                var DeltaLatitude = EndLocation.Latitude - StartLocation.Latitude;

                var Distance = Math.Sqrt(Math.Pow(DeltaLongitude,2) + Math.Pow(DeltaLatitude, 2));
                return Distance;
            }
        }
    }
}
