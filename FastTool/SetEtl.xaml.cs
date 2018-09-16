using FastTool.Base;
using FastModel.DataModel;
using System.Collections.Generic;
using System.Windows;
using FastData;
using System.Windows.Controls;
using FastUntility.Base;

namespace FastTool
{
    /// <summary>
    /// SetEtl.xaml 的交互逻辑
    /// </summary>
    public partial class SetEtl : Window
    {
        public SetEtl()
        {
            InitializeComponent();
            Common.InitWindows(this, "ETL策略", false);

            if (AppCache.GetLink.Host != null)
                Bussiness.ItemsSource = AppCache.GetAllBusiness;

            InitUpdateCount();
            InitUpdateDay();
            InitUpdateTime();
            InitDataMonth();
            InitIsDel();
        }

        #region 显示
        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bussiness_Selected(object sender, RoutedEventArgs e)
        {
            var item = (sender as DataGrid).SelectedItem as Data_Business;

            if (item != null)
            {
                var list= UpdateTime.ItemsSource as List<BindModel>;
                var temp = list.Find(a => a.value == item.UpdateTime);
                UpdateTime.SelectedItem = temp;

                list = UpdateDay.ItemsSource as List<BindModel>;
                temp = list.Find(a => a.value == item.UpdateDay);
                UpdateDay.SelectedItem = temp;

                list = UpdateCount.ItemsSource as List<BindModel>;
                temp = list.Find(a => a.value == item.UpdateCount);
                UpdateCount.SelectedItem = temp;
                
                list = DataMonth.ItemsSource as List<BindModel>;
                temp = list.Find(a => a.value == item.SaveDataMonth);
                DataMonth.SelectedItem = temp;

                list = IsDel.ItemsSource as List<BindModel>;
                temp = list.Find(a => a.value == item.Policy.ToInt(0));
                IsDel.SelectedItem = temp;
            }
            else
            {
                InitUpdateCount();
                InitUpdateDay();
                InitUpdateTime();
                InitDataMonth();
                InitIsDel();
            }
        }
        #endregion

        #region 初始化更新时间
        /// <summary>
        /// 初始化更新时间
        /// </summary>
        private void InitUpdateTime()
        {
            var list = new List<BindModel>();
            for (var i=0;i < 24;i++)
            {
                var item = new BindModel();
                item.key = string.Format("{0} 点",i);
                item.value = i;
                list.Add(item);
            }

            UpdateTime.ItemsSource = list;            
        }
        #endregion

        #region 初始化更新天数
        /// <summary>
        /// 初始化更新天数
        /// </summary>
        private void InitUpdateDay()
        {
            var list = new List<BindModel>();
            for (var i = 1; i <= 7; i++)
            {
                var item = new BindModel();
                item.key = string.Format("{0} 天", i);
                item.value = i;
                list.Add(item);
            }

            UpdateDay.ItemsSource = list;
        }
        #endregion

        #region 初始化关键主键
        /// <summary>
        /// 初始化关键主键
        /// </summary>
        private void InitIsDel()
        {
            var list = new List<BindModel>();
            var item = new BindModel();            
            item.key = "重复保留";
            item.value = 0;
            list.Add(item);

            var info = new BindModel();
            info.key = "重复删除";
            info.value = 1;
            list.Add(info);
            
            var temp = new BindModel();
            temp.key = "重复更新";
            temp.value = 2;
            list.Add(temp);

            IsDel.ItemsSource = list;
        }
        #endregion

        #region 初始化批量条数
        /// <summary>
        /// 初始化批量条数
        /// </summary>
        private void InitUpdateCount()
        {
            var list = new List<BindModel>();
            for (var i = 1; i <= 10; i++)
            {
                var item = new BindModel();
                item.key = string.Format("{0} 万条", i);
                item.value = i;
                list.Add(item);
            }

            UpdateCount.ItemsSource = list;
        }
        #endregion

        #region 初始化增量数据存放月数
        /// <summary>
        /// 初始化增量数据存放月数
        /// </summary>
        private void InitDataMonth()
        {
            var list = new List<BindModel>();
            for (var i = 1; i <= 24; i++)
            {
                var item = new BindModel();
                item.key = string.Format("{0} 个月", i);
                item.value = i;
                list.Add(item);
            }
            
            DataMonth.ItemsSource = list;
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_SaveData(object sender, RoutedEventArgs e)
        {
            var item = Bussiness.SelectedItem as Data_Business;

            if (item == null)
                CodeBox.Show("请求选择业务", this);
            else
            {
                item.UpdateDay = (UpdateDay.SelectedItem as BindModel).value;
                item.UpdateCount = (UpdateCount.SelectedItem as BindModel).value;
                item.UpdateTime = (UpdateTime.SelectedItem as BindModel).value;
                item.SaveDataMonth = (DataMonth.SelectedItem as BindModel).value;
                item.Policy = (IsDel.SelectedItem as BindModel).value.ToStr();

                var isSuccess = FastWrite.Update<Data_Business>(item, a => a.Id == item.Id, a => new { a.UpdateCount, a.UpdateTime, a.UpdateDay,a.SaveDataMonth,a.Policy }).IsSuccess;
                if (isSuccess)
                {
                    Bussiness.ItemsSource = AppCache.GetAllBusiness;
                    Common.UpdateWindow();
                    CodeBox.Show("保存成功", this);
                }
                else
                    CodeBox.Show("保存失败", this);
            }
        }
        #endregion

        private class BindModel
        {
            public string key { get; set; }

            public int value { get; set; }
        }
    }    
}
