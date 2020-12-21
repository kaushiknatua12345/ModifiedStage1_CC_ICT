using System.Collections.Generic;
using System.Data.SqlClient;
using EmployeeTDSCalculator.Common;
using EmployeeTDSCalculator.BLL;

namespace EmployeeTDSCalculator.DAL
{
    public class EmployeeDL
    {
        public readonly string InsertRecord = "INSERT INTO tblEmployeeTDSInfo(Employee_Id, Employee_Name, Employee_CTC, TDS_Amount) " +
                 "VALUES(@Employee_Id,@Employee_Name,@Employee_CTC,@TDS_Amount)";
        public readonly string DisplayRecords = "SELECT * FROM tblEmployeeTDSInfo";
        public bool AddEmployeeRecords(EmployeeBO employeeTdsInfo)
        {
            try
            {
                using (SqlConnection _sqlConn = new SqlConnection(DBHandler.ConnectionString))
                {
                    using (SqlCommand cmdAddEmployeeTDSRecord = new SqlCommand(InsertRecord, _sqlConn))
                    {
                        cmdAddEmployeeTDSRecord.Parameters.AddWithValue("@Employee_Id", employeeTdsInfo.EmployeeId);
                        cmdAddEmployeeTDSRecord.Parameters.AddWithValue("@Employee_Name", employeeTdsInfo.EmployeeName);
                        cmdAddEmployeeTDSRecord.Parameters.AddWithValue("@Employee_CTC", employeeTdsInfo.EmployeeCTC);
                        cmdAddEmployeeTDSRecord.Parameters.AddWithValue("@TDS_Amount", employeeTdsInfo.TdsAmount);
                        _sqlConn.Open();

                        int rowsAdded = cmdAddEmployeeTDSRecord.ExecuteNonQuery();

                        if (rowsAdded > 0)
                            return true;
                        else
                            return false;
                    }
                }

            }
            catch (SqlException)
            {
                return false;
            }


        }

        public List<EmployeeBO> DisplayAllRecords()
        {
            List<EmployeeBO> employeeTdsList = new List<EmployeeBO>();
            try
            {
                using (SqlConnection _sqlConn = new SqlConnection(DBHandler.ConnectionString))
                {
                    using (SqlCommand cmdDisplayAllEmployeeTDSRecords = new SqlCommand(DisplayRecords, _sqlConn))
                    {
                        _sqlConn.Open();

                        using (SqlDataReader sqlData = cmdDisplayAllEmployeeTDSRecords.ExecuteReader())
                        {
                            if (sqlData.HasRows)
                            {
                                while (sqlData.Read())
                                {
                                    EmployeeBO employeeTds = new EmployeeBO
                                    {
                                        EmployeeId = sqlData["Employee_Id"].ToString(),
                                        EmployeeName = sqlData["Employee_Name"].ToString(),
                                        EmployeeCTC = double.Parse(sqlData["Employee_CTC"].ToString()),
                                        TdsAmount = double.Parse(sqlData["TDS_Amount"].ToString())
                                    };

                                    employeeTdsList.Add(employeeTds);
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

            return employeeTdsList;
        }
    }
}
