using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using MileAway.Models;

namespace MileAway.Repositories
{
    public class CostsRepository
    {
        public static object Calculated { get; private set; }

        public static bool AddCostRepair(Costs costs)
        {
            using var connect = DbUtils.GetDbConnection();
            try
            {
                var addCostRepair = connect.Execute("INSERT INTO costs (Typecost_Id, License, Cost, Date_Of_Cost, Invoice_Doc) VALUES (@TypecostId, @License, @Cost, @DateOfCost, @InvoiceDoc)", new
                {
                    TypecostId = costs.Typecost_Id,
                    License = costs.License,
                    Cost = costs.Cost,
                    DateOfCost = costs.Date_Of_Cost,
                    InvoiceDoc = "" //costs.Invoice_Doc
                });

                return addCostRepair == 1;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        public static bool AddCostFuel(Costs costs) 
        {
            using var connect = DbUtils.GetDbConnection();
            try
            {
                var addCostFuel = connect.Execute("INSERT INTO costs (Typecost_Id, License, Cost, Fuel_Quantity, Date_Of_Cost) VALUES (@TypecostId, @License, @Cost, @Fuel_Quantity, @DateOfCost)", new
                {
                    TypecostId = costs.Typecost_Id,
                    License = costs.License,
                    Cost = costs.Cost,
                    Fuel_Quantity = costs.Fuel_Quantity,
                    DateOfCost = costs.Date_Of_Cost,
                });

                return addCostFuel == 1;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        public static List<Costs> GetCostsByLicense(string license)
        {
            using var connect = DbUtils.GetDbConnection();

            var costs = connect.Query<Costs>("SELECT * FROM costs WHERE License = @License",
                new
                {
                    License = license
                }

            ).ToList();
            return costs;
        }

        public static List<Costs> GetCostsByLicenseInner(string license)
        {
            using var connect = DbUtils.GetDbConnection();

            var costs = connect.Query<Costs>("SELECT costs.*, typecosts.TypeCost_Name FROM costs INNER JOIN typecosts ON costs.TypeCost_ID = typecosts.TypeCost_ID WHERE License = @License ORDER BY Date_Of_Cost",
                new
                {
                    License = license
                }

            ).ToList();
            return costs;
        }
        public static List<Costs> GetCostsByEmail(string email)
        {
            using var connect = DbUtils.GetDbConnection();

            var costs = connect.Query<Costs>("SELECT costs.Cost, costs.Date_Of_Cost, vehicles.License, vehicles.Email, typecosts.TypeCost_Name FROM costs INNER JOIN typecosts ON costs.TypeCost_ID = typecosts.TypeCost_ID INNER JOIN vehicles ON costs.License = vehicles.License WHERE vehicles.Email = @Email",
                new
                {
                    Email = email
                }

            ).ToList();
            return costs;
        }

        public static bool FixedCostsMontly(string license)
        {
            using var connect = DbUtils.GetDbConnection();

            var roadtax = connect.Query<Costs>(
            "SELECT Cost,TypeCost_ID, Date_Of_Cost FROM costs WHERE License = @License AND TypeCost_ID = 3 ORDER BY Date_Of_Cost DESC",
            new
            {
                License = license
            }).ToList();

            var insurance = connect.Query<Costs>(
            "SELECT Cost,TypeCost_ID, Date_Of_Cost FROM costs WHERE License = @License AND TypeCost_ID = 4 ORDER BY Date_Of_Cost DESC",
            new
            {
                License = license
            }).ToList();

            var dateOne = DateTime.Now;
            var dateTax = roadtax[0].Date_Of_Cost;
            var dateInsurance = insurance[0].Date_Of_Cost;

            TimeSpan differnceTax = dateOne - dateTax;
            TimeSpan differnceInsurance = dateOne - dateInsurance;

            if (dateTax.Month == 2)
            {
                if (differnceTax.Days >= 27)
                {
                    var dateTaxCalculated = dateTax.AddMonths(1);
                    addFixedMonthlyTax(roadtax[0].Cost, license, dateTaxCalculated);
                }
            }
            else if (dateTax.Month == 1 || dateTax.Month == 3 || dateTax.Month == 5 || dateTax.Month == 7 || dateTax.Month == 8 || dateTax.Month == 10 || dateTax.Month == 12)
            {
                if (differnceTax.Days >= 31)
                {
                    var dateTaxCalculated = dateTax.AddMonths(1);
                    addFixedMonthlyTax(roadtax[0].Cost, license, dateTaxCalculated);
                }
            }
            else if (dateTax.Month == 4 || dateTax.Month == 6 || dateTax.Month == 9 || dateTax.Month == 11)
            {
                if (differnceTax.Days >= 30)
                {
                    var dateTaxCalculated = dateTax.AddMonths(1);
                    addFixedMonthlyTax(roadtax[0].Cost, license, dateTaxCalculated);
                }
            }

            if (dateInsurance.Month == 2)
            {
                if (differnceInsurance.Days >= 27)
                {
                    var dateInsuranceCalculated = dateInsurance.AddMonths(1);
                    addFixedMonthlyInsurance(insurance[0].Cost, license, dateInsuranceCalculated);
                }
            }
            else if (dateInsurance.Month == 1 || dateInsurance.Month == 3 || dateInsurance.Month == 5 || dateInsurance.Month == 7 || dateInsurance.Month == 8 || dateInsurance.Month == 10 || dateInsurance.Month == 12)
            {
                if (differnceInsurance.Days >= 31)
                {
                    var dateInsuranceCalculated = dateInsurance.AddMonths(1);
                    addFixedMonthlyInsurance(insurance[0].Cost, license, dateInsuranceCalculated);
                }
            }
            else if (dateInsurance.Month == 4 || dateInsurance.Month == 6 || dateInsurance.Month == 9 || dateInsurance.Month == 11)
            {
                if (differnceInsurance.Days >= 30)
                {
                    var dateInsuranceCalculated = dateInsurance.AddMonths(1);
                    addFixedMonthlyInsurance(insurance[0].Cost, license, dateInsuranceCalculated);
                }
            }

            return true;
        }

        public static bool addFixedMonthlyTax(double price, string vehicleLicense, DateTime date) 
        {
            using var connect = DbUtils.GetDbConnection();
            try
            {
                var roadTaxResult = connect.Execute("INSERT INTO costs (TypeCost_ID, License, Cost, Date_Of_Cost) VALUES (@TypeCost_ID, @License, @Cost, @Date_Of_Cost)", new
                {
                    TypeCost_ID = 3,
                    License = vehicleLicense,
                    Cost = price,
                    Date_Of_Cost = date
                });
                //TODO: add date of cost
                return roadTaxResult == 1;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        public static bool addFixedMonthlyInsurance(double price, string vehicleLicense, DateTime date)
        {
            using var connect = DbUtils.GetDbConnection();
            try
            {
                var InsuranceResult = connect.Execute("INSERT INTO costs (TypeCost_ID, License, Cost, Date_Of_Cost) VALUES (@TypeCost_ID, @License, @Cost, @Date_Of_Cost)", new
                {
                    TypeCost_ID = 4,
                    License = vehicleLicense,
                    Cost = price,
                    Date_Of_Cost = date
                });
                //TODO: add date of cost
                return InsuranceResult == 1;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        public static IList<double?> GetPureCostsByLicense(string license)
        {
            using var connect = DbUtils.GetDbConnection();

            IList<double?> costs = connect.Query<double?>("SELECT costs.Cost FROM costs INNER JOIN typecosts ON costs.TypeCost_ID = typecosts.TypeCost_ID INNER JOIN vehicles ON costs.License = vehicles.License WHERE vehicles.License = @License",
                new
                {
                    License = license
                }

            ).ToList();
            if (costs != null)
                return costs;
            return null;
        }

        public static bool AddFixedCosts(FixedCosts fixedCosts, string vehicleLicense)
        {
            {
                using var connect = DbUtils.GetDbConnection();
                try
                {
                    var insuranceResult = connect.Execute("INSERT INTO costs (TypeCost_ID, License, Cost, Date_Of_Cost) VALUES (@TypeCost_ID, @License, @Cost, @Date_Of_Cost)", new
                    {
                        TypeCost_ID = 4,
                        License = vehicleLicense,
                        Cost = fixedCosts.Insurance,
                        Date_Of_Cost = fixedCosts.Insurance_Date
                    });
                    var roadTaxResult = connect.Execute("INSERT INTO costs (TypeCost_ID, License, Cost, Date_Of_Cost) VALUES (@TypeCost_ID, @License, @Cost, @Date_Of_Cost)", new
                    {
                        TypeCost_ID = 3,
                        License = vehicleLicense,
                        Cost = fixedCosts.Road_Tax,
                        Date_Of_Cost = fixedCosts.Road_Tax_Date
                    });
                    //TODO: add date of cost
                    return insuranceResult == 1 && roadTaxResult == 1;
                }
                catch (MySqlException e)
                {
                    return false;
                }
            }
        }

        public static FixedCosts GetFixedCostsByLicense(string license)
        {
            using var connect = DbUtils.GetDbConnection();

            var roadtax = connect.Query<Costs>(
                "SELECT Cost,TypeCost_ID FROM costs WHERE License = @License AND TypeCost_ID = 3",
                new
                {
                    License = license
                }).ToList();

            var insurance = connect.Query<Costs>(
            "SELECT Cost,TypeCost_ID FROM costs WHERE License = @License AND TypeCost_ID = 4",
            new
            {
                License = license
            }).ToList();

            var fixedCosts = new FixedCosts();
            fixedCosts.Road_Tax = Convert.ToInt32(roadtax[0].Cost);
            fixedCosts.Insurance = Convert.ToInt32(insurance[0].Cost);
            return fixedCosts;
        }

        public static bool DeleteVehicleCosts(string license)
        {
            using var connect = DbUtils.GetDbConnection();
            try
            {
                var deleteCosts = connect.Execute("DELETE FROM costs WHERE License = @License", new
                {
                    License = license
                });

                return deleteCosts != 0;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }
    }
}
