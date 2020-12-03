using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMonthlyPaySlip.Models
{
    public class TaxThreshold
    {
        public decimal MinimumThreshold { get; set; }
        public decimal MaximumThreshold { get; set; }
        public decimal ThresholdRate { get; set; }
    }
}
