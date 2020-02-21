using FastData.Core.Property;
using System;

namespace FastEtlService.core.DataModel
{
    /// <summary>
    /// 日志表
    /// </summary>
    [Table(Comments = "日志表")]
    public class Data_Log
    {
        /// <summary>
        /// 日记id
        /// </summary>
        [Column(Comments = "日记id", DataType = "varchar2", Length = 64, IsKey = true)]
        public string Id { get; set; }

        /// <summary>
        /// 业务表名
        /// </summary>
        [Column(Comments = "业务表名", DataType = "varchar2", Length = 32, IsNull = false)]
        public string TableName { get; set; }

        /// <summary>
        /// 开始抽取时间
        /// </summary>
        [Column(Comments = "开始抽取时间", DataType = "date", IsNull = false)]
        public DateTime BeginDateTime { get; set; }

        /// <summary>
        /// 结束抽取时间
        /// </summary>
        [Column(Comments = "结束抽取时间", DataType = "date", IsNull = false)]
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// 状态(1=成功,0=失败)
        /// </summary>
        [Column(Comments = "状态(1=成功,0=失败)", DataType = "varchar2", Length = 1, IsNull = false)]
        public int State { get; set; }

        /// <summary>
        /// 成功条数
        /// </summary>
        [Column(Comments = "成功条数", DataType = "number(10)", IsNull = false)]
        public int SuccessCount { get; set; }

        /// <summary>
        /// 出错信息
        /// </summary>
        [Column(Comments = "出错信息", DataType = "clob", IsNull = true)]
        public string ErrorMsg { get; set; }
    }
}
