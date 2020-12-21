using EmployeeTDSCalculator.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Data.SqlClient;
using System.Reflection;
using System.Globalization;
using System.Configuration;
using System.Transactions;
using NUnit.Framework.Interfaces;
using System.Text.RegularExpressions;
using EmployeeTDSCalculator.BLL;
using EmployeeTDSCalculator.Common;
using System.Data;

namespace EmployeeTDSCalculator.Tests
{

    public class RollbackAttribute : Attribute, ITestAction
    {
        private TransactionScope transaction;

        public void BeforeTest(ITest test)
        {
            transaction = new TransactionScope();
        }
        public void AfterTest(ITest test)
        {
            transaction.Dispose();
        }
        public ActionTargets Targets
        {
            get { return ActionTargets.Test; }
        }
    }


    [TestFixture]
    public class EmployeeTDSDataTests
    {
        Assembly assembly;
        Type clsEmployeeTDSData;
        Type clsSalaryValidator;
        Type clsEmployeeTDSInfo;
        Type clsTDSCalculation;
        DBHandler DBHandler;
        EmployeeDL EmployeeTDSData;

        [SetUp]
        public void Setup()
        {
            DBHandler = new DBHandler();
            EmployeeTDSData = new EmployeeDL();
            assembly = Assembly.Load("EmployeeTDSCalculator");
            clsEmployeeTDSData = assembly.GetType("EmployeeTDSCalculator.DAL.EmployeeDL");
            clsSalaryValidator = assembly.GetType("EmployeeTDSCalculator.BLL.SalaryValidator");
            clsTDSCalculation = assembly.GetType("EmployeeTDSCalculator.BLL.TDSCalculation");
            clsEmployeeTDSInfo = assembly.GetType("EmployeeTDSCalculator.BLL.EmployeeBO");
        }

        [TestCase]
        public void TestBasicChecks()
        {
            Assert.IsNotNull(clsSalaryValidator, "Class SalaryValidator NOT implemented OR check spelling");
            Assert.IsNotNull(clsTDSCalculation, "Class TDSCalculation NOT implemented OR check spelling");
            Assert.IsNotNull(clsEmployeeTDSInfo, "Class EmployeeDL NOT implemented OR check spelling");
        }

        [TestCase]
        public void AddEmployeeRecords_WhenNoSuchMethodFound_WarnUser()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            if (clsEmployeeTDSData != null)
            {
                MethodInfo testMethod = clsEmployeeTDSData.GetMethod("AddEmployeeRecords", allBindings);

                Assert.IsNotNull(testMethod, "Method AddEmployeeRecords NOT implemented OR check spelling");
            }
            else
            {
                Assert.Fail("No class with the name 'EmployeeDL' is implemented OR Did you change the class name");
            }
        }

