using System.Data;
using System.Linq;
using System;
using System.Collections.Generic;

namespace DashboardMainDemo {
    public class WebsiteStatisticsDataGenerator {
        public class WebsiteStatisticsItem {
            public int Count { get; set; }
            
            // VB conversion
            private DateTime _date;
            public DateTime Date { get { return _date; } set { _date = value; } }
            //
            
            public string TrafficSource { get; set; }
            public string TrafficSourceDetails { get; set; }
            public string Browser { get; set; }
            public string BrowserDetails { get; set; }
        }
        interface IChanceInterval {
            double Chance { get; set; }
        }
        class DataPairElement : IChanceInterval {
            public string Data { get; set; }
            public string DataDetails { get; set; }
            public double Chance { get; set; }
            double IChanceInterval.Chance {
                get { return Chance; }
                set { Chance = value; }
            }
        }
        class UserDataElement : IChanceInterval {
            public string UserId { get; set; }
            public double Chance { get; set; }
            double IChanceInterval.Chance {
                get { return Chance; }
                set { Chance = value; }
            }
        }

        readonly Random rand = new Random();
        readonly List<WebsiteStatisticsItem> items = new List<WebsiteStatisticsItem>();        
        public IEnumerable<WebsiteStatisticsItem> WebsiteStatistics { get { return items; } }

        public WebsiteStatisticsDataGenerator() {
            InitializeData();
        }
        void InitializeData() {
            IList<DataPairElement> dataTrafficSourceList = GetTrafficSourceData();
            IList<DataPairElement> browsersDataList = GetBrowserData();
            IList<UserDataElement> usersDataList = GetUsersData(10000);
            DateTime currentDate = DateTime.Today.AddYears(-1);
            DateTime endDate = DateTime.Today.AddDays(-1);
            while (currentDate < endDate) {
                double monthModifier = 1 + 0.03 * Math.Abs(currentDate.Month - 6);
                int baseCount = rand.Next(100000, 150000);
                foreach (DataPairElement browserData in browsersDataList) {
                    foreach (DataPairElement trafficData in dataTrafficSourceList) {
                        WebsiteStatisticsItem item = new WebsiteStatisticsItem();
                        item.Count = Convert.ToInt32(baseCount * (browserData.Chance / 100) * (trafficData.Chance / 100) * monthModifier);
                        item.Date = currentDate;
                        item.TrafficSource = trafficData.Data;
                        item.TrafficSourceDetails = trafficData.DataDetails;
                        item.Browser = browserData.Data;
                        item.BrowserDetails = browserData.DataDetails;
                        items.Add(item);
                    }
                }
                currentDate = currentDate.AddMonths(1);
            }
        }
        IList<UserDataElement> GetUsersData(int count) {
            IList<UserDataElement> result = Enumerable
                .Range(0, count)
                .Select(i => new UserDataElement() { UserId = Guid.NewGuid().ToString(), Chance = rand.Next(1, 3) })
                .ToList();
            InitChance(result.Cast<IChanceInterval>().ToList());
            return result;
        }
        IList<DataPairElement> GetBrowserData() {
            IList<DataPairElement> result = new List<DataPairElement>();
            result.Add(new DataPairElement() { Chance = 2.37, Data = "IE", DataDetails = "11" });
            result.Add(new DataPairElement() { Chance = 0.23, Data = "IE", DataDetails = "Others" });
            result.Add(new DataPairElement() { Chance = 3.5, Data = "Edge", DataDetails = "Latest" });

            result.Add(new DataPairElement() { Chance = 4.41, Data = "Chrome", DataDetails = "Latest" });
            result.Add(new DataPairElement() { Chance = 59.19, Data = "Chrome", DataDetails = "Others" });

            result.Add(new DataPairElement() { Chance = 6.41, Data = "Firefox", DataDetails = "Latest" });

            result.Add(new DataPairElement() { Chance = 14.2, Data = "Safari", DataDetails = "Latest" });

            result.Add(new DataPairElement() { Chance = 3.23, Data = "UC", DataDetails = "Latest" });
            result.Add(new DataPairElement() { Chance = 3.65, Data = "Opera", DataDetails = "Latest" });
            result.Add(new DataPairElement() { Chance = 2.81, Data = "Unknown", DataDetails = "Unknown" });

            InitChance(result.Cast<IChanceInterval>().ToList());
            return result;
        }
        IList<DataPairElement> GetTrafficSourceData() {
            IList<DataPairElement> result = new List<DataPairElement>();
            result.Add(new DataPairElement() { Chance = 51.0, Data = "Direct", DataDetails = "Direct" });
            result.Add(new DataPairElement() { Chance = 24.0, Data = "Referring Site", DataDetails = "Facebook" });
            result.Add(new DataPairElement() { Chance = 02.0, Data = "Referring Site", DataDetails = "Google Ads" });
            result.Add(new DataPairElement() { Chance = 05.0, Data = "Referring Site", DataDetails = "Reddit" });
            result.Add(new DataPairElement() { Chance = 13.3, Data = "Referring Site", DataDetails = "Twitter" });
            result.Add(new DataPairElement() { Chance = 02.3, Data = "Referring Site", DataDetails = "LinkedIn" });
            result.Add(new DataPairElement() { Chance = 03.3, Data = "Search Engine", DataDetails = "Bing" });
            result.Add(new DataPairElement() { Chance = 10.3, Data = "Search Engine", DataDetails = "Google" });
            result.Add(new DataPairElement() { Chance = 02.3, Data = "Search Engine", DataDetails = "Yahoo" });
            InitChance(result.Cast<IChanceInterval>().ToList());
            return result;
        }
        void InitChance(IList<IChanceInterval> dataList) {
            double sum = dataList.Sum(d => d.Chance);
            foreach (IChanceInterval data in dataList)
                data.Chance = 100 * data.Chance / sum;
        }
    }
}
