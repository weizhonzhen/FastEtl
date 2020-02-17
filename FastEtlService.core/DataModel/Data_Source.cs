namespace FastEtlService.core.DataModel
{
    /// <summary>
    /// 数据源
    /// </summary>
    public class Data_Source
    {
        /// <summary>
        /// 数据源id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// 连接名
        /// </summary>
        public string LinkName { get; set; }
    }
}