using FastData.Core.Property;
using System.ComponentModel.DataAnnotations;

namespace FastEtlWeb.DataModel
{
    /// <summary>
    /// 业务配置表详情表
    /// </summary>
    [Table(Comments = "业务配置表详情表")]
    public class Data_Business_Details
    {
        /// <summary>
        /// 业务明细Id
        /// </summary>
        [Column(Comments = "业务明细Id", DataType = "varchar2", Length = 64, IsKey = true)]
        public string FieldId { get; set; }

        /// <summary>
        /// 业务id
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "业务id")]
        [Column(Comments = "业务id", DataType = "varchar2", Length = 64,IsNull =false)]
        public string Id { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "字段名")]
        [Column(Comments = "字段名", DataType = "varchar2", Length = 16, IsNull =false)]
        public string FieldName { get; set; }

        /// <summary>
        /// 数据源id
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "数据源")]
        [Column(Comments = "数据源id", DataType = "varchar2", Length = 64, IsNull = false)]
        public string DataSourceId { get; set; }

        /// <summary>
        /// 源表名
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "源表名")]
        [Column(Comments = "源表名", DataType = "varchar2", Length = 32, IsNull = true)]
        public string TableName { get; set; }

        /// <summary>
        /// 源列名
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "源列名")]
        [Column(Comments = "源列名", DataType = "varchar2", Length = 32, IsNull = true)]
        public string ColumnName { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "主键")]
        [Column(Comments = "主键", DataType = "varchar2", Length = 32, IsNull = true)]
        public string Key { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column(Comments = "排序", DataType = "varchar2", Length = 32, IsNull = true)]
        public string OrderBy { get; set; }

        /// <summary>
        /// 字典
        /// </summary>
        [Column(Comments = "字典", DataType = "varchar2", Length = 64, IsNull = true)]
        public string Dic { get; set; }

        /// <summary>
        /// Sql
        /// </summary>
        [Column(Comments = "Sql", DataType = "varchar2", Length = 1024, IsNull = true)]
        public string Sql { get; set; }
    }
}
