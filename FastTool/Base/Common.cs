using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FastEtlTool.Base
{
    /// <summary>
    /// 公用类
    /// </summary>
    public static class Common
    {   
        #region checkbox全选
        /// <summary>
        /// checkbox全选
        /// </summary>
        /// <param name="box"></param>
        /// <param name="grid"></param>
        public static void CheckAllBox(CheckBox box,DataGrid grid,string colName)
        {
            var tempCol = grid.Columns[0] as DataGridTemplateColumn;

            foreach (var item in grid.Items)
            {
                var element = grid.Columns[0].GetCellContent(item);
                if (element != null)
                {
                    var iBox = tempCol.CellTemplate.FindName(colName, element) as CheckBox;
                    if (iBox != null)
                        iBox.IsChecked = box.IsChecked;
                }
            }
        }
        #endregion
                        
        #region 选择一行
        /// <summary>
        /// 选择一行
        /// </summary>
        /// <param name="datagrid"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public static void GetDataGridRow(DataGrid datagrid, int rowIndex,bool isCheck=true)
        {
            var row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            if (row != null)
            {
                datagrid.UpdateLayout();
                row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
                row.IsSelected = isCheck;
            }
        }
        #endregion
                
        #region 获取datagrid中的自定义对象单元格
        /// <summary>
        /// 获取datagrid中的自定义对象单元格
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grid">DataGrid</param>
        /// <param name="columnId">列数</param>
        /// <param name="templateName">自定义列名</param>
        /// <param name="item">datagrid 行</param>
        /// <returns></returns>
        public static T GetTemplateColumn<T>(DataGrid grid, int columnId, string templateName, object item) where T : class,new()
        {
            try
            {
                if (item == null)
                    return null;

                var tempCol = grid.Columns[columnId] as DataGridTemplateColumn;
                var element = grid.Columns[columnId].GetCellContent(item);

                if (element != null)
                    return tempCol.CellTemplate.FindName(templateName, element) as T;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region datagrid 排序
        /// <summary>
        /// datagrid 排序
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sortFiled"></param>
        /// <param name="sort"></param>
        public static void DataGridSort(DataGrid grid,string sortFiled,ListSortDirection sort)
        {
            var view = CollectionViewSource.GetDefaultView(grid.ItemsSource);
            view.SortDescriptions.Clear();
            var sd = new SortDescription(sortFiled, sort);
            view.SortDescriptions.Add(sd);
        }
        #endregion

        #region 弹出窗口任务栏不显示并指定父窗口
        /// <summary>
        /// 弹出窗口任务栏不显示并指定父窗口
        /// </summary>
        /// <param name="win">新窗体</param>
        /// <param name="owner">拥有者</param>
        /// <param name="isCloseOwner">是否关闭父窗休</param>
        /// <param name="isOwnerOwer">是否父窗体的拥有者</param>
        public static void OpenWin(Window win, Window owner, bool isCloseOwner = false, bool isOwnerOwer = false)
        {
            try
            {
                if (isOwnerOwer)
                    win.Owner = owner.Owner;
                else
                    win.Owner = owner;

                if (isCloseOwner)
                    owner.Close();

                win.ShowInTaskbar = false;
                win.ShowDialog();
            }
            catch(Exception ex)
            {
                //log.SaveLog(ex.ToString(), "openWin_exp");
            }
        }
        #endregion

        #region 实时更新窗体
        /// <summary>
        /// 实时更新窗体
        /// </summary>
        public static void UpdateWindow()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(delegate(object f)
                {
                    ((DispatcherFrame)f).Continue = false;
                    return null;
                }), frame);

            Dispatcher.PushFrame(frame);
        }
        #endregion

        #region 弹出选择目录对话框
        /// <summary>
        /// 弹出选择目录对话框
        /// </summary>
        /// <returns></returns>
        public static string FolderBrowserDialog()
        {
            var fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != string.Empty)
                return fbd.SelectedPath;
            else
                return "";
        }
        #endregion
                                        
        #region 定义窗口类型
        /// <summary>
        /// 定义窗口类型
        /// </summary>
        /// <param name="win"></param>
        public static void InitWindows(Window win,string title="", bool IsBackground = true, ResizeMode rm = ResizeMode.NoResize, WindowStartupLocation wl = WindowStartupLocation.CenterOwner)
        {
            if (title == "")
                win.Title = FastApp.Config.Title;
            else
                win.Title = title;

            var imgUrl = string.Format("{0}{1}",AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", "") , "img/logo.ico");
            var bgUrl = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", ""), "img/bg.jpg");
            win.WindowStartupLocation = wl;
            win.ResizeMode = rm;
            var uri = new Uri(imgUrl, UriKind.RelativeOrAbsolute);
            win.Icon = BitmapFrame.Create(uri);
            if (IsBackground)
                win.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(bgUrl)) };
            else
                win.Background = (Brush)TypeDescriptor.GetConverter(typeof(Brush)).ConvertFromInvariantString("#efefef");
        }
        #endregion

    }
}
