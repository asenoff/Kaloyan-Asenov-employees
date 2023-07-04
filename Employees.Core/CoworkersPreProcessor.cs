using Employees.Core.Employees;
using Employees.Core.Interfaces;
using Employees.Common;

namespace Employees.Core
{
    public class CoworkersPreProcessor
    {
        private IDataProcessor _dataProcessor;

        public CoworkersPreProcessor(IDataProcessor dataProcessor)
        {
            _dataProcessor = dataProcessor;
        }

        public Dictionary<ulong, Dictionary<uint, int>> PreProcess(string data)
        {
            List<RawEmployeeProjectModel> rawData = _dataProcessor.GetRawData<RawEmployeeProjectModel>(data);

            Dictionary<uint, Dictionary<uint, List<(DateTime, DateTime)>>> rawDataGroupedByProject
                = new Dictionary<uint, Dictionary<uint, List<(DateTime, DateTime)>>>();
            GroupDataByProjects(rawData, rawDataGroupedByProject);

            Dictionary<ulong, Dictionary<uint, int>> aggregatedDataByEmployeePairs = new Dictionary<ulong, Dictionary<uint, int>>();
            CalculateDaysCollaboratedByEmployeePairsAndProjects(rawDataGroupedByProject, aggregatedDataByEmployeePairs);

            return aggregatedDataByEmployeePairs;
        }

        private static void CalculateDaysCollaboratedByEmployeePairsAndProjects(Dictionary<uint, Dictionary<uint, List<(DateTime, DateTime)>>> rawDataByProject, Dictionary<ulong, Dictionary<uint, int>> aggregatedDataByEmployees)
        {
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
                            aggregatedDataByEmployees.Add(keyPair, new Dictionary<uint, int>());
                        }

                        int daysCountForPair = GetDaysCollaboratedByEmployeesAndProject(project, employee1ID, employee2ID);
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
        }

        private static int GetDaysCollaboratedByEmployeesAndProject(KeyValuePair<uint, Dictionary<uint, List<(DateTime, DateTime)>>> project, uint employee1ID, uint employee2ID)
        {
            int daysCountForPair = 0;
            List<(DateTime, DateTime)> timeIntervalsEmployee1 = project.Value[employee1ID];
            List<(DateTime, DateTime)> timeIntervalsEmployee2 = project.Value[employee2ID];
            foreach ((DateTime, DateTime) timeInterval in timeIntervalsEmployee1)
            {
                DateTime startDateInterval1 = timeInterval.Item1.Date;
                DateTime endDateInterval1 = timeInterval.Item2.Date;
                foreach ((DateTime, DateTime) innerTimeInterval in timeIntervalsEmployee2)
                {
                    DateTime startDateInterval2 = innerTimeInterval.Item1.Date;
                    DateTime endDateInterval2 = innerTimeInterval.Item2.Date;
                    if (startDateInterval1 == endDateInterval2 || endDateInterval1 == startDateInterval2)
                    {
                        daysCountForPair++;
                    }

                    if (endDateInterval1 > startDateInterval2 && endDateInterval1 < endDateInterval2)
                    {
                        TimeSpan duration;
                        if (startDateInterval1 > startDateInterval2)
                        {
                            duration = endDateInterval1.Subtract(startDateInterval1);
                        }
                        else
                        {
                            duration = endDateInterval1.Subtract(startDateInterval2);
                        }

                        daysCountForPair += (int)duration.TotalDays;
                    }

                    if (startDateInterval1 < endDateInterval2 && endDateInterval2 < endDateInterval1)
                    {
                        TimeSpan duration;
                        if (startDateInterval1 < startDateInterval2)
                        {
                            duration = endDateInterval2.Subtract(startDateInterval2);
                        }
                        else
                        {
                            duration = endDateInterval2.Subtract(startDateInterval1);
                        }

                        daysCountForPair += (int)duration.TotalDays;
                    }
                }
            }

            return daysCountForPair;
        }

        private static void GroupDataByProjects(List<RawEmployeeProjectModel> rawData, Dictionary<uint, Dictionary<uint, List<(DateTime, DateTime)>>> rawDataByProject)
        {
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
        }
    }
}