using Employees.Core.Coworking.Interfaces;
using Employees.Common;
using Employees.Core.Employees;

namespace Employees.Core.Coworking
{
    public class TopCoworkersProcessor : ITopCoworkers
    {
        private readonly ICoworkersPreProcessor _preProcessor;
        public TopCoworkersProcessor(ICoworkersPreProcessor preProcessor)
        {
            _preProcessor = preProcessor;
        }

        public List<TopEmployeePairModel> GetTopCoworkers(string data)
        {
            Dictionary<ulong, Dictionary<uint, List<(DateTime, DateTime)>>> groupedByProject = _preProcessor.PreProcess(data);
            Dictionary<ulong, List<(DateTime, DateTime)>> intervalsByEmployeePair = CalculateIntersectionsByEmployeePair(groupedByProject);
            List<TopEmployeePairModel> topEmployees = GetTopEmployees(intervalsByEmployeePair);
            return topEmployees;
        }

        private List<TopEmployeePairModel> GetTopEmployees(Dictionary<ulong, List<(DateTime, DateTime)>> intervalsByEmployeePair)
        {
            List<TopEmployeePairModel> daysByPair = new();
            var highestScore = 0;
            foreach (var employeePair in intervalsByEmployeePair)
            {
                int count = 0;
                foreach (var interval in employeePair.Value)
                {
                    count += (int)(interval.Item2 - interval.Item1).TotalDays + 1;
                }

                if(count > highestScore)
                {
                    highestScore = count;
                }

                (uint, uint) employeeIDs = EmployeesKeys.GetEmployeeIDs(employeePair.Key);
                daysByPair.Add(
                    new TopEmployeePairModel()
                    {
                        Employee1ID = employeeIDs.Item1,
                        Employee2ID = employeeIDs.Item2,
                        DaysCollaborated = count
                    }); ;
            }


            return daysByPair.Where(p => p.DaysCollaborated == highestScore).ToList();
        }

        private Dictionary<ulong, List<(DateTime, DateTime)>> CalculateIntersectionsByEmployeePair(Dictionary<ulong, Dictionary<uint, List<(DateTime, DateTime)>>> data)
        {
            Dictionary<ulong, List<(DateTime, DateTime)>> combinedByPair = new();
            foreach (var employeePair in data)
            {
                List<(DateTime, DateTime)> referenceIntervals = employeePair.Value.ElementAt(0).Value;
                uint referenceProjectID = employeePair.Value.ElementAt(0).Key;
                foreach (var projectIntervals in employeePair.Value)
                {
                    if (projectIntervals.Key == referenceProjectID)
                    {
                        continue;
                    }

                    List<(DateTime, DateTime)> intersections = GetCombinedIntervals(referenceIntervals, projectIntervals.Value);
                    referenceIntervals = intersections;
                }

                combinedByPair.Add(employeePair.Key, referenceIntervals);
            }

            return combinedByPair;
        }

        public static List<(DateTime, DateTime)> GetCombinedIntervals(List<(DateTime, DateTime)> intervals1, List<(DateTime, DateTime)> intervals2)
        {
            List<(DateTime, DateTime)> combinedIntervals = new List<(DateTime, DateTime)>();
            if(intervals1.Count == 0 && intervals2.Count == 0)
            {
                return combinedIntervals;
            }

            combinedIntervals.AddRange(intervals1);
            combinedIntervals.AddRange(intervals2);
            combinedIntervals.Sort((x, y) => x.Item1.CompareTo(y.Item1));

            List<(DateTime, DateTime)> combinedResult = new List<(DateTime, DateTime)>();

            (DateTime, DateTime) currentInterval = combinedIntervals[0];
            for (int i = 1; i < combinedIntervals.Count; i++)
            {
                (DateTime, DateTime) interval = combinedIntervals[i];

                if (interval.Item1 <= currentInterval.Item2)
                {
                    currentInterval = (currentInterval.Item1, interval.Item2);
                }
                else
                {
                    combinedResult.Add(currentInterval);
                    currentInterval = interval;
                }
            }

            combinedResult.Add(currentInterval);
            return combinedResult;
        }
    }
}
