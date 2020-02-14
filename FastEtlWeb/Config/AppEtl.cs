public static class AppEtl
{
    public static string Db = "Etl";

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
        public readonly static string Oracle = "Oracle.ManagedDataAccess";

        /// <summary>
        /// mysql
        /// </summary>
        public readonly static string MySql = "MySql.Data";

        /// <summary>
        /// sql server
        /// </summary>
        public readonly static string SqlServer = "System.Data";
    }

    public class FactoryClient
    {
        /// <summary>
        /// oracle
        /// </summary>
        public readonly static string Oracle = "Oracle.ManagedDataAccess.Client.OracleClientFactory";

        /// <summary>
        /// mysql
        /// </summary>
        public readonly static string MySql = "MySql.Data.MySqlClient.MySqlClientFactory";


        /// <summary>
        /// sql server
        /// </summary>
        public readonly static string SqlServer = "System.Data.SqlClient.SqlClientFactory";
    }

    public class CacheKey
    {
        public static string Table = "tableList_{0}";
        public static string Column = "columnList_{0}_{1}";
    }
}

