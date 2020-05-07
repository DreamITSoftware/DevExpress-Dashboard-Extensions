using DevExpress.DashboardWeb;
using System;
using System.Data;
using System.IO;

namespace DashboardMainDemo {
    public static class DataLoader {
        static string GetRelativePath(string name) {
            string dataDirectoryPath = (string)AppDomain.CurrentDomain.GetData("DataDirectory");
            return Path.Combine(dataDirectoryPath, "Data", name);
        }
        static DataSet LoadData(string fileName) {
            string path = GetRelativePath(string.Format("{0}.xml", fileName));
            DataSet ds = new DataSet();
            ds.ReadXml(path, XmlReadMode.ReadSchema);
            return ds;
        }
        public static DataSet LoadSales() {
            return LoadData("DashboardSales");
        }
        public static DataSet LoadEmployees() {
            return LoadData("DashboardEmployeesAndDepartments");
        }
        public static DataSet LoadCustomerSupport() {
            return LoadData("DashboardCustomerSupport");
        }
        public static DataSet LoadRevenueByIndustry() {
            return LoadData("DashboardRevenueByIndustry");
        }
        public static DataSet LoadEuroEnergyStatistics() {
            return LoadData("DashboardEnergyStatictics");
        }
        public static DataSet LoadEnergyConsumptionTotal() {
            return LoadData("DashboardEnergyConsumptionTotal");
        }
        public static DataSet LoadEnergyConsumptionBySector() {
            return LoadData("DashboardEnergyConsumptionBySector");
        }
        public static string GetImagesFolder() {
            return GetRelativePath("ProductDetailsImages");
        }

        public static void LoadData(DataLoadingWebEventArgs e) {
            switch (e.DataSourceComponentName) {
                case "siteVisitosDataSource":
                    WebsiteStatisticsDataGenerator data = new WebsiteStatisticsDataGenerator();
                    e.Data = data.WebsiteStatistics;
                    break;
                case "dsMonthlySales":
                    SalesPerformanceDataGenerator dataGeneratorMS = new SalesPerformanceDataGenerator(LoadSales());
                    dataGeneratorMS.Generate();
                    e.Data = dataGeneratorMS.MonthlySales;
                    break;
                case "dsTotalSales":
                    SalesPerformanceDataGenerator dataGeneratorTS = new SalesPerformanceDataGenerator(LoadSales());
                    dataGeneratorTS.Generate();
                    e.Data = dataGeneratorTS.TotalSales;
                    break;
                case "dsKeyMetrics":
                    SalesPerformanceDataGenerator dataGeneratorKM = new SalesPerformanceDataGenerator(LoadSales());
                    dataGeneratorKM.Generate();
                    e.Data = dataGeneratorKM.KeyMetrics;
                    break;
                case "dsEmployees":
                    HumanResourcesData humanResourcesDataEmployees = new HumanResourcesData(LoadEmployees());
                    e.Data = humanResourcesDataEmployees.EmployeeData;
                    break;
                case "dsDepartments":
                    HumanResourcesData humanResourcesDataDepartments = new HumanResourcesData(LoadEmployees());
                    e.Data = humanResourcesDataDepartments.DepartmentData;
                    break;
                case "dsCountries":
                    e.Data = EnergyStaticticsDataHelper.Generate(LoadEuroEnergyStatistics());
                    break;
                case "dsSales":
                    SalesOverviewDataGenerator salesOverviewDataGenerator = new SalesOverviewDataGenerator(LoadSales());
                    salesOverviewDataGenerator.Generate();
                    e.Data = salesOverviewDataGenerator.Data;
                    break;
                case "dsSalesDetails":
                    SalesDetailsDataGenerator salesDetailsDataGenerator = new SalesDetailsDataGenerator(LoadSales());
                    salesDetailsDataGenerator.Generate();
                    e.Data = salesDetailsDataGenerator.Data;
                    break;
                case "dsStatistics":
                    e.Data = RevenueByIndustryDataHelper.Generate(LoadRevenueByIndustry());
                    break;
                case "dsRevenueAnalysis":
                    RevenueAnalysisDataGenerator revenueAnalysisDataGenerator = new RevenueAnalysisDataGenerator(LoadSales());
                    revenueAnalysisDataGenerator.Generate();
                    e.Data = revenueAnalysisDataGenerator.Data;
                    break;
                case "dsCustomerSupport":
                    CustomerSupportData customerSupportData = new CustomerSupportData(LoadCustomerSupport(), LoadEmployees());
                    e.Data = customerSupportData.CustomerSupport;
                    break;
                case "dsConsumptionTotal":
                    e.Data = EnergyConsumptionDataHelper.GenerateTotal(LoadEnergyConsumptionTotal());
                    break;
                case "dsConsumptionBySector":
                    e.Data = EnergyConsumptionDataHelper.GenerateBySector(LoadEnergyConsumptionBySector());
                    break;
            }
        }
    }
}
