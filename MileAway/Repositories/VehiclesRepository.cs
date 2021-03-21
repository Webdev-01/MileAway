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
                var vehicleResult = connect.Execute("INSERT INTO vehicles (Email, License, Brand_Name, Model_Name, Manufacturing_Year, FuelType, Color, Mileage_Km, Vehicle_Image) VALUES (@Email, @License, @BrandName, @ModelName, @ManufacturingYear, @FuelType, @Color, @MileageKm, @VehicleImage)", new
                {
                    Email = vehicles.Email,
                    License = vehicles.License,
                    BrandName = vehicles.Brand_Name,
                    ModelName = vehicles.Model_Name,
                    ManufacturingYear = vehicles.Manufacturing_Year,
                    FuelType = vehicles.FuelType,
                    Color = vehicles.Color,
                    MileageKm = vehicles.Mileage_Km,
                    VehicleImage = vehicles.Vehicle_Image
                });

                return vehicleResult == 1;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }
        public static List<Vehicles> GetVehiclesByEmail(string email)
        {
            using var connect = DbUtils.GetDbConnection();

            var vehicles = connect.Query<Vehicles>("SELECT * FROM vehicles WHERE Email = @Email",
                new
                {
                    Email = email
                }

            ).ToList();
            return vehicles;
        }

        public static Vehicles GetVehicleByLicense(string license)
        {
            using var connect = DbUtils.GetDbConnection();

            var vehicle = connect.QueryFirstOrDefault<Vehicles>("SELECT * FROM vehicles WHERE License = @License",
                new
                {
                    License = license
                }

            );
            return vehicle;
        }
    }
}
