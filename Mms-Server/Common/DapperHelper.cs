using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Mms_Server.Common
{
    public class DapperHelper
    {
        public static string _connectionString { get; set; }

        //获取connection对象
        public static IDbConnection CreateConnection()
        {
            IDbConnection conn=new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}
