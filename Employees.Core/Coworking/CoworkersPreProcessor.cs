using Employees.Core.Employees;
using Employees.Core.Interfaces;
using Employees.Common;
using Employees.Core.Coworking.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Employees.Core.Coworking
{
    public class CoworkersPreProcessor  : ICoworkersPreProcessor
    {
        private IDataProcessor _dataProcessor;

        public CoworkersPreProcessor(IDataProcessor dataProcessor)
        {
            _dataProcessor = dataProcessor;
        }

        public Dictionary<ulong, Dictionary<uint, List<(DateTime, DateTime)>>> PreProcess(string data)
        {
            List<RawEmployeeProjectModel> rawData = _dataProcessor.GetRawData(data);
            Dictionary<uint, Dictionary<uint, List<(DateTime, DateTime)>>> rawDataGroupedByProject = GroupDataByProjects(rawData);
            Dictionary<ulong, Dictionary<uint, List<(DateTime, DateTime)>>> intervalsCollaborated = CalculateIntervalsCollaborated(rawDataGroupedByProject);
            return intervalsCollaborated;
        }

        private Dictionary<ulong, Dictionary<uint, List<(DateTime, DateTime)>>> CalculateIntervalsCollaborated(Dictionary<uint, Dictionary<uint, List<(DateTime, DateTime)>>> rawDataByProject)
        {
            var aggregatedDataByEmployees = new Dictionary<ulong, Dictionary<uint, List<(DateTime, DateTime)>>>();
            foreach (var project in rawDataByProject)
            {
                var employees = project.Value.Keys;
                List<uint> orderedEmployees = employees.ToList();
                orderedEmployees.Sort((a, b) => a > b ? 1 : -1);

                for (int i = 0; i < orderedEmployees.Count; i++)
                {
                    uint employee1ID = orderedEmployees[i];
                    for (int j = i + 1; j < orderedEmployees.Count; j++)
                    {
                        uint employee2ID = orderedEmployees[j];
                        ulong keyPair = EmployeesKeys.GetCompositeEmployeesKey(employee1ID, employee2ID);
                        if (!aggregatedDataByEmployees.ContainsKey(keyPair))
                        {
                            aggregatedDataByEmployees.Add(keyPair, new Dictionary<uint, List<(DateTime, DateTime)>>());
                        }

                        List<(DateTime, DateTime)> intervalsForPair = GetIntervalsCollaboratedByEmployeesAndProject(project, employee1ID, employee2ID);
                        if (!aggregatedDataByEmployees[keyPair].ContainsKey(project.Key))
                        {
                            aggregatedDataByEmployees[keyPair].Add(project.Key, intervalsForPair);
                        }
                        else
                        {
                            throw new ArgumentException("duplicated project data");
                        }
                    }
                }
            }

            return aggregatedDataByEmployees;
        }

        private List<(DateTime, DateTime)> GetIntervalsCollaboratedByEmployeesAndProject(KeyValuePair<uint, Dictionary<uint, List<(DateTime, DateTime)>>> project, uint employee1ID, uint employee2ID)
        {
            List<(DateTime, DateTime)> intersections = new();
            List<(DateTime, DateTime)> timeIntervalsEmployee1 = AggregateOverlappingIntervals(project.Value[employee1ID]);
            List<(DateTime, DateTime)> timeIntervalsEmployee2 = AggregateOverlappingIntervals(project.Value[employee2ID]);
            foreach ((DateTime, DateTime) timeInterval in timeIntervalsEmployee1)
            {
                DateTime startDateInterval1 = timeInterval.Item1.Date;
                DateTime endDateInterval1 = timeInterval.Item2.Date;
                foreach ((DateTime, DateTime) innerTimeInterval in timeIntervalsEmployee2)
                {
                    DateTime startDateInterval2 = innerTimeInterval.Item1.Date;
                    DateTime endDateInterval2 = innerTimeInterval.Item2.Date;

                    // getting through different types of intersections
                    DateTime intersectionFromDate, intersectionToDate;
                    // one day overlap
                    if (startDateInterval1 == endDateInterval2)
                    {
                        intersectionFromDate = intersectionToDate = startDateInterval1;
                        intersections.Add((intersectionFromDate, endDateInterval2));
                        continue;
                    }

                    // one day overlap
                    if (endDateInterval1 == startDateInterval2)
                    {
                        intersectionFromDate = intersectionToDate = endDateInterval1;
                        intersections.Add((intersectionFromDate, intersectionToDate));
                        continue;
                    }

                    // overlap second
                    if (endDateInterval1 > startDateInterval2 && endDateInterval1 < endDateInterval2)
                    {
                        if (startDateInterval1 > startDateInterval2)
                        {
                            //consumes second interval
                            intersectionFromDate = startDateInterval1;
                            intersectionToDate = endDateInterval1;
                        }
                        else
                        {
                            //overlaps second interval
                            intersectionFromDate = startDateInterval2;
                            intersectionToDate = endDateInterval1;                            
                        }

                        intersections.Add((intersectionFromDate, intersectionToDate));
                        continue;
                    }

                    if (startDateInterval1 < endDateInterval2 && endDateInterval2 < endDateInterval1)
                    {
                        if (startDateInterval1 < startDateInterval2)
                        {
                            //consumes first interval
                            intersectionFromDate = startDateInterval2;
                            intersectionToDate = endDateInterval2;
                        }
                        else
                        {
                            //overlaps first interval
                            intersectionFromDate = startDateInterval1;
                            intersectionToDate = endDateInterval2;
                        }

                        intersections.Add((intersectionFromDate, intersectionToDate));
                        continue;
                    }
                }
            }

            return intersections;
        }

        private Dictionary<uint, Dictionary<uint, List<(DateTime, DateTime)>>> GroupDataByProjects(List<RawEmployeeProjectModel> rawData)
        {
            Dictionary<uint, Dictionary<uint, List<(DateTime, DateTime)>>> rawDataByProject
                = new();
            foreach (var line in rawData)
            {
                var projectID = line.ProjectID;
                var employeeID = line.EmployeeID;
                if (!rawDataByProject.ContainsKey(projectID))
                {
                    rawDataByProject.Add(line.ProjectID, new Dictionary<uint, List<(DateTime, DateTime)>>());
                }

                if (!rawDataByProject[projectID].ContainsKey(employeeID))
                {
                    rawDataByProject[projectID][employeeID] = new List<(DateTime, DateTime)>();
                }

                rawDataByProject[projectID][employeeID].Add((line.FromDate, line.ToDate));
            }

            return rawDataByProject;
        }

        private List<(DateTime, DateTime)> AggregateOverlappingIntervals(List<(DateTime, DateTime)> intervals)
        {
            if (intervals.Count <= 1)
            {
                return intervals;
            }

            intervals.Sort((a, b) => a.Item1.CompareTo(b.Item1));

            var result = new List<(DateTime, DateTime)> { intervals[0] };
            var currentInterval = result[0];

            for (int i = 1; i < intervals.Count; i++)
            {
                var interval = intervals[i];

                if (interval.Item1 <= currentInterval.Item2)
                {
                    if (interval.Item1 > currentInterval.Item2)
                    {
                        currentInterval.Item2 = interval.Item2;
                    }
                }
                else
                {
                    currentInterval = interval;
                    result.Add(currentInterval);
                }
            }

            return result;
        }
    }
}