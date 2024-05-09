using Core;
using Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Application
{
    public class ReportService : IReportService
    {

        IUnitOfWork unitOfWork;
        public ReportService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<ReportModel>> GenerateReport()
        {

            var arApJdeList = unitOfWork.GetRepository<ARAPJDE>().Get().ToList();
            var listReportModel = new List<ReportModel>();
            foreach (var item in arApJdeList)
            {
                var reportModel = new ReportModel();
                reportModel.ACCode = item.ACCode;
                reportModel.Description = item.Description;
                reportModel.SupplierCode = item.SupplierCode;
                reportModel.SupplierName = item.SupplierName;
                reportModel.ContractNo = item.contracNo;
                reportModel.DueDate = item.DueDate;
                reportModel.AmountInCtrmUSD = item.AmountInCtrm_USD?.ToString("G29") ?? String.Empty;
                reportModel.AmountInJDE = item.AmountInJDE.ToString("G29");
                reportModel.PDRate = GetPDRate(reportModel.SupplierCode) ?? String.Empty;
                reportModel.SFAcctTitle = GetSFAccTitle(item) ?? String.Empty;
                reportModel.Insurance = await GetInsuarance(reportModel.SFAcctTitle, reportModel.SupplierName);
                reportModel.InsuranceRate = GetInsuaranceRate(item.SupplierName) ?? String.Empty;
                reportModel.InsuranceLimitUSD = await GetInsuaranceLimitUSD(reportModel.Insurance, reportModel.SFAcctTitle, arApJdeList) ?? String.Empty;
                listReportModel.Add(reportModel);
            }


            foreach (var reportModel in listReportModel)
            {
                reportModel.ExpectedLoss = GetExpectedLoss(reportModel.PDRate, reportModel.AmountInJDE, reportModel.Insurance);
                reportModel.NetExposure = await GetNetExposure(reportModel.Insurance, reportModel.AmountInJDE, reportModel.SFAcctTitle, reportModel.InsuranceRate, listReportModel, reportModel.InsuranceLimitUSD);
            }
            return listReportModel;
        }


        private string? GetPDRate(string SupplierCode)
        {
            Expression<Func<CPMappings, bool>> expression = x => x.abcode_number == SupplierCode;
            return unitOfWork.GetRepository<CPMappings>()?.Get(x => x.abcode_number == SupplierCode).Select(x => x.PDRate).FirstOrDefault();


        }


        private string GetExpectedLoss(string pdRate, string AmountInJDE, string insurance)
        {
            if (string.IsNullOrEmpty(pdRate) || string.IsNullOrEmpty(AmountInJDE) || string.IsNullOrEmpty(insurance))
                return string.Empty;

            bool insuranceValue = insurance == "1";
            decimal expectedLoss = Convert.ToDecimal(pdRate) * Convert.ToDecimal(AmountInJDE);

            if (insuranceValue)
                return Math.Round(expectedLoss, 2).ToString();
            else
                return expectedLoss.ToString();
        }

        private string? GetInsuaranceRate(string supplierName)
        {
            Expression<Func<Insurance, bool>> expression = S => S.cp_name == supplierName;
            var value = unitOfWork.GetRepository<Insurance>()?.Get().AsQueryable().Where(expression).Select(x => x.InsuranceRate).FirstOrDefault();
            var rate = value * 100;
            return rate?.ToString("G29");
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="insurance"></param>
        /// <param name="amountInJDE"></param>
        /// <returns></returns>
        private async Task<string> GetNetExposure(string insurance, string amountInJDE, string SFAcctTitle, string insuranceRate, List<ReportModel> reportModelList, string InsuranceLimitUSD)
        {

            int insuranceValue = int.Parse(insurance);

            if (insuranceValue == 0)
                return amountInJDE;

            decimal sum = await Task.Run(() =>
            {
                return reportModelList
                    .Where(d => d.SFAcctTitle == SFAcctTitle)
                    .Sum(s => decimal.TryParse(s.AmountInCtrmUSD, out decimal amount) ? amount : 0);
            });

            decimal sum2 = await Task.Run(() =>
            {
                return reportModelList
                    .Where(d => d.SFAcctTitle == SFAcctTitle)
                    .Sum(s => decimal.TryParse(s.InsuranceLimitUSD, out decimal amount) ? amount : 0);
            });

            string result = sum < sum2
                ? (Convert.ToDecimal(amountInJDE) - (Convert.ToDecimal(amountInJDE) * Convert.ToDecimal(insuranceRate)) / 100).ToString()
                : InsuranceLimitUSD;

            return result;
        }


        private async Task<string> GetInsuaranceLimitUSD(string insuarance, string sFAcctTitle, List<ARAPJDE> list)
        {

            return await Task.Run(() =>
            {
                var value = Convert.ToInt32(insuarance);
                if (value == 0)
                    return "0";
                else
                {
                    Expression<Func<Insurance, bool>> expression = x => x.cp_name == sFAcctTitle;
                    var insuranceObject = unitOfWork.GetRepository<Insurance>()?.Get(x => x.cp_name == sFAcctTitle).FirstOrDefault();

                    if (insuranceObject != null)
                    {
                        var limitUSDValue = Convert.ToInt32(insuranceObject.limit__c_usd);
                        var insuranceList = unitOfWork.GetRepository<Insurance>()?.Get().ToList();
                        var noOfOccurrence = list.Count(s => s.SupplierName == sFAcctTitle);

                        if (noOfOccurrence == 0)
                            return "0";
                        else
                            return Math.Ceiling((decimal)limitUSDValue / noOfOccurrence).ToString();
                    }
                    else
                        return "0";
                }
            });

        }


        private async Task<string> GetInsuarance(string SFAcctTitle, string SupplierName)
        {
            return await Task.Run(() =>
            {
                Expression<Func<Insurance, bool>> expression = x => x.cp_name == SFAcctTitle || x.cp_name == SupplierName;
                var insuranceCount = unitOfWork.GetRepository<Insurance>()?.Get().AsQueryable().Where(expression)
                    .Count();

                return insuranceCount > 0 ? "1" : "0";
            });

        }

        private string? GetSFAccTitle(ARAPJDE aRAPJDE)
        {
            Expression<Func<CPMappings, bool>> expression = x => x.abcode_number == aRAPJDE.SupplierCode;
            return unitOfWork.GetRepository<CPMappings>()?.Get().AsQueryable().Where(expression).Select(x => x.SalesForceCPName).FirstOrDefault();
        }

    }
}
