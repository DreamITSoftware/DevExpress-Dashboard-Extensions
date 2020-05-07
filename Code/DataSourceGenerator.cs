using DevExpress.DashboardCommon;
using DevExpress.DataAccess.Sql;

namespace DashboardMainDemo {
    public static class DataSourceGenerator {
        public static DashboardSqlDataSource CreateNWindDataSource() {
            DashboardSqlDataSource dashboardSqlDataSource1 = new DashboardSqlDataSource("NWind DataSource", "NorthwindConnectionString");
            dashboardSqlDataSource1.DataProcessingMode = DataProcessingMode.Client;
            dashboardSqlDataSource1.Queries.Add(SelectQueryFluentBuilder
                .AddTable("Categories")
                    .SelectColumns("CategoryName", "Description")
                .Join("Products", "CategoryID")
                    .SelectColumns("QuantityPerUnit", "UnitsInStock", "UnitsOnOrder", "ReorderLevel", "Discontinued", "ProductName")
                    .SelectColumn("UnitPrice", "Products_UnitPrice")
                .Join("OrderDetailsExtended", "ProductName")
                    .SelectColumns("Quantity", "UnitPrice", "Discount")
                .Join("Orders", "OrderID")
                    .SelectColumns("OrderDate", "RequiredDate", "ShippedDate", "ShipVia", "Freight", "ShipName", "ShipAddress", "ShipCity")
                .Join("Customers", "CustomerID")
                    .SelectColumns("CompanyName", "ContactName", "ContactTitle", "City", "Region", "Country")
                .Build("Orders"));
            return dashboardSqlDataSource1;
        }
    }
}
