using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;

public static class DbProviderFactories
{
    public static DbProviderFactory GetFactory(string dbType)
    {
        try
        {
            var ProviderName = "";
            var FactoryClient = "";

            if (dbType.ToLower() == AppEtl.DataDbType.MySql.ToLower())
            {
                ProviderName = AppEtl.Provider.MySql;
                FactoryClient = AppEtl.FactoryClient.MySql;
            }

            if (dbType.ToLower() == AppEtl.DataDbType.Oracle.ToLower())
            {
                ProviderName = AppEtl.Provider.Oracle;
                FactoryClient = AppEtl.FactoryClient.Oracle;
            }

            if (dbType.ToLower() == AppEtl.DataDbType.SqlServer.ToLower())
            {
                ProviderName = AppEtl.Provider.SqlServer;
                FactoryClient = AppEtl.FactoryClient.SqlServer;
            }

            var assembly = AppDomain.CurrentDomain.GetAssemblies().ToList().Find(a => a.FullName.Split(',')[0] == ProviderName);
            if (assembly == null)
                assembly = Assembly.Load(ProviderName);

            var type = assembly.GetType(FactoryClient, false);
            object instance = null;

            if (type != null)
                instance = type.InvokeMember("Instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty, null, type, null);

            return instance as DbProviderFactory;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
