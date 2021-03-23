using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Microsoft.AspNetCore.Http;
using MileAway.Models;
using MileAway.Repositories;

namespace MileAway.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        [BindProperty]
        public List<Vehicles> Vehicles { get; set; }
        [BindProperty]
        public List<Costs> Costs { get; set; }


        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToPage("Login");
            }

            Costs = CostsRepository.GetCostsByEmail(HttpContext.Session.GetString("email"));
            Vehicles = VehiclesRepository.GetVehiclesByEmail(HttpContext.Session.GetString("email"));


            Chart chart = new Chart();

            chart.Type = Enums.ChartType.Line;
            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();
            data.Labels = new List<string>() { "January", "February", "March", "April", "May", "June", "July" };

                data.Datasets = new List<Dataset>();
                Random random = new Random();
                //TODO: do something with x-axis, dates doesnt work
                //TODO: average each month on y-axis?
                foreach (var vehicle in Vehicles)
                {
                    int[,] randomColor = new int[3, 1] { { random.Next(0, 255) }, { random.Next(0, 255) }, { random.Next(0, 255) }};
                    IList<double?> pureCosts = CostsRepository.GetPureCostsByLicense(vehicle.License);
                    LineDataset dataset = new LineDataset()
                    {
                        Label = vehicle.Brand_Name + ' ' + vehicle.Model_Name,
                        Data = pureCosts,
                        Fill = "false",
                        LineTension = 0.1,
                        BackgroundColor = ChartColor.FromRgba((byte)randomColor[0,0], (byte)randomColor[1, 0], (byte)randomColor[2, 0], 0.4),
                        BorderColor = ChartColor.FromRgb((byte)randomColor[0, 0], (byte)randomColor[1, 0], (byte)randomColor[2, 0]),
                        BorderCapStyle = "butt",
                        BorderDash = new List<int> { },
                        BorderDashOffset = 0.0,
                        BorderJoinStyle = "miter",
                        PointBorderColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
                        PointBackgroundColor = new List<ChartColor> { ChartColor.FromHexString("#ffffff") },
                        PointBorderWidth = new List<int> { 1 },
                        PointHoverRadius = new List<int> { 5 },
                        PointHoverBackgroundColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
                        PointHoverBorderColor = new List<ChartColor> { ChartColor.FromRgb(220, 220, 220) },
                        PointHoverBorderWidth = new List<int> { 2 },
                        PointRadius = new List<int> { 1 },
                        PointHitRadius = new List<int> { 10 },
                        SpanGaps = false
                    };
                    data.Datasets.Add(dataset);

                }
            chart.Data = data;

            ViewData["chart"] = chart;


            return Page();
        }

    }
}
