using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTDSCalculator.BLL
{
    public class SalaryValidator
    {
        public string ValidateSalary(double salary)
        {
            
            if (salary<0)
            {
                return "Given Salary is invalid";
            }
            else
            {
                return null;
            }
        }
    }
}
