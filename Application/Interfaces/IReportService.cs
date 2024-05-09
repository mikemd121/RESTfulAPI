namespace Application
{
    public interface IReportService
    {
         Task<List<ReportModel>> GenerateReport();
    }
}
