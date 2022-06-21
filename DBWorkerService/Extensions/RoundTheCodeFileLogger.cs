using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBWorkerService.Extensions
{
    public class RoundTheCodeFileLogger : ILogger
    {
        protected readonly RoundTheCodeFileLoggerProvider _roundTheCodeLoggerFileProvider;

        public RoundTheCodeFileLogger([NotNull] RoundTheCodeFileLoggerProvider roundTheCodeLoggerFileProvider)
        {
            _roundTheCodeLoggerFileProvider = roundTheCodeLoggerFileProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        private Object logFileLock = new Object();
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var fullFilePath = _roundTheCodeLoggerFileProvider.Options.FolderPath
                .Replace("{year}", DateTime.Now.ToString("yyyy"))
                .Replace("{month}", DateTime.Now.ToString("MMM")) 
                + "\\" + _roundTheCodeLoggerFileProvider.Options.FilePath
                .Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd"));

            var logRecord = string.Format("{0} [{1}] {2}", logLevel.ToString(), formatter(state, exception), exception != null ? exception.StackTrace : "");

            lock (logFileLock)
            {
                using (var streamWriter = new StreamWriter(fullFilePath, true))
                {
                    streamWriter.WriteLine(logRecord);
                }
            }
        }
    }
}
