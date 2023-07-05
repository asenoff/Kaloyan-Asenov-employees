using Employees.Core.Employees;
using Employees.Core.Coworking;
using Employees.Infrastructure;

namespace Employees.IntegrationTests.Core
{
    [TestClass]
    public class EmployeesCoreTopPairIntegrationTests
    {
        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesCommonProject_OneResult()
        {
            var mockDataProcessor = new MockTestDataProcessor(MockTestDataProcessor.UseCase.TwoEmployeesCommonProject);
            CoworkersPreProcessor preProcessor = new CoworkersPreProcessor(mockDataProcessor);
            TopCoworkersProcessor top = new TopCoworkersProcessor(preProcessor);
            List<TopEmployeePairModel> result = top.GetTopCoworkers("");
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesCommonProject_Employee1IDRight()
        {
            var mockDataProcessor = new MockTestDataProcessor(MockTestDataProcessor.UseCase.TwoEmployeesCommonProject);
            CoworkersPreProcessor preProcessor = new CoworkersPreProcessor(mockDataProcessor);
            TopCoworkersProcessor top = new TopCoworkersProcessor(preProcessor);
            List<TopEmployeePairModel> result = top.GetTopCoworkers("");
            Assert.AreEqual(1u, result[0].Employee1ID);
        }

        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesCommonProject_Employee2IDRight()
        {
            var mockDataProcessor = new MockTestDataProcessor(MockTestDataProcessor.UseCase.TwoEmployeesCommonProject);
            CoworkersPreProcessor preProcessor = new CoworkersPreProcessor(mockDataProcessor);
            TopCoworkersProcessor top = new TopCoworkersProcessor(preProcessor);
            List<TopEmployeePairModel> result = top.GetTopCoworkers("");
            Assert.AreEqual(2u, result[0].Employee2ID);
        }

        [TestMethod]
        public void ProcessTopEmployees_TwoEmployeesCommonProject_DaysCoworkedRight()
        {
            var mockDataProcessor = new MockTestDataProcessor(MockTestDataProcessor.UseCase.TwoEmployeesCommonProject);
            CoworkersPreProcessor preProcessor = new CoworkersPreProcessor(mockDataProcessor);
            TopCoworkersProcessor top = new TopCoworkersProcessor(preProcessor);
            List<TopEmployeePairModel> result = top.GetTopCoworkers("");
            Assert.AreEqual(8,result[0].DaysCollaborated);
        }
    }
}