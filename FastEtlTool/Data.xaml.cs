using FastEtlTool.Base;
using FastEtlModel.DataModel;
using System.Collections.Generic;
using System.Windows;
using System;
using FastData.Context;
using FastData;
using System.Windows.Controls;
using FastEtlModel.CacheModel;
using FastUntility.Base;
using System.Threading.Tasks;

namespace FastEtlTool
{
    /// <summary>
    /// Data.xaml 的交互逻辑
    /// </summary>
    public partial class Data : Window
    {
        public Data()
        {
            InitializeComponent();
            Common.InitWindows(this, "业务数据集", false);

            Bussiness.ItemsSource = AppCache.GetAllBusiness;
        }

        #region 保存业务配置
        /// <summary>
        /// 保存业务配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_SaveData(object sender, RoutedEventArgs e)
        {
            using (var db = new DataContext())
            {
                var isUpdateTable = false;
                var isSuccess = true;
                db.BeginTrans();
                var main = Bussiness.SelectedItem as Data_Business;
                
                if (main == null||string.IsNullOrEmpty(main.Name))
                {
                    CodeBox.Show("业务名称不能为空", this);
                    return;
                }

                if (string.IsNullOrEmpty(main.TableName))
                {
                    CodeBox.Show("业务表名不能为空", this);
                    return;
                }
                
                #region 业务
                if (string.IsNullOrEmpty(main.Id))
                {
                    //默认每天更新
                    main.UpdateDay = 1;

                    //默认晚上2点更新
                    main.UpdateTime = 2;

                    //默认更新条数1万
                    main.UpdateCount = 1;

                    //默认上次更新时间
                    main.LastUpdateTime = DateTime.Now;

                    //关联主键策略(1=重复删除,0=重复保留，2=重复更新)
                    main.Policy = "1";

                    //增加业务
                    main.Id = Guid.NewGuid().ToString();
                    isSuccess = db.Add(main).writeReturn.IsSuccess;

                    //创建表
                    if (isSuccess)
                        isSuccess = DataSchema.CreateTable(db, main);

                    //表备注
                    if (isSuccess)
                        isSuccess = DataSchema.UpdateTableComment(db, main);
                }
                else
                {
                    //修改业务
                    if (isSuccess)
                    {
                        var oldTableName = FastRead.Query<Data_Business>(a => a.Id == main.Id, a => new { a.Name }).ToDic(db).GetValue("name").ToString();
                        isSuccess = db.Update<Data_Business>(main, a => a.Id == main.Id, a => new {a.Name }).writeReturn.IsSuccess;

                        if (oldTableName != main.TableName)
                        {
                            if (DataSchema.IsExistsTable(db, oldTableName))
                            {
                                isUpdateTable = true;

                                //修改表名
                                if (isSuccess)
                                    isSuccess = DataSchema.UpdateTableName(db, main, oldTableName);

                                //修改表备注
                                if (isSuccess)
                                    isSuccess = DataSchema.UpdateTableComment(db, main);
                            }
                            else
                                DataSchema.CreateTable(db, main);
                        }
                    }
                }
                #endregion

                #region 业务明细
                foreach (var temp in BussinessDetails.Items)
                {
                    var leaf = temp as Data_Business_Details;

                    if (leaf == null)
                        continue;

                    if (string.IsNullOrEmpty(leaf.FieldName))
                        continue;

                    //数据源id
                    if (Common.GetTemplateColumn<ComboBox>(BussinessDetails, 1, "DataSourceBox", temp).SelectedItem == null)
                        continue;
                    var dataSource = (Common.GetTemplateColumn<ComboBox>(BussinessDetails, 1, "DataSourceBox", temp).SelectedItem as Data_Source);
                    leaf.DataSourceId = dataSource.Id;

                    //表名
                    if (Common.GetTemplateColumn<ComboBox>(BussinessDetails, 2, "TabelBox", temp).SelectedItem != null)                       
                        leaf.TableName = (Common.GetTemplateColumn<ComboBox>(BussinessDetails, 2, "TabelBox", temp).SelectedItem as Cache_Table).Name;

                    //源列名
                    var columnInfo = new Cache_Column();
                    if (Common.GetTemplateColumn<ComboBox>(BussinessDetails, 3, "ColumnBox", temp).SelectedItem != null)
                    {
                        columnInfo = Common.GetTemplateColumn<ComboBox>(BussinessDetails, 3, "ColumnBox", temp).SelectedItem as Cache_Column;
                        leaf.ColumnName = columnInfo.Name;
                    }

                    //主键
                    if (Common.GetTemplateColumn<ComboBox>(BussinessDetails, 4, "KeyBox", temp).SelectedItem != null)
                    {
                        columnInfo = Common.GetTemplateColumn<ComboBox>(BussinessDetails, 4, "KeyBox", temp).SelectedItem as Cache_Column;
                        leaf.Key = columnInfo.Name;
                    }

                    //排序
                    if (Common.GetTemplateColumn<ComboBox>(BussinessDetails, 5, "OrderByBox", temp).SelectedItem != null)
                    {
                        columnInfo = Common.GetTemplateColumn<ComboBox>(BussinessDetails, 5, "OrderByBox", temp).SelectedItem as Cache_Column;
                        leaf.OrderBy = columnInfo.Name;
                    }

                    //字典
                    if (Common.GetTemplateColumn<ComboBox>(BussinessDetails, 6, "DicBox", temp).SelectedItem != null)
                    {
                        var dic = Common.GetTemplateColumn<ComboBox>(BussinessDetails, 6, "DicBox", temp).SelectedItem as Data_Dic;
                        if (dic != null && !string.IsNullOrEmpty(dic.Id))
                            leaf.Dic = dic.Id;
                    }

                    //sql
                    leaf.Sql = (temp as Data_Business_Details).Sql;

                    if (string.IsNullOrEmpty(leaf.FieldId))
                    {
                        leaf.FieldId = Guid.NewGuid().ToString();
                        leaf.Id = main.Id;

                        //增加业务明细
                        if (isSuccess)
                            isSuccess = db.Add(leaf).writeReturn.IsSuccess;

                        //增加列
                        columnInfo = Common.GetTemplateColumn<ComboBox>(BussinessDetails, 3, "ColumnBox", temp).SelectedItem as Cache_Column;
                        if (isSuccess)
                            isSuccess = DataSchema.AddColumn(db, main, leaf, columnInfo, dataSource);

                        //修改备注
                        if (isSuccess)
                            isSuccess = DataSchema.UpdateColumnComment(db, main, leaf, columnInfo, dataSource);
                    }
                    else
                    {
                        //修改业务明细
                        if (isSuccess)
                            isSuccess = db.Update<Data_Business_Details>(leaf, a => a.FieldId == leaf.FieldId, a => new { a.FieldName, a.DataSourceId, a.ColumnName, a.Key,a.OrderBy, a.TableName,a.Dic,a.Sql }).writeReturn.IsSuccess;

                        //列增加修改
                        if (isSuccess)
                        {
                            if (DataSchema.IsExistsColumn(db, main.Name, leaf.FieldName))
                                isSuccess = DataSchema.UpdateColumn(db, main, leaf, columnInfo, dataSource);
                            else
                                isSuccess = DataSchema.AddColumn(db, main, leaf, columnInfo, dataSource);
                        }
                    }
                }
                #endregion

                if (isSuccess)
                {
                    if (isUpdateTable)
                    {
                        Bussiness.ItemsSource = AppCache.GetAllBusiness;
                        Common.UpdateWindow();
                    }

                    db.SubmitTrans();
                    CodeBox.Show("保存业务成功", this);
                }
                else
                {
                    db.RollbackTrans();
                    CodeBox.Show("保存业务失败", this);
                }
            }
        }
        #endregion

