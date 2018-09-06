using FastData.Property;

namespace FastModel.DataModel
{
    /// <summary>
    /// 业务配置表详情表
    /// </summary>
    [Table(Comments = "业务配置表详情表")]
    public class Base_Business_Details
    {
        /// <summary>
        /// 业务明细Id
        /// </summary>
        [Column(Comments = "业务明细Id", DataType = "varchar2", Length = 64, IsKey = true)]
        public string FieldId { get; set; }

        /// <summary>
        /// 业务id
        /// </summary>
        [Column(Comments = "业务id", DataType = "varchar2", Length = 64,IsNull =false)]
        public string Id { get; set; }
        
        /// <summary>
        /// 字段名
        /// </summary>
        [Column(Comments = "字段名", DataType = "varchar2", Length = 16, IsNull =false)]
        public string FieldName { get; set; }

        /// <summary>
        /// 数据源id
        /// </summary>
        [Column(Comments = "数据源id", DataType = "varchar2", Length = 64, IsNull = false)]
        public string DataSourceId { get; set; }

        /// <summary>
        /// 源表名
        /// </summary>
        [Column(Comments = "源表名", DataType = "varchar2", Length = 32, IsNull = true)]
        public string TableName { get; set; }

        /// <summary>
        /// 源列名
        /// </summary>
        [Column(Comments = "源列名", DataType = "varchar2", Length = 32, IsNull = true)]
        public string ColumnName { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [Column(Comments = "主键", DataType = "varchar2", Length = 32, IsNull = true)]
        public string Key { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column(Comments = "排序", DataType = "varchar2", Length = 32, IsNull = true)]
        public string OrderBy { get; set; }

        /// <summary>
        /// 是否字典(1=是,0=否)
        /// </summary>
        [Column(Comments = "是否字典(1=是,0=否)", DataType = "varchar2", Length = 1, IsNull = true)]
        public string IsDic { get; set; }

        /// <summary>
        /// Sql
        /// </summary>
        [Column(Comments = "Sql", DataType = "varchar2", Length = 1024, IsNull = true)]
        public string Sql { get; set; }
    }
}
