namespace FastEtlWeb.Model
{
    public class PageSizeModel
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageId { get; set; }

        /// <summary>
        /// 每页多少条
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 共多少页
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        ///  共多少条 
        /// </summary>
        public int TotalRecord { get; set; }

        /// <summary>
        /// 表单id
        /// </summary>
        public string FormId { get; set; }

        /// <summary>
        /// html填充id
        /// </summary>
        public string ContentId { get; set; }

        /// <summary>
        /// 请求url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 分页请求方法
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 表格id
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// 是否父节点
        /// </summary>
        public bool IsParent { get; set; }

        /// <summary>
        /// 是否排序
        /// </summary>
        public bool IsOrderBy { get; set; }
    }
}