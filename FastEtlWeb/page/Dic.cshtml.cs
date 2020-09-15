using System.Collections.Generic;
using FastData.Core.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using FastData.Core;
using FastEtlWeb.DataModel;
using FastUntility.Core.Base;
using FastData.Core.Repository;

namespace FastEtlWeb.Pages
{
    public class DicModel : PageModel
    {
        private readonly IFastRepository IFast;
        public DicModel(IFastRepository _IFast)
        {
            IFast = _IFast;
        }

        /// <summary>
        /// ÁÐ±í
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostDicList(int PageSize, int PageId,string DicId)
        {
            using (var db = new DataContext(AppEtl.Db))
            {
                var page = new FastUntility.Core.Page.PageModel();
               
                page.PageId = PageId == 0 ? 1 : PageId;
                page.PageSize = PageSize == 0 ? 10 : PageSize;
                var list = new FastUntility.Core.Page.PageResult();
                if (string.IsNullOrEmpty(DicId))
                    list = IFast.Query<Data_Dic_Details>(a => a.DicId != null).OrderBy<Data_Dic_Details>(a => new { a.DicId }).ToPage(page, db);
                else
                    list = IFast.Query<Data_Dic_Details>(a => a.DicId == DicId).OrderBy<Data_Dic_Details>(a => new { a.DicId }).ToPage(page, db);

                return new PartialViewResult
                {
                    ViewName = "Partial/DicList",
                    ViewData = new ViewDataDictionary<FastUntility.Core.Page.PageResult>(ViewData, list)
                };
            }
        }

        /// <summary>
        /// É¾³ý
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostDel(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new JsonResult(new { success = false, msg = "É¾³ýÊ§°Ü" });
            using (var db = new DataContext(AppEtl.Db))
            {
                if (IFast.Query<Data_Business_Details>(a => a.Dic == id).ToCount(db) == 0)
                {
                    var dicId = IFast.Query<Data_Dic_Details>(a => a.Id == id, a => new { a.DicId }).ToDic(db).GetValue("DicId").ToStr();
                    if (IFast.Delete<Data_Dic_Details>(a => a.Id == id, db).IsSuccess)
                    {
                        if (IFast.Query<Data_Dic_Details>(a => a.DicId == dicId).ToCount(db) == 0)
                            IFast.Delete<Data_Dic>(a => a.Id == dicId);
                        return new JsonResult(new { success = true, msg = "É¾³ý³É¹¦" });
                    }
                    else
                        return new JsonResult(new { success = false, msg = "É¾³ýÊ§°Ü" });
                }
                else
                    return new JsonResult(new { success = false, msg = "×Öµä´æÔÚÊ¹ÓÃÖÐ" });
            }
        }
    }
}
