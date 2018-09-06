using FastData;
using FastData.Context;
using FastModel.CacheModel;
using FastModel.DataModel;
using System.Collections.Generic;

namespace FastTool.Base
{
    public static class AppCache
    {
        private static string GetTableKey(Base_DataSource link) { return string.Format("tableList_{0}", link.Host); }
        private static string GetColumnKey(Base_DataSource link,string table) { return string.Format("columnList_{0}_{1}", link.Host,table); }

        //连接
        public static Base_DataSource GetLink { get { return CacheDb.Get<Base_DataSource>("Link"); } }
        public static void SetLink(Base_DataSource item) { CacheDb.Set<Base_DataSource>("Link", item); }
        public static void RemoveLink() { CacheDb.Remove("Link"); }
        public static List<Base_DataSource> GetAllLink { get { return FastRead.Query<Base_DataSource>(a => a.Id != null).ToList<Base_DataSource>(); } }

        //业务集
        public static List<Base_Business> GetAllBusiness{ get { return FastRead.Query<Base_Business>(a => a.Id != null).ToList<Base_Business>(); }}

        public static List<Base_Business_Details> GetBusinessDetails(string Id){return FastRead.Query<Base_Business_Details>(a => a.Id == Id).ToList<Base_Business_Details>(); }
        
        //表list
        public static List<Cache_Table> GetTableList(Base_DataSource link) { return CacheDb.Get<List<Cache_Table>>(GetTableKey(link)) ?? new List<Cache_Table>(); }
        public static void SetTableList(List<Cache_Table> item, Base_DataSource link) { CacheDb.Set<List<Cache_Table>>(GetTableKey(link), item); }
        public static bool ExistsTable(Base_DataSource link) { return CacheDb.Exists(GetTableKey(link)); }

        //列list
        public static List<Cache_Column> GetColumnList(Base_DataSource link, string table) { return CacheDb.Get<List<Cache_Column>>(GetColumnKey(link,table)) ?? new List<Cache_Column>(); }
        public static void SetColumnList(List<Cache_Column> item, Base_DataSource link, string table) { CacheDb.Set<List<Cache_Column>>(GetColumnKey(link,table), item); }
        public static bool ExistsColumn(Base_DataSource link, string table) { return CacheDb.Exists(GetColumnKey(link, table)); }

    }
}
