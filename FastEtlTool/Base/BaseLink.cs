using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using FastEtlModel.DataModel;
using System;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace FastEtlTool.Base
{
    public static class BaseLink
    {           
        #region 控件值给DataLink
        /// <summary>
        ///  控件值给DataLink
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="dbConn"></param>
        /// <param name="txtHostName"></param>
        /// <param name="txtUserName"></param>
        /// <param name="txtPwd"></param>
        /// <param name="txtPort"></param>
        /// <param name="txtServerName"></param>
        /// <param name="labServerName"></param>
        /// <returns></returns>
        public static Data_Source ControlsToData(string dbType, TextBox txtHostName, TextBox txtUserName, TextBox txtPwd,
            TextBox txtPort, TextBox txtServerName, Label labServerName, bool isLink, TextBox txtLinkName)
        {
            var item = new Data_Source();
            item.Type = dbType;
            item.Host = txtHostName.Text.Trim();
            item.UserName = txtUserName.Text.Trim();
            item.PassWord = txtPwd.Text.Trim();
            item.Port = txtPort.Text.Trim();
            item.ServerName = txtServerName.Text.Trim();

            if (isLink)
            {
                if (txtLinkName.Text.Trim() == "")
                    item.LinkName = GetLinkName(AppCache.GetLink);
                else
                    item.LinkName = txtLinkName.Text.Trim();
            }

            return item;
        }
        #endregion

        #region DataLink给控件值
        /// <summary>
        /// DataLink给控件值
        /// </summary>
        /// <param name="link"></param>
        /// <param name="txtHostName"></param>
        /// <param name="txtUserName"></param>
        /// <param name="txtPwd"></param>
        /// <param name="txtPort"></param>
        /// <param name="txtServerName"></param>
        /// <param name="labServerName"></param>
        /// <param name="txtLinkName"></param>
        public static void DataToControls(Data_Source link, ref TextBox txtHostName, ref TextBox txtUserName, ref TextBox txtPwd,
                                           ref TextBox txtPort, ref TextBox txtServerName, ref Label labServerName, ref TextBox txtLinkName)
        {
            txtHostName.Text = link.Host;
            txtUserName.Text = link.UserName;
            txtPwd.Text = link.PassWord;
            txtPort.Text = link.Port;
            txtServerName.Text = link.ServerName;
            txtLinkName.Text = link.LinkName;

            if (link.Type == FastApp.DataDbType.Oracle)
            {
                labServerName.Content = "服务名：";
                txtPort.Text = "1521";
            }
            else if (link.Type == FastApp.DataDbType.SqlServer)
            {
                labServerName.Content = "库名称：";
                txtPort.Text = "1433";
            }
            else if (link.Type == FastApp.DataDbType.MySql)
            {
                txtPort.Text = "3306";
                labServerName.Content = "库名称：";
            }
        }
        #endregion

        #region ComboBoxItem选中
        /// <summary>
        /// ComboBoxItem选中
        /// </summary>
        /// <param name="item"></param>
        /// <param name="selectValue"></param>
        public static void ComboBoxSelect(ComboBox box, string value, bool isDataLink = false)
        {
            foreach (var item in box.Items)
            {
                if (isDataLink)
                {
                    var boxItem = item as Data_Source;
                    if (boxItem.LinkName == value)
                        box.SelectedItem = item;
                }
                else
                {
                    var boxItem = item as ComboBoxItem;
                    if (boxItem.Content.ToString() == value)
                        box.SelectedItem = item;
                }
            }
        }
        #endregion

        #region 连接初始化
        /// <summary>
        /// 连接初始化
        /// </summary>
        /// <param name="port">端口</param>
        /// <param name="serveName">服务名</param>
        /// <param name="dbType">数据库类型</param>
        public static string InitLink(ref TextBox txtPort, ref Label labServerName, ComboBox box)
        {
            var boxItem = box.SelectedItem as ComboBoxItem;
            var dbType = boxItem.Content.ToString();

            if (txtPort != null && labServerName != null)
            {
                if (dbType == FastApp.DataDbType.Oracle)
                {
                    txtPort.Text = "1521";
                    labServerName.Content = "服务名：";
                }
                else if (dbType == FastApp.DataDbType.SqlServer)
                {
                    txtPort.Text = "1433";
                    labServerName.Content = "库名称：";
                }
                else if (dbType == FastApp.DataDbType.MySql)
                {
                    txtPort.Text = "3306";
                    labServerName.Content = "库名称：";
                }
            }

            return dbType;
        }
        #endregion

        #region 获取连接字符串
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string GetConnStr(string DbType, TextBox UserName, TextBox UserPwd, TextBox HostName, TextBox Port, TextBox ServerName)
        {
            var connStr = "";

            if (DbType == FastApp.DataDbType.Oracle)
            {
                connStr = string.Format("User Id={0};Password={1};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT={3})))(CONNECT_DATA=(SERVICE_NAME={4})));pooling=true;Min Pool Size=1;Max Pool Size=5;"
                                    , UserName.Text.Trim(), UserPwd.Text.Trim(), HostName.Text.Trim(), Port.Text.Trim(), ServerName.Text.Trim());
            }
            else if (FastApp.DataDbType.SqlServer == DbType)
            {
                connStr = string.Format("Data Source={0},{1};Initial Catalog={2};User Id={3};Password={4};pooling=true;Min Pool Size=1;Max Pool Size=5;"
                                    , HostName.Text.Trim(), Port.Text.Trim(), ServerName.Text.Trim(), UserName.Text.Trim(), UserPwd.Text.Trim());
            }
            else if (FastApp.DataDbType.MySql == DbType)
            {
                connStr = string.Format("server={0};port={1};Database={2};user id={3};password={4};pooling=true;Min Pool Size=1;Max Pool Size=5;CharSet=utf8;"
                                   , HostName.Text.Trim(), Port.Text.Trim(), ServerName.Text.Trim(), UserName.Text.Trim(), UserPwd.Text.Trim());
            }

            return connStr;
        }
        #endregion

        #region 获取连接字符串
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string GetConnStr(Data_Source link)
        {
            var connStr = "";

            if (link.Type == FastApp.DataDbType.Oracle)
            {
                connStr = string.Format("User Id={0};Password={1};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT={3})))(CONNECT_DATA=(SERVICE_NAME={4})));pooling=true;Min Pool Size=1;Max Pool Size=5;"
                                    , link.UserName, link.PassWord, link.Host,link.Port, link.ServerName);
            }
            else if (FastApp.DataDbType.SqlServer == link.Type)
            {
                connStr = string.Format("Data Source={0},{1};Initial Catalog={2};User Id={3};Password={4};pooling=true;Min Pool Size=1;Max Pool Size=5;"
                                    , link.Host, link.Port, link.ServerName, link.UserName, link.PassWord);
            }
            else if (FastApp.DataDbType.MySql == link.Type)
            {
                connStr = string.Format("server={0};port={1};Database={2};user id={3};password={4};pooling=true;Min Pool Size=1;Max Pool Size=5;CharSet=utf8;"
                                   , link.Host, link.Port, link.ServerName, link.UserName, link.PassWord);
            }

            return connStr;
        }
        #endregion

        #region 连接检测
        /// <summary>
        /// 连接检测
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="dbConn">连接串</param>
        public static bool CheckLink(string dbType, string dbConn)
        {
            try
            {
                var refValue = false;
                if (dbType == FastApp.DataDbType.Oracle)
                {
                    #region oracle
                    using (var conn = new OracleConnection(dbConn))
                    {
                        try
                        {
                            conn.Open();
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    #endregion
                }
                else if (dbType == FastApp.DataDbType.SqlServer)
                {
                    #region sqlserver
                    using (var conn = new SqlConnection(dbConn))
                    {
                        try
                        {
                            conn.Open();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                    }
                    #endregion
                }
                else if (dbType == FastApp.DataDbType.MySql)
                {
                    #region mysql
                    using (var conn = new MySqlConnection(dbConn))
                    {
                        try
                        {
                            conn.Open();
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    #endregion
                }

                return refValue;
            }
            catch
            {
                return false;
            }
        }
        #endregion
                
        #region 连接名格式
        /// <summary>
        /// 连接名格式
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="userName">用户名</param>
        /// <param name="serverValue">数据库名</param>
        /// <returns></returns>
        public static string GetLinkName(Data_Source item)
        {
            return string.Format("{0}_{1}_{2}", item.Type, item.UserName, item.ServerName);
        }
        #endregion
    }
}
