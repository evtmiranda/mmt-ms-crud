using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ms_crud_rest.HelperClasses
{
    public class SqlHelper
    {
        public static SqlConnection AbreConexao()
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["marmitex"].ConnectionString);

            return sqlConn;
        } 
    }
}
