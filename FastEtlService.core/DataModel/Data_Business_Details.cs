namespace FastEtlService.core.DataModel
{
    /// <summary>
    /// 业务配置表详情表
    /// </summary>
    public class Data_Business_Details
    {
        /// <summary>
        /// 业务明细Id
        /// </summary>
        public string FieldId { get; set; }

        /// <summary>
        /// 业务id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 数据源id
        /// </summary>
        public string DataSourceId { get; set; }

        /// <summary>
        /// 源表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 源列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// 字典
        /// </summary>
        public string Dic { get; set; }

        /// <summary>
        /// Sql
        /// </summary>
        public string Sql { get; set; }
    }
}
