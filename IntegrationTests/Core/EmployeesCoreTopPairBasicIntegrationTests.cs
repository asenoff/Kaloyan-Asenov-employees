using Employees.Core.Employees;
using Employees.Core.Coworking;
using Employees.Infrastructure;

namespace Employees.IntegrationTests.Core
{
    [TestClass]
    public class EmployeesCoreTopPairBasicIntegrationTests
    {
        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesCommonProject_OneResult()
        {
            var mockDataProcessor = new BasicMockDataProcessor(BasicMockDataProcessor.SimplePairUseCase.TwoEmployeesCommonProjectLeftTopOverlap);
            List<TopEmployeePairModel> result = Process(mockDataProcessor);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesCommonProject_Employee1IDRight()
        {
            var mockDataProcessor = new BasicMockDataProcessor(BasicMockDataProcessor.SimplePairUseCase.TwoEmployeesCommonProjectLeftTopOverlap);
            List<TopEmployeePairModel> result = Process(mockDataProcessor);
            Assert.AreEqual(1u, result[0].Employee1ID);
        }

        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesCommonProject_Employee2IDRight()
        {
            var mockDataProcessor = new BasicMockDataProcessor(BasicMockDataProcessor.SimplePairUseCase.TwoEmployeesCommonProjectLeftTopOverlap);
            List<TopEmployeePairModel> result = Process(mockDataProcessor);
            Assert.AreEqual(2u, result[0].Employee2ID);
        }

        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesCommonProject_DaysCoworkedRight_LeftTopOverlap()
        {
            var mockDataProcessor = new BasicMockDataProcessor(BasicMockDataProcessor.SimplePairUseCase.TwoEmployeesCommonProjectLeftTopOverlap);
            List<TopEmployeePairModel> result = Process(mockDataProcessor);
            Assert.AreEqual(9,result[0].DaysCollaborated);
        }

        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesCommonProject_DaysCoworkedRight_RightTopOverlap()
        {
            var mockDataProcessor = new BasicMockDataProcessor(BasicMockDataProcessor.SimplePairUseCase.TwoEmployeesCommonProjectRightTopOverlap);
            List<TopEmployeePairModel> result = Process(mockDataProcessor);
            Assert.AreEqual(6, result[0].DaysCollaborated);
        }

        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesCommonProject_DaysCoworkedRight_CompleteBottomOverlap()
        {
            var mockDataProcessor = new BasicMockDataProcessor(BasicMockDataProcessor.SimplePairUseCase.TwoEmployeesCommonProjectCompleteBottomOverlap);
            List<TopEmployeePairModel> result = Process(mockDataProcessor);
            Assert.AreEqual(6, result[0].DaysCollaborated);
        }

        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesCommonProject_DaysCoworkedRight_CompleteTopOverlap()
        {
            var mockDataProcessor = new BasicMockDataProcessor(BasicMockDataProcessor.SimplePairUseCase.TwoEmployeesCommonProjectCompleteTopOverlap);
            List<TopEmployeePairModel> result = Process(mockDataProcessor);
            Assert.AreEqual(8, result[0].DaysCollaborated);
        }

        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesCommonProject_DaysCoworkedRight_LeftTopOneDayOverlap()
        {
            var mockDataProcessor = new BasicMockDataProcessor(BasicMockDataProcessor.SimplePairUseCase.TwoEmplyoyeesCommonProjectLeftTopOneDayOverlap);
            List<TopEmployeePairModel> result = Process(mockDataProcessor);
            Assert.AreEqual(1, result[0].DaysCollaborated);
        }

        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesCommonProject_DaysCoworkedRight_RightTopOneDayOverlap()
        {
            var mockDataProcessor = new BasicMockDataProcessor(BasicMockDataProcessor.SimplePairUseCase.TwoEmplyoyeesCommonProjectRightTopOneDayOverlap);
            List<TopEmployeePairModel> result = Process(mockDataProcessor);
            Assert.AreEqual(1, result[0].DaysCollaborated);
        }



        private static List<TopEmployeePairModel> Process(BasicMockDataProcessor mockDataProcessor)
        {
            CoworkersPreProcessor preProcessor = new(mockDataProcessor);
            TopCoworkersProcessor top = new(preProcessor);
            List<TopEmployeePairModel> result = top.GetTopCoworkers("");
            return result;
        }
    }
}