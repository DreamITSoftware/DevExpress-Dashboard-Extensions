using System;
using System.Collections.Generic;
using System.Data;

namespace DashboardMainDemo {
    public class SalesPerformanceDataGenerator : SalesDataGenerator {
        public class TotalSalesItem {
            public string State { get; set; }
            public string Category { get; set; }
            public string Product { get; set; }
            public decimal RevenueYTD { get; set; }
            public decimal RevenueYTDTarget { get; set; }
            public decimal RevenueQTD { get; set; }
            public decimal RevenueQTDTarget { get; set; }
            public int UnitsSoldYTD { get; set; }
            public int UnitsSoldYTDTarget { get; set; }
        }

        public class MonthlySalesItem {
            public string State { get; set; }
            public string Product { get; set; }
            public string Category { get; set; }
            public DateTime CurrentDate { get; set; }
            public decimal Revenue { get; set; }
            public decimal RevenueTarget { get; set; }
            public int UnitsSold { get; set; }
            public int UnitsSoldTarget { get; set; }
        }

        public class KeyMetricsItem {
            public decimal RevenueYTD { get; set; }
            public decimal RevenueYTDTarget { get; set; }
            public decimal ExpensesYTD { get; set; }
            public decimal ExpensesYTDTarget { get; set; }
            public decimal ProfitYTD { get; set; }
            public decimal ProfitYTDTarget { get; set; }
            public decimal AvgOrderSizeYTD { get; set; }
            public decimal AvgOrderSizeYTDTarget { get; set; }
            public int NewCustomersYTD { get; set; }
            public int NewCustomersYTDTarget { get; set; }
            public float MarketShare { get; set; }
        }

        readonly List<MonthlySalesItem> _monthlySales = new List<MonthlySalesItem>();
        readonly List<TotalSalesItem> _totalSales = new List<TotalSalesItem>();
        readonly KeyMetricsItem item = new KeyMetricsItem();

        public IEnumerable<MonthlySalesItem> MonthlySales { get { return _monthlySales; } }
        public IEnumerable<TotalSalesItem> TotalSales { get { return _totalSales; } }
        public IEnumerable<KeyMetricsItem> KeyMetrics { get { return new KeyMetricsItem[] { item }; } }

        public SalesPerformanceDataGenerator(DataSet dataSet)
            : base(dataSet) {
        }
        protected override void Generate(Context context) {
            TotalSalesItem tsItem = new TotalSalesItem { State = context.State, Category = context.CategoryName, Product = context.ProductName };
            int y = DateTime.Today.Year - 1;
            for(int month = 1; month <= 12; month++) {
                DateTime dt = new DateTime(y, month, 1);
                context.UnitsSoldGenerator.Next();
                int uSold = context.UnitsSoldGenerator.UnitsSold;
                int uSoldTarget = context.UnitsSoldGenerator.UnitsSoldTarget;
                decimal rev = uSold * context.ListPrice;
                decimal revTarget = uSoldTarget * context.ListPrice;
                _monthlySales.Add(new MonthlySalesItem {
                    State = context.State,
                    Product = context.ProductName,
                    Category = context.CategoryName,
                    CurrentDate = dt,
                    UnitsSold = uSold,
                    UnitsSoldTarget = uSoldTarget,
                    Revenue = rev,
                    RevenueTarget = revTarget
                });
                tsItem.RevenueYTD += rev;
                tsItem.RevenueYTDTarget += revTarget;
                tsItem.UnitsSoldYTD += uSold;
                tsItem.UnitsSoldYTDTarget += uSoldTarget;
                if(month >= 10 && month <= 12) {
                    tsItem.RevenueQTD += rev;
                    tsItem.RevenueQTDTarget += revTarget;
                }
                item.RevenueYTD += rev;
                item.RevenueYTDTarget += revTarget;
            }
            _totalSales.Add(tsItem);
        }
        protected override void EndGenerate() {
            base.EndGenerate();
            item.ExpensesYTD = item.RevenueYTDTarget * 0.2m;
            item.ExpensesYTDTarget = item.RevenueYTDTarget * 0.1999m;
            item.ProfitYTD = item.RevenueYTD - item.ExpensesYTD;
            item.ProfitYTDTarget = item.RevenueYTDTarget - item.ExpensesYTDTarget;
            item.AvgOrderSizeYTD = item.RevenueYTD * 0.006m;
            item.AvgOrderSizeYTDTarget = item.RevenueYTDTarget * 0.0055m;
            item.NewCustomersYTD = (int)Math.Round(item.RevenueYTD * 0.0013m);
            item.NewCustomersYTDTarget = (int)Math.Round(item.RevenueYTDTarget * 0.00125m);
            item.MarketShare = 0.23f;
        }
    }
}
