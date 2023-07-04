﻿namespace Employees.Common
{
    public class EmployeesKeys
    {
        public static ulong GetCompositeEmployeesKey(uint employeeKey1, uint employeeKey2)
        {
            if(employeeKey1 == employeeKey2)
            {
                throw new ArgumentException("employee keys should differ");
            }

            if (employeeKey1 == 0 || employeeKey2 == 0)
            {
                throw new ArgumentException("employee keys should both be non-zero");
            }

            var keyLength = sizeof(uint);
            ulong result;
            if(employeeKey1 < employeeKey2)
            {
                result = (employeeKey1 << keyLength) & employeeKey2;
                return result;
            }
            else
            {
                result = (employeeKey2 << keyLength) & employeeKey1;
                return result;
            }
        }

        public static (uint, uint) GetEmployeeIDs(ulong compositeKey)
        {
            var idLength = sizeof(uint);
            var id1 = (uint) (compositeKey >> idLength);
            var id2 = (uint) (compositeKey & uint.MaxValue);

            return (id1, id2);
        }
    }
}