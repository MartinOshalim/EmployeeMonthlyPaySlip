using EmployeeMonthlyPaySlip.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMonthlyPaySlip.Services.Logger.Interface
{
    public interface ILogger
    {
        void Log(LogType logType, string errorMessage);
    }
}
