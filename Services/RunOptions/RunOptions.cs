using EmployeeMonthlyPaySlip.Enums;
using EmployeeMonthlyPaySlip.Services.Logger.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EmployeeMonthlyPaySlip.Services.RunOptions
{
    public class RunOptions
    {
        public String RunMethod { get; set; }
        public String FullName { get; set; }
        public decimal GrossSalary { get; set; }
        public string Input { get; set; }

        private ILogger _logger;
        private bool _userQuit = false;

        /// <summary>
        /// Class used to capture and store the parameters entered to generate the monthly payslip.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public RunOptions(ServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILogger>();
        }

        /// <summary>
        /// Resets the options and reads the input.
        /// </summary>
        public void ReadInput()
        {
            ResetOptions();
            Input = Console.ReadLine();
        }

        /// <summary>
        /// Resets the previous options entered.
        /// </summary>
        public void ResetOptions()
        {
            RunMethod = "";
            FullName = "";
            GrossSalary = 0.00m;
        }

        /// <summary>
        /// Function to check in the input the user has entered is valid.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            // If no arguments have been provided, log error to log and screen and return.
            if (string.IsNullOrEmpty(Input))
            {
                IncorrectArugmentsError(Input, "No arguements were provided.");
                return false;
            }

            //Regex pattern to match on any words or words and spaces within double quotes
            //Caters for dollar sign and points at the end also.
            var regex = new Regex(@"[\w\.\,\$]+|""[\w\s]*""");
            var inputList = regex.Matches(Input).Cast<Match>().ToList();


            // If 1 argument has been provided check that its the quit command.
            if (inputList.Count == 1 &&
                (String.Compare(Input, "q", StringComparison.OrdinalIgnoreCase) == 0
                || String.Compare(Input, "quit", StringComparison.OrdinalIgnoreCase) == 0))
            {
                _userQuit = true;
                return false;
            }

            // If not enough arguments have been provided, log error to log and screen and return.
            if (inputList.Count < 3)
            {
                IncorrectArugmentsError(Input, "Not enough arguements provided.");
                return false;
            }


            // Read first arguement and check if its a valid arguement
            RunMethod = inputList[0].Value;
            if (String.Compare(RunMethod, "GenerateMonthlyPayslip", StringComparison.OrdinalIgnoreCase) != 0)
            {
                IncorrectArugmentsError(Input, "Invalid method provided.");
                return false;
            }

            // Read the 2nd argument and assign it to Name
            FullName = inputList[1].Value;


            // Read the 3rd argument and ensure its a salary by parsing it to a decimal.
            if (Decimal.TryParse(inputList[2].Value.Trim('$'), out var grossSalary))
            {
                GrossSalary = grossSalary;
            }
            else
            {
                IncorrectArugmentsError(Input, "Gross salary entered was invalid.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Function to write and log an error message when invalid arguments are provided.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="additionalMessage"></param>
        public void IncorrectArugmentsError(string input, string additionalMessage)
        {
            Console.WriteLine($"Please enter valid arugements such as 'GenerateMonthlyPayslip \"Mary Song\" 60000'. {additionalMessage}");
            Console.WriteLine();

            _logger.Log(LogType.Error, $"Error input provided is invalid: {input}. Additional Info:{additionalMessage}");
        }


        /// <summary>
        /// Function to check if the user has decided they want to quit the program.
        /// </summary>
        /// <returns></returns>
        public bool HasUserQuit()
        {
            return _userQuit;
        }
    }
}
