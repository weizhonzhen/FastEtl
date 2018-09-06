using FastData.Property;
using System;

namespace FastModel.DataModel
{
    /// <summary>
    /// 数据源
    /// </summary>
    [Serializable]
    [Table(Comments = "数据源")]
    public class Base_DataSource
    {
        /// <summary>
        /// 数据源id
        /// </summary>
        [Column(Comments = "数据源id", DataType = "varchar2", Length = 64, IsKey = true)]
        public string Id { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        [Column(Comments = "数据库类型", DataType = "varchar2", Length = 12,IsNull =false)]
        public string Type { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Column(Comments = "用户名", DataType = "varchar2", Length = 32, IsNull = false)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column(Comments = "密码", DataType = "varchar2", Length = 32, IsNull = false)]
        public string PassWord { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        [Column(Comments = "主机", DataType = "varchar2", Length = 12, IsNull = false)]
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        [Column(Comments = "端口", DataType = "varchar2", Length = 12, IsNull = false)]
        public string Port { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        [Column(Comments = "服务名", DataType = "varchar2", Length = 12, IsNull = false)]
        public string ServerName { get; set; }

        /// <summary>
        /// 连接名
        /// </summary>
        [Column(Comments = "连接名", DataType = "varchar2", Length = 12, IsNull = false)]
        public string LinkName { get; set; }
    }
}
