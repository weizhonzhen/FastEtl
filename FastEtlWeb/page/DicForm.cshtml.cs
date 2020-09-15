using System;
using FastData.Core;
using FastData.Core.Context;
using FastData.Core.Model;
using FastData.Core.Repository;
using FastEtlWeb.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FastEtlWeb.Pages
{
    public class DicFormModel : PageModel
    {
        public Data_Dic_Details info = new Data_Dic_Details();
        private readonly IFastRepository IFast;
        public DicFormModel(IFastRepository _IFast)
        {
            IFast = _IFast;
        }

        public void OnGet(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                using (var db = new DataContext(AppEtl.Db))
                {
                    info = IFast.Query<Data_Dic_Details>(a => a.Id == id).ToItem<Data_Dic_Details>(db);
                }
            }
        }

        /// <summary>
        /// 操作
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IActionResult OnPostDicForm(Data_Dic_Details item)
        {
            var info = new WriteReturn();
            using (var db = new DataContext(AppEtl.Db))
            {
                if (IFast.Query<Data_Dic_Details>(a => a.Id == item.Id).ToCount(db) == 0)
                {
                    item.Id = Guid.NewGuid().ToString();
                    info = db.Add(item).writeReturn;
                }
                else
                    info = db.Update<Data_Dic_Details>(item, a => a.Id == item.Id).writeReturn;

                if (info.IsSuccess)
                    return new JsonResult(new { success = true, msg = "操作成功" });
                else
                    return new JsonResult(new { success = false, msg = info.Message });
            }
        }
    }
}
