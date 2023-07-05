using Employees.Core.Coworking.Interfaces;
using Employees.Common;
using Employees.Core.Employees;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            List<TopEmployeePairModel> daysByPair = new List<TopEmployeePairModel>();
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
            Dictionary<ulong, List<(DateTime, DateTime)>> intersectionsByPair = new Dictionary<ulong, List<(DateTime, DateTime)>>();
            foreach (var employeePair in data)
            {
                List <(DateTime, DateTime)> referenceIntervals = employeePair.Value.ElementAt(0).Value;
                uint referenceProjectID = employeePair.Value.ElementAt(0).Key;
                foreach (var projectIntervals in employeePair.Value)
                {
                    if (projectIntervals.Key == referenceProjectID)
                    {
                        continue;
                    }

                    List<(DateTime, DateTime)> intersections = new List<(DateTime, DateTime)> ();
                    foreach (var interval1 in referenceIntervals)
                    {
                        foreach (var interval2 in projectIntervals.Value)
                        {
                            if (interval1.Item1 <= interval2.Item2 && interval2.Item1 <= interval1.Item2)
                            {
                                var intersection = (
                                    interval1.Item1 > interval2.Item1 ? interval1.Item1 : interval2.Item1,
                                    interval1.Item2 < interval2.Item2 ? interval1.Item2 : interval2.Item2
                                );

                                intersections.Add(intersection);
                            }
                        }
                    }

                    referenceIntervals = intersections;
                }

                intersectionsByPair.Add(employeePair.Key, referenceIntervals);
            }

            return intersectionsByPair;
        }
    }
}