        [TestCase]
        public void CalculateTaxAmount_WhenNoSuchMethodFound_WarnUser()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            if (clsTDSCalculation != null)
            {
                MethodInfo testMethod = clsTDSCalculation.GetMethod("CalculateTaxAmount", allBindings);

                Assert.IsNotNull(testMethod, "Method CalculateTDS NOT implemented OR check spelling");
            }
            else
            {
                Assert.Fail("No class with the name 'TDSCalculation' is implemented OR Did you change the class name");
            }
        }

        [TestCase]
        public void DisplayAllRecords_WhenNoSuchMethodFound_WarnUser()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            if (clsEmployeeTDSData != null)
            {
                MethodInfo testMethod = clsEmployeeTDSData.GetMethod("DisplayAllRecords", allBindings);

                Assert.IsNotNull(testMethod, "Method DisplayAllRecords NOT implemented OR check spelling");
            }
            else
            {
                Assert.Fail("No class with the name 'EmployeeDL' is implemented OR Did you change the class name");
            }
        }

        [TestCase]
        public void ValidateSalary_WhenNoSuchMethodFound_WarnUser()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            if (clsSalaryValidator != null)
            {
                MethodInfo testMethod = clsSalaryValidator.GetMethod("ValidateSalary", allBindings);

                Assert.IsNotNull(testMethod, "Method ValidateSalary NOT implemented OR check spelling");
            }
            else
            {
                Assert.Fail("No class with the name 'SalaryValidator' is implemented OR Did you change the class name");
            }
        }

        [TestCase]
        [Category("Database")]
        public void GetConnectionStringFromAppConfig()
        {
            string expectedConnString = "data source=localhost;initial catalog=TDSCalculatorDb;integrated security=true;".Trim();
            string actualConnString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;

            Assert.AreEqual(expectedConnString, actualConnString, "Verify the connection string parameters");
        }

        [TestCase]
        [Category("Database")]
        public void ConnectAndDisconnectFromDatabase()
        {
            SqlConnection conn = new SqlConnection(DBHandler.ConnectionString);
            conn.Open();
            bool connected = conn.State == ConnectionState.Open;
            conn.Close();
            bool disconnected = conn.State == ConnectionState.Closed;

            Assert.IsTrue(connected);
            Assert.IsTrue(disconnected);
        }

        [TestCase]
        public void ValidateSalary_InvalidSalaryInputValue_ReturnsErrorMessage()
        { 
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            MethodInfo testMethod = clsSalaryValidator.GetMethod("ValidateSalary", allBindings);

            TestBasicChecks();

            ConstructorInfo classConstructor = clsSalaryValidator.GetConstructor(Type.EmptyTypes);
            object programClassObject = classConstructor.Invoke(new object[] { });

            double salary = -900000;

            string result1 = (string)testMethod.Invoke(programClassObject, new object[] { salary });

            salary = 400000;
            string result2 = (string)testMethod.Invoke(programClassObject, new object[] { salary });

            Assert.That(result1.Equals("Given Salary is invalid") && result2 == null, "Verify ValidateSalary logic");

        }

        [TestCase]
        public void CalculateTaxAmount_AcceptCTC_ReturnTDS()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            Object tdsCalculationObj = null;

            Type tdsCalculation = assembly.GetType("EmployeeTDSCalculator.BLL.TDSCalculation");

            if (tdsCalculation != null)
            {
                try
                {
                    tdsCalculationObj = Activator.CreateInstance(tdsCalculation);

                }
                catch (Exception)
                {
                    Assert.Fail("Could not create instance for TDSCalculation");
                }

                MethodInfo testMethod = tdsCalculation.GetMethod("CalculateTaxAmount", allBindings);

                double empCTC = 50000;

                double empTDS1 = (double)testMethod.Invoke(tdsCalculationObj, new Object[] { empCTC });

                empCTC = 1200000;

                double empTDS2 = (double)testMethod.Invoke(tdsCalculationObj, new Object[] { empCTC });

                Assert.IsTrue(empTDS1 == 0 && empTDS2 == 180000, "Verify the logic to calculate TDS");

            }
            else
            {
                Assert.Fail("No class with the name 'TDSCalculation' is implemented OR implement the class public. \n");
            }
        }

        [TestCase]
        public void CheckEmployeeIdFormat_ValidEmployeeId_ReturnsTrue()
        {
            string employeeId = "EMP80001";
            bool result1 = Program.CheckEmployeeIdFormat(employeeId);

            employeeId = "ES12345";
            bool result2 = Program.CheckEmployeeIdFormat(employeeId);

            Assert.IsTrue((result1 == true && result2 == false), "Verify the employee id format");
        }

        [TestCase]
        [Category("Database")]
        [Rollback]
        public void CheckForDuplicateEmployeeId_DuplicateEmployeeId_ReturnsTrue()
        {
            List<EmployeeBO> testEmployeeTDSRecords = new List<EmployeeBO>
            {
                new EmployeeBO{ EmployeeId="EMP60001",EmployeeName="Venkat",EmployeeCTC=500000,TdsAmount=50000},
                new EmployeeBO{ EmployeeId="EMP60002",EmployeeName="Prakash",EmployeeCTC=600000,TdsAmount=60000},
                new EmployeeBO{ EmployeeId="EMP60003",EmployeeName="Nivetha",EmployeeCTC=700000,TdsAmount=70000},
                new EmployeeBO{ EmployeeId="EMP60004",EmployeeName="Gokul",EmployeeCTC=800000,TdsAmount=80000}
            };
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            MethodInfo methodInfo = clsEmployeeTDSData.GetMethod("AddEmployeeRecords", allBindings);
            TestBasicChecks();
            ConstructorInfo classConstructor = clsEmployeeTDSData.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });

            for (int i = 0; i < testEmployeeTDSRecords.Count; i++)
            {
                methodInfo.Invoke(classObject, new object[] { testEmployeeTDSRecords[i] });
            }
            string employeeId = "EMP60001";
            bool result1 = Program.CheckForDuplicateEmployeeId(employeeId);            

            Assert.IsTrue((result1), "Verify the existence of employee id entered");
        }

        [TestCase]
        [Category("Database")]
        [Rollback]
        public void AddEmployeeRecords_ValidEmployeeObject_ReturnsTrue()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            MethodInfo methodInfo = clsEmployeeTDSData.GetMethod("AddEmployeeRecords", allBindings);
            TestBasicChecks();
            ConstructorInfo classConstructor = clsEmployeeTDSData.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });

            EmployeeBO employeeData = new EmployeeBO
            {
                EmployeeId = "EMP10045",
                EmployeeName = "Venkat",
                EmployeeCTC = 700000,
                TdsAmount = 70000
            };
            bool result1 = (bool)methodInfo.Invoke(classObject, new object[] { employeeData });

            int rowCount = 0;

            if (result1)
            {
                using (SqlConnection connection = new SqlConnection("data source=localhost;initial catalog=TDSCalculatorDb;integrated security=true;"))
                {
                    using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM tblEmployeeTDSInfo WHERE Employee_Id=@empId", connection))
                    {
                        command.Parameters.AddWithValue("empId", employeeData.EmployeeId);

                        connection.Open();

                        rowCount = (int)command.ExecuteScalar();

                    }
                }
            }
            else
            {
                Assert.Fail("Either it's a duplicate id or verify the program logic to insert data");
            }

            employeeData = new EmployeeBO
            {
                EmployeeId = null,
                EmployeeName = "Venkat",
                EmployeeCTC = 700000,
                TdsAmount = 70000
            };

            bool result2 = (bool)methodInfo.Invoke(classObject, new object[] { employeeData });

            Assert.That(rowCount, Is.EqualTo(1), "Either it's a duplicate id or verify the program logic to insert employee TDS data");
            Assert.IsFalse(result2, "Verify whether the employee object has properly initialized or not");
        }

        [TestCase]
        [Category("Database")]
        [Rollback]
        public void DisplayAllRecords_WhenDataFoundInTheDatabase_ReturnsAllEmployeesRecords()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            MethodInfo methodInfo = clsEmployeeTDSData.GetMethod("DisplayAllRecords", allBindings);
            ConstructorInfo classConstructor = clsEmployeeTDSData.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });

            List<EmployeeBO> testEmployeeTDSRecords = new List<EmployeeBO>
            {
                  new EmployeeBO{ EmployeeId="EMP60001",EmployeeName="Venkat",EmployeeCTC=500000,TdsAmount=50000},
                new EmployeeBO{ EmployeeId="EMP60002",EmployeeName="Prakash",EmployeeCTC=600000,TdsAmount=60000},
                new EmployeeBO{ EmployeeId="EMP60003",EmployeeName="Nivetha",EmployeeCTC=700000,TdsAmount=70000},
                new EmployeeBO{ EmployeeId="EMP60004",EmployeeName="Gokul",EmployeeCTC=800000,TdsAmount=80000}
            };

            MethodInfo insertEmployeeTDSRecord = clsEmployeeTDSData.GetMethod("AddEmployeeRecords", allBindings);

            for (int i = 0; i < testEmployeeTDSRecords.Count; i++)
            {
                insertEmployeeTDSRecord.Invoke(classObject, new object[] { testEmployeeTDSRecords[i] });
            }

            IList<EmployeeBO> employeeTDSTableRecords = methodInfo.Invoke(classObject, new object[] { }) as IList<EmployeeBO>;

            Assert.IsNotNull(employeeTDSTableRecords, "Verify the logic to retrieve all data from the table");

            Assert.IsTrue(employeeTDSTableRecords.Count >= testEmployeeTDSRecords.Count, "Mismatch in the records retrieved by your query with the actual records in the database");
        }
    }
}