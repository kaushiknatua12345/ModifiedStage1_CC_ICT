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

	//Fill your code to implement InsertRecords method here
        
	
	
	//Fill your code to implement DisplayAllRecords method here
        

	
	//Fill your code to implement DisplayRecordsByLowestCommuteFare method here        

        
    }
}
