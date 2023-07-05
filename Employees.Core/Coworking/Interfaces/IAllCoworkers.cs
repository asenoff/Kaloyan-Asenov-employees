using Employees.Core.Employees;

namespace Employees.Core.Coworking.Interfaces
{
    public interface IAllCoworkers
    {
        public List<EmployeesProjectDaysModel> GetAllCoworkers(string data);
    }
}
