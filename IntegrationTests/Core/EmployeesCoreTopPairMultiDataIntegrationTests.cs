using Employees.Core.Employees;
using Employees.Core.Coworking;
using Employees.Infrastructure;
using static Employees.Infrastructure.ExtendedMockDataProcessor;

namespace Employees.IntegrationTests.Core
{
    [TestClass]
    public class EmployeesCoreTopPairMultiDataIntegrationTests
    {
        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesTwoProjectsSingleIntervals_DaysCoworked()
        {
            var mockDataProcessor = new ExtendedMockDataProcessor(MultiDataUseCase.TwoEmployeesTwoCommonProjectsSingleIntervals);
            List<TopEmployeePairModel> result = Process(mockDataProcessor);
            Assert.AreEqual(15, result[0].DaysCollaborated);
        }

        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesTwoProjectsSingleIntervals_ZeroDaysCoworked()
        {
            var mockDataProcessor = new ExtendedMockDataProcessor(MultiDataUseCase.TwoEmployeesTwoCommonProjectsNoIntervalMatch);
            List<TopEmployeePairModel> result = Process(mockDataProcessor);
            Assert.AreEqual(0, result[0].DaysCollaborated);
        }

        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesTwoProjectsSingleIntervals_MultiIntervalsPerProject()
        {
            var mockDataProcessor = new ExtendedMockDataProcessor(MultiDataUseCase.TwoEmployeesOneProjectsMultiIntervals);
            List<TopEmployeePairModel> result = Process(mockDataProcessor);
            Assert.AreEqual(3, result[0].DaysCollaborated);
        }

        private static List<TopEmployeePairModel> Process(ExtendedMockDataProcessor mockDataProcessor)
        {
            CoworkersPreProcessor preProcessor = new(mockDataProcessor);
            TopCoworkersProcessor top = new(preProcessor);
            List<TopEmployeePairModel> result = top.GetTopCoworkers("");
            return result;
        }
    }
}