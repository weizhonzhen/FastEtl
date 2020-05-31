using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FastData.Core;
using FastEtlWeb.DataModel;
using Microsoft.AspNetCore.Html;
using System;
using FastUntility.Core.Base;

namespace Microsoft.AspNetCore.Mvc.Rendering
{
    public static class ExtendHtml
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlContent DropDownListForData<TModel, TProperty>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var dic = new Dictionary<string, object>();
            var typeList = new List<SelectListItem>();
       
            typeList.Add(new SelectListItem { Text = AppEtl.DataDbType.Oracle, Value = AppEtl.DataDbType.Oracle });
            typeList.Add(new SelectListItem { Text = AppEtl.DataDbType.MySql, Value = AppEtl.DataDbType.MySql });
            typeList.Add(new SelectListItem { Text = AppEtl.DataDbType.SqlServer, Value = AppEtl.DataDbType.SqlServer });

            return html.DropDownListFor(expression, typeList, htmlAttributes);
        }

        /// <summary>
        /// 字典
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="selectValue"></param>
        /// <returns></returns>
        public static IHtmlContent DropDownListForDic<TModel, TProperty>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes,object selectValue)
        {
            var typeList = new List<SelectListItem>();
            var list = FastRead.Query<Data_Dic>(a => a.Id != "", null, AppEtl.Db).ToList<Data_Dic>();

            foreach (var item in list)
            {
                if (selectValue.ToStr() == item.Id)
                    typeList.Add(new SelectListItem { Text = item.Name, Value = item.Id, Selected = true });
                else
                    typeList.Add(new SelectListItem { Text = item.Name, Value = item.Id});
            }

            return html.DropDownListFor(expression, typeList, htmlAttributes);
        }

        /// <summary>
        /// 字典
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public static IHtmlContent DropDownListForDic(this IHtmlHelper html,string name, object htmlAttributes,bool isAll=false)
        {
            var typeList = new List<SelectListItem>();
            var list = FastRead.Query<Data_Dic>(a => a.Id != "", null, AppEtl.Db).ToList<Data_Dic>();

            if(isAll)
                typeList.Add(new SelectListItem { Text = "全部", Value = "" });

            foreach (var item in list)
            {
                typeList.Add(new SelectListItem { Text = item.Name, Value = item.Id });
            }
           
            return html.DropDownList(name, typeList, htmlAttributes);
        }

        /// <summary>
        /// 数据源
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlContent DropDownListForDataSource<TModel, TProperty>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var typeList = new List<SelectListItem>();
            var list = FastRead.Query<Data_Source>(a => a.Id != "", null, AppEtl.Db).ToList<Data_Source>();

            foreach (var item in list)
            {
                typeList.Add(new SelectListItem { Text = string.Format("{0}({1}:{2})",item.LinkName,item.Host,item.Port), Value = item.Id });
            }

            return html.DropDownListFor(expression, typeList, htmlAttributes);
        }
    }
}
