using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using EmployeeTDSCalculator.BLL;
using EmployeeTDSCalculator.DAL;

namespace EmployeeTDSCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            EmployeeBO employeeData = new EmployeeBO();
            EmployeeDL employeeTDSData = new EmployeeDL();
            SalaryValidator salaryValidator = new SalaryValidator();
            TDSCalculation tdsCalculation = new TDSCalculation();

            string loopInput = string.Empty;
            List<EmployeeBO> employeeTdsList = new List<EmployeeBO>();

            Console.WriteLine("Welcome Admin to Employee TDS Calculator Application");
            do
            {
                int choice = 0;
                try
                {
                    Console.WriteLine("\nMenu:\nPress 1 to enter new record\n" +
                        "Press 2 to display record");
                    Console.WriteLine("Enter your choice:");

                    choice = int.Parse(Console.ReadLine());
                }
                catch (FormatException fe)
                {
                    Console.WriteLine("Invalid Menu Format..Enter Numbers either 1 or 2");
                }

                switch (choice)
                {
                    case 1:
                        try
                        {
                            bool employeeIdPatternCheck = false;
                            bool employeeIdAlreadyExistsCheck = false;
                            bool employeeIdIsEmptyCheck = false;
                            do
                            {
                                string employeeId = string.Empty;
                                Console.WriteLine("\nEnter Employee Id[Starts with EMP and next 5 will be digits]: ");
                                try
                                {
                                    employeeId = Console.ReadLine();

                                    if (string.IsNullOrEmpty(employeeId) || employeeId == "\u0020")
                                    {
                                        throw new ArgumentNullException("Employee ID can not be empty");
                                    }
                                    else
                                    {
                                        employeeIdIsEmptyCheck = false;
                                    }

                                    if (CheckEmployeeIdFormat(employeeId) == true)
                                    {
                                        employeeIdPatternCheck = false;

                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid Employee Id");
                                        employeeIdPatternCheck = true;
                                    }

                                    if (CheckForDuplicateEmployeeId(employeeId) == true)
                                    {
                                        Console.WriteLine("Employee Id already exists..Try another Id");
                                        employeeIdAlreadyExistsCheck = true;
                                    }
                                    else
                                    {
                                        employeeIdAlreadyExistsCheck = false;
                                    }

                                    employeeData.EmployeeId = employeeId;
                                }
                                catch (FormatException fe)
                                {
                                    Console.WriteLine(fe.Message);
                                    employeeIdPatternCheck = true;
                                }
                                catch (ArgumentNullException ae)
                                {
                                    Console.WriteLine(ae.Message);
                                    employeeIdIsEmptyCheck = true;
                                }
                            }
                            while (employeeIdPatternCheck == true || employeeIdAlreadyExistsCheck == true || employeeIdIsEmptyCheck == true);

                            bool nullEmployeeNameCheck = false;
                            do
                            {
                                try
                                {
                                    Console.WriteLine("Enter Employee Name:");
                                    string employeeName = Console.ReadLine();
                                    if (string.IsNullOrEmpty(employeeName) || employeeName == "\u0020")
                                    {
                                        throw new ArgumentNullException("Employee Name can not be empty");
                                    }
                                    else
                                    {
                                        nullEmployeeNameCheck = false;
                                    }
                                    employeeData.EmployeeName = employeeName;
                                }
                                catch (ArgumentNullException ar)
                                {
                                    Console.WriteLine(ar.Message);
                                    nullEmployeeNameCheck = true;
                                }
                            }
                            while (nullEmployeeNameCheck == true);

                            string ctcValidator = null;
                            bool salaryEmptyCheck = false;
                            do
                            {
                                try
                                {
                                    Console.WriteLine("Enter Employee CTC:");
                                    double salary = Convert.ToDouble(Console.ReadLine());

                                    if (string.IsNullOrEmpty(salary.ToString()) || salary.ToString() == "\u0020")
                                    {
                                        throw new FormatException();
                                    }
                                    else
                                    {
                                        salaryEmptyCheck = false;
                                        ctcValidator = salaryValidator.ValidateSalary(salary);
                                        if (ctcValidator != null)
                                        {
                                            Console.WriteLine(ctcValidator);
                                        }
                                        else
                                        {
                                            employeeData.EmployeeCTC = salary;
                                        }
                                    }
                                }
                                catch (FormatException ar)
                                {
                                    Console.WriteLine("Employee Salary can not be Blank");
                                    salaryEmptyCheck = true;
                                }
                            }
                            while (ctcValidator != null || salaryEmptyCheck == true);

                            employeeData.TdsAmount= tdsCalculation.CalculateTaxAmount(employeeData.EmployeeCTC);

                            bool insertResult = employeeTDSData.AddEmployeeRecords(employeeData);
                            if (insertResult == true)
                            {
                                Console.WriteLine("Employee TDS Records Successfully Inserted");
                                employeeTdsList.Add(employeeData);
                            }
                        }

                        catch
                        {
                            Console.WriteLine("Data Insertion Failed. Either Employee Id already exists or wrong application logic");
                        }
                        break;

                    case 2:
                        Console.WriteLine("\nDisplay All Employees TDS Records");
                        Console.WriteLine("{0,-20}{1,-20}{2,-20}{3}", "Employee Id", "Employee Name", "Employee CTC",
                            "TDS Amount");
                        List<EmployeeBO> displayRecords = employeeTDSData.DisplayAllRecords();
                        if (displayRecords.Count > 0)
                        {
                            foreach (var data in displayRecords)
                            {
                                Console.WriteLine("{0,-20}{1,-20}{2,-20}{3}", data.EmployeeId,
                                    data.EmployeeName, data.EmployeeCTC, data.TdsAmount);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No Employee Information present under EmployeeTDS Table");
                        }
                        break;
                }
                Console.WriteLine("\nPress YES to repeat Menu...Any other key to stop");
                loopInput = Console.ReadLine();
            }
            while (loopInput.Equals("yes", StringComparison.InvariantCultureIgnoreCase));

            Console.WriteLine("\nThank you for using the application. Have a nice day");

        }


        public static bool CheckEmployeeIdFormat(string employeeId)
        {
           //Fill your code here
        }

        public static bool CheckForDuplicateEmployeeId(string employeeId)
        {
            //Fill your code here
        }
    }
}
