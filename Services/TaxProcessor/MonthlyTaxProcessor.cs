using EmployeeMonthlyPaySlip.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMonthlyPaySlip.Services.TaxProcessor
{
    public class MonthlyTaxProcessor : TaxProcessorBase
    {
        public MonthlyTaxProcessor(ServiceProvider serviceProvider) : base(serviceProvider)
        {
            // Setup the offical Australian tax thresholds.
            TaxableThresholds = new List<TaxThreshold>
            {
                new TaxThreshold { MinimumThreshold = 0, MaximumThreshold = 20000, ThresholdRate = 0},
                new TaxThreshold { MinimumThreshold = 20000, MaximumThreshold = 40000, ThresholdRate = 0.10m},
                new TaxThreshold { MinimumThreshold = 40000, MaximumThreshold = 80000, ThresholdRate = 0.20m},
                new TaxThreshold { MinimumThreshold = 80000, MaximumThreshold = 180000, ThresholdRate = 0.30m},
                new TaxThreshold { MinimumThreshold = 180000, MaximumThreshold = decimal.MaxValue, ThresholdRate = 0.40m},
            };
        }


        /// <summary>
        /// Calculates and returns the monthly tax rate.
        /// </summary>
        /// <returns></returns>
        public override decimal CalculateMonthlyTax(decimal annualSalary)
        {
            var yearlyTax = 0.00m;
            
            foreach(var threshold in TaxableThresholds)
            {
                if(annualSalary >= threshold.MaximumThreshold)
                {
                    yearlyTax += (threshold.MaximumThreshold - threshold.MinimumThreshold) * threshold.ThresholdRate;
                }
                else if(annualSalary < threshold.MaximumThreshold && annualSalary > threshold.MinimumThreshold)
                {
                    yearlyTax += (annualSalary - threshold.MinimumThreshold) * threshold.ThresholdRate;
                }
            }

            return yearlyTax / 12;
        }
    }
}
