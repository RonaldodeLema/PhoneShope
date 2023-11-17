using System.Diagnostics;
using System.Globalization;
using System.Text;
using CsvHelper;
using Internals.Models;
using Internals.Repository;
using Internals.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSite.Controllers;

[Authorize]
public class HomeController : Controller
{

    private readonly IOrderRepository _orderRepository;

    public HomeController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IActionResult> Index()
    {
        var statistic = new Statistics()
        {
            GetMonthly = await _orderRepository.GetEarningMonthly(DateTime.Now.Month),
            GetAnnually = await _orderRepository.GetEarningAnnually(DateTime.Now.Year),
            GetOrderPending = await _orderRepository.GetAllOrderPending(),
        };
        return View(statistic);
    }
    [Authorize("Export_DataCsv")]
    public async Task<IActionResult> ExportDataToCsv()
    {
        var listDataExport = _orderRepository.ExportCsvAnnually(DateTime.Now.Year);
        var builder = new StringBuilder();
        var stringWriter = new StringWriter(builder);

        // Writing the CSV file
        await using (var csv = new CsvWriter(stringWriter, CultureInfo.InvariantCulture))
        {
            await csv.WriteRecordsAsync(listDataExport.Result);
        }

        var buffer = Encoding.ASCII.GetBytes(stringWriter.ToString());
        return File(buffer, "text/csv", "ReportEarningManually.csv");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}