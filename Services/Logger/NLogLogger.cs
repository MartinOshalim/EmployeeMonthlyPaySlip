using EmployeeMonthlyPaySlip.Enums;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMonthlyPaySlip.Services.Logger
{
    public class NLogLogger : Interface.ILogger
    {
        private static readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        public void Log(LogType logType, string errorMessage)
        {
            switch (logType)
            {
                case LogType.Trace:
                    _logger.Trace(errorMessage);
                    break;
                case LogType.Debug:
                    _logger.Debug(errorMessage);
                    break;
                case LogType.Info:
                    _logger.Info(errorMessage);
                    break;
                case LogType.Warn:
                    _logger.Warn(errorMessage);
                    break;
                case LogType.Error:
                    _logger.Error(errorMessage);
                    break;
                case LogType.Fatal:
                    _logger.Fatal(errorMessage);
                    break;
            }
        }
    }
}
