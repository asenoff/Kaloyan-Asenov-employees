using Employees.Core.Employees;

namespace Employees.Core.Interfaces
{
    public interface IDataProcessor
    {
        // TODO remove where
        public List<T> GetRawData<T>(string contents) where T : RawEmployeeProjectModel;
    }
}
