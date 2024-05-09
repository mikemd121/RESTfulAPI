using Application;
using Core;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProjectUnitTests
{
    public class ReportServiceTest
    {
        private readonly Mock<IUnitOfWork> unitOfWork;
        public ReportServiceTest()
        {
            unitOfWork = new Mock<IUnitOfWork>();
        }

        /*If the count of elements in the `List<ReportModel>` matches the count of elements in 
          the `List<ARAPJDE>` GetSampleData(), the test passes. This is because in the `ReportService`, 
          the number of records in `List<ReportModel>` should be equivalent to the number 
          of records in `List<ARAPJDE>`. This equality is crucial because the service only
          performs mapping between these two sets of objects.
         */

        [Fact]
        private async Task GenerateReport_ListOfData_DataExistInRepo()
        {

            unitOfWork.Setup(x => x.GetRepository<ARAPJDE>().Get(
                                       It.IsAny<Expression<Func<ARAPJDE, bool>>>(), // Filter
                                       It.IsAny<Func<IQueryable<ARAPJDE>, IOrderedQueryable<ARAPJDE>>>(), // OrderBy
                                       It.IsAny<string>(), // IncludeProperties
                                       It.IsAny<int>(), // Start
                                       It.IsAny<int>() // Length
                                   )).Returns(GetSampleData);

            var service = new ReportService(unitOfWork.Object);
            var actionResult =await service.GenerateReport();
            var actual = actionResult as IEnumerable<ReportModel>;
            Assert.Equal(GetSampleData().Count(), actual.Count());
     
        }

        private  List<ARAPJDE> GetSampleData()
        {
            List<ARAPJDE> output = new List<ARAPJDE>()
        {
            new ARAPJDE
            {
               ACCode = "d1",
               Description ="d1",
               SupplierCode ="d1",
               SupplierName ="d1",
               contracNo = "d1",
               DueDate = "d1",
               AmountInCtrm_USD =1 ,
               AmountInJDE = 1,
            },

              new ARAPJDE
            {
               ACCode = "d2",
               Description ="d2",
               SupplierCode ="d2",
               SupplierName ="d2",
               contracNo = "d2",
               DueDate = "d2",
               AmountInCtrm_USD =2 ,
               AmountInJDE = 3,
            },

                new ARAPJDE
            {
               ACCode = "d3",
               Description ="d3",
               SupplierCode ="d3",
               SupplierName ="d3",
               contracNo = "d3",
               DueDate = "d3",
               AmountInCtrm_USD =3 ,
               AmountInJDE = 3,
            },
        };
            return output;
        }

    }
}
