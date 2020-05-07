using System;
using System.Data;

namespace DashboardMainDemo {
    public abstract class SalesDataGenerator {
        public class Context {
            public string State { get; private set; }
            public string ProductName { get; private set; }
            public string CategoryName { get; private set; }
            public decimal ListPrice { get; private set; }
            public UnitsSoldRandomGenerator UnitsSoldGenerator { get; private set; }

            public Context(string st, string prodName, string catName, decimal lPrice, UnitsSoldRandomGenerator uSoldGenerator) {
                State = st;
                ProductName = prodName;
                CategoryName = catName;
                ListPrice = lPrice;
                UnitsSoldGenerator = uSoldGenerator;
            }
        }

        protected static string GetState(DataRow region) {
            return (string)region["Region"];
        }
        protected static string GetProductName(DataRow product) {
            return (string)product["Name"];
        }
        protected static decimal GetListPrice(DataRow product) {
            return (decimal)product["ListPrice"];
        }

        readonly DataTable categoriesTable;
        readonly DataTable productsTable;
        readonly DataTable regionsTable;
        readonly Random rand = new Random(1);
        readonly ProductClasses _prodClasses;
        readonly RegionClasses _regClasses;

        protected DataRowCollection Regions { get { return regionsTable.Rows; } }
        protected DataRowCollection Products { get { return productsTable.Rows; } }
        protected ProductClasses ProdClasses { get { return _prodClasses; } }
        protected RegionClasses RegClasses { get { return _regClasses; } }
        protected Random Random { get { return rand; } }

        protected SalesDataGenerator(DataSet ds) {
            categoriesTable = ds.Tables["Categories"];
            productsTable = ds.Tables["Products"];
            regionsTable = ds.Tables["Regions"];
            _prodClasses = new ProductClasses(productsTable.Rows);
            _regClasses = new RegionClasses(regionsTable.Rows);
        }
        protected double GetRegionWeight(DataRow region) {
            return _regClasses[(int)region["RegionID"]];
        }
        protected ProductClass GetProductClass(DataRow product) {
            return _prodClasses[(int)product["ProductID"]];
        }
        protected string GetCategoryName(DataRow product) {
            return (string)categoriesTable.Select(string.Format("CategoryID = {0}", product["CategoryID"]))[0]["CategoryName"];
        }
        protected virtual UnitsSoldRandomGenerator CreateUnitsSoldGenerator(double regionWeight, ProductClass productClass) {
            return new UnitsSoldRandomGenerator(rand, (int)Math.Ceiling(productClass.SaleProbability * regionWeight));
        }
        protected abstract void Generate(Context context);
        protected virtual void EndGenerate() {
        }
        public void Generate() {
            foreach(DataRow region in Regions) {
                string state = GetState(region);
                double regionWeight = GetRegionWeight(region);
                foreach(DataRow product in Products) {
                    UnitsSoldRandomGenerator unitsSoldgenerator = CreateUnitsSoldGenerator(regionWeight, GetProductClass(product));
                    Generate(new Context(state, GetProductName(product), GetCategoryName(product), GetListPrice(product), unitsSoldgenerator));
                }
            }
            EndGenerate();
        }
    }
}
