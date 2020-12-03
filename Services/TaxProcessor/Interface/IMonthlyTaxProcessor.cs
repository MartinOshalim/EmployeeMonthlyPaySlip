using EmployeeMonthlyPaySlip.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMonthlyPaySlip.Services.TaxProcessor.Interface
{
    /// <summary>
    /// Tax Processor interface, any new tax algorithm will need to implement this for backwards compatibility.
    /// </summary>
    public interface IMonthlyTaxProcessor
    {
        public decimal GrossAnnualIncome { get; set; }
        public List<TaxThreshold> TaxableThresholds { get; set; }
        public decimal MonthlyGrossIncome { get; set; }
        public decimal MonthlyIncomeTax { get; set; }
        public decimal MonthlyNetIncome { get; set; }
        public bool RunCalculations(Employee employee);
        public abstract decimal CalculateMonthlyTax(decimal annualSalary);
        public decimal CalculateMonthlyGrossIncome(decimal annualSalary);
    }
}
