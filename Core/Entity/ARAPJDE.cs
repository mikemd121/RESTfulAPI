using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ARAPJDE
    {

        public string ACCode { get; set; }
        public string Description { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string contracNo { get; set; }
        public string DueDate { get; set; }
        public decimal? AmountInCtrm_USD { get; set; }
        public decimal AmountInJDE { get; set; }

    }
}
