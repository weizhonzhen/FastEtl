using FastEtlTool.Base;
using System;
using System.Windows;
using System.Windows.Forms;

namespace FastEtlTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {                        
            InitializeComponent();
            Common.InitWindows(this,"",true, ResizeMode.CanMinimize, WindowStartupLocation.CenterScreen);
            
            #region 托盘初始化
            var notifyIcon = new NotifyIcon();
            notifyIcon.BalloonTipText = FastApp.Config.Title;
            notifyIcon.Text = FastApp.Config.Title;

            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            notifyIcon.Visible = true;

            //打开菜单项
            var open = new MenuItem("打开 FastEtl");
            open.Click += new EventHandler(Show);

            //退出菜单项
           var exit = new MenuItem("退出 FastEtl");
            exit.Click += new EventHandler(Close);

            //关联托盘控件
            MenuItem[] childen = new MenuItem[] { open, exit };
            notifyIcon.ContextMenu = new ContextMenu(childen);

            //双击
            notifyIcon.MouseDoubleClick += new MouseEventHandler((o, e) => { if (e.Button == MouseButtons.Left) this.Show(o, e); });
            #endregion
        }

        #region 托盘方法
        #region 打开工具
        /// <summary>
        /// 打开工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Show(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            this.ShowInTaskbar = true;
            this.Activate();
        }
        #endregion

        #region 隐藏工具
        /// <summary>
        /// 打开工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hide(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Visibility = System.Windows.Visibility.Hidden;
        }
        #endregion

        #region 退出工具
        /// <summary>
        /// 退出工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        #endregion

        #region 重写关闭
        /// <summary>
        /// 重写关闭
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        #endregion

        #endregion

        #region 打开数据源
        /// <summary>
        /// 打开数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenDs(object sender, RoutedEventArgs e)
        {
            var dsForm = new DataSource();
            Common.OpenWin(dsForm, this);
        }
        #endregion

        #region 打开业务数据
        /// <summary>
        /// 打开业务数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenData(object sender, RoutedEventArgs e)
        {
            var dataForm = new Data();
            Common.OpenWin(dataForm, this);
        }
        #endregion

        #region 打开ETL策略
        /// <summary>
        /// 打开ETL策略
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenEtl(object sender, RoutedEventArgs e)
        {
            var etl = new SetEtl();
            Common.OpenWin(etl, this);
        }
        #endregion

        #region 打开字典
        /// <summary>
        /// 打开字典
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenDic(object sender, RoutedEventArgs e)
        {
            var dic = new DataDic();
            Common.OpenWin(dic, this);
        }
        #endregion
    }
}
