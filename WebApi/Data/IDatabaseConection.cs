using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApi.Data
{
        public interface IDatabaseConection
        {
            IDbConnection GetConnection();
        }

}

