using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastEtlWeb.Filter
{
    public class CheckFilter : IAsyncPageFilter
    {
        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var result = new Dictionary<string, object>();
                var item = context.ModelState.Values.ToList().Find(a => a.Errors.Count > 0);
                var msg = item.Errors.Where(a => !string.IsNullOrEmpty(a.ErrorMessage)).Take(1).SingleOrDefault().ErrorMessage;
                result.Add("success", false);
                result.Add("msg", msg);
                context.Result = new JsonResult(result);
                return;
            }
            await next.Invoke();
        }
    }
}
