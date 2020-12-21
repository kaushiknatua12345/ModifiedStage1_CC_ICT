using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EmployeeTDSCalculator.BLL
{
    public class EmployeeBO
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public double EmployeeCTC { get; set; }
        public double TdsAmount { get; set; }

        public EmployeeBO()
        {

        }

        public EmployeeBO(string employeeId, string employeeName, double employeeCTC, double tdsAmount)
        {
            EmployeeId = employeeId;
            EmployeeName = employeeName;
            EmployeeCTC = employeeCTC;
            TdsAmount = tdsAmount;
        }

       
    }
}
