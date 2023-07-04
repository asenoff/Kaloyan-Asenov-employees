using Employees.Core.Employees;
using Employees.Core.Interfaces;

namespace Employees.Core
{
    internal class AllCoworkersProcessor : IAllCoworkers
    {
        public AllCoworkersProcessor(CoworkersPreProcessor processor)
        {

        }

        public List<EmployeesProjectDaysModel> GetAllCoworkers(int page, int size)
        {
            throw new NotImplementedException();
        }
    }
}
