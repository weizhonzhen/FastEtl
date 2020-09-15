using FastData.Core;
using FastData.Core.Context;
using FastData.Core.Repository;
using FastEtlWeb.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FastEtlWeb.Pages
{
    public class DicTypeModel : PageModel
    {
        private readonly IFastRepository IFast;
        public DicTypeModel(IFastRepository _IFast)
        {
            IFast = _IFast;
        }

        public IActionResult OnPostDicTypeList(int PageSize, int PageId, string DicId)
        {
            using (var db = new DataContext(AppEtl.Db))
            {
                var page = new FastUntility.Core.Page.PageModel();

                page.PageId = PageId == 0 ? 1 : PageId;
                page.PageSize = PageSize == 0 ? 6 : PageSize;
                var list = new FastUntility.Core.Page.PageResult();
                if (string.IsNullOrEmpty(DicId))
                    list = IFast.Query<Data_Dic>(a => a.Id != null).OrderBy<Data_Dic>(a => new { a.Name }).ToPage(page, db);
                else
                    list = IFast.Query<Data_Dic>(a => a.Id == DicId).OrderBy<Data_Dic>(a => new { a.Name }).ToPage(page, db);

                return new PartialViewResult
                {
                    ViewName = "Partial/DicTypeList",
                    ViewData = new ViewDataDictionary<FastUntility.Core.Page.PageResult>(ViewData, list)
                };
            }
        }
    }   
}
