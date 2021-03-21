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
    }
}
