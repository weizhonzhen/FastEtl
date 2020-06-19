using System;
using System.Collections.Generic;
using FastData.Core;
using FastData.Core.Context;
using FastData.Core.Model;
using FastData.Core.Repository;
using FastEtlWeb.DataModel;
using FastUntility.Core.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FastEtlWeb.Pages
{
    public class BusinessFormModel : PageModel
    {
        public Data_Business info = new Data_Business();
        private IFastRepository IFast;
        public BusinessFormModel(IFastRepository _IFast)
        {
            IFast = _IFast;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="id"></param>
        public void OnGet(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                using (var db = new DataContext(AppEtl.Db))
                {
                    info = IFast.Query<Data_Business>(a => a.Id == id).ToItem<Data_Business>(db);
                }
            }
        }


        /// <summary>
        /// 操作
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IActionResult OnPostBusinessForm(Data_Business item)
        {
            var info = new WriteReturn();
            using (var db = new DataContext(AppEtl.Db))
            {
                db.BeginTrans();
                if (IFast.Query<Data_Business>(a => a.Id == item.Id).ToCount(db) == 0)
                {
                    item.Id = Guid.NewGuid().ToString();
                    info = db.Add(item).writeReturn;
                    if (info.IsSuccess)
                       info = DataSchema.CreateTable(db, item);
                }
                else
                {
                    var oldTableName = IFast.Query<Data_Business>(a => a.Id == item.Id, a => new { a.TableName }).ToDic(db).GetValue("TableName").ToStr();
                    info = db.Update<Data_Business>(item, a => a.Id == item.Id).writeReturn;

                    if (info.IsSuccess)
                    {
                        if (oldTableName.ToLower() != item.TableName.ToLower())
                        {
                            DataSchema.DropTable(db, oldTableName);
                            info = DataSchema.CreateTable(db, item);
                        }
                        else
                            DataSchema.UpdateTableComment(db, item);
                    }
                }

                if (info.IsSuccess)
                {
                    db.SubmitTrans();
                    return new JsonResult(new { success = true, msg = "操作成功" });
                }
                else
                {
                    db.RollbackTrans();
                    return new JsonResult(new { success = false, msg = info.Message }); 
                }
            }
        }
    }
}
