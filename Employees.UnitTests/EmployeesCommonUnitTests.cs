using Employees.Common;

namespace Employees.UnitTests
{
    [TestClass]
    public class EmployeesCommonUnitTests
    {
        [TestMethod]
        public void EmployeesCommon_EncodeAndDecodeCoworkersIDs_Coworker1()
        {
            uint coworker1ID = 5;
            uint coworker2ID = 65;
            ulong compositeKey = EmployeesKeys.GetCompositeEmployeesKey(coworker2ID, coworker1ID);
            (uint, uint) ids = EmployeesKeys.GetEmployeeIDs(compositeKey);
            Assert.AreEqual(coworker1ID, ids.Item1);
        }

        [TestMethod]
        public void EmployeesCommon_EncodeAndDecodeCoworkersIDs_Coworker2()
        {
            uint coworker1ID = 5;
            uint coworker2ID = 65;
            ulong compositeKey = EmployeesKeys.GetCompositeEmployeesKey(coworker2ID, coworker1ID);
            (uint, uint) ids = EmployeesKeys.GetEmployeeIDs(compositeKey);
            Assert.AreEqual(coworker2ID, ids.Item2);
        }
    }
}