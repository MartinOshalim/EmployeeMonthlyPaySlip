using EmployeeMonthlyPaySlip.Enums;
using EmployeeMonthlyPaySlip.Models;
using EmployeeMonthlyPaySlip.Services.Common;
using EmployeeMonthlyPaySlip.Services.Logger.Interface;
using EmployeeMonthlyPaySlip.Services.TaxProcessor.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMonthlyPaySlip.Services.TextDisplayer
{
    /// <summary>
    /// Class that contains methods to display results back to the user.
    /// </summary>
    public class TextDisplayer
    {
        private ILogger _logger;

        public TextDisplayer(ServiceProvider serviceProvider)
        {
            _logger = serviceProvider?.GetService<ILogger>();
        }

        public void DisplayPayslip(Employee employee, IMonthlyTaxProcessor taxProcessor)
        {
            _logger.Log(LogType.Trace, $"------------- Payslip Display Output ------------");

            Console.WriteLine();

            Console.WriteLine($"Monthly Payslip for: {employee.Name}");
            _logger.Log(LogType.Trace, $"Monthly Payslip for: {employee.Name}");

            Console.WriteLine($"Gross Monthly Income: ${string.Format("{0:0.00}", CommonFunctions.RoundFigure(taxProcessor.MonthlyGrossIncome))}");
            _logger.Log(LogType.Trace, $"Gross Monthly Income: ${string.Format("{0:0.00}", CommonFunctions.RoundFigure(taxProcessor.MonthlyGrossIncome))}");

            Console.WriteLine($"Monthly Income Tax: ${string.Format("{0:0.00}", CommonFunctions.RoundFigure(taxProcessor.MonthlyIncomeTax))}");
            _logger.Log(LogType.Trace, $"Monthly Income Tax: ${string.Format("{0:0.00}", CommonFunctions.RoundFigure(taxProcessor.MonthlyIncomeTax))}");

            Console.WriteLine($"Net Monthly Income: ${string.Format("{0:0.00}", CommonFunctions.RoundFigure(taxProcessor.MonthlyNetIncome))}");
            _logger.Log(LogType.Trace, $"Net Monthly Income: ${string.Format("{0:0.00}", CommonFunctions.RoundFigure(taxProcessor.MonthlyNetIncome))}");
            
            Console.WriteLine();

            _logger.Log(LogType.Trace, $"------------- Payslip Display Output End------------");
        }
    }
}
