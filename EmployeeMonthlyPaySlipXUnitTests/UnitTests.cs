using EmployeeMonthlyPaySlip.Enums;
using EmployeeMonthlyPaySlip.Models;
using EmployeeMonthlyPaySlip.Services;
using EmployeeMonthlyPaySlip.Services.Logger;
using EmployeeMonthlyPaySlip.Services.Logger.Interface;
using EmployeeMonthlyPaySlip.Services.RunOptions;
using EmployeeMonthlyPaySlip.Services.TaxProcessor;
using EmployeeMonthlyPaySlip.Services.TaxProcessor.Factory;
using EmployeeMonthlyPaySlip.Services.TaxProcessor.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace EmployeeMonthlyPaySlipXUnitTests
{
    public class UnitTests
    {

        private static ILogger _logger;
        private static ServiceProvider _serviceProvider;


        public UnitTests()
        {
            //Setup dependency Injection for logger.
            _serviceProvider = new ServiceCollection()
            .AddSingleton<ILogger, NLogLogger>()
            .BuildServiceProvider();

            _logger = _serviceProvider.GetService<ILogger>();
        }

        /// <summary>
        /// Australian Tax Validator Testing - Valid Data
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="expectedMonthlySalary"></param>
        [Theory]
        [MemberData(nameof(AustralianTaxProcessorValidTestData))]
        public void AustralianTaxProcessorValidTests(Employee employee, decimal expectedMonthlySalary)
        {
            var taxProcessorFactory = new TaxProcessorFactory(_serviceProvider);
            var taxProcessor = taxProcessorFactory.CreateTaxProcessor(TaxProcessorType.Australian);

            taxProcessor.RunCalculations(employee);

            Assert.Equal(taxProcessor.MonthlyNetIncome, expectedMonthlySalary);
        }
        public static IEnumerable<Object[]> AustralianTaxProcessorValidTestData()
        {
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 0 }, 0 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 60000 }, 4500 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 80000 }, 5833.33 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 120000 }, 8166.67 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 200000 }, 12666.67 };
        }

        /// <summary>
        /// Australian Tax Validator Testing - Invalid Data
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="expectedMonthlySalary"></param>
        [Theory]
        [MemberData(nameof(AustralianTaxProcessorInvalidTestData))]
        public void AustralianTaxProcessorInvalidTests(Employee employee, decimal expectedMonthlySalary)
        {
            var taxProcessorFactory = new TaxProcessorFactory(_serviceProvider);
            var taxProcessor = taxProcessorFactory.CreateTaxProcessor(TaxProcessorType.Australian);

            taxProcessor.RunCalculations(employee);

            Assert.NotEqual(taxProcessor.MonthlyNetIncome, expectedMonthlySalary);
        }
        public static IEnumerable<Object[]> AustralianTaxProcessorInvalidTestData()
        {
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 0 }, 1 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 60000 }, 1 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 80000 }, 1 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 120000 }, 1 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 200000 }, 1 };
        }

        /// <summary>
        /// Tax Free Validator Testing - Valid Data
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="expectedMonthlySalary"></param>
        [Theory]
        [MemberData(nameof(TaxFreeProcessorValidTestData))]
        public void TaxFreeProcessorValidTests(Employee employee, decimal expectedMonthlySalary)
        {
            var taxProcessorFactory = new TaxProcessorFactory(_serviceProvider);
            var taxProcessor = taxProcessorFactory.CreateTaxProcessor(TaxProcessorType.TaxFree);

            taxProcessor.RunCalculations(employee);

            Assert.Equal(taxProcessor.MonthlyNetIncome, expectedMonthlySalary);
        }
        public static IEnumerable<Object[]> TaxFreeProcessorValidTestData()
        {
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 0 }, 0 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 60000 }, 5000 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 80000 }, 6666.67 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 120000 }, 10000 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 200000 }, 16666.67 };
        }

        /// <summary>
        /// Tax Free Validator Testing - Invalid Data
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="expectedMonthlySalary"></param>
        [Theory]
        [MemberData(nameof(TaxFreeProcessorInvalidTestData))]
        public void TaxFreeProcessorInvalidTests(Employee employee, decimal expectedMonthlySalary)
        {
            var taxProcessorFactory = new TaxProcessorFactory(_serviceProvider);

            var taxProcessor = taxProcessorFactory.CreateTaxProcessor(TaxProcessorType.TaxFree);

            taxProcessor.RunCalculations(employee);

            Assert.NotEqual(taxProcessor.MonthlyNetIncome, expectedMonthlySalary);
        }
        public static IEnumerable<Object[]> TaxFreeProcessorInvalidTestData()
        {
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 0 }, 1 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 60000 }, 1 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 80000 }, 1 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 120000 }, 1 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 200000 }, 1 };
        }

        /// <summary>
        /// Full Validator Testing - Valid Data
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="expectedMonthlySalary"></param>
        [Theory]
        [MemberData(nameof(FullTaxProcessorValidTestData))]
        public void FullTaxProcessorValidTests(Employee employee, decimal expectedMonthlySalary)
        {
            var taxProcessorFactory = new TaxProcessorFactory(_serviceProvider);
            var taxProcessor = taxProcessorFactory.CreateTaxProcessor(TaxProcessorType.AllTax);

            taxProcessor.RunCalculations(employee);

            Assert.Equal(taxProcessor.MonthlyNetIncome, expectedMonthlySalary);
        }
        public static IEnumerable<Object[]> FullTaxProcessorValidTestData()
        {
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 0 }, 0 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 60000 }, 0 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 80000 }, 0 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 120000 }, 0 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 200000 }, 0 };
        }

        /// <summary>
        /// Full Validator Testing - Invalid Data
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="expectedMonthlySalary"></param>
        [Theory]
        [MemberData(nameof(FullTaxProcessorInvalidTestData))]
        public void FullTaxProcessorInvalidTests(Employee employee, decimal expectedMonthlySalary)
        {
            var taxProcessorFactory = new TaxProcessorFactory(_serviceProvider);
            var taxProcessor = taxProcessorFactory.CreateTaxProcessor(TaxProcessorType.AllTax);

            taxProcessor.RunCalculations(employee);

            Assert.NotEqual(taxProcessor.MonthlyNetIncome, expectedMonthlySalary);
        }
        public static IEnumerable<Object[]> FullTaxProcessorInvalidTestData()
        {
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 0 }, 1 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 60000 }, 1 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 80000 }, 1 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 120000 }, 1 };
            yield return new object[] { new Employee() { Name = "Martin Oshalim", AnnualSalary = 200000 }, 1 };

        }

        /// <summary>
        /// Tax Factory Production - Valid Testing
        /// </summary>
        /// <param name="taxType"></param>
        /// <param name="expectedFactory"></param>
        [Theory]
        [MemberData(nameof(TaxFactoryProductionValidTestData))]
        public void TaxFactoryProductionValidTest(TaxProcessorType taxType, object expectedFactory)
        {
            var taxProcessorFactory = new TaxProcessorFactory(_serviceProvider);
            var taxProcessor = taxProcessorFactory.CreateTaxProcessor(taxType).GetType();

            Assert.True(expectedFactory.Equals(taxProcessor));
        }
        public static IEnumerable<Object[]> TaxFactoryProductionValidTestData()
        {
            yield return new object[] { TaxProcessorType.Australian, new MonthlyTaxProcessor(_serviceProvider).GetType() };
            yield return new object[] { TaxProcessorType.AllTax, new MonthlyMockAllTaxProcessor(_serviceProvider).GetType() };
            yield return new object[] { TaxProcessorType.TaxFree, new MonthlyMockTaxFreeProcessor(_serviceProvider).GetType() };
        }

        /// <summary>
        /// Tax Factory Production - Invalid Testing
        /// </summary>
        /// <param name="taxType"></param>
        /// <param name="expectedFactory"></param>
        [Theory]
        [MemberData(nameof(TaxFactoryProductionInvalidTestData))]
        public void TaxFactoryProductionInvalidTest(TaxProcessorType taxType, object expectedFactory)
        {
            var taxProcessorFactory = new TaxProcessorFactory(_serviceProvider);
            var taxProcessor = taxProcessorFactory.CreateTaxProcessor(taxType).GetType();

            Assert.False(expectedFactory.Equals(taxProcessor));
        }
        public static IEnumerable<Object[]> TaxFactoryProductionInvalidTestData()
        {
            yield return new object[] { TaxProcessorType.AllTax, new MonthlyTaxProcessor(_serviceProvider).GetType() };
            yield return new object[] { TaxProcessorType.TaxFree, new MonthlyMockAllTaxProcessor(_serviceProvider).GetType() };
            yield return new object[] { TaxProcessorType.Australian, new MonthlyMockTaxFreeProcessor(_serviceProvider).GetType() };
        }

        /// <summary>
        /// Run Options - Valid Tests
        /// </summary>
        /// <param name="input"></param>
        [Theory]
        [MemberData(nameof(RunOptionsValidTestData))]
        public void RunOptionsValidTests(string input)
        {
            var runOptions = new RunOptions(_serviceProvider) { Input = input };
            Assert.True(runOptions.IsValid());
        }
        public static IEnumerable<Object[]> RunOptionsValidTestData()
        {
            yield return new object[] { "GenerateMonthlyPayslip \"Martin Oshalim\" 60000" };
            yield return new object[] { "GenerateMonthlyPayslip \"Martin MiddleName Oshalim\" 60000" };
            yield return new object[] { "GenerateMonthlyPayslip \"Martin Oshalim\" 1" };
            yield return new object[] { "GenerateMonthlyPayslip \"Martin Oshalim\" 123" };
            yield return new object[] { "GenerateMonthlyPayslip \"Martin 123 Oshalim\" 130000.00" };
            yield return new object[] { "GenerateMonthlyPayslip \"Martin aaa aaa aa Oshalim\" 80000.00" };
            yield return new object[] { "GenerateMonthlyPayslip \"Martin\" 80000.00" };
            yield return new object[] { "GenerateMonthlyPayslip \"    \" 80000.00" };
            yield return new object[] { "GenerateMonthlyPayslip \" \" 80000.00" };
            yield return new object[] { "GenerateMonthlyPayslip \"\" 80000.00" };
            yield return new object[] { "GenerateMonthlyPayslip Martin 80000.00" };
            yield return new object[] { "GenerateMonthlyPayslip aaaa 80000.00" };
            yield return new object[] { "GenerateMonthlyPayslip aaaa $80000.00" };
            yield return new object[] { "GenerateMonthlyPayslip aaaa $1.00" };
            yield return new object[] { "GenerateMonthlyPayslip \"Martin Oshalim\" $10000.00" };
        }


        /// <summary>
        /// Run Options - Invalid Tests
        /// </summary>
        /// <param name="input"></param>
        [Theory]
        [MemberData(nameof(RunOptionsInvalidTestData))]
        public void RunOptionsInvalidTests(string input)
        {
            var runOptions = new RunOptions(_serviceProvider) { Input = input };
            Assert.False(runOptions.IsValid());
        }
        public static IEnumerable<Object[]> RunOptionsInvalidTestData()
        {
            yield return new object[] { "GenerateMonthlyPay \"Martin Oshalim\" 60000" };
            yield return new object[] { "GenerateMonthlyPayslip \"Martin MiddleName Oshalim\" aaa" };
            yield return new object[] { "GenerateMonthlyPayslip \"Martin Oshalim\"" };
            yield return new object[] { "\"Martin Oshalim\" 123" };
            yield return new object[] { "GenerateMonthlyPayslip 80000.00" };
            yield return new object[] { "GenerateMonthlyPayslip aaaa $" };
        }


        /// <summary>
        /// Run Options - Valid Quit Test 
        /// </summary>
        /// <param name="input"></param>
        [Theory]
        [MemberData(nameof(RunOptionsQuitTestValidTestData))]
        public void RunOptionsQuitTestValid(string input)
        {
            var runOptions = new RunOptions(_serviceProvider) { Input = input };
            runOptions.IsValid();
            Assert.True(runOptions.HasUserQuit());
        }
        public static IEnumerable<Object[]> RunOptionsQuitTestValidTestData()
        {
            yield return new object[] { "Quit" };
            yield return new object[] { "QuiT" };
            yield return new object[] { "QUit" };
            yield return new object[] { "QuIt" };
            yield return new object[] { "q" };
            yield return new object[] { "Q" };
        }


        /// <summary>
        /// Run Options - Invalid Quit Test 
        /// </summary>
        /// <param name="input"></param>
        [Theory]
        [MemberData(nameof(RunOptionsQuitTestInvalidTestData))]
        public void RunOptionsQuitTestInvalid(string input)
        {
            var runOptions = new RunOptions(_serviceProvider) { Input = input };
            runOptions.IsValid();
            Assert.False(runOptions.HasUserQuit());
        }
        public static IEnumerable<Object[]> RunOptionsQuitTestInvalidTestData()
        {
            yield return new object[] { "Exit" };
            yield return new object[] { "ExiT" };
            yield return new object[] { "EXit" };
            yield return new object[] { "EXIT" };
            yield return new object[] { "e" };
            yield return new object[] { "E" };
            yield return new object[] { "EXITNOW!" };
        }

    }
}
