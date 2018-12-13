using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace bellcast.Models
{
    public class WeatherForecast
    {
        public double Temp { get; set; }
        public double Humid { get; set; }
        public string WeatherIcon { get; set; }
        public string WeatherMain { get; set; }
        public string City { get; set; }
        public string Date { get; set; }
    }
}