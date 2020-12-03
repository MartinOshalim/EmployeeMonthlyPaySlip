using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMonthlyPaySlip.Enums
{
    public enum LogType
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    };


    public enum TaxProcessorType
    {
        Australian,
        TaxFree,
        AllTax
    };
}
