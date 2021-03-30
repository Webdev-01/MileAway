﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using MileAway.Models;
using NodaTime;

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
                    TypecostId = 2,
                    License = costs.License,
                    Cost = costs.Cost,
                    DateOfCost = costs.Date_Of_Cost,
                    InvoiceDoc = ""
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
                    TypecostId = 1,
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

            var dateToday = DateTime.Now;
            var dateTax = roadtax[0].Date_Of_Cost;
            var dateInsurance = insurance[0].Date_Of_Cost;

            LocalDate start = new LocalDate(dateToday.Year, dateToday.Month, dateToday.Day);
            LocalDate end = new LocalDate(dateTax.Year, dateTax.Month, dateTax.Day);
            Period period = Period.Between(start, end);
            int differenceTax = Math.Abs(period.Months);

            var dateTaxCalculated = dateTax;
            for (var i = 0; i < differenceTax; i++)
            {
                dateTaxCalculated = dateTaxCalculated.AddMonths(1);
                addFixedMonthlyTax(roadtax[0].Cost, license, dateTaxCalculated);
            }

            end = new LocalDate(dateInsurance.Year, dateInsurance.Month, dateInsurance.Day);
            period = Period.Between(start, end);
            int differenceInsurance = Math.Abs(period.Months);

            var dateInsuranceCalculated = dateInsurance;
            for (var i = 0; i < differenceInsurance; i++)
            {
                dateInsuranceCalculated = dateInsuranceCalculated.AddMonths(1);
                addFixedMonthlyInsurance(insurance[0].Cost, license, dateInsuranceCalculated);
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
                return InsuranceResult == 1;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        public static IList<double?> GetAnnualCosts(string license, int year)
        {
            using var connect = DbUtils.GetDbConnection();
            List<double?> costs = new List<double?>();
            for (int i = 0; i < 12; i++)
            {
                var avgMonthlyCosts = connect.QuerySingleOrDefault<double?>("SELECT AVG(costs.Cost) FROM costs INNER JOIN typecosts ON costs.TypeCost_ID = typecosts.TypeCost_ID INNER JOIN vehicles ON costs.License = vehicles.License WHERE vehicles.License = @License AND EXTRACT(month from Date_Of_Cost) = @Month AND EXTRACT(year from Date_Of_Cost) = @Year",
                    new
                    {
                        License = license,
                        Month = i,
                        Year = year
                    }
                );
                if (avgMonthlyCosts == null)
                    costs.Add(0);
                else
                    costs.Add(avgMonthlyCosts);
            }
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
