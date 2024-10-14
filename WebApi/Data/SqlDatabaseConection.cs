using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace WebApi.Data
{
    public class SqlDatabaseConnection : IDatabaseConection
    {
        private readonly IConfiguration _configuration;

        public SqlDatabaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetConnection()
        {
            var connectionString = _configuration.GetConnectionString("MySqlConection");
            return new MySqlConnection(connectionString);
        }

    }


}