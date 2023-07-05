using Employees.Core.Employees;

namespace Employees.Core.Coworking.Interfaces
{
    public interface ITopCoworkers
    {
        public List<TopEmployeePairModel> GetTopCoworkers(string data);
    }
}
