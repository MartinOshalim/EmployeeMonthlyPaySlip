using EmployeeMonthlyPaySlip.Models.Interface;
using EmployeeMonthlyPaySlip.Services.Logger.Interface;
using EmployeeMonthlyPaySlip.Services.TaxProcessor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMonthlyPaySlip.Models
{
    /// <summary>
    /// Basic Employee class that stores the employee name and gross salary.
    /// </summary>
    public class Employee
    {
        public string Name { get; set; }
        public decimal AnnualSalary { get; set; }
    }
}
