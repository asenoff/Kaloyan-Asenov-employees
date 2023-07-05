using Employees.Core.Employees;
using Employees.Core.Coworking;
using Employees.Infrastructure;

namespace Employees.IntegrationTests.Core
{
    [TestClass]
    public class EmployeesCoreAllPairsCSVIntegrationTests
    {
        [TestMethod]
        public void ProcessAllEmployees_SampleCSV_SixResults()
        {
            string filePath = "../../../Core/CSVs/sampleFile15Rows.csv";
            string fileContents = File.ReadAllText(filePath);
            List<EmployeesProjectDaysModel> result = Process(fileContents);
            Assert.AreEqual(6, result.Count);
        }

        [TestMethod]
        public void ProcessAllEmployees_SampleBigCSV_TensOfThousandsOfResults()
        {
            string filePath = "../../../Core/CSVs/sampleFile10000Rows.csv";
            string fileContents = File.ReadAllText(filePath);
            List<EmployeesProjectDaysModel> result = Process(fileContents);
            Assert.IsTrue(result.Count > 20000);
        }

        [TestMethod]
        public void ProcessAllEmployees_SampleEnormousCSV_HundredsOfThousandsOfResults()
        {
            string filePath = "../../../Core/CSVs/sampleFile100000Rows.csv";
            string fileContents = File.ReadAllText(filePath);
            List<EmployeesProjectDaysModel> result = Process(fileContents);
            Assert.IsTrue(result.Count > 200000);
        }

        private static List<EmployeesProjectDaysModel> Process(string fileData)
        {
            CSVDataProcessor dataProcessor = new CSVDataProcessor();
            dataProcessor.GetRawData(fileData);
            CoworkersPreProcessor preProcessor = new(dataProcessor);
            AllCoworkersProcessor all = new(preProcessor);
            List<EmployeesProjectDaysModel> result = all.GetAllCoworkers(fileData);
            return result;
        }
    }
}