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
                var Addcost = connect.Execute("INSERT INTO costs (Cost_ID, Typecost_Id, Vehicle_Id, Cost, Date_Of_Cost, Invoice_Doc) VALUES (@CostId, @TypecostId, @VehicleId, @Cost, @DateOfCost, InvoiceDoc)", new
                {
                    CostId = costs.Cost_Id,
                    TypecostId = costs.Typecost_Id,
                    VehicleId = costs.Vehicle_Id,
                    Cost = costs.Cost,
                    DateOfCost = costs.Date_Of_Cost,
                    InvoiceDoc = costs.Invoice_Doc
                });

                return true;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }
    }
}
