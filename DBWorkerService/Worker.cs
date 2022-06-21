using DBWorkerService.DbAccess;
using DBWorkerService.Extensions;
using DBWorkerService.Models;

namespace DBWorkerService
{
    public class Worker : BackgroundService
    {
        private int workingCount = 0;
        private readonly ILogger<Worker> _logger;
        private readonly OracleDbHelper _oracledbHelper;

        private readonly SqlServerDbHelper _sqlServerdbHelper;

        private int PullRowCount;
        private double WorkingIntervalMinutes;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _oracledbHelper = new OracleDbHelper();
            _sqlServerdbHelper = new SqlServerDbHelper();
            PullRowCount = Convert.ToInt32(AppSettings.PullRowCount);
            WorkingIntervalMinutes = Convert.ToDouble(AppSettings.WorkingIntervalMinutes);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}",DateTime.Now);

                int max_Loss_Seqs_sql = await Task.Run(() => _sqlServerdbHelper.Get_Loss_Seq_Max_Val());
                _logger.LogInformation("{time}: Current maximum loss sequence in Sql Server: {0}", DateTime.Now, max_Loss_Seqs_sql);

                int max_Loss_Seqs = await Task.Run(() => _oracledbHelper.Get_Loss_Seq_Max_Val());
                _logger.LogInformation("{time}: Current maximum loss sequence in Oracle: {0}", DateTime.Now, max_Loss_Seqs);



                int max_Prft_Seqs_sql = await Task.Run(() => _sqlServerdbHelper.Get_Prft_Seq_Max_Val());
                _logger.LogInformation("{time}: Current maximum profit sequence in Sql Server: {0}", DateTime.Now, max_Prft_Seqs_sql);

                int max_Prft_Seqs = await Task.Run(() => _oracledbHelper.Get_Prft_Seq_Max_Val());
                _logger.LogInformation("{time}: Current maximum profit sequence in Oracle: {0}", DateTime.Now, max_Prft_Seqs);


                List<Igw_Loss_Record> Igw_D_Stat_OG_Loss_Record = new List<Igw_Loss_Record>();
                if (max_Loss_Seqs_sql < max_Loss_Seqs)
                {
                    for(int i = max_Loss_Seqs_sql + 1; i<= max_Loss_Seqs; i+= PullRowCount)
                    {
                        await Task.Run(() =>
                            Igw_D_Stat_OG_Loss_Record.AddRange(_oracledbHelper.GetLossRecords(
                                i, (max_Loss_Seqs - i + 1)>= PullRowCount ? 
                                i + PullRowCount - 1 : max_Loss_Seqs)));
                    }


                    IEnumerable<Igw_Loss_Record_SqlServer> lossRecordList = Igw_D_Stat_OG_Loss_Record.GroupBy(
                        l => new
                        {
                            l.Dest_Code,
                            l.Dest_Prefix,
                            l.Dest_Name,
                            l.Calling_Operator,
                            l.International_Carrier,
                            l.Carrier_Rate_USD,
                            l.Y_Rate_USD,
                            l.X_Rate_BDT,
                            l.Y_Rate_BDT,
                            l.Z_Rate_BDT,
                            l.BillingCycle,
                            l.Partition_Day,
                            l.Exchange_Rate
                        }).Select(cs => new Igw_Loss_Record_SqlServer
                        {
                            Dest_Code = cs.Key.Dest_Code,
                            Dest_Prefix = cs.Key.Dest_Prefix,
                            Dest_Name = cs.Key.Dest_Name,
                            Calling_Operator = cs.Key.Calling_Operator,
                            International_Carrier = cs.Key.International_Carrier,
                            Total_Calls = cs.Sum(c => c.Call_Count),
                            Total_Min = cs.Sum(c => c.Duration_Min),
                            Total_Min_Pulse = cs.Sum(c => c.Duration_Min_15s_Pulse),
                            Carrier_Rate_USD = cs.Key.Carrier_Rate_USD,
                            Y_Rate_USD = cs.Key.Y_Rate_USD,
                            X_Rate_BDT = cs.Key.X_Rate_BDT,
                            Y_Rate_BDT = cs.Key.Y_Rate_BDT,
                            Z_Rate_BDT = cs.Key.Z_Rate_BDT,
                            Total_Revenue_BDT = cs.Sum(c => c.IGW_Revenue_BDT),
                            Total_Expense_USD = cs.Sum(c => c.IGW_Expense_USD),
                            Total_Expense_BDT = cs.Sum(c => c.IGW_Expense_BDT),
                            Total_Loss_BDT = cs.Sum(c => c.IGW_Loss_BDT),
                            BillingCycle = cs.Key.BillingCycle,
                            Partition_Day = cs.Key.Partition_Day,
                            Exchange_Rate = cs.Key.Exchange_Rate
                        }).ToList().OrderByDescending(d => d.Total_Loss_BDT)
                        .OrderByDescending(e => e.Total_Min);

                    _logger.LogInformation("{time}: Group by loss count: {0}", DateTime.Now, lossRecordList.Count());


                    string info = _sqlServerdbHelper.PushLossRecordtoSqlServer(lossRecordList);
                    if (info == "OK")
                    {
                        string ret = _sqlServerdbHelper.Update_Loss_Seq_Max_Val(max_Loss_Seqs);
                        if(ret == "OK")
                        {
                            _logger.LogInformation("{time}: Inserted {0} rows into Sql Server loss records from {1} Oracle records.", DateTime.Now, lossRecordList.Count(), max_Loss_Seqs - max_Loss_Seqs_sql);
                        }
                        else
                        {
                            _logger.LogInformation("{time}: Error in updating loss sequence in Sql Server: {0}", DateTime.Now, ret);
                        }
                    }
                    else
                    {
                        _logger.LogInformation("{time}: Error in pushing loss records to Sql Server: {0}", DateTime.Now, info);
                    }
                }
                else
                {
                    _logger.LogInformation("{time}: No loss record to push to Sql Server.", DateTime.Now);
                }

