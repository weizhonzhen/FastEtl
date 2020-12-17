using FastData;
using FastData.Context;
using FastEtlModel.CacheModel;
using FastEtlModel.DataModel;
using System.Collections.Generic;

namespace FastEtlTool.Base
{
    public static class AppCache
    {
        private static string GetTableKey(Data_Source link) { return string.Format("tableList_{0}", link.Host); }
        private static string GetColumnKey(Data_Source link,string table) { return string.Format("columnList_{0}_{1}", link.Host,table); }

        //连接
        public static Data_Source GetLink { get { return CacheDb.Get<Data_Source>("Link"); } }
        public static void SetLink(Data_Source item) { CacheDb.Set<Data_Source>("Link", item); }
        public static void RemoveLink() { CacheDb.Remove("Link"); }
        public static List<Data_Source> GetAllLink { get { return FastRead.Query<Data_Source>(a => a.Id != null).ToList<Data_Source>(); } }

        //业务集
        public static List<Data_Business> GetAllBusiness{ get { return FastRead.Query<Data_Business>(a => a.Id != null).ToList<Data_Business>(); }}

        public static List<Data_Business_Details> GetBusinessDetails(string Id){return FastRead.Query<Data_Business_Details>(a => a.Id == Id).ToList<Data_Business_Details>(); }
        
        //表list
        public static List<Cache_Table> GetTableList(Data_Source link) { return CacheDb.Get<List<Cache_Table>>(GetTableKey(link)) ?? new List<Cache_Table>(); }
        public static void SetTableList(List<Cache_Table> item, Data_Source link) { CacheDb.Set<List<Cache_Table>>(GetTableKey(link), item); }
        public static bool ExistsTable(Data_Source link) { return CacheDb.Exists(GetTableKey(link)); }

        //列list
        public static List<Cache_Column> GetColumnList(Data_Source link, string table) { return CacheDb.Get<List<Cache_Column>>(GetColumnKey(link,table)) ?? new List<Cache_Column>(); }
        public static void SetColumnList(List<Cache_Column> item, Data_Source link, string table) { CacheDb.Set<List<Cache_Column>>(GetColumnKey(link,table), item); }
        public static bool ExistsColumn(Data_Source link, string table) { return CacheDb.Exists(GetColumnKey(link, table)); }

    }
}
