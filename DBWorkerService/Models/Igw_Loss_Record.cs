using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBWorkerService.Models
{
    public class Igw_Loss_Record
    {
        [Key]
        public int Seq_Num { get; set; }
        [Required]
        public string Dest_Code { get; set; }
        [Required]
        public string Dest_Name { get; set; }
        [Required]
        public string In_Trunk { get; set; }
        [Required]
        public string Out_Trunk { get; set; }
        public double Duration_Min { get; set; }
        public double Duration_Min_15s_Pulse { get; set; }
        [Required]
        public string Calling_Operator { get; set; }
        [Required]
        public string International_Carrier { get; set; }
        public double Carrier_Rate_USD { get; set; }
        public double X_Rate_BDT { get; set; }
        public double Y_Rate_USD { get; set; }
        public double Y_Rate_BDT { get; set; }
        public double Z_Rate_BDT { get; set; }
        [Required]
        public string Hourly { get; set; }
        public double IGW_Revenue_BDT { get; set; }
        public double IGW_Expense_USD { get; set; }
        public double IGW_Expense_BDT { get; set; }
        public double IGW_Loss_BDT { get; set; }
        [Required]
        public string BillingCycle { get; set; }
        [Required]
        public string Partition_Day { get; set; }
        public double Exchange_Rate { get; set; }
        public int Call_Count { get; set; }
        public string Dest_Prefix { get; set; } = "";
        public DateTime Insertion_Time { get; set; }
    }
}
