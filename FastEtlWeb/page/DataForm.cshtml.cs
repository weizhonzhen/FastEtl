using FastEtlWeb.DataModel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FastData.Core;
using FastData.Core.Model;
using FastData.Core.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FastData.Core.Repository;

namespace FastEtlWeb.Pages
{
    public class DataFormModel : PageModel
    {
        public Data_Source info = new Data_Source();
        private IFastRepository IFast;
        public DataFormModel(IFastRepository _IFast)
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
                using(var db=new DataContext(AppEtl.Db))
                {
                    info = IFast.Query<Data_Source>(a => a.Id == id).ToItem<Data_Source>(db);
                }
            }
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IActionResult OnPostTest(Data_Source item)
        {
            var dbConn = AppCommon.GetConnStr(item);
            if (AppCommon.TestLink(item.Type, dbConn))
                return new JsonResult(new { success = true, msg = "连接成功" });
            else
                return new JsonResult(new { success = false, msg = "连接失败" });
        }

        /// <summary>
        /// 操作
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IActionResult OnPostDataForm(Data_Source item)
        {
            var dbConn = AppCommon.GetConnStr(item);
            if (!AppCommon.TestLink(item.Type, dbConn))
                return new JsonResult(new { success = false, msg = "连接失败" });
           
            var info = new WriteReturn();
            using (var db = new DataContext(AppEtl.Db))
            {
                if (IFast.Query<Data_Source>(a => a.Id == item.Id).ToCount(db) == 0)
                {
                    item.Id = Guid.NewGuid().ToString();
                    info = db.Add(item).writeReturn; 
                }
                else
                    info = db.Update<Data_Source>(item, a => a.Id == item.Id).writeReturn;

                if (info.IsSuccess)
                    return new JsonResult(new { success = true, msg = "操作成功" });
                else
                    return new JsonResult(new { success = false, msg = info.Message });
            }
        }

        /// <summary>
        /// 加载数据源
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IActionResult OnPostLoadCache(Data_Source item)
        {
            Task.Run(() => { DataSchema.InitTable(item, true); });
            return new JsonResult(new { success = true, msg = "操作成功" });
        }

        public IActionResult OnPostUpdateCache(Data_Source item)
        {
            Task.Run(() => { DataSchema.InitTable(item, false); });
            return new JsonResult(new { success = true, msg = "操作成功" });
        }
    }
}
