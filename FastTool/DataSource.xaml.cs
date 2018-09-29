using FastEtlTool.Base;
using System.Windows;
using FastData;
using System;
using FastEtlModel.DataModel;
using FastData.Context;
using System.Configuration;

namespace FastEtlTool
{
    /// <summary>
    /// Ds.xaml 的交互逻辑
    /// </summary>
    public partial class DataSource: Window
    {
        //连接串
        public string dbConn = "";

        //数据库
        public string dbType = "";

        public DataSource()
        {
            InitializeComponent();
            Common.InitWindows(this, "数据源", false);

            dbTypeLink.ItemsSource = AppCache.GetAllLink;

            InitLinkInfo();
        }

        #region 测试数据库连接
        /// <summary>
        /// 测试数据库连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Conn_Click(object sender, RoutedEventArgs e)
        {
            dbConn = BaseLink.GetConnStr(dbType, txtUserName, txtPwd, txtHostName, txtPort, txtServerName);

            if (BaseLink.CheckLink(dbType, dbConn))
                CodeBox.Show("连接成功！", this);
            else
                CodeBox.Show("连接失败！", this);
        }
        #endregion        

        #region 数据库选择中
        /// <summary>
        /// 数据库选择中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box_Selected(object sender, RoutedEventArgs e)
        {
            dbType = BaseLink.InitLink(ref txtPort, ref labServerName, boxDbType);
        }
        #endregion

        #region 连接名选择中
        /// <summary>
        /// 连接名选择中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Link_Selected(object sender, RoutedEventArgs e)
        {
            var boxItem = dbTypeLink.SelectedItem as Data_Source;

            if (boxItem != null)
            {
                AppCache.SetLink(boxItem);
                InitLinkInfo();
            }
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            dbConn = BaseLink.GetConnStr(dbType, txtUserName, txtPwd, txtHostName, txtPort, txtServerName);

            if (!BaseLink.CheckLink(dbType, dbConn))
                CodeBox.Show("连接数据库失败！", this);
            else
            {
                var buildLink = BaseLink.ControlsToData(dbType, txtHostName, txtUserName, txtPwd, txtPort, txtServerName, labServerName, true, txtLinkName);
                buildLink.LinkName = buildLink.LinkName == "" ? BaseLink.GetLinkName(buildLink) : buildLink.LinkName;
                buildLink.Id = Guid.NewGuid().ToString();
                AppCache.SetLink(buildLink);

                if (FastRead.Query<Data_Source>(a => a.LinkName == buildLink.LinkName).ToCount() == 1)
                {
                    FastWrite.Update<Data_Source>(buildLink, a => a.LinkName == buildLink.LinkName,
                        a => new { a.Host, a.PassWord, a.Port, a.ServerName, a.Type, a.UserName });
                }
                else
                    FastWrite.Add(buildLink);

                CodeBox.Show("连接数据库成功！", this);
            }
        }
        #endregion         

        #region 初始化连接信息
        /// <summary>
        /// 初始化连接信息
        /// </summary>
        private void InitLinkInfo()
        {
            BaseLink.DataToControls(AppCache.GetLink, ref txtHostName, ref txtUserName, ref txtPwd, ref txtPort, ref txtServerName, ref labServerName, ref txtLinkName);
            BaseLink.ComboBoxSelect(boxDbType, AppCache.GetLink.Type);
            BaseLink.ComboBoxSelect(dbTypeLink, AppCache.GetLink.LinkName, true);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Del_Click(object sender, RoutedEventArgs e)
        {
            var item = dbTypeLink.SelectedItem as Data_Source;
            if (item != null)
            {
                if (FastRead.Query<Data_Business_Details>(a => a.DataSourceId == item.Id).ToCount() == 0)
                {
                    FastWrite.Delete<Data_Source>(a => a.LinkName == item.LinkName);
                    AppCache.RemoveLink();
                    dbTypeLink.ItemsSource = AppCache.GetAllLink;
                    InitLinkInfo();
                    CodeBox.Show("删除成功", this);
                }
                else
                {
                    InitLinkInfo();
                    CodeBox.Show("删除失败数据源在使用中", this);
                }
            }
        }
        #endregion

        #region 加载数据源
        /// <summary>
        /// 加载数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            DataSchema.InitTable(AppCache.GetLink, true);
            CodeBox.Show("加载数据源成功！", this);
        }
        #endregion

        #region 更新数据源
        /// <summary>
        /// 更新数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateData_Click(object sender, RoutedEventArgs e)
        {
            DataSchema.InitTable(AppCache.GetLink, false);
            CodeBox.Show("更新数据源成功！", this);
        }
        #endregion 
    }
}
