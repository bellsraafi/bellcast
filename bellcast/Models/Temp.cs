using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bellcast.Models
{
    public class Temp
    {
        public int Id { get; set; }
        public double day { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public double night { get; set; }
        public double eve { get; set; }
        public double morn { get; set; }
        public int cityId { get; set; }
        public int dateTime { get; set; }
    }
}