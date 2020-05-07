using System.Collections.Generic;
using System.Data;

namespace DashboardMainDemo {
    public static class RevenueByIndustryDataHelper {
        public static IList<RevenueByIndustryDataRow> Generate(DataSet dataSet) {
            IList<RevenueByIndustryDataRow> res = new List<RevenueByIndustryDataRow>();
            DataRowCollection data = dataSet.Tables["Statistics"].Rows;
            foreach(DataRow row in data) {
                res.Add(new RevenueByIndustryDataRow {
                    City = (string)row["City"],
                    Industry = (string)row["Industry"],
                    State = (string)row["State"],
                    Latitude = (double)row["Latitude"],
                    Longitude = (double)row["Longitude"],
                    Revenue = (decimal)row["Revenue"]
                });
            }
            return res;
        }
    }

    public class RevenueByIndustryDataRow {
        public string City { get; set; }
        public string Industry { get; set; }
        public string State { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal Revenue { get; set; }
    }
}
