using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bellcast.Models
{
    public class WeatherInfo
    {
        public City2 city { get; set; }
        public string cod { get; set; }
        public double message { get; set; }
        public int cnt { get; set; }
        public List<List> list { get; set; }
    }
}