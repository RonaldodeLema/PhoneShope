using Internals.ViewModels;

namespace Internals.Repository;

public interface IOrderRepository
{
    Task<double> GetEarningMonthly(int month);
    Task<double> GetEarningAnnually(int year);
    Task<int> GetAllOrderPending();

    Task<List<DataExportCsv>> ExportCsvAnnually(int year);
}