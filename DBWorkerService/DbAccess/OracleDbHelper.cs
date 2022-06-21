using DBWorkerService.Models;
using DBWorkerService.Services;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBWorkerService.DbAccess
{
    public class OracleDbHelper
    {
        private OracleDbContext _oracleDbContext;

        private DbContextOptions<OracleDbContext> GetAllOptions()
        {
            var optionBuilder = new DbContextOptionsBuilder<OracleDbContext>();
            optionBuilder.UseOracle(AppSettings.OracleConnectionString);
            return optionBuilder.Options;
        }

        public int Get_Loss_Seq_Max_Val()
        {
            using (_oracleDbContext = new OracleDbContext(GetAllOptions()))
            {
                try
                {
                    var seq_rec = _oracleDbContext.Loss_Seq_Max_Val.FromSqlRaw("select Loss_Seq from Loss_Seq_Max_Val").ToList().FirstOrDefault();

                    if (seq_rec != null)
                        return seq_rec.Loss_Seq;
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
            using (_oracleDbContext = new OracleDbContext(GetAllOptions()))
            {
                try
                {
                    var seq_rec = _oracleDbContext.Prft_Seq_Max_Val.FromSqlRaw("select Prft_Seq from Prft_Seq_Max_Val").ToList().FirstOrDefault();

                    if (seq_rec != null)
                        return seq_rec.Prft_Seq;
                    else
                        return 0;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public List <Igw_Prft_Record> GetPrftRecords(int start_seq, int end_seq)
        {
            using (_oracleDbContext = new OracleDbContext(GetAllOptions()))
            {
                try
                {
                    var prftRecords = _oracleDbContext.Igw_D_Stat_OG_Prft_Record.FromSqlRaw("select * from igw_d_stat_og_prft_record t where t.seq_num between {0} and {1}",
                            start_seq.ToString(), end_seq.ToString()).ToList();                   if (prftRecords != null)
                        return prftRecords;
                    else
                        return new List<Igw_Prft_Record>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        public List<Igw_Loss_Record> GetLossRecords(int start_seq, int end_seq)
        {
            {
                using (_oracleDbContext = new OracleDbContext(GetAllOptions()))
                {
                    try
                    {
                        var lossRecords =  _oracleDbContext.Igw_D_Stat_OG_Loss_Record
                            .FromSqlRaw("select * from igw_d_stat_og_loss_record t where t.seq_num between {0} and {1}", 
                            start_seq.ToString(), end_seq.ToString()).ToList();
                        if (lossRecords != null)
                            return lossRecords;
                        else
                            return new List<Igw_Loss_Record>();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
