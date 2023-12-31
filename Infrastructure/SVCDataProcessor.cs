﻿using Employees.Core.Employees;
using Employees.Core.Interfaces;
using CsvHelper;
using System.Globalization;

namespace Employees.Infrastructure
{
    public class CSVDataProcessor : IDataProcessor
    {
        private readonly string[] DATE_FORMATS = {
            "yyyy-MM-dd",
            "dd/MM/yyyy",
            "yyyy/MM/dd",
            "dd-MM-yyyy",
            "yyyy/dd/MM",
            "dd.MM.yyyy",
            "yyyy.MM.dd",
            "dd/MM/yy",
            "yy/MM/dd",
            "MMMM dd, yyyy",
            "MMM dd, yyyy",
            "dd MMMM yyyy",
            "dd MMM yyyy",
            "MMMM dd, yy",
            "MMM dd, yy",
            "dd MMMM yy",
            "dd MMM yy",
            "MMMM, yyyy",
            "MMM, yyyy",
            "yyyy MMMM",
            "yyyy MMM",
        };

        private const string NULL_PLACEHOLDER = "NULL";

        public List<RawEmployeeProjectModel> GetRawData(string contents)
        {
            List<RawEmployeeProjectModel> records = new List<RawEmployeeProjectModel>();
            using (TextReader reader = new StringReader(contents))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    string fromDateStr = csv.GetField("DateFrom")?.Trim() ?? "";
                    string toDateStr = csv.GetField("DateTo")?.Trim() ?? "";

                    DateTime fromDate;
                    if (!DateTime.TryParseExact(fromDateStr, DATE_FORMATS, CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDate))
                    {
                        throw new ArgumentException($"unknown date format in DateFrom in CSV {fromDateStr}");
                    }

                    DateTime toDate;
                    if(toDateStr.ToUpper() == NULL_PLACEHOLDER)
                    {
                        toDate = DateTime.Now;
                    }
                    else
                    {
                        if(!DateTime.TryParseExact(toDateStr, DATE_FORMATS, CultureInfo.InvariantCulture, DateTimeStyles.None, out toDate))
                        {
                            throw new ArgumentException($"unknown date format in DateTo in CSV {toDateStr}");
                        }
                    }

                    if(fromDate > toDate)
                    {
                        throw new ArgumentException($"start date ${fromDateStr} should be before end date ${toDateStr}");
                    }

                    var record = new RawEmployeeProjectModel
                    {
                        EmployeeID = csv.GetField<uint>("EmpID"),
                        ProjectID = csv.GetField<uint>("ProjectID"),
                        FromDate = fromDate,
                        ToDate = toDate
                    };

                    records.Add(record);
                }
            }

            return records;
        }
    }
}