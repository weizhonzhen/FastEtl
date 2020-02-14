using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FastData.Core;
using FastData.Core.Context;
using FastEtlWeb.DataModel;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FastEtlWeb.Pages
{
    public class DataModel : PageModel
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostDataList(int PageSize,int PageId)
        {
            using (var db = new DataContext(AppEtl.Db))
            {
                var page = new FastUntility.Core.Page.PageModel();
                page.PageId = PageId == 0 ? 1 : PageId;
                page.PageSize = PageSize == 0 ? 10 : PageSize;
                var list = FastRead.Query<Data_Source>(a => a.Id != "").ToPage(page, db);
           
                return new PartialViewResult
                {
                    ViewName = "Partial/DataList",
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
            if(string.IsNullOrEmpty(id))
                return new JsonResult(new { success = false, msg = "删除失败" });

            using (var db = new DataContext(AppEtl.Db))
            {
                if (FastRead.Query<Data_Business_Details>(a => a.DataSourceId == id).ToCount(db) == 0)
                {
                    if (FastWrite.Delete<Data_Source>(a => a.Id == id, db).IsSuccess)
                        return new JsonResult(new { success = true, msg = "删除成功" });
                    else
                        return new JsonResult(new { success = false, msg = "删除失败" });
                }
                else
                    return new JsonResult(new { success = false, msg = "数据源存在使用中" });
            }
        }
    }
}
