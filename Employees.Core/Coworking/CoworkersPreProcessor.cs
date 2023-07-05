using Employees.Core.Employees;
using Employees.Core.Interfaces;
using Employees.Common;
using Employees.Core.Coworking.Interfaces;

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
            List<RawEmployeeProjectModel> rawData = _dataProcessor.GetRawData<RawEmployeeProjectModel>(data);
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

                        List<(DateTime, DateTime)> daysCountForPair = GetIntervalsCollaboratedByEmployeesAndProject(project, employee1ID, employee2ID);
                        if (!aggregatedDataByEmployees[keyPair].ContainsKey(project.Key))
                        {
                            aggregatedDataByEmployees[keyPair].Add(project.Key, daysCountForPair);
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
            List<(DateTime, DateTime)> intersections = new List<(DateTime, DateTime)>();
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
                    if (startDateInterval1 <= endDateInterval2 && startDateInterval2 <= startDateInterval1)
                    {
                        DateTime fromDate = DateTime.Compare(startDateInterval1, startDateInterval2) > 0 ? startDateInterval1 : startDateInterval2;
                        DateTime toDate = DateTime.Compare(endDateInterval1, endDateInterval2) < 0 ? endDateInterval1 : endDateInterval2;

                        intersections.Add((fromDate, toDate));
                    }
                }
            }

            return intersections;
        }

        private Dictionary<uint, Dictionary<uint, List<(DateTime, DateTime)>>> GroupDataByProjects(List<RawEmployeeProjectModel> rawData)
        {
            Dictionary<uint, Dictionary<uint, List<(DateTime, DateTime)>>> rawDataByProject
                = new Dictionary<uint, Dictionary<uint, List<(DateTime, DateTime)>>>();
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