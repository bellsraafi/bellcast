using bellcast.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace bellcast.Controllers
{
    public class HomeController : Controller
    {
        private WeatherInfo weatherInfo;
        private DefaultConnection db = new DefaultConnection();

        public List<WeatherForecast> GetTodayWeather()
        {
            List<WeatherForecast> model = new List<WeatherForecast>();

            weatherInfo = GetWeatherInfo();
            if (weatherInfo != null)
            {
                model.Add(new WeatherForecast()
                {
                    Temp = (weatherInfo.list[0].temp.max + weatherInfo.list[0].temp.max) / 2.00,
                    Humid = weatherInfo.list[0].humidity,
                    WeatherIcon = weatherInfo.list[0].weather[0].icon.ToString(),
                    WeatherMain = weatherInfo.list[0].weather[0].main.ToString(),
                    City = weatherInfo.city.name.ToString(),
                    Date = ConvertDate(weatherInfo.list[0].dt)
                });
            }else
            {
                var dbTemp = db.Temperatures.OrderByDescending(t => t.dateTime).FirstOrDefault();

                model.Add(new WeatherForecast()
                {
                    Temp = (dbTemp.max + dbTemp.max) / 2.00,
                    Humid = db.Humidities.OrderByDescending(h => h.dateTime).FirstOrDefault().value,
                    WeatherIcon = db.Weathers.OrderByDescending(w => w.dateTime).FirstOrDefault().icon,
                    WeatherMain = db.Weathers.OrderByDescending(w => w.dateTime).FirstOrDefault().main,
                    City = db.Cities.OrderByDescending(c => c.cityId).FirstOrDefault().name,
                    Date = ConvertDate(db.Weathers.OrderByDescending(
                           w => w.dateTime).FirstOrDefault().dateTime)
                });
            }
            return model;
            //return PartialView(model);
        }
        public ActionResult Index()
        {
            GetWeatherInfo();

            //Load Temperature graph data
            //var t = db.Temperatures.SqlQuery("select * from Temperature").ToList();
            var temp = from t in db.Temperatures select t;
            temp.ToList();

            ArrayList tdata = new ArrayList();
            tdata.Add(new ArrayList { "Hours", "Temperature" });
            foreach (Temperature t in temp)
            {
                tdata.Add(new ArrayList { ConvertDate(t.dateTime).ToString(), (t.max + t.max)/2 });
            }

            string tdataStr = JsonConvert.SerializeObject(tdata, Formatting.None);
            ViewBag.tData = new HtmlString(tdataStr);

            //Load Humidity graph data
            var humid = from h in db.Humidities select h;
            humid.ToList();

            ArrayList hdata = new ArrayList();
            hdata.Add(new ArrayList { "Hours", "Humidity" });
            foreach (Humidity h in humid)
            {
                hdata.Add(new ArrayList { ConvertDate(h.dateTime).ToString(), h.value });
            }

            string hdataStr = JsonConvert.SerializeObject(hdata, Formatting.None);
            ViewBag.hData = new HtmlString(hdataStr);

            ViewBag.Weather = GetTodayWeather();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            ViewBag.Weather = GetTodayWeather();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ViewBag.Weather = GetTodayWeather();

            return View();
        }
        public ActionResult Disease()
        {
            ViewBag.Message = "Your contact page.";

            ArrayList hheader = new ArrayList { "Hour", "Disease" ,"Forecast"};
            ArrayList hdata1 = new ArrayList { "1:00", "0.5","Moderate" };
            ArrayList hdata2 = new ArrayList { "2:00", "0.1" ,"Very Low"};
            ArrayList hdata3 = new ArrayList { "3:00", "0.4","Low" };
            ArrayList hdata4 = new ArrayList { "4:00", "0.3","Low" };
            ArrayList hdata5 = new ArrayList { "5:00", "0","Moderate" };
            ArrayList hdata6 = new ArrayList { "6:00", "0.3" ,"Low"};
            ArrayList hdata7 = new ArrayList { "7:00", "0.5","Moderate" };
            ArrayList hdata8 = new ArrayList { "8:00", "0.1","Very Low" };
            ArrayList hdata9 = new ArrayList { "9:00", "0","Very Low" };
            ArrayList hdata10 = new ArrayList { "10:00", "0.6","Moderate" };

            ArrayList hdata = new ArrayList { hheader, hdata1, hdata2, hdata3, hdata4, hdata5, hdata6, hdata7, hdata8, hdata9, hdata10 };
            string hdataStr = JsonConvert.SerializeObject(hdata, Formatting.None);

            ViewBag.hData = new HtmlString(hdataStr);
            ViewBag.Weather = GetTodayWeather();

            return View();
        }
        public ActionResult Control()
        {
            ViewBag.Message = "Your contact page.";
            ViewBag.Weather = GetTodayWeather();

            return View();
        }
        public WeatherInfo GetWeatherInfo()
        {
            City2 city = new City2();
            city.name = "zaria";
            string appId = "53a1ac331bbef7a7bb2799ab4f25f091";
            string url = string.Format("http://api.openweathermap.org/data/2.5/forecast/daily?q={0}&units=metric&cnt=1&APPID={1}", city.name, appId);
            using (System.Net.WebClient client = new System.Net.WebClient())
            {

                try
                {
                    string json = client.DownloadString(url);

                    weatherInfo = (new JavaScriptSerializer()).Deserialize<WeatherInfo>(json);

                    UpdateTemp(weatherInfo);
                    UpdateHumid(weatherInfo);
                    UpdateWeather(weatherInfo);
                    UpdateLWD(weatherInfo);
                }
                catch (Exception)
                {}
                return weatherInfo;
            }
        }

        public static void UpdateTemp(WeatherInfo wInfo)
        {
            Temperature temp = new Temperature();
            temp.min = wInfo.list[0].temp.min;
            temp.max = wInfo.list[0].temp.max;
            temp.day = wInfo.list[0].temp.day;
            temp.night = wInfo.list[0].temp.night;
            temp.morn = wInfo.list[0].temp.morn;
            temp.eve = wInfo.list[0].temp.eve;
            temp.dateTime = wInfo.list[0].dt;
            temp.cityId = wInfo.city.id;

            using (DefaultConnection cn = new DefaultConnection())
            {
                /*int dt = wInfo.list[0].dt;
                bool dateExist = cn.Temperatures.Any(d => d.dateTime.Equals(dt));
                if (dateExist)
                {}
                else
                {*/
                    cn.Temperatures.Add(temp);
                    cn.SaveChanges();
               // }
            }
        }
        public static void UpdateHumid(WeatherInfo wInfo)
        {
            Humidity humid = new Humidity();
            humid.value = wInfo.list[0].humidity;
            humid.dateTime = wInfo.list[0].dt;
            humid.cityId = wInfo.city.id;

            using(DefaultConnection cn = new DefaultConnection())
            {
                int dt = wInfo.list[0].dt;
                bool dateExist = cn.Humidities.Any(d => d.dateTime.Equals(dt));
                if (dateExist)
                {}
                else
                {
                    cn.Humidities.Add(humid);
                    cn.SaveChanges();
                }
            }
        }
        public static void UpdateWeather(WeatherInfo wInfo)
        {
            Weather weather = new Weather();
            weather.main = wInfo.list[0].weather[0].main;
            weather.description = wInfo.list[0].weather[0].description;
            weather.icon = wInfo.list[0].weather[0].icon;
            weather.dateTime = wInfo.list[0].dt;
            weather.cityId = wInfo.city.id;

            using(DefaultConnection cn = new DefaultConnection())
            {
                int dt = wInfo.list[0].dt;
                bool dateExist = cn.Weathers.Any(d => d.dateTime.Equals(dt));
                if (dateExist)
                {}
                else
                {
                    cn.Weathers.Add(weather);
                    cn.SaveChanges();
                }
            }
        }
        public static void UpdateLWD(WeatherInfo wInfo)
        {
            LeafWetnessDuration lwd = new LeafWetnessDuration();
            if (wInfo.list[0].humidity > 90)
            {
                lwd.value = true;
                lwd.dateTime = wInfo.list[0].dt;
                lwd.cityId = wInfo.city.id;

                using (DefaultConnection cn = new DefaultConnection())
                {
                    int dt = wInfo.list[0].dt;
                    bool dateExist = cn.LeafWetnessDurations.Any(d => d.dateTime.Equals(dt));
                    if (dateExist)
                    {}
                    else
                    {
                        cn.LeafWetnessDurations.Add(lwd);
                        cn.SaveChanges();
                    }
                }
            }
            else
            {
                lwd.value = false;
                lwd.dateTime = wInfo.list[0].dt;
                lwd.cityId = wInfo.city.id;

                using (DefaultConnection cn = new DefaultConnection())
                {
                    int dt = wInfo.list[0].dt;
                    bool dateExist = cn.LeafWetnessDurations.Any(d => d.dateTime.Equals(dt));
                    if (dateExist)
                    {}
                    else
                    {
                        cn.LeafWetnessDurations.Add(lwd);
                        cn.SaveChanges();
                    }
                }
            }
        }

        public string ConvertDate(double timestamp)
        {
            // This is an example of a UNIX timestamp for the date/time 11-04-2005 09:25.
            //= 1113211532;

            // First make a System.DateTime equivalent to the UNIX Epoch.
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

            // Add the number of seconds in UNIX timestamp to be converted.
            dateTime = dateTime.AddSeconds(timestamp);

            // The dateTime now contains the right date/time so to format the string,
            // use the standard formatting methods of the DateTime object.
            string printDate = dateTime.ToShortDateString();
            //dateTime.ToShortTimeString();

            return printDate;
        }
    }
}