        #region 删除业务配置
        /// <summary>
        /// 删除业务配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_DelData(object sender, RoutedEventArgs e)
        {
            var main = Bussiness.SelectedItem as Data_Business;

            if (main == null || string.IsNullOrEmpty(main.Name))
            {
                CodeBox.Show("请选择业务", this);
                return;
            }

            using (var db = new DataContext())
            {
                var isSuccess = true;
                db.BeginTrans();
                isSuccess = db.Delete<Data_Business>(a => a.Id == main.Id).writeReturn.IsSuccess;

                if (isSuccess)
                    isSuccess = db.Delete<Data_Business_Details>(a => a.Id == main.Id).writeReturn.IsSuccess;

                if (isSuccess)
                    isSuccess = db.ExecuteSql(string.Format("drop table {0}", main.TableName), null, false).writeReturn.IsSuccess;

                if (isSuccess)
                {
                    db.SubmitTrans();
                    Bussiness.ItemsSource = AppCache.GetAllBusiness;
                    Common.UpdateWindow();

                    CodeBox.Show("删除业务成功", this);
                }
                else
                {
                    db.RollbackTrans();
                    CodeBox.Show("删除业务失败", this);
                }
            }
        }
        #endregion

        #region 选中数据源
        /// <summary>
        /// 选中数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataSource_Selected(object sender, RoutedEventArgs e)
        {
            BindComboBox(BussinessDetails.SelectedItem, BussinessDetails, true);
        }
        #endregion

        #region 选中数据源-表
        /// <summary>
        /// 选中数据源-表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Table_Selected(object sender, RoutedEventArgs e)
        {
            BindComboBox(BussinessDetails.SelectedItem, BussinessDetails, false, true);
        }
        #endregion
       
