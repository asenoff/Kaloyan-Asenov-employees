using Employees.Core.Employees;
using Employees.Core.Interfaces;

namespace Employees.Infrastructure
{
    public class SVCDataProcessor : IDataProcessor
    {
        List<RawEmployeeProjectModel> IDataProcessor.GetRawData(string contents)
        {
            throw new NotImplementedException();
        }
    }
}