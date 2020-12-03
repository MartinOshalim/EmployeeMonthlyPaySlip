using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMonthlyPaySlip.Models
{
     /// <summary>
     /// Class that represents the settings defined in the appsettings.json file.
     /// </summary>
    public class ProgramSettings
    {
        //Values [ 0 = Real Tax Algorithm, 1 = No Tax Mock Algorithm, 2 = 100% Tax Mock Algorithm], default is 0.
        public int? TFNAlgorithm { get; set; }
    }
}
