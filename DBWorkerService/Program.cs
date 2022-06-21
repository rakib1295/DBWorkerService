using DBWorkerService;
using DBWorkerService.Services;
using Microsoft.EntityFrameworkCore;
using DBWorkerService.Extensions;

IHostBuilder hostbuilder = Host.CreateDefaultBuilder(args);

hostbuilder.ConfigureLogging((hostBuilderContext, logging) =>
    {
        logging.AddRoundTheCodeFileLogger(options =>
        {
            hostBuilderContext.Configuration.GetSection("Logging").GetSection("RoundTheCodeFile").GetSection("Options").Bind(options);
        });
    });

hostbuilder.ConfigureServices((hostcontext, services) =>
    {


        IConfiguration configuration = hostcontext.Configuration;
        AppSettings.Configuration = configuration;
        AppSettings.OracleConnectionString = configuration.GetConnectionString("OracleConnection");
        AppSettings.SqlServerConnectionString = configuration.GetConnectionString("SqlServerConnection");

        IConfigurationSection configurationSection = configuration.GetSection("Miscellaneous");
        AppSettings.PullRowCount = configurationSection.GetValue<string>("PullRowCount");
        AppSettings.WorkingIntervalMinutes = configurationSection.GetValue<string>("WorkingIntervalMinutes");


        var sqlServerOptionBuilder = new DbContextOptionsBuilder<SqlServerDbContext>();
        sqlServerOptionBuilder.UseSqlServer(AppSettings.SqlServerConnectionString);

        var oracleOptionBuilder = new DbContextOptionsBuilder<OracleDbContext>();
        oracleOptionBuilder.UseOracle(AppSettings.OracleConnectionString);

        //services.AddDbContext<OracleDbContext>(options => options.UseOracle(AppSettings.OracleConnectionString));

        //services.AddDbContext<SqlServerDbContext>(options => options.UseSqlServer(AppSettings.SqlServerConnectionString));

        services.AddScoped(d => new SqlServerDbContext(sqlServerOptionBuilder.Options));
        services.AddScoped(d => new OracleDbContext(oracleOptionBuilder.Options));
        services.AddHostedService<Worker>();
    });
    
IHost host = hostbuilder.Build();

await host.RunAsync();
