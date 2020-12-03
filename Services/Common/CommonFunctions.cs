using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMonthlyPaySlip.Services.Common
{
    public static class CommonFunctions
    {
        public static decimal RoundFigure(decimal figure)
        {
            return Math.Round(figure, 2);
        }
    }
}
