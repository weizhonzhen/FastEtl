using FastData.Property;
using System;

namespace FastEtlModel.DataModel
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
        [Column(Comments = "业务id", DataType = "varchar2", Length = 64, IsKey = true)]
        public string Id { get; set; }

        /// <summary>
        /// 业务表名
        /// </summary>
        [Column(Comments = "业务表名", DataType = "varchar2", Length = 64, IsNull =false)]
        public string TableName { get; set; }

        /// <summary>
        /// 业务名称
        /// </summary>
        [Column(Comments = "业务名称", DataType = "varchar2", Length = 16, IsNull = false)]
        public string Name { get; set; }

        /// <summary>
        /// 更新时间(点)
        /// </summary>
        [Column(Comments = "更新时间(点)", DataType = "number(2)",IsNull = false)]
        public decimal UpdateTime { get; set; }

        /// <summary>
        /// 更新频率(天)
        /// </summary>
        [Column(Comments = "更新频率(天)", DataType = "number(2)",IsNull = false)]
        public decimal UpdateDay { get; set; }

        /// <summary>
        /// 抽取条数(万)
        /// </summary>
        [Column(Comments = "抽取条数(万)", DataType = "number(3)", IsNull = false)]
        public decimal UpdateCount { get; set; }

        /// <summary>
        /// 上次更新时间
        /// </summary>
        [Column(Comments = "上次更新时间", DataType = "date", IsNull = false)]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 增量数据存放月数
        /// </summary>
        [Column(Comments = "增量数据存放月数", DataType = "number(3)", IsNull = false)]
        public decimal SaveDataMonth { get; set; }

        /// <summary>
        /// 关联主键策略(1=重复删除,0=重复保留，2=重复更新)
        /// </summary>
        [Column(Comments = "关联主键策略(1=重复删除,0=重复保留，2=重复更新)", DataType = "varchar2", Length = 1, IsNull = false)]
        public string Policy { get; set; }
    }
}
