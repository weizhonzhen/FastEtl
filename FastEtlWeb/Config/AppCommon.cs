using FastEtlWeb.DataModel;
using Microsoft.AspNetCore.Http;
using System.Linq;

public static class AppCommon
{
    /// <summary>
    /// 获取客户Ip
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetClientIp(HttpContext context)
    {
        var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(ip))
        {
            ip = context.Connection.RemoteIpAddress.ToString();
        }
        return ip == "::1" ? "127.0.0.1" : ip;
    }

    /// <summary>
    /// 获取连接字符串
    /// </summary>
    public static string GetConnStr(Data_Source item)
    {
        var connStr = "";

        if (item.Type.ToLower() == AppEtl.DataDbType.Oracle.ToLower())
            connStr = string.Format("User Id={0};Password={1};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT={3})))(CONNECT_DATA=(SERVICE_NAME={4})));pooling=true;Min Pool Size=1;Max Pool Size=5;"
                                , item.UserName.Trim(), item.PassWord.Trim(), item.Host.Trim(), item.Port.Trim(), item.ServerName.Trim());
        else if (AppEtl.DataDbType.SqlServer.ToLower() == item.Type.ToLower())
            connStr = string.Format("Data Source={0},{1};Initial Catalog={2};User Id={3};Password={4};pooling=true;Min Pool Size=1;Max Pool Size=5;"
                                , item.Host.Trim(), item.Port.Trim(), item.ServerName.Trim(), item.UserName.Trim(), item.PassWord.Trim());
        else if (AppEtl.DataDbType.MySql.ToLower() == item.Type.ToLower())
            connStr = string.Format("server={0};port={1};Database={2};user id={3};password={4};pooling=true;Min Pool Size=1;Max Pool Size=5;CharSet=utf8;"
                               , item.Host.Trim(), item.Port.Trim(), item.ServerName.Trim(), item.UserName.Trim(), item.PassWord.Trim());

        return connStr;
    }

    /// <summary>
    /// 连接检测
    /// </summary>
    /// <param name="dbType">数据库类型</param>
    /// <param name="dbConn">连接串</param>
    public static bool TestLink(string dbType, string dbConn)
    {
        try
        {
            using (var conn = DbProviderFactories.GetFactory(dbType).CreateConnection())
            {
                conn.ConnectionString = dbConn;
                conn.Open();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
}

