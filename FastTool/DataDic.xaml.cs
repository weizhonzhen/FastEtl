using FastModel.DataModel;
using FastTool.Base;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using FastData;
using FastData.Context;
using System;

namespace FastTool
{
    /// <summary>
    /// DataDic.xaml 的交互逻辑
    /// </summary>
    public partial class DataDic : Window
    {
        public DataDic()
        {
            InitializeComponent();

            Common.InitWindows(this, "字典对照", false);

            InitDic();
        }
        
        #region 全选
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxAll_dicClick(object sender, RoutedEventArgs e)
        {
            Common.CheckAllBox((sender as CheckBox), Dic, "dicBox");
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
            using (var db = new DataContext())
            {
                var success = false;
                var main = Dic.SelectedItem as Data_Dic;
                if (string.IsNullOrEmpty(main.Id))
                {
                    main.Id = Guid.NewGuid().ToString();
                    success = FastWrite.Add(main).IsSuccess;
                }
                else
                   success = FastWrite.Update(main, a => a.Id == main.Id, a => new { a.Name }).IsSuccess;
                
                foreach (var item in DicLeaf.Items)
                {
                    var temp = item as Data_Dic_Details;

                    if (temp == null)
                        continue;

                    temp.DicId = main.Id;

                    if (temp != null)
                    {
                        if (string.IsNullOrEmpty(temp.Value))
                        {
                            CodeBox.Show("字典值不能为空", this);
                            continue;
                        }
                        if (string.IsNullOrEmpty(temp.ContrastValue))
                        {
                            CodeBox.Show("字典对照值不能为空", this);
                            continue;
                        }

                        if (string.IsNullOrEmpty(temp.Id))
                        {
                            temp.Id = Guid.NewGuid().ToString();
                            if (success)
                                success = db.Add(temp).writeReturn.IsSuccess;
                        }
                        else
                            success = db.Update(temp, a => a.Id == temp.Id, a => new { a.Name, a.Value, a.ContrastValue }).writeReturn.IsSuccess;
                    }
                }
            }
            
            CodeBox.Show("保存成功", this);
        }
        #endregion

        #region 删除字典类型
        /// <summary>
        /// 删除字典类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Del_Dic_Click(object sender, RoutedEventArgs e)
        {
            var success = false;
            var item = Dic.SelectedItem as Data_Dic;

            if (!string.IsNullOrEmpty(item.Id))
                success = FastWrite.Delete<Data_Dic>(a => a.Id == item.Id).IsSuccess;

            if (success)
            {
                InitDic();
                CodeBox.Show("删除成功", this);
            }
            else
                CodeBox.Show("删除失败", this);
        }
        #endregion

        #region 删除字典明细
        /// <summary>
        /// 删除字典明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Del_DicLeaf_Click(object sender, RoutedEventArgs e)
        {
            var count = 0;
            foreach (var item in Dic.Items)
            {
                var box = Common.GetTemplateColumn<CheckBox>(Dic, 0, "dicBox", item);
                if (box != null && box.IsChecked == true)
                {
                    var temp = item as Data_Dic_Details;
                    if (temp != null && !string.IsNullOrEmpty(temp.Id))
                        FastWrite.Delete<Data_Dic_Details>(a => a.Id == temp.Id);
                    count++;
                }
            }

            if (count == 0)
                CodeBox.Show("请选择字典明细", this);
            else
            {
                InitDic();
                CodeBox.Show("删除成功", this);
            }
        }
        #endregion

        #region 初始化字典数据
        /// <summary>
        /// 初始化字典数据
        /// </summary>
        private void InitDic()
        {
            var list = FastRead.Query<Data_Dic>(a => a.Id != null).ToList<Data_Dic>();
            list.Add(new Data_Dic());
            Dic.ItemsSource = list;

            var tempList = new List<Data_Dic_Details>();
            tempList.Add(new Data_Dic_Details());
            DicLeaf.ItemsSource = tempList;
        }
        #endregion

        #region 选中数据源
        /// <summary>
        /// 选中数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dic_Selected(object sender, RoutedEventArgs e)
        {
            var item = Dic.SelectedItem as Data_Dic;

            if (item != null && !string.IsNullOrEmpty(item.Id))
            {
                var list = FastRead.Query<Data_Dic_Details>(a => a.DicId == item.Id).ToList<Data_Dic_Details>();
                list.Add(new Data_Dic_Details());
                DicLeaf.ItemsSource = list;
            }
            else
            {
                var list = new List<Data_Dic_Details>();
                list.Add(new Data_Dic_Details());
                DicLeaf.ItemsSource = list;
            }
        }
        #endregion
    }
}
