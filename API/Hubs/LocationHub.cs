using API.Data;
using BLL.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
            try
            {
                //await Clients.Caller.SendAsync("DetectDanger", Guid.NewGuid(), DangerType.SAFE);

                var UserPosition = new Location(Latitude, Longitude);
                var ListOfDangers = _context.Dangers.ToList();
                var DangerClosets = ListOfDangers.OrderByDescending(danger => Location.CalculateDistance(UserPosition, new Location(danger.Latitude, danger.Longitude))).FirstOrDefault();
                if (DangerClosets == null) return;

                var DangerLocation = new Location(DangerClosets.Latitude, DangerClosets.Longitude);
                switch (DangerClosets.DangerType)
                {
                    case BLL.Enums.DangerType.RADAR:
                        if (Location.CalculateDistance(UserPosition, DangerLocation) <= 0.300) //0.5 Km = 500m
                            await Clients.Caller.SendAsync("DetectDanger", DangerClosets.ID, DangerClosets.DangerType);
                        else
                            await Clients.Caller.SendAsync("DetectDanger", Guid.NewGuid(), DangerType.SAFE);
                        break;
                    case BLL.Enums.DangerType.BUMP:
                    case BLL.Enums.DangerType.PIT:
                        if (Location.CalculateDistance(UserPosition, DangerLocation) <= 0.050) //0.05 Km = 50 m
                            await Clients.Caller.SendAsync("DetectDanger", DangerClosets.ID, DangerClosets.DangerType);
                        else
                            await Clients.Caller.SendAsync("DetectDanger", Guid.NewGuid(), DangerType.SAFE);
                        break;
                }
            }
            catch (Exception ex)
            {
                var StackTrace = new StackTrace(ex, true).GetFrame(0);
                _context.Loggers.Add(new BLL.Models.Diagnostic.Logger
                {
                    ID = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Type = ex.GetType().Name,
                    Class = StackTrace?.GetFileName(),
                    Method = StackTrace?.GetMethod()?.Name,
                    Line = StackTrace?.GetFileLineNumber() ?? -1,
                    Message = ex.Message,
                    Inner = ex.InnerException?.Message,
                    Json = ex.ToString()
                });
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
