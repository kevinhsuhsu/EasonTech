using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Eason
{
    public class SystemManager
    {
        public static SqlConnection Conn ;
        public SystemManager()
        {

        }

        public static SqlConnection DBConnection()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString);
            string connStr = sqlBuilder.ConnectionString;
            return Conn = new SqlConnection(connStr);
        }
    }
}