        #region 显示业务配置详情
        /// <summary>
        /// 显示业务配置详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bussiness_Selected(object sender, RoutedEventArgs e)
        {
            var item = (sender as DataGrid).SelectedItem as Data_Business;
            if (item != null)
            {
                var list = AppCache.GetBusinessDetails(item.Id);

                if (list.Count == 0)
                    list.Add(new Data_Business_Details());

                BussinessDetails.ItemsSource = list;
                Common.UpdateWindow();
                var taskList = new List<Task>();
                foreach (var temp in BussinessDetails.Items)
                {
                    taskList.Add(Task.Factory.StartNew(() =>
                    {
                        BindComboBox(temp, BussinessDetails);
                    }));
                }
                Task.WaitAll(taskList.ToArray());

                foreach (var temp in BussinessDetails.Items)
                {
                    BussinessDetails.SelectedItem = temp;
                }
            }
            else
            {
                var list = new List<Data_Business_Details>();
                list.Add(new Data_Business_Details());
                BussinessDetails.ItemsSource = list;
            }
        }
        #endregion

        #region 初始化业务配置详情
        /// <summary>
        /// 初始化业务配置详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BussinessDetails_Selected(object sender, RoutedEventArgs e)
        {
            var item = (sender as DataGrid).SelectedItem as Data_Business_Details??new Data_Business_Details();
            if (item != null)
            {
                BindComboBox(item, BussinessDetails);
            }
        }
        #endregion

        #region 绑定datagrid中combobox
        /// <summary>
        /// 绑定datagrid中combobox
        /// </summary>
        private void BindComboBox(object selectedItem, DataGrid grid, bool isDataSource = false, bool isTable = false)
        {
            var item = selectedItem as Data_Business_Details ?? new Data_Business_Details();

            if (item == null)
                return;

            //数据源
            var dataBox = Common.GetTemplateColumn<ComboBox>(grid, 1, "DataSourceBox", selectedItem);
            if (dataBox != null && dataBox.ItemsSource == null)
            {
                var itemSource = AppCache.GetAllLink;
                dataBox.ItemsSource = itemSource;
                dataBox.SelectedItem = itemSource.Find(a => a.Id == item.DataSourceId);
            }

            Common.UpdateWindow();

            //表名
            var tableBox = Common.GetTemplateColumn<ComboBox>(grid, 2, "TabelBox", selectedItem);

            if (isDataSource && tableBox != null)
                tableBox.ItemsSource = null;

            if (tableBox != null && tableBox.ItemsSource == null)
            {
                if (dataBox.SelectedItem != null)
                {
                    var itemSource = AppCache.GetTableList(dataBox.SelectedItem as Data_Source) ?? new List<Cache_Table>();

                    if (itemSource.Count == 0)
                        itemSource.Add(new Cache_Table { Name="请加载数据源" });

                    tableBox.ItemsSource = itemSource;
                    tableBox.SelectedItem = itemSource.Find(a => a.Name == item.TableName);
                }
            }

            Common.UpdateWindow();

            //列名
            var columnBox = Common.GetTemplateColumn<ComboBox>(grid, 3, "ColumnBox", selectedItem);

            if (isTable && columnBox != null)
                columnBox.ItemsSource = null;

            if (columnBox != null && columnBox.ItemsSource == null)
            {
                if (tableBox.SelectedItem != null)
                {
                    var itemSource = AppCache.GetColumnList(dataBox.SelectedItem as Data_Source, (tableBox.SelectedItem as Cache_Table).Name);
                    columnBox.ItemsSource = itemSource;
                    columnBox.SelectedItem = itemSource.Find(a => a.Name == item.ColumnName);
                }
            }

            //主键
            var keyBox = Common.GetTemplateColumn<ComboBox>(grid, 4, "KeyBox", selectedItem);

            if (isTable && keyBox != null)
                keyBox.ItemsSource = null;

            if (keyBox != null && keyBox.ItemsSource == null)
            {
                if (tableBox.SelectedItem != null)
                {
                    var itemSource = AppCache.GetColumnList(dataBox.SelectedItem as Data_Source, (tableBox.SelectedItem as Cache_Table).Name);
                    keyBox.ItemsSource = itemSource;
                    keyBox.SelectedItem = itemSource.Find(a => a.Name == item.Key);
                }
            }

            //排序
            var orderByBox = Common.GetTemplateColumn<ComboBox>(grid, 5, "OrderByBox", selectedItem);

            if (isTable && orderByBox != null)
                orderByBox.ItemsSource = null;

            if (orderByBox != null && orderByBox.ItemsSource == null)
            {
                if (tableBox.SelectedItem != null)
                {
                    var itemSource = AppCache.GetColumnList(dataBox.SelectedItem as Data_Source, (tableBox.SelectedItem as Cache_Table).Name);
                    orderByBox.ItemsSource = itemSource;
                    orderByBox.SelectedItem = itemSource.Find(a => a.Name == item.OrderBy);
                }
            }

            //字典
            var dicBox = Common.GetTemplateColumn<ComboBox>(grid, 6, "DicBox", selectedItem);

            if (isTable && dicBox != null)
                dicBox.ItemsSource = null;

            if (dicBox != null && dicBox.ItemsSource == null)
            {
                if (tableBox.SelectedItem != null)
                {
                    var tempSource = FastRead.Query<Data_Dic>(a => a.Id != null).ToList<Data_Dic>();
                    dicBox.ItemsSource = tempSource;
                    dicBox.SelectedItem = tempSource.Find(a => a.Id == item.Dic);
                }
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
