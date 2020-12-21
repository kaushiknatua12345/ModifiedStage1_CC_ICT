using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeCommuteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;
using System.Configuration;
using System.Transactions;
using NUnit.Framework.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace OfficeCommuteTests
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
    public class OfficeCommuteTests
    {
        Assembly assembly;
        Type clsOfficeCommuteBO;
        Type clsOfficeCommuteDAO;
        Type clsProgram;

        OfficeCommuteBO OfficeCommuteBO;
        Program Program;

        [SetUp]
        public void SetUp()
        {
            OfficeCommuteBO = new OfficeCommuteBO();
            Program = new Program();
            assembly = Assembly.Load("OfficeCommuteService");
            clsOfficeCommuteBO = assembly.GetType("OfficeCommuteService.OfficeCommuteBO");
            clsOfficeCommuteDAO = assembly.GetType("OfficeCommuteService.OfficeCommuteDAO");
            clsProgram = assembly.GetType("OfficeCommuteService.Program");
        }

        [TestCase]
        public void TestBasicChecks()
        {
            NUnit.Framework.Assert.IsNotNull(clsOfficeCommuteDAO, "Class OfficeCommuteDAO NOT implemented OR check spelling");
            NUnit.Framework.Assert.IsNotNull(clsOfficeCommuteBO, "Class OfficeCommuteBO NOT implemented OR check spelling");
            NUnit.Framework.Assert.IsNotNull(clsProgram, "Class Program NOT implemented OR check spelling");
        }

        [TestCase]
        public void InsertRecords_MethodExistence_Test()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            if (clsOfficeCommuteBO != null)
            {
                MethodInfo insertRecordsMethod = clsOfficeCommuteBO.
                    GetMethod("InsertRecords", allBindings);

                NUnit.Framework.Assert.That(insertRecordsMethod != null, "Method InsertRecords NOT implemented OR check spelling");
            }
            else
            {
                NUnit.Framework.Assert.Fail("No class with the name 'OfficeCommuteBO' is implemented OR Did you change the class name");
            }
        }

        [TestCase]
        public void DisplayAllRecords_MethodExistence_Test()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            if (clsOfficeCommuteBO != null)
            {
                MethodInfo displayAllRecordsMethod = clsOfficeCommuteBO.
                    GetMethod("DisplayAllRecords", allBindings);

                NUnit.Framework.Assert.That(displayAllRecordsMethod != null, "Method DisplayAllRecords NOT implemented OR check spelling");
            }
            else
            {
                NUnit.Framework.Assert.Fail("No class with the name 'OfficeCommuteBO' is implemented OR Did you change the class name");
            }
        }

        [TestCase]
        public void DisplayRecordsByLowestCommuteFare_MethodExistence_Test()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            if (clsOfficeCommuteBO != null)
            {
                MethodInfo displayRecordsByLowestCommuteFareMethod = clsOfficeCommuteBO.
                    GetMethod("DisplayRecordsByLowestCommuteFare", allBindings);

                NUnit.Framework.Assert.That(displayRecordsByLowestCommuteFareMethod != null, "Method DisplayRecordsByLowestCommuteFare NOT implemented OR check spelling");
            }
            else
            {
                NUnit.Framework.Assert.Fail("No class with the name 'clsOfficeCommuteBO' is implemented OR Did you change the class name");
            }
        }
        [TestCase]
        public void CheckEmployeeId_WhenEmployeeIdFormatCorrect_ReturnsTrue()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            string employeeId = "EXT5001";
            MethodInfo methodInfo = clsProgram.GetMethod("CheckEmployeeIdFormat", allBindings);
            ConstructorInfo classConstructor = clsProgram.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });
            bool checkEmployeeId = (bool)methodInfo.Invoke(classObject, new object[] { employeeId });
            NUnit.Framework.Assert.IsTrue(checkEmployeeId);
        }

        [TestCase]
        public void CheckEmployeeId_WhenEmployeeIdFormatInCorrect_ReturnsFalse()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            string employeeId = "ABCD1234";
            MethodInfo methodInfo = clsProgram.GetMethod("CheckEmployeeIdFormat", allBindings);
            ConstructorInfo classConstructor = clsProgram.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });
            bool checkEmployeeId = (bool)methodInfo.Invoke(classObject, new object[] { employeeId });
            NUnit.Framework.Assert.IsFalse(checkEmployeeId);
        }

        [TestCase]
        public void CheckEmployeeId_WhenNoDuplicateIdPresentInDatabase_ReturnsTrue()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            string employeeId = "EXT5001";
            MethodInfo methodInfo = clsProgram.GetMethod("CheckForDuplicateEmployeeId", allBindings);
            ConstructorInfo classConstructor = clsProgram.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });
            bool checkEmployeeId = (bool)methodInfo.Invoke(classObject, new object[] { employeeId });
            NUnit.Framework.Assert.IsTrue(checkEmployeeId);
        }

        [TestCase]
        public void CheckEmployeeId_WhenDuplicateIdIsPresentInDatabase_ReturnsFalse()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            string employeeId = "EXT1001";
            MethodInfo methodInfo = clsProgram.GetMethod("CheckForDuplicateEmployeeId", allBindings);
            ConstructorInfo classConstructor = clsProgram.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });
            bool checkEmployeeId = (bool)methodInfo.Invoke(classObject, new object[] { employeeId });
            NUnit.Framework.Assert.IsFalse(checkEmployeeId);
        }

        [TestCase]
        public void CheckEmployeeType_WhenTypeEnteredIsExternalOrInternal_ReturnsTrue()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            string employeeType = "EXTERNAL";
            MethodInfo methodInfo = clsProgram.GetMethod("CheckEmployeeTypeInput", allBindings);
            ConstructorInfo classConstructor = clsProgram.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });
            bool checkEmployeeType = (bool)methodInfo.Invoke(classObject, new object[] { employeeType });
            NUnit.Framework.Assert.IsTrue(checkEmployeeType);
        }

        [TestCase]
        public void CheckEmployeeType_WhenTypeEnteredIsNotExternalOrInternal_ReturnsFalse()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            string employeeType = "Vendor";
            MethodInfo methodInfo = clsProgram.GetMethod("CheckEmployeeTypeInput", allBindings);
            ConstructorInfo classConstructor = clsProgram.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });
            bool checkEmployeeType = (bool)methodInfo.Invoke(classObject, new object[] { employeeType });
            NUnit.Framework.Assert.IsFalse(checkEmployeeType);
        }

        [TestCase]
        public void CheckEmployeeType_WhenTypeEnteredMatchesWithEmployeeIdType_ReturnsTrue()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            string employeeId = "EXT5001";
            string employeeType = "EXTERNAL";
            MethodInfo methodInfo = clsProgram.GetMethod("CheckIfEmployeeTypeMatchesWithEmployeeIdType", allBindings);
            ConstructorInfo classConstructor = clsProgram.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });
            bool checkEmployeeType = (bool)methodInfo.Invoke
                (classObject, new object[] { employeeId.Substring(0, 3), employeeType.Substring(0, 3) });
            NUnit.Framework.Assert.IsTrue(checkEmployeeType);
        }

        [TestCase]
        public void CheckEmployeeType_WhenTypeEnteredDoesNotMatcheWithEmployeeIdType_ReturnsFalse()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            string employeeId = "EXT5001";
            string employeeType = "INTERNAL";
            MethodInfo methodInfo = clsProgram.GetMethod("CheckIfEmployeeTypeMatchesWithEmployeeIdType", allBindings);
            ConstructorInfo classConstructor = clsProgram.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });
            bool checkEmployeeType = (bool)methodInfo.Invoke
                (classObject, new object[] { employeeId.Substring(0, 3), employeeType.Substring(0, 3) });
            NUnit.Framework.Assert.IsFalse(checkEmployeeType);
        }

        [TestCase]
        public void CheckTravelDistance_WhenTraveDistanceIsWithinLowerLimitValue_ReturnsTrue()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            float travelDistance = 2.8F;
            MethodInfo methodInfo = clsProgram.GetMethod("CheckTravelDistance", allBindings);
            ConstructorInfo classConstructor = clsProgram.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });
            bool checkTravelDistance = (bool)methodInfo.Invoke
                (classObject, new object[] { travelDistance });
            NUnit.Framework.Assert.IsTrue(checkTravelDistance);
        }

        [TestCase]
        public void CheckTravelDistance_WhenTraveDistanceIsWithinUpperLimitValue_ReturnsTrue()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            float travelDistance = 30.0F;
            MethodInfo methodInfo = clsProgram.GetMethod("CheckTravelDistance", allBindings);
            ConstructorInfo classConstructor = clsProgram.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });
            bool checkTravelDistance = (bool)methodInfo.Invoke
                (classObject, new object[] { travelDistance });
            NUnit.Framework.Assert.IsTrue(checkTravelDistance);
        }
        [TestCase]
        public void CheckTravelDistance_WhenTravelDistanceValueIsLessThanLowerLimitValue_ReturnsFalse()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            float travelDistance = 1.99F;
            MethodInfo methodInfo = clsProgram.GetMethod("CheckTravelDistance", allBindings);
            ConstructorInfo classConstructor = clsProgram.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });
            bool checkTravelDistance = (bool)methodInfo.Invoke
                (classObject, new object[] { travelDistance });
            NUnit.Framework.Assert.IsFalse(checkTravelDistance);
        }

        [TestCase]
        public void CheckTravelDistance_WhenTravelDistanceValueIsMoreThanUpperLimitValue_ReturnsFalse()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            float travelDistance = 30.5F;
            MethodInfo methodInfo = clsProgram.GetMethod("CheckTravelDistance", allBindings);
            ConstructorInfo classConstructor = clsProgram.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });
            bool checkTravelDistance = (bool)methodInfo.Invoke
                (classObject, new object[] { travelDistance });
            NUnit.Framework.Assert.IsFalse(checkTravelDistance);
        }

        [TestCase]
        public void FareCalculation_OnValidInputs_ReturnsExpectedFare()
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            string employeeType = "EXTERNAL";
            float travelDistance = 5;
            MethodInfo methodInfo = clsProgram.GetMethod("FareCalculation", allBindings);
            ConstructorInfo classConstructor = clsProgram.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });
            double Fare = (double)methodInfo.Invoke
                (classObject, new object[] { employeeType, travelDistance });
            NUnit.Framework.Assert.That(550, Is.EqualTo(Fare));
        }

        [TestCase]
        [Category("Database")]
        public void GetConnectionStringFromAppConfig()
        {
            string actualString = OfficeCommuteBO.ConnectionString;
            string expectedString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
            NUnit.Framework.Assert.AreEqual(expectedString, actualString);
        }

        [TearDown]
        public void TearDown()
        {
            OfficeCommuteBO = null;
        }

        [TestCase]
        [Category("Database")]
        public void ConnectAndDisconnectFromDatabase()
        {
            SqlConnection conn = new SqlConnection(OfficeCommuteBO.ConnectionString);
            conn.Open();
            bool connected = conn.State == ConnectionState.Open;
            conn.Close();
            bool disconnected = conn.State == ConnectionState.Closed;
            NUnit.Framework.Assert.IsTrue(connected);
            NUnit.Framework.Assert.IsTrue(disconnected);
        }

        [TestCase]
        [Category("Database")]
        [Rollback]
        public void InsertRecords_WhenValidInputs_ReturnsTrue()
        {
            string employeeId = "EXT5001";
            string employeeName = "Hammis";
            string employeeType = "EXTERNAL";
            float travelDistance = 8.3F;
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            MethodInfo methodInfo = clsProgram.GetMethod("FareCalculation", allBindings);
            ConstructorInfo classConstructor = clsProgram.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });
            double Fare = (double)methodInfo.Invoke
                (classObject, new object[] { employeeType, travelDistance });

            MethodInfo insertMethodInfo = clsOfficeCommuteBO.GetMethod("InsertRecords", allBindings);
            ConstructorInfo OfficeCommuteBOConstructor = clsOfficeCommuteBO.GetConstructor(Type.EmptyTypes);
            object OfficeCommuteBOObject = OfficeCommuteBOConstructor.Invoke(new object[] { });
            int inserted = (int)insertMethodInfo.Invoke
                (OfficeCommuteBOObject, new object[]
                {new OfficeCommuteDAO(){EmployeeId=employeeId,EmployeeName=employeeName,
                EmployeeType=employeeType,TravelDistance=travelDistance,CommuteCharge=Fare} });

            NUnit.Framework.Assert.AreEqual(1, inserted);
        }

        [TestCase]
        [Category("Database")]
        [Rollback]
        public void InsertRecords_WhenInValidInputs_ReturnsFalse()
        {
            string employeeId = "EXT5001";
            string employeeName = null;
            string employeeType = "EXTERNAL";
            float travelDistance = 8.3F;
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            MethodInfo methodInfo = clsProgram.GetMethod("FareCalculation", allBindings);
            ConstructorInfo classConstructor = clsProgram.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });
            double Fare = (double)methodInfo.Invoke
                (classObject, new object[] { employeeType, travelDistance });

            MethodInfo insertMethodInfo = clsOfficeCommuteBO.GetMethod("InsertRecords", allBindings);
            ConstructorInfo OfficeCommuteBOConstructor = clsOfficeCommuteBO.GetConstructor(Type.EmptyTypes);
            object OfficeCommuteBOObject = OfficeCommuteBOConstructor.Invoke(new object[] { });
            int inserted = (int)insertMethodInfo.Invoke
                (OfficeCommuteBOObject, new object[]
                {new OfficeCommuteDAO(){EmployeeId=employeeId,EmployeeName=employeeName,
                EmployeeType=employeeType,TravelDistance=travelDistance,CommuteCharge=Fare} });

            NUnit.Framework.Assert.AreEqual(0, inserted);
        }

        [Test]
        [Category("Database")]
        [Rollback]
        public void DisplayAllRecords_WhenCalled_ReturnAllEmployeesCommuteRecords()
        {

            List<OfficeCommuteDAO> testOfficeCommuteRecords = new List<OfficeCommuteDAO>
            {
                new OfficeCommuteDAO{ EmployeeId="EXT6001",EmployeeName="Venkat",EmployeeType="External",TravelDistance=15.7F,CommuteCharge=1256},
                new OfficeCommuteDAO{ EmployeeId="INT6001",EmployeeName="Prakash",EmployeeType="Internal",TravelDistance=10.0F,CommuteCharge=500},
                new OfficeCommuteDAO{ EmployeeId="EXT6002",EmployeeName="Nivetha",EmployeeType="External",TravelDistance=15.7F,CommuteCharge=1256},
                new OfficeCommuteDAO{ EmployeeId="INT6001",EmployeeName="Gokul",EmployeeType="Internal",TravelDistance=8.0F,CommuteCharge=800}
            };
            for (int i = 0; i < testOfficeCommuteRecords.Count; i++)
            {
                OfficeCommuteBO.InsertRecords(testOfficeCommuteRecords[i]);
            }

            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            MethodInfo methodInfo = clsOfficeCommuteBO.GetMethod("DisplayAllRecords", allBindings);
            ConstructorInfo classConstructor = clsOfficeCommuteBO.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });

            IList<OfficeCommuteDAO> officeCommuteTableRecords =
               methodInfo.Invoke
                (classObject, new object[] { }) as IList<OfficeCommuteDAO>;

            NUnit.Framework.Assert.IsNotNull(officeCommuteTableRecords);
            NUnit.Framework.Assert.IsTrue(officeCommuteTableRecords.Count >= testOfficeCommuteRecords.Count);
        }

        [Test]
        [Rollback]
        [Category("Database")]
        public void DisplayAllRecords_WhenNoRecordsFoundInDB_ReturnAnEmptyList()
        {
            using (OfficeCommuteBO._sqlConn = new SqlConnection(OfficeCommuteBO.ConnectionString))
            {
                SqlCommand deleteCmd = new SqlCommand("DELETE FROM tblOfficeCommuteApp", OfficeCommuteBO._sqlConn);
                OfficeCommuteBO._sqlConn.Open();
                deleteCmd.ExecuteNonQuery();
            }
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            MethodInfo methodInfo = clsOfficeCommuteBO.GetMethod("DisplayAllRecords", allBindings);
            ConstructorInfo classConstructor = clsOfficeCommuteBO.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });

            IList<OfficeCommuteDAO> officeCommuteTableRecords =
               methodInfo.Invoke
                (classObject, new object[] { }) as IList<OfficeCommuteDAO>;

            NUnit.Framework.Assert.That(officeCommuteTableRecords, Is.Empty);
        }

        [Test]
        [Category("Database")]
        [Rollback]
        public void DisplayRecordsByLowestCommuteFare_WhenCalled_ReturnsAllRecordsWithLowestCommuteFare()
        {
            List<OfficeCommuteDAO> testOfficeCommuteRecords = new List<OfficeCommuteDAO>
            {
                new OfficeCommuteDAO{ EmployeeId="EXT6001",EmployeeName="Venkat",EmployeeType="External",TravelDistance=15.7F,CommuteCharge=1256},
                new OfficeCommuteDAO{ EmployeeId="INT6001",EmployeeName="Prakash",EmployeeType="Internal",TravelDistance=10.0F,CommuteCharge=500},
                new OfficeCommuteDAO{ EmployeeId="EXT6002",EmployeeName="Nivetha",EmployeeType="External",TravelDistance=15.7F,CommuteCharge=1256},
                new OfficeCommuteDAO{ EmployeeId="INT6001",EmployeeName="Gokul",EmployeeType="Internal",TravelDistance=8.0F,CommuteCharge=800}
            };
            double minFare = testOfficeCommuteRecords.Min(m => m.CommuteCharge);

            for (int i = 0; i < testOfficeCommuteRecords.Count; i++)
            {
                OfficeCommuteBO.InsertRecords(testOfficeCommuteRecords[i]);
            }

            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            MethodInfo methodInfo = clsOfficeCommuteBO.GetMethod("DisplayRecordsByLowestCommuteFare", allBindings);
            ConstructorInfo classConstructor = clsOfficeCommuteBO.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });

            IList<OfficeCommuteDAO> officeCommuteRecordsWithLowestFare =
               methodInfo.Invoke
                (classObject, new object[] { }) as IList<OfficeCommuteDAO>;

            NUnit.Framework.Assert.IsNotNull(officeCommuteRecordsWithLowestFare);
            NUnit.Framework.Assert.IsTrue(officeCommuteRecordsWithLowestFare.Count >= testOfficeCommuteRecords.FindAll(m => m.CommuteCharge == minFare).Count);
        }

        [Test]
        [Rollback]
        [Category("Database")]
        public void DisplayRecordsByLowestCommuteFare_WhenNoRecordsFoundInDB_ReturnAnEmptyList()
        {
            using (OfficeCommuteBO._sqlConn = new SqlConnection(OfficeCommuteBO.ConnectionString))
            {
                SqlCommand deleteCmd = new SqlCommand("DELETE FROM tblOfficeCommuteApp", OfficeCommuteBO._sqlConn);
                OfficeCommuteBO._sqlConn.Open();
                deleteCmd.ExecuteNonQuery();
            }
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            MethodInfo methodInfo = clsOfficeCommuteBO.GetMethod("DisplayRecordsByLowestCommuteFare", allBindings);
            ConstructorInfo classConstructor = clsOfficeCommuteBO.GetConstructor(Type.EmptyTypes);
            object classObject = classConstructor.Invoke(new object[] { });

            IList<OfficeCommuteDAO> officeCommuteTableLowestFareRecord =
               methodInfo.Invoke
                (classObject, new object[] { }) as IList<OfficeCommuteDAO>;

            NUnit.Framework.Assert.That(officeCommuteTableLowestFareRecord, Is.Empty);
        }

    }
}