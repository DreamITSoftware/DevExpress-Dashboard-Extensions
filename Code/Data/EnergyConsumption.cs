using System;
using System.Collections.Generic;
using System.Data;

namespace DashboardMainDemo {
    public static class EnergyConsumptionDataHelper {
        public static IList<EnergyConsumptionTotalDataRow> GenerateTotal(DataSet dataSet) {
            IList<EnergyConsumptionTotalDataRow> res = new List<EnergyConsumptionTotalDataRow>();
            DataRowCollection data = dataSet.Tables["CountriesTotal"].Rows;
            foreach(DataRow row in data) {
                res.Add(new EnergyConsumptionTotalDataRow {
                    Country = (string)row["Country"],
                    Year = (DateTime)row["Year"],
                    Consumption = (double)row["Consumption"],
                    Latitude = (double)row["Latitude"],
                    Longitude = (double)row["Longitude"],
                    Production = (double)row["Production"]
                });
            }
            return res;
        }
        public static IList<EnergyConsumptionBySectorDataRow> GenerateBySector(DataSet dataSet) {
            IList<EnergyConsumptionBySectorDataRow> res = new List<EnergyConsumptionBySectorDataRow>();
            DataRowCollection data = dataSet.Tables["CountriesBySector"].Rows;
            foreach(DataRow row in data) {
                res.Add(new EnergyConsumptionBySectorDataRow {
                    Country = (string)row["Country"],
                    Sector = (string)row["Sector"],
                    Year = (DateTime)row["Year"],
                    Consumption = row["Consumption"] != DBNull.Value ? (double)row["Consumption"] : 0,
                    Latitude = (double)row["Latitude"],
                    Longitude = (double)row["Longitude"]
                });
            }
            return res;
        }
    }

    public class EnergyConsumptionTotalDataRow {
        public string Country { get; set; }
        public DateTime Year { get; set; }
        public double Consumption { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Production { get; set; }
    }

    public class EnergyConsumptionBySectorDataRow {
        public string Country { get; set; }
        public string Sector { get; set; }
        public DateTime Year { get; set; }
        public double Consumption { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
