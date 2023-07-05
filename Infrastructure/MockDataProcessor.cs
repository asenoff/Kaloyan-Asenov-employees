using Employees.Core.Interfaces;
using Employees.Core.Employees;

namespace Employees.Infrastructure
{
    public class MockDataProcessor : IDataProcessor
    {
        public List<T>? GetRawData<T>(string contents) where T : RawEmployeeProjectModel
        {
            List<RawEmployeeProjectModel> result = new List<RawEmployeeProjectModel>
            {
                new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-20), ToDate = DateTime.Now.AddDays(-2) },
                new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-1) }
            };

            return result as List<T>;
        }
    }
}