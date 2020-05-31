namespace FastEtlWeb.Cache
{
    public class CacheColumn
    {

        /// <summary>
        /// 显示列名
        /// </summary>
        public string ShowName { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 精度
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// 小数点位数
        /// </summary>
        public int Scale { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsKey { get; set; }
    }
}
