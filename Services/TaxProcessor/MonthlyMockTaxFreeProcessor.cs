using EmployeeMonthlyPaySlip.Models;
using EmployeeMonthlyPaySlip.Services.Logger.Interface;
using EmployeeMonthlyPaySlip.Services.TaxProcessor.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMonthlyPaySlip.Services.TaxProcessor
{
    /// <summary>
    /// Simple mock algorithm for testing purposes.
    /// </summary>
    public class MonthlyMockTaxFreeProcessor : TaxProcessorBase
    {
        public MonthlyMockTaxFreeProcessor(ServiceProvider serviceProvider) : base(serviceProvider)
        {
  
        }

        /// <summary>
        /// Calculates and returns the monthly tax rate.
        /// </summary>
        /// <returns></returns>
        public override decimal CalculateMonthlyTax(decimal annualSalary)
        {
            return 0.00m;
        }
    }
}
