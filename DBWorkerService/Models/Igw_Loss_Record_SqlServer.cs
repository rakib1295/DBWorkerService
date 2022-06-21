using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBWorkerService.Models
{
    public class Igw_Loss_Record_SqlServer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Dest_Code { get; set; }
        public string Dest_Prefix { get; set; } = "";
        [Required]
        public string Dest_Name { get; set; }
        [Required]
        public string Calling_Operator { get; set; }
        [Required]
        public string International_Carrier { get; set; }
        public double Total_Min { get; set; }
        public double Total_Min_Pulse { get; set; }
        public int Total_Calls { get; set; }
        public double Carrier_Rate_USD { get; set; }
        public double Y_Rate_USD { get; set; }
        public double X_Rate_BDT { get; set; }
        public double Y_Rate_BDT { get; set; }
        public double Z_Rate_BDT { get; set; }
        public double Total_Revenue_BDT { get; set; }
        public double Total_Expense_USD { get; set; }
        public double Total_Expense_BDT { get; set; }
        public double Total_Loss_BDT { get; set; }
        [Required]
        public string BillingCycle { get; set; }
        [Required]
        public string Partition_Day { get; set; }
        public double Exchange_Rate { get; set; }
    }
}
