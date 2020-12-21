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
            progarm.Menu();
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
                            if (CheckEmployeeIdFormat(employeeId)==false)
                            {
                                Console.WriteLine("Wrong Employee Id Entered..Try Again");
                                employeeIdCheckFormatLoopInput = true;
                                employeeIdCheckDuplicateLoopInput = false;                                
                            }
                            else if(CheckForDuplicateEmployeeId(employeeId)==false)
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

                            if (CheckEmployeeTypeInput(employeeType) == false)
                            {
                                Console.WriteLine("Invalid Employee Type..Try Again");
                                employeeTypeCheckLoop = true;
                                employeeTypeWithEmployeeIdMatchLoop = false;
                            }
                            else if
                           (CheckIfEmployeeTypeMatchesWithEmployeeIdType(employeeId,employeeType)==false)
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
                            if (CheckTravelDistance(travelDistance) == false)
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

                        double commuteCharge = FareCalculation(employeeType, travelDistance);
                        commuteData.CommuteCharge = commuteCharge;

                        int insertResult = commuteOperations.InsertRecords(commuteData);
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
                        IList<OfficeCommuteDAO> commuteList = commuteOperations.DisplayAllRecords();
                        foreach(var commuteRecord in commuteList)
                        {
                            Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}{4}",
                                commuteRecord.EmployeeId, commuteRecord.EmployeeName,
                                commuteRecord.EmployeeType, commuteRecord.TravelDistance,
                                commuteRecord.CommuteCharge.ToString("0.00"));
                        }
                        break;

                    case 3:
                        Console.WriteLine("\nDisplay Employees With Lowest Commute Fare:");
                        Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}{4}",
                            "Employee Id", "Name", "Employee Type", "Distance", "Commute Fare");
                        IList<OfficeCommuteDAO> lowestCommuteList = commuteOperations.DisplayRecordsByLowestCommuteFare();
                        foreach (var commuteRecord in lowestCommuteList)
                        {
                            Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}{4}",
                                commuteRecord.EmployeeId, commuteRecord.EmployeeName,
                                commuteRecord.EmployeeType, commuteRecord.TravelDistance,
                                commuteRecord.CommuteCharge.ToString("0.00"));
                        }
                        break;                       
                   
                }
                Console.WriteLine("\nPress yes to continue your work..Any other key to terminate operation");
                loopInput = Console.ReadLine();
                
            }
            while (loopInput.Equals("yes", StringComparison.InvariantCultureIgnoreCase));
            }

        public bool CheckEmployeeIdFormat(string employeeId)
        {
            if (Regex.IsMatch(employeeId, @"^[INT|EXT]{3}[0-9]{4}$") == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckForDuplicateEmployeeId(string employeeId)
        {
            IList<OfficeCommuteDAO> commuteDataList = commuteDataOperations.DisplayAllRecords();
            var data = commuteDataList.Where(x => x.EmployeeId.Equals(employeeId, StringComparison.InvariantCultureIgnoreCase));
            if (data.Count()>0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckEmployeeTypeInput(string employeeType)
        {
            if((!employeeType.Equals("Internal",StringComparison.InvariantCultureIgnoreCase))
                && (!employeeType.Equals("External", StringComparison.InvariantCultureIgnoreCase)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckIfEmployeeTypeMatchesWithEmployeeIdType(string employeeType,string employeeId)
        {
            if(employeeId.Substring(0, 3).Equals
                (employeeType.Substring(0,3),StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckTravelDistance(float travelDistance)
        {
            if (travelDistance >= 2 && travelDistance <= 30)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public double FareCalculation(string EmployeeType,float travelDistance)
        {
            double commuteFare = 0;

            if (EmployeeType.Equals("Internal", StringComparison.InvariantCultureIgnoreCase))
            {
                if (travelDistance <= 10)
                {
                    commuteFare = travelDistance * 70;
                }
                else if (travelDistance > 10 && travelDistance <= 20)
                {
                    commuteFare = travelDistance * 90;
                }
                else
                {
                    commuteFare = travelDistance * 110;
                }

            }
            if (EmployeeType.Equals("External", StringComparison.InvariantCultureIgnoreCase))
            {
                if (travelDistance <= 10)
                {
                    commuteFare = travelDistance * 110;
                }
                else if (travelDistance > 10 && travelDistance <= 20)
                {
                    commuteFare = travelDistance * 130;
                }
                else
                {
                    travelDistance = travelDistance * 150;
                }
            }
            return commuteFare;
        }


    }
}
