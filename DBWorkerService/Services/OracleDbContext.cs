using DBWorkerService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBWorkerService.Services
{
    public class OracleDbContext : DbContext
    {
        public DbSet<Igw_Loss_Record> Igw_D_Stat_OG_Loss_Record { get; set; }
        public DbSet<Igw_Prft_Record> Igw_D_Stat_OG_Prft_Record { get; set; }
        public DbSet<Max_Loss_Seq> Loss_Seq_Max_Val { get; set; }
        public DbSet<Max_Prft_Seq> Prft_Seq_Max_Val { get; set; }
        public OracleDbContext(DbContextOptions<OracleDbContext> options) : base(options)
        {

        }
    }
}
