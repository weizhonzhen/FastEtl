using FastUntility.Core.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FastEtlWeb.Filter
{
    public class ErrorFilter: ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            filterContext.ExceptionHandled = true;

            filterContext.Result = new JsonResult(new { success = false, msg = filterContext.Exception.Message });

            BaseLog.SaveLog(string.Format("ip:{0}，错误内容:{1},Message:{2}", AppCommon.GetClientIp(filterContext.HttpContext), filterContext.Exception.StackTrace, filterContext.Exception.Message), "web_error");
        }
    }
}
