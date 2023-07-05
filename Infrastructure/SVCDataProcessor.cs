using Employees.Core.Interfaces;

namespace Employees.Infrastructure
{
    public class SVCDataProcessor : IDataProcessor
    {
        List<T> IDataProcessor.GetRawData<T>(string contents)
        {
            throw new NotImplementedException();
        }
    }
}