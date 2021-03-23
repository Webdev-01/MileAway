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

        public static bool AddCost(Costs costs)
        {
            using var connect = DbUtils.GetDbConnection();
            try
            {
                var addCost = connect.Execute("INSERT INTO costs (Cost_ID, Typecost_Id, License, Cost, Fuel_Quantity, Date_Of_Cost, Invoice_Doc) VALUES (@CostId, @TypecostId, @License, @Cost, @Fuel_Quantity, @DateOfCost, InvoiceDoc)", new
                {
                    CostId = costs.Cost_Id,
                    TypecostId = costs.Typecost_Id,
                    License = costs.License,
                    Cost = costs.Cost,
                    Fuel_Quantity = costs.Fuel_Quantity,
                    DateOfCost = costs.Date_Of_Cost,
                    InvoiceDoc = costs.Invoice_Doc
                });

                return addCost == 1;
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

        public static bool AddFixedCosts(double insurance, double roadTax, string vehicleLicense)
        {
            {
                using var connect = DbUtils.GetDbConnection();
                try
                {
                    var insuranceResult = connect.Execute("INSERT INTO costs (TypeCost_ID, License, Cost, Date_Of_Cost) VALUES (@TypeCost_ID, @License, @Cost, @Date_Of_Cost)", new
                    {
                        TypeCost_ID = 4,
                        License = vehicleLicense,
                        Cost = insurance,
                        Date_Of_Cost = DateTime.Now
                    });
                    var roadTaxResult = connect.Execute("INSERT INTO costs (TypeCost_ID, License, Cost, Date_Of_Cost) VALUES (@TypeCost_ID, @License, @Cost, @Date_Of_Cost)", new
                    {
                        TypeCost_ID = 3,
                        License = vehicleLicense,
                        Cost = roadTax,
                        Date_Of_Cost = DateTime.Now
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

            var costs = connect.Query<Costs>(
                "SELECT Cost,TypeCost_ID FROM costs WHERE License = @License AND (TypeCost_ID = 3 OR TypeCost_ID = 4)",
                new
                {
                    License = license
                }).ToList();
            var fixedCosts = new FixedCosts();
            fixedCosts.Road_Tax = costs[0].Cost;
            fixedCosts.Insurance = costs[1].Cost;
            return fixedCosts;
        }
    }
}