                List<Igw_Prft_Record> Igw_D_Stat_OG_Prft_Record = new List<Igw_Prft_Record>();
                if (max_Prft_Seqs_sql < max_Prft_Seqs)
                {
                    for (int i = max_Prft_Seqs_sql + 1; i <= max_Prft_Seqs; i += PullRowCount)
                    {
                        await Task.Run(() =>
                            Igw_D_Stat_OG_Prft_Record.AddRange(_oracledbHelper.GetPrftRecords(
                                i, (max_Prft_Seqs - i + 1) >= PullRowCount ? 
                                i + PullRowCount - 1 : max_Prft_Seqs)));
                    }

                    IEnumerable<Igw_Prft_Record_SqlServer> prftRecordList = Igw_D_Stat_OG_Prft_Record.GroupBy(
                        l => new
                        {
                            l.Dest_Code,
                            l.Dest_Prefix,
                            l.Dest_Name,
                            l.Calling_Operator,
                            l.International_Carrier,
                            l.Carrier_Rate_USD,
                            l.Y_Rate_USD,
                            l.X_Rate_BDT,
                            l.Y_Rate_BDT,
                            l.Z_Rate_BDT,
                            l.BillingCycle,
                            l.Partition_Day,
                            l.Exchange_Rate
                        }).Select(cs => new Igw_Prft_Record_SqlServer
                        {
                            Dest_Code = cs.Key.Dest_Code,
                            Dest_Prefix = cs.Key.Dest_Prefix,
                            Dest_Name = cs.Key.Dest_Name,
                            Calling_Operator = cs.Key.Calling_Operator,
                            International_Carrier = cs.Key.International_Carrier,
                            Total_Calls = cs.Sum(c => c.Call_Count),
                            Total_Min = cs.Sum(c => c.Duration_Min),
                            Total_Min_Pulse = cs.Sum(c => c.Duration_Min_15s_Pulse),
                            Carrier_Rate_USD = cs.Key.Carrier_Rate_USD,
                            Y_Rate_USD = cs.Key.Y_Rate_USD,
                            X_Rate_BDT = cs.Key.X_Rate_BDT,
                            Y_Rate_BDT = cs.Key.Y_Rate_BDT,
                            Z_Rate_BDT = cs.Key.Z_Rate_BDT,
                            Total_Revenue_BDT = cs.Sum(c => c.IGW_Revenue_BDT),
                            Total_Expense_USD = cs.Sum(c => c.IGW_Expense_USD),
                            Total_Expense_BDT = cs.Sum(c => c.IGW_Expense_BDT),
                            Total_Profit_BDT = cs.Sum(c => c.IGW_Profit_BDT),
                            BillingCycle = cs.Key.BillingCycle,
                            Partition_Day = cs.Key.Partition_Day,
                            Exchange_Rate = cs.Key.Exchange_Rate
                        }).ToList().OrderByDescending(d => d.Total_Profit_BDT)
                        .OrderByDescending(e => e.Total_Min);


                    _logger.LogInformation("{time}: Group by Profit count: {0}", DateTime.Now, prftRecordList.Count());

                    string info = _sqlServerdbHelper.PushPrftRecordtoSqlServer(prftRecordList);
                    if (info == "OK")
                    {
                        string ret = _sqlServerdbHelper.Update_Prft_Seq_Max_Val(max_Prft_Seqs);
                        if (ret == "OK")
                        {
                            _logger.LogInformation("{time}: Inserted {0} rows into Sql Server profit records from {1} Oracle records.", DateTime.Now, prftRecordList.Count(), max_Prft_Seqs - max_Prft_Seqs_sql);
                        }
                        else
                        {
                            _logger.LogInformation("{time}: Error in updating profit sequence in Sql Server: {0}", DateTime.Now, ret);
                        }
                    }
                    else
                    {
                        _logger.LogInformation("{time}: Error in pushing profit records to Sql Server: {0}", DateTime.Now, info);
                    }
                }
                else
                {
                    _logger.LogInformation("{time}: No profit record to push to Sql Server.", DateTime.Now);
                }


                _logger.LogInformation("*************************************** {0} ***************************************", ++workingCount);


                await Task.Delay(TimeSpan.FromMinutes(WorkingIntervalMinutes), stoppingToken);
            }
        }
    }
}