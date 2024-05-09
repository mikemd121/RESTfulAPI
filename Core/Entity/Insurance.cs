using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core
{
    public class Insurance
    {
       
        public string bizdate { get; set; }
        public string cp_master_id { get; set; }
        public string cp_name { get; set; }
        public string limit__c_usd { get; set; }
        public decimal PDRate { get; set; }
        public decimal InsuranceRate { get; set; }
    }
}
