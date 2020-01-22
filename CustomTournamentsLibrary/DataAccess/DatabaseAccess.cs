using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsLibrary.DataAccess
{
    public static class DatabaseAccess
    {
        public static string GetConnectionString(string name = "CustomTournamentsDB")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
