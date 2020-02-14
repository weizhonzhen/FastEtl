using FastData.Core.Property;
using System;
using System.ComponentModel.DataAnnotations;

namespace FastEtlWeb.DataModel
{
    /// <summary>
    /// 业务配置表 
    /// </summary>
    [Table(Comments = "业务配置表")]
    public class Data_Business
    {
        /// <summary>
        /// 业务id
        /// </summary>
        [StringLength(64, ErrorMessage = "{0}64")]
        [Display(Name = "业务id")]
        [Column(Comments = "业务id", DataType = "varchar2", Length = 64, IsKey = true)]
        public string Id { get; set; }

        /// <summary>
        /// 业务表名
        /// </summary>
        [StringLength(64, ErrorMessage = "{0}64")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "业务表名")]
        [Column(Comments = "业务表名", DataType = "varchar2", Length = 64, IsNull =false)]
        public string TableName { get; set; }

        /// <summary>
        /// 业务名称
        /// </summary>
        [StringLength(16, ErrorMessage = "{0}最大长度16")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "业务名称")]
        [Column(Comments = "业务名称", DataType = "varchar2", Length = 16, IsNull = false)]
        public string Name { get; set; }

        /// <summary>
        /// 更新时间(点)
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "更新时间")]
        [Column(Comments = "更新时间(点)", DataType = "number(2)",IsNull = false)]
        public decimal UpdateTime { get; set; }

        /// <summary>
        /// 更新频率(天)
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "更新频率")]
        [Column(Comments = "更新频率(天)", DataType = "number(2)",IsNull = false)]
        public decimal UpdateDay { get; set; }

        /// <summary>
        /// 抽取条数(万)
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "抽取条数")]
        [Column(Comments = "抽取条数(万)", DataType = "number(3)", IsNull = false)]
        public decimal UpdateCount { get; set; }

        /// <summary>
        /// 上次更新时间
        /// </summary>
        [Column(Comments = "上次更新时间", DataType = "date", IsNull = false)]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 增量数据存放年
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "增量数据存放年")]
        [Column(Comments = "增量数据存放年", DataType = "number(3)", IsNull = false)]
        public decimal SaveDataMonth { get; set; }

        /// <summary>
        /// 关联主键策略(1=重复删除,0=重复保留，2=重复更新)
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "关联主键策略")]
        [Column(Comments = "关联主键策略(1=重复删除,0=重复保留，2=重复更新)", DataType = "varchar2", Length = 1, IsNull = false)]
        public string Policy { get; set; }
    }
}
