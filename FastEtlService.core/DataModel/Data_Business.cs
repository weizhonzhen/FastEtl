using System;

namespace FastEtlService.core.DataModel
{
    /// <summary>
    /// 业务配置表 
    /// </summary>
    public class Data_Business
    {
        /// <summary>
        /// 业务id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 业务表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 业务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 更新时间(点)
        /// </summary>
        public decimal UpdateTime { get; set; }

        /// <summary>
        /// 更新频率(天)
        /// </summary>
        public decimal UpdateDay { get; set; }

        /// <summary>
        /// 抽取条数(万)
        /// </summary>
        public decimal UpdateCount { get; set; }

        /// <summary>
        /// 上次更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 增量数据存放年
        /// </summary>
        public decimal SaveDataMonth { get; set; }

        /// <summary>
        /// 关联主键策略(1=重复删除,0=重复保留，2=重复更新)
        /// </summary>
        public string Policy { get; set; }
    }
}
