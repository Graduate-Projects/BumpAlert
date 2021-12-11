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
                        if (Location.CalculateDistance(UserPosition, DangerLocation) <= 300 )
                            await Clients.Caller.SendAsync("DetectDanger", DangerClosets.ID, DangerClosets.DangerType);
                        //else
                        //    await Clients.Caller.SendAsync("DetectDanger", Guid.NewGuid(), DangerType.SAFE);
                        break;
                    case BLL.Enums.DangerType.BUMP:
                    case BLL.Enums.DangerType.PIT:
                        if (Location.CalculateDistance(UserPosition, DangerLocation) <= 300 )
                            await Clients.Caller.SendAsync("DetectDanger", DangerClosets.ID, DangerClosets.DangerType);
                        //else
                        //    await Clients.Caller.SendAsync("DetectDanger", Guid.NewGuid(), DangerType.SAFE);
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

            /// <summary>
            /// methods to find the distance between 2 coordinates in meter unit
            /// </summary>
            /// <param name="point1">first point location</param>
            /// <param name="point2">second point location</param>
            /// <returns>distance between this points in meter</returns>
            public static double CalculateDistance(Location point1, Location point2)
            {
                var d1 = point1.Latitude * (Math.PI / 180.0);
                var num1 = point1.Longitude * (Math.PI / 180.0);
                var d2 = point2.Latitude * (Math.PI / 180.0);
                var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
                var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                         Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
                return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
            }
        }
    }
}
