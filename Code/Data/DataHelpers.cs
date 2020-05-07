﻿using System.Collections.Generic;
using System.Data;
using System.Collections;
using System;

namespace DashboardMainDemo {
    public static class DataHelper {
        public static double Random(Random rnd, double deviation, bool positive) {
            int rand = rnd.Next(positive  ? 0 : - 1000000, 1000000);
            return (double)rand / 1000000 * deviation;
        }
        public static double Random(Random rnd, double deviation) {
            return Random(rnd, deviation, false);
        }
    }

    public class ProductClass {
        readonly List<int> productIDs = new List<int>();
        readonly decimal? minPrice;
        readonly decimal? maxPrice;

        public double SaleProbability { get; private set; }

        public ProductClass(decimal? minPrice, decimal? maxPrice, double saleProbability) {
            this.minPrice = minPrice;
            this.maxPrice = maxPrice;
            SaleProbability = saleProbability;
        }
        public bool AddProduct(int productID, decimal price) {
            bool satisfyMinPrice = !minPrice.HasValue || price >= minPrice.Value;
            bool satisfyMaxPrice = !maxPrice.HasValue || price < maxPrice.Value;
            if(satisfyMinPrice && satisfyMaxPrice) {
                productIDs.Add(productID);
                return true;
            }
            return false;
        }
        public bool ContainsProduct(int productID) {
            return productIDs.Contains(productID);
        }
    }

    public class ProductClasses : List<ProductClass> {
        public new ProductClass this[int productID] {
            get {
                foreach(ProductClass productClass in this)
                    if(productClass.ContainsProduct(productID))
                        return productClass;
                throw new ArgumentException("procutID");
            }
        }

        public ProductClasses(ICollection products) {
            Add(new ProductClass(null, 100m, 0.5));
            Add(new ProductClass(100m, 500m, 0.4));
            Add(new ProductClass(500m, 1500m, 0.3));
            Add(new ProductClass(1500m, null, 0.2));
            foreach(DataRow product in products) {
                int productID = (int)product["ProductID"];
                decimal listPrice = (decimal)product["ListPrice"];
                foreach(ProductClass productClass in this)
                    if(productClass.AddProduct(productID, listPrice))
                        break;
            }
        }
    }

    public class RegionClasses : Dictionary<int, double> {
        public RegionClasses(ICollection regions) {
            int? numberEmployeesMin = null;
            foreach(DataRow region in regions) {
                short numberEmployees = (short)region["NumberEmployees"];
                numberEmployeesMin = numberEmployeesMin.HasValue ? Math.Min(numberEmployeesMin.Value, numberEmployees) : numberEmployees;
            }
            foreach(DataRow region in regions)
                Add((int)region["RegionID"], (short)region["NumberEmployees"] / (double)numberEmployeesMin.Value);
        }
    }

    public class UnitsSoldRandomGenerator {
        protected static int MinUnitsSold = 5;

        readonly Random rnd;
        readonly int startUnitsSold;
        int? prevUnitsSold;
        int? prevPrevUnitsSold;
        bool isFirst = true;

        public int UnitsSold { get; private set; }
        public int UnitsSoldTarget { get; private set; }

        public UnitsSoldRandomGenerator(Random rand, int startUnitsSold) {
            this.rnd = rand;
            this.startUnitsSold = Math.Max(startUnitsSold, MinUnitsSold);
        }
        protected virtual double GetUnitsSoldDeviation() {
            return UnitsSold * 0.5;
        }
        public void Next() {
            if(isFirst) {
                UnitsSold = startUnitsSold;
                isFirst = false;
            } else {
                UnitsSold = UnitsSold + (int)Math.Round(DataHelper.Random(rnd, GetUnitsSoldDeviation()));
                UnitsSold = Math.Max(UnitsSold, MinUnitsSold);
            }
            int unitsSoldSum = UnitsSold;
            int count = 1;
            if(prevUnitsSold.HasValue) {
                unitsSoldSum += prevUnitsSold.Value;
                count++;
            }
            if(prevPrevUnitsSold.HasValue) {
                unitsSoldSum += prevPrevUnitsSold.Value;
                count++;
            }
            UnitsSoldTarget = (int)Math.Round((double)unitsSoldSum / count);
            UnitsSoldTarget = UnitsSoldTarget + (int)Math.Round(DataHelper.Random(rnd, UnitsSoldTarget));
            prevPrevUnitsSold = prevUnitsSold;
            prevUnitsSold = UnitsSold;
        }
    }
}
