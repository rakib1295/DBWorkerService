using DBWorkerService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBWorkerService.Services
{
    public class SqlServerDbContext : DbContext
    {
        public DbSet<Igw_Loss_Record_SqlServer> Igw_D_Stat_OG_Loss_Record { get; set; }
        public DbSet<Igw_Prft_Record_SqlServer> Igw_D_Stat_OG_Prft_Record { get; set; }
        public DbSet<PRFT_SEQ_MAX_VAL_SQL> PRFT_SEQ_MAX_VAL_SQL { get; set; }
        public DbSet<LOSS_SEQ_MAX_VAL_SQL> LOSS_SEQ_MAX_VAL_SQL { get; set; }

        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options)
        {

        }
    }
}
