using System.Collections.Generic;
using System.Data;

namespace DashboardMainDemo {
    public static class ChampionsLeagueStatisticsDataHelper {
        public static IList<ChampionsLeagueStatisticsDataRow> Generate(DataSet dataSet) {
            IList<ChampionsLeagueStatisticsDataRow> res = new List<ChampionsLeagueStatisticsDataRow>();
            DataRowCollection data = dataSet.Tables["Data"].Rows;
            foreach(DataRow row in data) {
                res.Add(new ChampionsLeagueStatisticsDataRow {
                    Country = (string)row["Country"],
                    Club = (string)row["Club"],
                    Season = (string)row["Season"],
                    Won = (int)row["Won"],
                    Drawn = (int)row["Drawn"],
                    Lost = (int)row["Lost"],
                    Goals_for = (int)row["Goals_for"],
                    Goals_against = (int)row["Goals_against"]
                });
            }
            return res;
        }
    }

    public class ChampionsLeagueStatisticsDataRow {
        public string Country { get; set; }
        public string Club { get; set; }
        public string Season { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int Goals_for { get; set; }
        public int Goals_against { get; set; }
    }
}
