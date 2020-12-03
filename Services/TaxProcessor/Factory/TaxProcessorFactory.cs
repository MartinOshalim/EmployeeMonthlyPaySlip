using EmployeeMonthlyPaySlip.Enums;
using EmployeeMonthlyPaySlip.Models;
using EmployeeMonthlyPaySlip.Services.Logger.Interface;
using EmployeeMonthlyPaySlip.Services.TaxProcessor.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMonthlyPaySlip.Services.TaxProcessor.Factory
{
    /// <summary>
    /// Tax Processor Factory to return different algorithm tax methods.
    /// </summary>
    public class TaxProcessorFactory
    {
        private ServiceProvider _serviceProvider;
        private ILogger _logger;
        private ProgramSettings _programSettings;

        public TaxProcessorFactory(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider?.GetService<ILogger>();
            _programSettings = serviceProvider.GetService<ProgramSettings>();
        }

        /// <summary>
        /// Function that returns a class that implements the IMonthlyTaxProcessor, which will be used to calculate the payslip.
        /// Used in unit tests.
        /// </summary>
        /// <param name="taxProcessorType"></param>
        /// <returns></returns>
        public IMonthlyTaxProcessor CreateTaxProcessor(TaxProcessorType taxProcessorType)
        {
            return GetTaxAlgorithm(taxProcessorType);
        }

        /// <summary>
        /// Function that returns a class that implements the IMonthlyTaxProcessor, which will be used to calculate the payslip.
        /// This uses the appsettings file to select the algorithm
        /// </summary>
        /// <returns></returns>
        public IMonthlyTaxProcessor CreateTaxProcessor()
        {    
            var algorithmChoice = _programSettings?.TFNAlgorithm ?? 0;
            var taxProcessorType = (TaxProcessorType)algorithmChoice;
            return GetTaxAlgorithm(taxProcessorType);
        }

        private IMonthlyTaxProcessor GetTaxAlgorithm(TaxProcessorType taxProcessorType)
        {
            switch (taxProcessorType)
            {
                case TaxProcessorType.Australian:
                    _logger.Log(LogType.Trace, "Factory returning MonthlyAustralianTaxProcessor tax processor.");
                    return new MonthlyTaxProcessor(_serviceProvider);
                case TaxProcessorType.TaxFree:
                    _logger.Log(LogType.Trace, "Factory returning MonthlyMockTaxFreeProcessor tax processor.");
                    return new MonthlyMockTaxFreeProcessor(_serviceProvider);
                case TaxProcessorType.AllTax:
                    _logger.Log(LogType.Trace, "Factory returning MonthlyMockAllTaxProcessor tax processor.");
                    return new MonthlyMockAllTaxProcessor(_serviceProvider);
                default:
                    _logger.Log(LogType.Trace, "Factory returning MonthlyAustralianTaxProcessor tax processor.");
                    return new MonthlyTaxProcessor(_serviceProvider);
            }
        }
    }
}
