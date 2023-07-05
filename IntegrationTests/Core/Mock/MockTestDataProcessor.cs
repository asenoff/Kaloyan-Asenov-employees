using Employees.Core.Interfaces;
using Employees.Core.Employees;

namespace Employees.Infrastructure
{
    public class MockTestDataProcessor : IDataProcessor
    {
        private List<RawEmployeeProjectModel> testData = new();
        internal UseCase Case { get; private set; }
        public MockTestDataProcessor(UseCase useCase) 
        {
            Case = useCase;
            switch (useCase)
            {
                case UseCase.TwoEmployeesCommonProjectLeftTopOverlap:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-20), ToDate = DateTime.Now.AddDays(-2) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-1) }
                    };
                    break;
                case UseCase.TwoEmployeesCommonProjectRightTopOverlap:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-3) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-20), ToDate = DateTime.Now.AddDays(-5) }
                    };
                    break;
                case UseCase.TwoEmployeesCommonProjectCompleteBottomOverlap:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-20), ToDate = DateTime.Now.AddDays(-3) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-5) }
                    };
                    break;
                case UseCase.TwoEmployeesCommonProjectCompleteTopOverlap:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-3) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-30), ToDate = DateTime.Now.AddDays(-1) }
                    };
                    break;
                case UseCase.TwoEmplyoyeesCommonProjectLeftTopOneDayOverlap:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-3) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-3), ToDate = DateTime.Now.AddDays(-1) }
                    };
                    break;
                case UseCase.TwoEmplyoyeesCommonProjectRightTopOneDayOverlap:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-3) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-30), ToDate = DateTime.Now.AddDays(-10) }
                    };
                    break;
                default:
                    testData = new List<RawEmployeeProjectModel>();
                    break;
            }
        }

        public List<RawEmployeeProjectModel> GetRawData(string contents)
        {
            return testData;
        }

        public enum UseCase
        {
            TwoEmployeesCommonProjectLeftTopOverlap,
            TwoEmployeesCommonProjectRightTopOverlap,
            TwoEmployeesCommonProjectCompleteBottomOverlap,
            TwoEmployeesCommonProjectCompleteTopOverlap,
            TwoEmplyoyeesCommonProjectLeftTopOneDayOverlap,
            TwoEmplyoyeesCommonProjectRightTopOneDayOverlap
        }
    }
}