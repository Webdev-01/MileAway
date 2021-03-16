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
    public class VehiclesRepository
    {

        public static bool AddVehicle(Vehicles vehicles)
        {
            using var connect = DbUtils.GetDbConnection();
            try
            {
                var Addcost = connect.Execute("INSERT INTO vehicles (Vehicle_Id, User_Id, License, Brand_Name, Model_Name, Manufacturing_Year, Color, Mileage_Km, Vehicle_Image) VALUES (@VehicleID, @UserID, @License, @BrandName, @ModelName, @ManufacturingYear, @Color, @MileageKm, @VehicleImage)", new
                {
                    VehicleID = vehicles.Vehicle_Id,
                    UserID = vehicles.User_Id,
                    License = vehicles.License,
                    BrandName = vehicles.Brand_Name,
                    ModelName = vehicles.Model_Name,
                    ManufacturingYear = vehicles.Manufacturing_Year,
                    Color = vehicles.Color,
                    MileageKm = vehicles.Mileage_Km,
                    VehicleImage = vehicles.Vehicle_Image
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
