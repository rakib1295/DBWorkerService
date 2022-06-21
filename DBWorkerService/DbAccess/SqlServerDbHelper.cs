using DBWorkerService.Models;
using DBWorkerService.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBWorkerService.DbAccess
{
    public class SqlServerDbHelper
    {
        private SqlServerDbContext _SqlServerDbContext;

        private DbContextOptions<SqlServerDbContext> GetAllOptions()
        {
            var optionBuilder = new DbContextOptionsBuilder<SqlServerDbContext>();
            optionBuilder.UseSqlServer(AppSettings.SqlServerConnectionString);
            return optionBuilder.Options;
        }

        public int Get_Loss_Seq_Max_Val()
        {
            using (_SqlServerDbContext = new SqlServerDbContext(GetAllOptions()))
            {
                try
                {
                    IEnumerable<LOSS_SEQ_MAX_VAL_SQL> seq_rec = _SqlServerDbContext.LOSS_SEQ_MAX_VAL_SQL;

                    if (seq_rec != null)
                        return seq_rec.FirstOrDefault().Loss_Seq;
                    else
                        return 0;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public int Get_Prft_Seq_Max_Val()
        {
            using (_SqlServerDbContext = new SqlServerDbContext(GetAllOptions()))
            {
                try
                {
                    IEnumerable<PRFT_SEQ_MAX_VAL_SQL> seq_rec = _SqlServerDbContext.PRFT_SEQ_MAX_VAL_SQL;

                    if (seq_rec != null)
                        return seq_rec.FirstOrDefault().Prft_Seq;
                    else
                        return 0;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public string Update_Loss_Seq_Max_Val(int val)
        {
            using (_SqlServerDbContext = new SqlServerDbContext(GetAllOptions()))
            {
                try
                {
                    _SqlServerDbContext.LOSS_SEQ_MAX_VAL_SQL.FirstOrDefault().Loss_Seq = val;
                    _SqlServerDbContext.SaveChanges();

                    return "OK";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        public string Update_Prft_Seq_Max_Val(int val)
        {
            using (_SqlServerDbContext = new SqlServerDbContext(GetAllOptions()))
            {
                try
                {
                    _SqlServerDbContext.PRFT_SEQ_MAX_VAL_SQL.FirstOrDefault().Prft_Seq = val;
                    _SqlServerDbContext.SaveChanges();

                    return "OK";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }



        public string PushLossRecordtoSqlServer(IEnumerable<Igw_Loss_Record_SqlServer> lossRecordList)
        {
            using (_SqlServerDbContext = new SqlServerDbContext(GetAllOptions()))
            {
                try
                {
                    _SqlServerDbContext.Igw_D_Stat_OG_Loss_Record.AddRange(lossRecordList);
                    _SqlServerDbContext.SaveChanges();

                    return "OK";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        public string PushPrftRecordtoSqlServer(IEnumerable<Igw_Prft_Record_SqlServer> prftRecordList)
        {
            using (_SqlServerDbContext = new SqlServerDbContext(GetAllOptions()))
            {
                try
                {
                    _SqlServerDbContext.Igw_D_Stat_OG_Prft_Record.AddRange(prftRecordList);
                    _SqlServerDbContext.SaveChanges();

                    return "OK";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}
