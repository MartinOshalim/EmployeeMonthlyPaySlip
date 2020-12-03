using EmployeeMonthlyPaySlip.Models;
using EmployeeMonthlyPaySlip.Services.Common;
using EmployeeMonthlyPaySlip.Services.Logger.Interface;
using EmployeeMonthlyPaySlip.Services.TaxProcessor.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMonthlyPaySlip.Services.TaxProcessor
{
    /// <summary>
    /// Base tax processor class that contains all common functions between all tax different processors algorithms.
    /// </summary>
    public abstract class TaxProcessorBase : IMonthlyTaxProcessor
    {
        public decimal GrossAnnualIncome { get; set; }
        public List<TaxThreshold> TaxableThresholds { get; set; }
        public decimal MonthlyGrossIncome { get; set; }
        public decimal MonthlyIncomeTax { get; set; }
        public decimal MonthlyNetIncome { get; set; }

        private ILogger _logger;
        public TaxProcessorBase(ServiceProvider serviceProvider)
        {
            _logger = serviceProvider?.GetService<ILogger>();
        }

        /// <summary>
        /// Main function that is called to calculate the payslip.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public bool RunCalculations(Employee employee)
        {
            try
            {
                MonthlyGrossIncome = CalculateMonthlyGrossIncome(employee.AnnualSalary);
                MonthlyIncomeTax = CalculateMonthlyTax(employee.AnnualSalary);
                MonthlyNetIncome = CommonFunctions.RoundFigure(MonthlyGrossIncome - MonthlyIncomeTax);

                return true;
            }
            catch(Exception e)
            {
                _logger.Log(Enums.LogType.Error, $"Failed to run calcaulations with the following exception {e.Message}.");
                return false;
            }
        }

        /// <summary>
        /// Abstract class that is defined in the child class, will contain the different tax bracket algorithm.
        /// </summary>
        /// <param name="annualSalary"></param>
        /// <returns></returns>
        public abstract decimal CalculateMonthlyTax(decimal annualSalary);

        /// <summary>
        /// Calculates and returns the Gross annual salary.
        /// </summary>
        /// <returns></returns>
        public decimal CalculateMonthlyGrossIncome(decimal annualSalary)
        {
            return annualSalary / 12;
        }
    }
}
