using Employees.Core.Interfaces;
using Employees.Core.Employees;

namespace Employees.Infrastructure
{
    public class BasicMockDataProcessor : IDataProcessor
    {
        private List<RawEmployeeProjectModel> testData = new();
        internal SimplePairUseCase Case { get; private set; }
        public BasicMockDataProcessor(SimplePairUseCase useCase) 
        {
            Case = useCase;
            switch (useCase)
            {
                case SimplePairUseCase.TwoEmployeesCommonProjectLeftTopOverlap:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-20), ToDate = DateTime.Now.AddDays(-2) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-1) }
                    };
                    break;
                case SimplePairUseCase.TwoEmployeesCommonProjectRightTopOverlap:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-3) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-20), ToDate = DateTime.Now.AddDays(-5) }
                    };
                    break;
                case SimplePairUseCase.TwoEmployeesCommonProjectCompleteBottomOverlap:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-20), ToDate = DateTime.Now.AddDays(-3) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-5) }
                    };
                    break;
                case SimplePairUseCase.TwoEmployeesCommonProjectCompleteTopOverlap:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-3) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-30), ToDate = DateTime.Now.AddDays(-1) }
                    };
                    break;
                case SimplePairUseCase.TwoEmplyoyeesCommonProjectLeftTopOneDayOverlap:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-3) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-3), ToDate = DateTime.Now.AddDays(-1) }
                    };
                    break;
                case SimplePairUseCase.TwoEmplyoyeesCommonProjectRightTopOneDayOverlap:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-3) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-30), ToDate = DateTime.Now.AddDays(-10) }
                    };
                    break;
                case SimplePairUseCase.EmptyData:
                default:
                    testData = new List<RawEmployeeProjectModel>();
                    break;
            }
        }

        public List<RawEmployeeProjectModel> GetRawData(string contents)
        {
            return testData;
        }
    }

    public enum SimplePairUseCase
    {
        TwoEmployeesCommonProjectLeftTopOverlap,
        TwoEmployeesCommonProjectRightTopOverlap,
        TwoEmployeesCommonProjectCompleteBottomOverlap,
        TwoEmployeesCommonProjectCompleteTopOverlap,
        TwoEmplyoyeesCommonProjectLeftTopOneDayOverlap,
        TwoEmplyoyeesCommonProjectRightTopOneDayOverlap,
        EmptyData
    }
}