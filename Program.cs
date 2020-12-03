using EmployeeMonthlyPaySlip.Enums;
using EmployeeMonthlyPaySlip.Models;
using EmployeeMonthlyPaySlip.Services.Logger;
using EmployeeMonthlyPaySlip.Services.Logger.Interface;
using EmployeeMonthlyPaySlip.Services.RunOptions;
using EmployeeMonthlyPaySlip.Services.TaxProcessor.Factory;
using EmployeeMonthlyPaySlip.Services.TextDisplayer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

namespace EmployeeMonthlyPaySlip
{
    public class Program
    {
        static void Main(string[] args)
        {

            // Setup and build configration file.
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();



            //Setup dependency Injection for logger.
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ILogger, NLogLogger>();


            //Bind the json file properties to the class.
            var programSettings = Configuration.Get<ProgramSettings>();
            if (programSettings != null)
            {
                Configuration.Bind("ProgramSettings", programSettings);
                serviceCollection.AddSingleton(programSettings);
            }

            var serviceProvider = serviceCollection.BuildServiceProvider();

            
            //Helpful startup text.
            Console.WriteLine("---- Read Me ----");
            Console.WriteLine("---- To calculate your monthly payslip please enter valid arugements such as 'GenerateMonthlyPayslip \"[Your Name]\" [Gross Salary]'");
            Console.WriteLine("---- For example: GenerateMonthlyPayslip \"Mary Song\" 60000");
            Console.WriteLine("---- To quit, please enter q or quit as the input and hit enter.");
            Console.WriteLine("---- Thank you ----");
            Console.WriteLine("");
            Console.WriteLine("");

            // Step 1 - Create run options and start it to read in the arguments from the user.
            var runOptions = new RunOptions(serviceProvider);

            //Run if options are valid and keep program alive until user quits.
            do
            {
                // Read the options and generate the payslip if the options provided are valid.
                runOptions.ReadInput();
                if (runOptions.IsValid())
                {
                    Run(serviceProvider, runOptions);
                }

            } while (!runOptions.HasUserQuit());
        }

        /// <summary>
        /// Main program method that orchestrates all the individual services.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="runOptions"></param>
        static void Run(ServiceProvider serviceProvider, RunOptions runOptions)
        {
            var logger = serviceProvider?.GetService<ILogger>();
            
            try
            {
                logger.Log(LogType.Trace, "Running monthly payslip program..");


                //Create an employ with a salary
                logger.Log(LogType.Trace, "Creating an employee");
                var employee = new Employee()
                {
                    Name = runOptions.FullName,
                    AnnualSalary = runOptions.GrossSalary
                };


                // Create a Tax Processor factory
                logger.Log(LogType.Trace, "Creating a tax processing factory.");
                var taxProcessorFactory = new TaxProcessorFactory(serviceProvider);
      
                // Create a type of taxProcessor class
                var taxProcessor = taxProcessorFactory.CreateTaxProcessor();
                
                if (taxProcessor == null)
                {
                    logger.Log(LogType.Error, $"TaxProcessorFactory returned null from CreateTaxProcessor().");
                    return;
                }

                // Run the calculations to calculate monthly gross income, monthly income tax and , monthly net income.
                logger.Log(LogType.Trace, "Tax processor crunching calculations.");
                taxProcessor.RunCalculations(employee);

                //Display payslip.
                var textDisplayer = new TextDisplayer(serviceProvider);
                textDisplayer.DisplayPayslip(employee, taxProcessor);

                logger.Log(LogType.Trace, "finished running monthly payslip program..");

            }
            catch (Exception e)
            {
                logger.Log(LogType.Error, $"Failed to run program exception: {e.Message}");
                return;
            }
        }

    }
}
