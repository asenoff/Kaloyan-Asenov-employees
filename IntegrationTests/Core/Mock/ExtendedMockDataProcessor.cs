using Employees.Core.Interfaces;
using Employees.Core.Employees;

namespace Employees.Infrastructure
{
    public class ExtendedMockDataProcessor : IDataProcessor
    {
        private List<RawEmployeeProjectModel> testData = new();
        internal MultiDataUseCase Case { get; private set; }
        public ExtendedMockDataProcessor(MultiDataUseCase useCase) 
        {
            Case = useCase;
            switch (useCase)
            {
                case MultiDataUseCase.TwoEmployeesTwoCommonProjectsSingleIntervals:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-20), ToDate = DateTime.Now.AddDays(-2) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-1) },
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 2, FromDate = DateTime.Now.AddDays(-30), ToDate = DateTime.Now.AddDays(-23) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 2, FromDate = DateTime.Now.AddDays(-40), ToDate = DateTime.Now.AddDays(-25) }
                    };
                    break;

                case MultiDataUseCase.TwoEmployeesTwoCommonProjectsNoIntervalMatch:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-20), ToDate = DateTime.Now.AddDays(-2) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-40), ToDate = DateTime.Now.AddDays(-30) },
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 2, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now.AddDays(-3) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 2, FromDate = DateTime.Now.AddDays(-2), ToDate = DateTime.Now }
                    };
                    break;
                case MultiDataUseCase.TwoEmployeesOneProjectsMultiIntervals:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-25), ToDate = DateTime.Now.AddDays(-20) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-2), ToDate = DateTime.Now.AddDays(-1) },
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-30), ToDate = DateTime.Now.AddDays(-25) },
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now }
                    };
                    break;
                case MultiDataUseCase.ThreeEmployeesFiveProjectsMultiData:
                    testData = new List<RawEmployeeProjectModel>
                    {
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-30), ToDate = DateTime.Now },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 1, FromDate = DateTime.Now.AddDays(-60), ToDate = DateTime.Now.AddDays(-50) },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 2, FromDate = DateTime.Now.AddDays(-20), ToDate = DateTime.Now },
                        new RawEmployeeProjectModel { EmployeeID = 2, ProjectID = 5, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now },
                        new RawEmployeeProjectModel { EmployeeID = 1, ProjectID = 1, FromDate = DateTime.Now.AddDays(-5), ToDate = DateTime.Now },
                        new RawEmployeeProjectModel { EmployeeID = 3, ProjectID = 1, FromDate = DateTime.Now.AddDays(-30), ToDate = DateTime.Now.AddDays(-20) },
                        new RawEmployeeProjectModel { EmployeeID = 3, ProjectID = 1, FromDate = DateTime.Now.AddDays(-10), ToDate = DateTime.Now }
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

        public enum MultiDataUseCase
        {
            TwoEmployeesTwoCommonProjectsSingleIntervals,
            TwoEmployeesTwoCommonProjectsNoIntervalMatch,
            TwoEmployeesOneProjectsMultiIntervals,
        }
    }
}