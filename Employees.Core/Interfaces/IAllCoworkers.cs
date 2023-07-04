using Employees.Core.Employees;

namespace Employees.Core.Interfaces
{
    public interface IAllCoworkers
    {
        public List<EmployeesProjectDaysModel> GetAllCoworkers(int page, int size);
    }
}
