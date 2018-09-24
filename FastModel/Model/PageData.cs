using System.Collections.Generic;

namespace FastModel.Model
{
    /// <summary>
    /// 分页数据
    /// </summary>
    public class PageData : PageModel
    {
        public List<Dictionary<string, object>> list { set; get; } = new List<Dictionary<string, object>>();
    }
}
