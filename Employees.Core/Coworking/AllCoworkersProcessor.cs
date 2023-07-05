using Employees.Core.Coworking.Interfaces;
using Employees.Core.Employees;
using Employees.Common;

namespace Employees.Core.Coworking
{
    public class AllCoworkersProcessor : IAllCoworkers
    {
        private readonly ICoworkersPreProcessor _preProcessor;
        public AllCoworkersProcessor(ICoworkersPreProcessor preProcessor)
        {
            _preProcessor = preProcessor;
        }

        public List<EmployeesProjectDaysModel> GetAllCoworkers(string data)
        {
            Dictionary<ulong, Dictionary<uint, List<(DateTime, DateTime)>>> groupedByProject = _preProcessor.PreProcess(data);
            List<EmployeesProjectDaysModel> aggregatedDataByEmployeePairs = CalculateDaysByProjectAndPair(groupedByProject);
            return aggregatedDataByEmployeePairs;
        }

        private List<EmployeesProjectDaysModel> CalculateDaysByProjectAndPair(Dictionary<ulong, Dictionary<uint, List<(DateTime, DateTime)>>> data)
        {
            List<EmployeesProjectDaysModel> daysByPairAndProject = new();
            foreach (var employeePair in data)
            {
                foreach (var project in employeePair.Value)
                {                   
                    int count = 0;
                    foreach (var interval in project.Value)
                    {
                        count += (int)(interval.Item2 - interval.Item1).TotalDays + 1;
                    }

                    (uint, uint) employeeIDs = EmployeesKeys.GetEmployeeIDs(employeePair.Key);
                    daysByPairAndProject.Add(
                        new EmployeesProjectDaysModel() { 
                            Employee1ID = employeeIDs.Item1, 
                            Employee2ID = employeeIDs.Item2, 
                            ProjectID = project.Key, 
                            DaysOfCoworking = count }); ;
                }
            }

            return daysByPairAndProject;
        }
    }
}
