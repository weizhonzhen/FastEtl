using FastData.Core;
using FastData.Core.Context;
using FastEtlWeb.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FastEtlWeb.Pages
{
    public class BusinessModel : PageModel
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostBusinessList(int PageSize, int PageId)
        {
            using (var db = new DataContext(AppEtl.Db))
            {
                var page = new FastUntility.Core.Page.PageModel();
                page.PageId = PageId == 0 ? 1 : PageId;
                page.PageSize = PageSize == 0 ? 10 : PageSize;
                var list = FastRead.Query<Data_Business>(a => a.Id != "").ToPage(page, db);

                return new PartialViewResult
                {
                    ViewName = "Partial/BusinessList",
                    ViewData = new ViewDataDictionary<FastUntility.Core.Page.PageResult>(ViewData, list)
                };
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostDel(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new JsonResult(new { success = false, msg = "删除失败" });
            using (var db = new DataContext(AppEtl.Db))
            {
                if (FastRead.Query<Data_Business_Details>(a => a.Id == id).ToCount(db) == 0)
                {
                    if (FastWrite.Delete<Data_Business>(a => a.Id == id, db).IsSuccess)
                        return new JsonResult(new { success = true, msg = "删除成功" });
                    else
                        return new JsonResult(new { success = false, msg = "删除失败" });
                }
                else
                    return new JsonResult(new { success = false, msg = "业务集存在使用中" });
            }
        }
    }
}
