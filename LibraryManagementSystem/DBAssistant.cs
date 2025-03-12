using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryManagementSystem
{
    internal class DBAssistant
    {
        private static string connectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = LMS; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";
        
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

    }
}
