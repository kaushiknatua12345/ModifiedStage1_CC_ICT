using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace OfficeCommuteService
{
    public class Program
    {
        OfficeCommuteBO commuteDataOperations = new OfficeCommuteBO();       
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome Admin to Office Commute System");
            Program progarm = new Program();
            //Call the Menu method here
            Console.WriteLine("\nThank you for using the app. Have a nice day");
        }

        public void Menu()
        {
            OfficeCommuteDAO commuteData = new OfficeCommuteDAO();
            OfficeCommuteBO commuteOperations = new OfficeCommuteBO();
            string loopInput = string.Empty;            
            do
            {
                Console.WriteLine("\nMenu:\nEnter 1 to Add Employee Commute Records\n" +
                    "Enter 2 to Display All Employee Commute Records\n" +
                    "Enter 3 to Display All Employee Commute Records Who Are Paying Lowest Fare");
                int choice = 0;
                try
                {
                    choice = int.Parse(Console.ReadLine());
                }
                catch(FormatException fe)
                {
                    Console.WriteLine("Menu Entry is not in correct format. Enter numbers only");
                }
                switch(choice)
                {
                    case 1:
                     bool employeeIdCheckFormatLoopInput = false;
                     bool employeeIdCheckDuplicateLoopInput = false;
                        string employeeId = string.Empty;
                        do
                        {
                            Console.WriteLine("Enter Employee Id:");
                            employeeId = Console.ReadLine();
                            if (/*Call the CheckEmployeeIdFormat method here and check if the return is false*/)
                            {
                                Console.WriteLine("Wrong Employee Id Entered..Try Again");
                                employeeIdCheckFormatLoopInput = true;
                                employeeIdCheckDuplicateLoopInput = false;                                
                            }
                            else if(/*Call the CheckForDuplicateEmployeeId method here and check if the return is false*/)
                            {                         
                                 Console.WriteLine("Same Employee Id already present in database..Try With another ID");
                                 employeeIdCheckDuplicateLoopInput = true;
                                employeeIdCheckFormatLoopInput = false;
                               
                            }
                            else
                            {
                                commuteData.EmployeeId = employeeId;
                                employeeIdCheckDuplicateLoopInput = false;
                                employeeIdCheckFormatLoopInput = false;
                            }
                        }
                        while (employeeIdCheckFormatLoopInput == true || employeeIdCheckDuplicateLoopInput==true);

                        Console.WriteLine("Enter Employee Name:");
                        commuteData.EmployeeName = Console.ReadLine();

                        bool employeeTypeCheckLoop = false;
                        bool employeeTypeWithEmployeeIdMatchLoop = false;
                        string employeeType = string.Empty;
                        do
                        {
                            Console.WriteLine("Enter Employee Type(External or Internal):");
                            employeeType = Console.ReadLine().ToUpper();

                            if (/*Call the CheckEmployeeTypeInput method here and check if the return is false*/)
                            {
                                Console.WriteLine("Invalid Employee Type..Try Again");
                                employeeTypeCheckLoop = true;
                                employeeTypeWithEmployeeIdMatchLoop = false;
                            }
                            else if
                           (/*Call the CheckIfEmployeeTypeMatchesWithEmployeeIdType method here and check if the return is false*/)
                            {
                                Console.WriteLine("Employee Type does not match with Employee Id type..Try Again");
                                employeeTypeWithEmployeeIdMatchLoop = true;
                                employeeTypeCheckLoop = false;
                            }
                            else
                            {
                                commuteData.EmployeeType = employeeType;
                                employeeTypeCheckLoop = false;
                                employeeTypeWithEmployeeIdMatchLoop = false;
                            }
                        }
                        while (employeeTypeCheckLoop == true || employeeTypeWithEmployeeIdMatchLoop== true);

                        float travelDistance = 0.0F;
                        bool travelDistanceCheckLoop = false;
                        do
                        {
                            Console.WriteLine("Enter Travel Distance:");
                            travelDistance = float.Parse(Console.ReadLine());
                            if (/*Call the CheckTravelDistance method here and check if the return is false*/)
                            {
                                Console.WriteLine("Invalid Travel Distance..Try Again");
                                travelDistanceCheckLoop = true;
                            }
                            else
                            {
                                commuteData.TravelDistance = travelDistance;
                                travelDistanceCheckLoop = false;
                            }
                        }
                        while (travelDistanceCheckLoop == true);

                        double commuteCharge = /* Call the FareCalculation method here to calculate fare and store
			it under the commuteCharge Variable */
                        commuteData.CommuteCharge = commuteCharge;

                        int insertResult = /* Call the InsertRecords methods here to add commute record into the database */
                        if(insertResult==1)
                        {
                            Console.WriteLine("Data Insertion Successful");
                        }
                        else
                        {
                            Console.WriteLine("Data Insertion Failed");
                        }
                        break;

                    case 2:
                        Console.WriteLine("\nDisplay All Employees Commute Infomation:");
                        Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}{4}",
                            "Employee Id", "Name", "Employee Type", "Distance", "Commute Fare");
                        
			/*Display all commute records from the returned List with 15 spaces between each record*/
                        
			break;

                    case 3:
                        Console.WriteLine("\nDisplay Employees With Lowest Commute Fare:");
                        Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}{4}",
                            "Employee Id", "Name", "Employee Type", "Distance", "Commute Fare");
                        
			/*Display all lowest fare commute records from the returned List with 15 spaces between each record*/
                        
			break;                       
                   
                }
                Console.WriteLine("\nPress yes to continue your work..Any other key to terminate operation");
                loopInput = Console.ReadLine();
                
            }
            while (loopInput.Equals("yes", StringComparison.InvariantCultureIgnoreCase));
            }

        public bool CheckEmployeeIdFormat(string employeeId)
        {
            /*Check Employee Id format according to Business Rule:1 specified in Requirement Document. If validation
	      fails return false; else return true */
        }

        public bool CheckForDuplicateEmployeeId(string employeeId)
        {
            /* Check if same Employee Id already exists in the table. If Id already exists in table return false;
	       else return true (Business Rule:2)*/
        }

        public bool CheckEmployeeTypeInput(string employeeType)
        {
            /* Check Employee Type according to Business Rule:3 specified in Requirement Document. If validation
	      fails return false; else return true */
        }

        public bool CheckIfEmployeeTypeMatchesWithEmployeeIdType(string employeeType,string employeeId)
        {
            /* Check if first 3 letters of Employee Type matches with first 3 letters of Employee Id. If data
	       matches return true; else return false (Business Rule:4). Note: The match checking should be case insensitive */
        }

        public bool CheckTravelDistance(float travelDistance)
        {
            /* Check Travel Distance according to Business Rule:5 specified in Requirement Document. If validation
	      fails return false; else return true */
        }

        public double FareCalculation(string EmployeeType,float travelDistance)
        {
            /* Fill your code here to calculate and return the Fare according to the Fare Calcuation Formula
	       given in Requirement Document */
        }


    }
}
