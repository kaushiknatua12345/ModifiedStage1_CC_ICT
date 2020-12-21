using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace OfficeCommuteService
{
    public class OfficeCommuteBO
    {
        public static readonly string insertRecords =
            "INSERT INTO tblOfficeCommuteApp(Employee_Id,Employee_Name,Employee_Type,Travel_Distance,Commute_Charge) " +
            "VALUES(@Employee_Id,@Employee_Name,@Employee_Type,@Travel_Distance,@Commute_Charge)";

        public static readonly string displayAllRecords = "SELECT * FROM tblOfficeCommuteApp";

        public static readonly string displayEmployeesWithMinCommuteFare =
            "SELECT * FROM tblOfficeCommuteApp WHERE Commute_Charge=(SELECT MIN(Commute_Charge) FROM tblOfficeCommuteApp)";

        public SqlConnection _sqlConn = null;
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
            }
        }

        public OfficeCommuteBO()
        {

        }       

        

        public int InsertRecords(OfficeCommuteDAO commuteData)
        {
            try
            {
                using (_sqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmdAddCommuteRecord = new SqlCommand(insertRecords, _sqlConn))
                    {
                        cmdAddCommuteRecord.Parameters.AddWithValue("@Employee_Id", commuteData.EmployeeId);
                        cmdAddCommuteRecord.Parameters.AddWithValue("@Employee_Name", commuteData.EmployeeName);
                        cmdAddCommuteRecord.Parameters.AddWithValue("@Employee_Type", commuteData.EmployeeType);
                        cmdAddCommuteRecord.Parameters.AddWithValue("@Travel_Distance", commuteData.TravelDistance);
                        cmdAddCommuteRecord.Parameters.AddWithValue("@Commute_Charge", commuteData.CommuteCharge);

                        _sqlConn.Open();

                        int rowsAdded = cmdAddCommuteRecord.ExecuteNonQuery();

                        if (rowsAdded > 0)
                            return 1;
                        else
                            return 0;
                    }
                }

            }
            catch (SqlException)
            {
                return 0;
            }
        }

        public IList<OfficeCommuteDAO> DisplayAllRecords()
        {
            List<OfficeCommuteDAO> commuteDataList = new List<OfficeCommuteDAO>();

            try
            {
                using (_sqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmdDisplayAllCommuteRecords = new SqlCommand(displayAllRecords, _sqlConn))
                    {

                        _sqlConn.Open();

                        using (SqlDataReader sqlData = cmdDisplayAllCommuteRecords.ExecuteReader())
                        {
                            if (sqlData.HasRows)
                            {
                                while (sqlData.Read())
                                {
                                    OfficeCommuteDAO commuteData = new OfficeCommuteDAO
                                    {
                                        EmployeeId = sqlData["Employee_Id"].ToString(),
                                        EmployeeName = sqlData["Employee_Name"].ToString(),
                                        EmployeeType = sqlData["Employee_Type"].ToString(),
                                        TravelDistance = float.Parse(sqlData["Travel_Distance"].ToString()),
                                        CommuteCharge = double.Parse(sqlData["Commute_Charge"].ToString())
                                    };

                                    commuteDataList.Add(commuteData);
                                }                                
                            }
                        }
                    }
                }

            }
            catch (SqlException)
            {
                return null;
            }

            return commuteDataList; 
        }

        public IList<OfficeCommuteDAO> DisplayRecordsByLowestCommuteFare()
        {
            List<OfficeCommuteDAO> commuteDataList = new List<OfficeCommuteDAO>();

            try
            {
                using (_sqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmdDisplayCommuteRecordsByLowestFare = 
                        new SqlCommand(displayEmployeesWithMinCommuteFare, _sqlConn))
                    {

                        _sqlConn.Open();

                        using (SqlDataReader sqlData = cmdDisplayCommuteRecordsByLowestFare.ExecuteReader())
                        {
                            if (sqlData.HasRows)
                            {
                                while (sqlData.Read())
                                {
                                    OfficeCommuteDAO commuteData = new OfficeCommuteDAO
                                    {
                                        EmployeeId = sqlData["Employee_Id"].ToString(),
                                        EmployeeName = sqlData["Employee_Name"].ToString(),
                                        EmployeeType = sqlData["Employee_Type"].ToString(),
                                        TravelDistance = float.Parse(sqlData["Travel_Distance"].ToString()),
                                        CommuteCharge = double.Parse(sqlData["Commute_Charge"].ToString())
                                    };

                                    commuteDataList.Add(commuteData);
                                }
                            }
                        }
                    }
                }

            }
            catch (SqlException)
            {
                return null;
            }

            return commuteDataList; 
        }
    }
}
