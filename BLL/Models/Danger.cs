﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class Danger
    {
        public Guid ID { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int ForwordBackword { get; set; }
        public Enums.DangerType DangerType { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
    }

}
