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
        /// <summary>
        /// Adds new vehicle
        /// </summary>
        /// <param name="vehicle">Vehicle details</param>
        /// <returns>True if success</returns>
        public static bool AddVehicle(Vehicles vehicle)
        {
            using var connect = DbUtils.GetDbConnection();
            try
            {
                var vehicleResult = connect.Execute("INSERT INTO vehicle (Email, License, Brand_Name, Model_Name, Manufacturing_Year, FuelType, Color, Mileage_Km, Vehicle_Image) VALUES (@Email, @License, @BrandName, @ModelName, @ManufacturingYear, @FuelType, @Color, @MileageKm, @VehicleImage)", new
                {
                    Email = vehicle.Email,
                    License = vehicle.License,
                    BrandName = vehicle.Brand_Name,
                    ModelName = vehicle.Model_Name,
                    ManufacturingYear = vehicle.Manufacturing_Year,
                    FuelType = vehicle.FuelType,
                    Color = vehicle.Color,
                    MileageKm = vehicle.Mileage_Km,
                    VehicleImage = vehicle.Vehicle_Image
                });

                return vehicleResult == 1;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all vehicles of a user by email
        /// </summary>
        /// <param name="email">Email of user</param>
        /// <returns>List of all vehicles of a user</returns>
        public static List<Vehicles> GetVehiclesByEmail(string email)
        {
            using var connect = DbUtils.GetDbConnection();

            var vehicles = connect.Query<Vehicles>("SELECT vehicle.*, AVG(costs.Cost) AS AvgCosts FROM vehicle INNER JOIN costs ON vehicle.License = costs.License WHERE vehicle.Email = @Email GROUP BY vehicle.License",
                new
                {
                    Email = email
                }
            ).ToList();
            return vehicles;
        }

        /// <summary>
        /// Get vehicle by license
        /// </summary>
        /// <param name="license">License of a vehicle</param>
        /// <returns>Vehicle details of selected vehicle or null if non correspond</returns>
        public static Vehicles GetVehicleByLicense(string license)
        {
            using var connect = DbUtils.GetDbConnection();

            var vehicle = connect.QueryFirstOrDefault<Vehicles>("SELECT * FROM vehicle WHERE License = @License",
                new
                {
                    License = license
                }

            );
            return vehicle;
        }

        /// <summary>
        /// Updates mileage of a vehicle
        /// </summary>
        /// <param name="license">License of a vehicle</param>
        /// <param name="mileage_KM">New mileage set in KM</param>
        /// <returns>True if success</returns>
        public static bool UpdateMileage_KM(string license, int mileage_KM)
        {
            using var connect = DbUtils.GetDbConnection();
            try
            {
                var UpdateMilage_KM = connect.Execute("UPDATE vehicle SET Mileage_KM = @MilageKM WHERE License = @License", new
                {
                    License = license,
                    MilageKM = mileage_KM
                });

                return UpdateMilage_KM == 1;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes a vehicle
        /// </summary>
        /// <param name="license">License of a vehicle</param>
        /// <returns>True if success</returns>
        public static bool DeleteVehicle(string license)
        {
            using var connect = DbUtils.GetDbConnection();
            try
            {
                var deleteVehicle = connect.Execute("DELETE FROM vehicle WHERE License = @License", new
                {
                    License = license
                });

                return deleteVehicle == 1;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }
    }
}
