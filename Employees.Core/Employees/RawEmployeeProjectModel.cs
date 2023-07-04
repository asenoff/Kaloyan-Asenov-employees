namespace Employees.Core.Employees
{
    public class RawEmployeeProjectModel
    {
        public uint EmployeeID { get; set; }

        public uint ProjectID { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
