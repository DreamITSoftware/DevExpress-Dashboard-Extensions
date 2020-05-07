using System;
using System.Collections.Generic;
using System.Data;

namespace DashboardMainDemo {
    public static class EnergyStaticticsDataHelper {
        public static IList<EnergyStaticticsDataRow> Generate(DataSet dataSet) {
            IList<EnergyStaticticsDataRow> res = new List<EnergyStaticticsDataRow>();
            DataRowCollection data = dataSet.Tables["Countries"].Rows;
            foreach(DataRow row in data) {
                res.Add(new EnergyStaticticsDataRow {
                    Country = (string)row["Country"],
                    EnergyType = (string)row["EnergyType"],
                    Year = (DateTime)row["Year"],
                    Import = row["Import"] != DBNull.Value ? (double)row["Import"] : 0,
                    Latitude = (double)row["Latitude"],
                    Longitude = (double)row["Longitude"],
                    Production = row["Production"] != DBNull.Value ? (double)row["Production"] : 0
                });
            }
            return res;
        }
    }

    public class EnergyStaticticsDataRow {
        public string Country { get; set; }
        public string EnergyType { get; set; }
        public DateTime Year { get; set; }
        public double Import { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Production { get; set; }
    }
}
