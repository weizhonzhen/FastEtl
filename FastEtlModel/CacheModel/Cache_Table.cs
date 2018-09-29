using System;
using System.Collections.Generic;

namespace FastEtlModel.CacheModel
{
    /// <summary>
    /// 表
    /// </summary>
    [Serializable]
    public class Cache_Table
    {
        /// <summary>
        /// 显示表名
        /// </summary>
        public string ShowName { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comments { get; set; }
    }
}
