using Application;
using ASP.NETCoreWebAPI;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ProjectUnitTests
{
    public class ReportControllerTest
    {

        private readonly Mock<IReportService> service;

        public ReportControllerTest()
        {
            service = new Mock<IReportService>();

        }

        [Fact]
        public async Task GetReportData_ListOfReportData_DataExistsInRepo()
        {
            var reportData = GetSampleReportData();
            service.Setup(x => x.GenerateReport()).Returns(GetSampleReportData);
            var controller = new ReportApiController(service.Object);

            var actionResult = controller.GetReportData();
            var result = await actionResult as OkObjectResult;
            var actual = result?.Value as IEnumerable<ReportModel>;
            Assert.IsType<OkObjectResult>(result);
            var sampleResult = await GetSampleReportData();
            Assert.Equal(sampleResult.Count, actual?.Count());
        }


        private Task<List<ReportModel>> GetSampleReportData()
        {
            List<ReportModel> output = new List<ReportModel>
        {
            new ReportModel
            {
       ACCode = "d1",
       Description= "d1",
       SupplierCode ="d1",
       SupplierName="d1",
       ContractNo="d1",
       DueDate="d1",
       AmountInCtrmUSD="d1",
       AmountInJDE="d1",
       PDRate="d1",
       ExpectedLoss="d1",
       SFAcctTitle="d1",
       Insurance="d1",
       InsuranceRate="d1",
       InsuranceLimitUSD="d1",
       NetExposure="d1",
    },
            new ReportModel
            {
        ACCode ="d2",
       Description= "d2",
       SupplierCode="d2",
       SupplierName="d2",
       ContractNo="d2",
       DueDate="d2",
       AmountInCtrmUSD="d2",
       AmountInJDE="d2",
       PDRate="d2",
       ExpectedLoss="d2",
       SFAcctTitle="d2",
       Insurance="d2",
       InsuranceRate="d2",
       InsuranceLimitUSD="d2",
       NetExposure="d2",
            },
            new ReportModel
            {
       ACCode ="d3",
       Description="d3",
       SupplierCode="d3",
       SupplierName="d3",
       ContractNo="d3",
       DueDate="d3",
       AmountInCtrmUSD="d3",
       AmountInJDE="d3",
       PDRate="d3",
       ExpectedLoss="d3",
       SFAcctTitle="d3",
       Insurance="d3",
       InsuranceRate="d3",
       InsuranceLimitUSD="d3",
       NetExposure="d3",
            }
        };
            return Task.FromResult(output);
        }

    }
}
