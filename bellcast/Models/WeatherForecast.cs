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
        protected void GetWeatherInfo(object sender, EventArgs e)
        {

            City city = new City();
            string appId = "53a1ac331bbef7a7bb2799ab4f25f091";
            string url = string.Format("http://api.openweathermap.org/data/2.5/forecast/daily?q={0}&units=metric&cnt=1&APPID={1}", city.name, appId);
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                string json = client.DownloadString(url);

                WeatherInfo weatherInfo = (new JavaScriptSerializer()).Deserialize<WeatherInfo>(json);
                LoadTemp(weatherInfo);
            }
        }

        static void LoadTemp(WeatherInfo wInfo)
        {
            /*double min_temp = wInfo.list[0].temp.min;
            double max_temp = wInfo.list[0].temp.max;
            double day_temp = wInfo.list[0].temp.day;
            double night_temp = wInfo.list[0].temp.night;
            double morn_temp = wInfo.list[0].temp.morn;
            double eve_temp = wInfo.list[0].temp.eve;
            int dateTime = wInfo.list[0].dt;
            int city_id = wInfo.city.id;*/

            try
            {
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

                DataTable tempTable = new DataTable();

                tempTable.Columns.Add(new DataColumn("min", typeof(double)));
                tempTable.Columns.Add(new DataColumn("max", typeof(double)));
                tempTable.Columns.Add(new DataColumn("day", typeof(double)));
                tempTable.Columns.Add(new DataColumn("night", typeof(double)));
                tempTable.Columns.Add(new DataColumn("morn", typeof(double)));
                tempTable.Columns.Add(new DataColumn("eve", typeof(double)));
                tempTable.Columns.Add(new DataColumn("cityId", typeof(int)));
                tempTable.Columns.Add(new DataColumn("dateTime", typeof(int)));

                DataRow tempDataRow = tempTable.NewRow();
                tempDataRow["min"] = wInfo.list[0].temp.min;
                tempDataRow["max"] = wInfo.list[0].temp.max;
                tempDataRow["day"] = wInfo.list[0].temp.day;
                tempDataRow["night"] = wInfo.list[0].temp.night;
                tempDataRow["morn"] = wInfo.list[0].temp.morn;
                tempDataRow["eve"] = wInfo.list[0].temp.eve;
                tempDataRow["cityId"] = wInfo.city.id;
                tempDataRow["dateTime"] = wInfo.list[0].dt;
                tempTable.Rows.Add(tempDataRow);

                using (SqlBulkCopy oSqlBulCopy = new SqlBulkCopy(cn))
                {
                    //oSqlBulCopy.BatchSize = 10000;
                    //oSqlBulCopy.BulkCopyTimeout = 10000;
                    oSqlBulCopy.ColumnMappings.Add("min", "min");
                    oSqlBulCopy.ColumnMappings.Add("max", "max");
                    oSqlBulCopy.ColumnMappings.Add("day", "day");
                    oSqlBulCopy.ColumnMappings.Add("night", "night");
                    oSqlBulCopy.ColumnMappings.Add("morn", "morn");
                    oSqlBulCopy.ColumnMappings.Add("eve", "eve");
                    oSqlBulCopy.ColumnMappings.Add("cityId", "cityId");
                    oSqlBulCopy.ColumnMappings.Add("dateTime", "dateTime");
                    oSqlBulCopy.DestinationTableName = "Temperature";
                    oSqlBulCopy.WriteToServer(tempTable);
                }
            }
            catch (Exception e)
            {
                string msg = e.ToString();
            }
        }
    }

    public class WeatherInfo
    {
        public City city { get; set; }
        public string cod { get; set; }
        public double message { get; set; }
        public int cnt { get; set; }
        public List<List> list { get; set; }
    }
    public class List
    {
        public int dt { get; set; }
        public Temp temp { get; set; }
        public double pressure { get; set; }
        public Humidity humidity { get; set; }
        public List<Weather> weather { get; set; }
        public double speed { get; set; }
        public int deg { get; set; }
        public int clouds { get; set; }
    }
}