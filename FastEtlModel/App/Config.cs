using System;
using System.Collections.Generic;
using System.IO;

namespace FastApp
{
    public static class Config
    {
        public static readonly string Title = "FastEtl";
    }

    public class DataDbType
    {
        /// <summary>
        /// oracle
        /// </summary>
        public readonly static string Oracle = "Oracle";

        /// <summary>
        /// mysql
        /// </summary>
        public readonly static string MySql = "MySql";

        /// <summary>
        /// sql server
        /// </summary>
        public readonly static string SqlServer = "SqlServer";
    }

    public class Provider
    {
        /// <summary>
        /// oracle
        /// </summary>
        public readonly static string Oracle = "Oracle.ManagedDataAccess.Client";

        /// <summary>
        /// mysql
        /// </summary>
         public readonly static string MySql = "MySql.Data.MySqlClient";

        /// <summary>
        /// sql server
        /// </summary>
        public readonly static string SqlServer = "System.Data.SqlClient";
    }
}
