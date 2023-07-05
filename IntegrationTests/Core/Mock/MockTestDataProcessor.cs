using Employees.Core.Interfaces;
using Employees.Core.Employees;

namespace Employees.Infrastructure
{
    public class MockTestDataProcessor : IDataProcessor
    {
        private List<RawEmployeeProjectModel> testData;
        internal UseCase Case { get; private set; }
        public MockTestDataProcessor(UseCase useCase) 
        {
            Case = useCase;
            switch (useCase)
            {
                case UseCase.TwoEmployeesCommonProject:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-20), ToDate = DateTime.Now.AddDays(-2) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-1) }
                    };
                    break;
                default:
                    testData = new List<RawEmployeeProjectModel>();
                    break;
            }
        }

        public List<T> GetRawData<T>(string contents) where T : RawEmployeeProjectModel
        {
            List<RawEmployeeProjectModel> result = new List<RawEmployeeProjectModel>
            {
                new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-20), ToDate = DateTime.Now.AddDays(-2) },
                new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-1) }
            };

            return result as List<T>;
        }

        public enum UseCase
        {
            TwoEmployeesCommonProject = 1
        }
    }
}