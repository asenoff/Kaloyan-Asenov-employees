using Employees.Core.Employees;

namespace Employees.Core.Interfaces
{
    public interface IDataProcessor
    {
        // TODO remove where
        public List<RawEmployeeProjectModel> GetRawData(string contents);
    }
}
