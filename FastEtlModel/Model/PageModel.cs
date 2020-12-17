namespace FastEtlModel.Model
{
    public class PageModel
    {
        /// <summary>
        /// 页数
        /// </summary>
        public long pageCount { get; set; }

        /// <summary>
        /// 第几页
        /// </summary>
        public long pageId { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public long pageSize { get; set; }
        
        /// <summary>
        /// 总条数
        /// </summary>
        public long total { get; set; }
    }
}
