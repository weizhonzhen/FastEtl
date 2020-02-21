using FastUntility.Core.Page;
using System.Collections.Generic;

namespace FastEtlService.core.Model
{
    /// <summary>
    /// 分页数据
    /// </summary>
    public class PageData : PageModel
    {
        public List<Dictionary<string, object>> list { set; get; } = new List<Dictionary<string, object>>();
    }
}
