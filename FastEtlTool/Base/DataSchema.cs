using FastData.Context;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using FastEtlModel.CacheModel;
using FastEtlModel.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FastUntility.Base;
using FastData.Model;
using FastApp;
using System.Threading.Tasks;

namespace FastEtlTool.Base
{
    /// <summary>
    /// 获取表 列
    /// </summary>
    public static class DataSchema
    {
        #region 表说明
        /// <summary>
        /// 表说明
        /// </summary>
        /// <returns></returns>
        public static void InitTable(Data_Source link, bool IsLoad)
        {
            if (AppCache.ExistsTable(link) && IsLoad)
                return;

            var list = new List<Cache_Table>();
            var dt = new DataTable();

            //oracle 表信息
            if (link.Type == DataDbType.Oracle)
            {
                #region oracle
                using (var conn = new OracleConnection(BaseLink.GetConnStr(link)))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "select a.table_name,comments from all_tables a inner join all_tab_comments b on a.TABLE_NAME=b.TABLE_NAME  and a.TABLESPACE_NAME!='SYSAUX' and a.TABLESPACE_NAME!='SYSTEM'";

                    var rd = cmd.ExecuteReader();
                    dt.Load(rd);
                    conn.Close();
                }
                #endregion
            }

            //sql server 表信息
            if (link.Type == DataDbType.SqlServer)
            {
                #region sqlserver
                using (var conn = new SqlConnection(BaseLink.GetConnStr(link)))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "select name,(select top 1 value from sys.extended_properties where major_id=object_id(a.name) and minor_id=0) as value from sys.objects a where type='U'";

                    var rd = cmd.ExecuteReader();
                    dt.Load(rd);
                    conn.Close();
                }
                #endregion
            }

            //mysql 表信息
            if (link.Type == DataDbType.MySql)
            {
                #region mysql
                using (var conn = new MySqlConnection(BaseLink.GetConnStr(link)))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = string.Format("select table_name, table_comment from information_schema.TABLES where table_schema='{0}' and table_type='BASE TABLE'", link.ServerName);

                    var rd = cmd.ExecuteReader();
                    dt.Load(rd);

                    conn.Close();
                }
                #endregion
            }

            foreach (DataRow item in dt.Rows)
            {
                var table = new Cache_Table();
                table.Comments = item.ItemArray[1] == DBNull.Value ? "" : item.ItemArray[1].ToString();
                table.Name = item.ItemArray[0] == DBNull.Value ? "" : item.ItemArray[0].ToString();
                list.Add(table);

                Parallel.Invoke(() => {
                    InitColumn(link, IsLoad, table.Name);
                });
            }

            AppCache.SetTableList(list, link);
        }
        #endregion

        #region 获取列的信息
        /// <summary>
        /// 获取列的信息
        /// </summary>
        /// <returns></returns>
        private static void InitColumn(Data_Source link, bool IsLoad, string tableName)
        {
            if (AppCache.ExistsColumn(link,tableName) && IsLoad)
                return;

            var list = new List<Cache_Column>();
            var dt = new DataTable();

            //oracle 列信息
            if (link.Type == DataDbType.Oracle)
            {
                #region oracle
                using (var conn = new OracleConnection(BaseLink.GetConnStr(link)))
                {
                    tableName = tableName.ToUpper();
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = @"select a.column_name,data_type,data_length,b.comments,
                                            (select count(0) from all_cons_columns aa, all_constraints bb
                                                where aa.constraint_name = bb.constraint_name and bb.constraint_type = 'P' and bb.table_name = '"
                                        + tableName + @"' and aa.column_name=a.column_name),(select count(0) from all_ind_columns t,all_indexes i 
                                            where t.index_name = i.index_name and t.table_name = i.table_name and t.table_name = '"
                                        + tableName + @"' and t.column_name=a.column_name),nullable,data_precision,data_scale
                                            from all_tab_columns a inner join all_col_comments b
                                            on a.table_name='" + tableName +
                                        "' and a.table_name=b.table_name and a.column_name=b.column_name order by a.column_id asc";
                    var rd = cmd.ExecuteReader();
                    dt.Load(rd);
                    rd.Close();
                    conn.Close();
                }
                #endregion
            }

            if (link.Type == DataDbType.SqlServer)
            {
                #region sql server
                using (var conn = new SqlConnection(BaseLink.GetConnStr(link)))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = @"select a.name,(select top 1 name from sys.systypes c where a.xtype=c.xtype) as type ,
                                        length,b.value,(select count(0) from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where TABLE_NAME='"
                                    + tableName + @"' and COLUMN_NAME=a.name),
                                        (SELECT count(0) FROM sysindexes aa JOIN sysindexkeys bb ON aa.id=bb.id AND aa.indid=bb.indid 
                                         JOIN sysobjects cc ON bb.id=cc.id  JOIN syscolumns dd ON bb.id=dd.id AND bb.colid=dd.colid 
                                         WHERE aa.indid NOT IN(0,255) AND cc.name='" + tableName + @"' and dd.name=a.name),isnullable,prec,scale
                                        from syscolumns a left join sys.extended_properties b 
                                        on major_id = id and minor_id = colid and b.name ='MS_Description' 
                                        where a.id=object_id('" + tableName + "') order by a.colid asc";
                    var rd = cmd.ExecuteReader();
                    dt.Load(rd);
                    rd.Close();
                    conn.Close();
                }
                #endregion
            }

            if (link.Type == DataDbType.MySql)
            {
                #region mysql
                using (var conn = new MySqlConnection(BaseLink.GetConnStr(link)))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = @"select column_name,data_type,character_maximum_length,column_comment,
                                            (select count(0) from INFORMATION_SCHEMA.KEY_COLUMN_USAGE a where TABLE_SCHEMA='" + link.ServerName
                                        + "' and TABLE_NAME='" + tableName + @"' and constraint_name='PRIMARY' and c.column_name=a.column_name),
                                            (SELECT count(0) from information_schema.statistics a where table_schema = '"
                                        + link.ServerName + "' and table_name = '" + tableName + @"' and c.column_name=a.column_name),
                                            is_nullable,numeric_precision,numeric_scale,column_type from information_schema.columns c where table_name='"
                                        + tableName + "'  order by ordinal_position asc";
                    var rd = cmd.ExecuteReader();
                    dt.Load(rd);
                    rd.Close();
                    conn.Close();
                }
                #endregion
            }

            foreach (DataRow item in dt.Rows)
            {
                var column = new Cache_Column();
                column.Name = (item.ItemArray[0] == DBNull.Value ? "" : item.ItemArray[0].ToString());
                column.Type = item.ItemArray[1] == DBNull.Value ? "" : item.ItemArray[1].ToString();
                column.Length = item.ItemArray[2] == DBNull.Value ? 0 : decimal.Parse(item.ItemArray[2].ToString());
                column.Comments = item.ItemArray[3] == DBNull.Value ? "" : item.ItemArray[3].ToString();
                column.Precision = item.ItemArray[7] == DBNull.Value ? 0 : int.Parse(item.ItemArray[7].ToString());
                column.ShowName = string.Format("{0}({1})", column.Name, column.Comments);

                list.Add(column);
            }

            AppCache.SetColumnList(list, link, tableName);
        }
        #endregion

        #region 列是否存在
        /// <summary>
        /// 列是否存在
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        public static bool IsExistsColumn(DataContext db ,string tableName,string columnName)
        {
            var sql = "";

            if (db.config.DbType == DataDbType.Oracle)
                sql = string.Format("select count(0) count from all_tab_columns where table_name='{0}' and column_name='{1}'", tableName.ToUpper(), columnName.ToUpper());

            if (db.config.DbType == DataDbType.SqlServer)
                sql = string.Format("select * from syscolumns where id = object_id('{0}') and name = '{1}'", tableName, columnName);

            if (db.config.DbType == DataDbType.MySql)
                sql = string.Format("select count(0) count from  information_schema.columns where table_name='{0}' and column_name='{1}'", tableName, columnName);

            return db.ExecuteSql(sql, null, false).DicList.First().GetValue("count").ToStr().ToInt(0) > 0;
        }
        #endregion

        #region 表是否存在
        /// <summary>
        /// 列不否存在
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tableName"></param>
        public static bool IsExistsTable(DataContext db, string tableName)
        {
            var sql = "";

            if (db.config.DbType == DataDbType.Oracle)
                sql = string.Format("select count(0) count from all_tables where table_name='{0}'", tableName.ToUpper());

            if (db.config.DbType == DataDbType.SqlServer)
                sql = string.Format("select count(0) count from sysobjects where name='{0}'", tableName);

            if (db.config.DbType == DataDbType.MySql)
                sql = string.Format("select table_name form information_schema.tables where table_name ='{0}'", tableName);

            return db.ExecuteSql(sql, null, false).DicList.First().GetValue("count").ToStr().ToInt(0) > 0;
        }
        #endregion

        #region 修改列备注
        /// <summary>
        /// 修改列备注
        /// </summary>
        /// <returns></returns>
        public static bool UpdateColumnComment(DataContext db, Data_Business table, Data_Business_Details column, Cache_Column columnInfo, Data_Source dataSource)
        {
            var sql = "";
            if (db.config.DbType == DataDbType.MySql)
                sql = string.Format("alter table {0} modify {1} {2} comment '{3}'", table.TableName, column.FieldName, GetFieldType(columnInfo,db.config,dataSource), columnInfo.Comments);

            if (db.config.DbType == DataDbType.Oracle)
                sql = string.Format("Comment on column {0}.{1} is '{2}'", table.TableName, column.FieldName, columnInfo.Comments);

            if (db.config.DbType == DataDbType.SqlServer)
            {
                sql = string.Format("select count(0) from syscolumns where id = object_id('{0}') and name='{1}' and exists(select 1 from sys.extended_properties where object_id('{0}') = major_id and colid = minor_id", table.TableName, column.FieldName);
                var count = db.ExecuteSql(sql, null, false).DicList[0]["count"].ToStr().ToInt(0);
                if (count >= 1)
                    sql = string.Format("exec sys.sp_updateextendedproperty N'MS_Description',N'{0}',N'SCHEMA', N'dbo', N'TABLE',N'{1}',N'column',N'{2}'", columnInfo.Comments, table.TableName, column.FieldName);
                else
                    sql = string.Format("exec sys.sp_addextendedproperty N'MS_Description',N'{0}',N'SCHEMA', N'dbo', N'TABLE',N'{1}',N'column',N'{2}'", columnInfo.Comments, table.TableName, column.FieldName);
            }

            return db.ExecuteSql(sql, null, false).writeReturn.IsSuccess;
        }
        #endregion

        #region 修改表备注
        /// <summary>
        /// 修改表备注
        /// </summary>
        /// <returns></returns>
        public static bool UpdateTableComment(DataContext db, Data_Business table)
        {
            var sql = "";
            if (db.config.DbType == DataDbType.MySql)
                sql = string.Format("alter table {0} comment '{1}'", table.TableName, table.Name);

            if (db.config.DbType == DataDbType.Oracle)
                sql = string.Format("Comment on table {0} is '{1}'", table.TableName, table.Name);

            if (db.config.DbType == DataDbType.SqlServer)
            {
                sql = string.Format("select count(0) count from sys.extended_properties where object_id('{0}')=major_id and minor_id=0", table.TableName);
                var count = db.ExecuteSql(sql, null, false).DicList[0]["count"].ToStr().ToInt(0);
                if (count >= 1)
                    sql = string.Format("execute sp_updateextendedproperty N'MS_Description', '{0}', N'user', N'dbo', N'table', N'{1}', NULL, NULL", table.TableName, table.Name);
                else
                    sql = string.Format("execute sp_addextendedproperty N'MS_Description', '{0}', N'user', N'dbo', N'table', N'{1}', NULL, NULL", table.TableName, table.Name);
            }

            return db.ExecuteSql(sql, null, false).writeReturn.IsSuccess;
        }
        #endregion

        #region 创建表
        /// <summary>
        /// 创建表
        /// </summary>
        /// <returns></returns>
        public static bool CreateTable(DataContext db, Data_Business table)
        {
            var isSuccess = false;
            if (db.config.DbType == DataDbType.Oracle)
            {
                isSuccess = db.ExecuteSql(string.Format("create table {0}(Id varchar2(64) primary key,AddTime date,key varchar2(255))", table.TableName), null, false).writeReturn.IsSuccess;
                if (isSuccess)
                {
                    db.ExecuteSql(string.Format("Comment on column {0}.Id is '主键'", table.TableName), null, false);
                    db.ExecuteSql(string.Format("Comment on column {0}.AddTime is '抽取时间'", table.TableName), null, false);
                    db.ExecuteSql(string.Format("Comment on column {0}.key is '关键主键'", table.TableName), null, false);
                }
            }

            if (db.config.DbType == DataDbType.MySql)
            {
                isSuccess = db.ExecuteSql(string.Format("create table {0}(Id varchar(64) primary key,AddTime date,key varchar(255))", table.TableName), null, false).writeReturn.IsSuccess;
                if (isSuccess)
                {
                    db.ExecuteSql(string.Format("alter table {0} modify Id varchar2(64) comment '主键'", table.TableName), null, false);
                    db.ExecuteSql(string.Format("alter table {0} modify AddTime date comment '抽取时间'", table.TableName), null, false);
                    db.ExecuteSql(string.Format("alter table {0} modify key varchar2(255) comment '关键主键'", table.TableName), null, false);
                }
            }

            if (db.config.DbType == DataDbType.SqlServer)
            {
                isSuccess = db.ExecuteSql(string.Format("create table {0}(Id varchar(64) primary key,AddTime date,key varchar(255))", table.TableName), null, false).writeReturn.IsSuccess;
                if (isSuccess)
                {
                    db.ExecuteSql(string.Format("exec sys.sp_addextendedproperty N'MS_Description',N'主键',N'SCHEMA', N'dbo', N'TABLE',N'{0}',N'column',N'Id'", table.TableName), null, false);
                    db.ExecuteSql(string.Format("exec sys.sp_addextendedproperty N'MS_Description',N'抽取时间',N'SCHEMA', N'dbo', N'TABLE',N'{0}',N'column',N'AddTime'", table.TableName), null, false);
                    db.ExecuteSql(string.Format("exec sys.sp_addextendedproperty N'MS_Description',N'关键主键',N'SCHEMA', N'dbo', N'TABLE',N'{0}',N'column',N'key'", table.TableName), null, false);
                }
            }
            
            return isSuccess;
        }
        #endregion

        #region 修改表名
        /// <summary>
        /// 修改表名
        /// </summary>
        /// <returns></returns>
        public static bool UpdateTableName(DataContext db, Data_Business table, string oldTableName)
        {
            var sql = "";
            if (db.config.DbType == DataDbType.Oracle)
                sql = string.Format("alter table {0} rename to {1}", oldTableName, table.TableName);

            if (db.config.DbType == DataDbType.MySql)
                sql = string.Format("alter table {0} rename{1}", oldTableName, table.TableName);

            if (db.config.DbType == DataDbType.SqlServer)
                sql = string.Format("exec sp_rename '{0}', '{1}' ", oldTableName, table.TableName);

            return db.ExecuteSql(sql, null, false).writeReturn.IsSuccess;
        }
        #endregion

        #region 增加列
        /// <summary>
        /// 增加列
        /// </summary>
        /// <returns></returns>
        public static bool AddColumn(DataContext db, Data_Business table,Data_Business_Details column, Cache_Column columnInfo, Data_Source dataSource)
        {
            return db.ExecuteSql(string.Format("alter table {0} add {1} {2}", table.TableName, column.FieldName, GetFieldType(columnInfo,db.config,dataSource)), null, false).writeReturn.IsSuccess;
        }
        #endregion
        
        #region 修改列
        /// <summary>
        /// 修改列
        /// </summary>
        /// <returns></returns>
        public static bool UpdateColumn(DataContext db, Data_Business table, Data_Business_Details column, Cache_Column columnInfo, Data_Source dataSource)
        {
            var sql = "";

            if (db.config.DbType == DataDbType.Oracle)
                sql = string.Format("alter table {0} modify {1} {2}", table.TableName, column.FieldName, GetFieldType(columnInfo,db.config,dataSource));

            if (db.config.DbType == DataDbType.MySql)
                sql = string.Format("alter table {0} modify {1} {2}", table.TableName, column.FieldName, GetFieldType(columnInfo,db.config,dataSource));

            if (db.config.DbType == DataDbType.SqlServer)
                sql = string.Format("alter table {0} alter column {1} {2}", table.TableName, column.FieldName, GetFieldType(columnInfo,db.config,dataSource));

            return db.ExecuteSql(sql, null, false).writeReturn.IsSuccess;
        }
        #endregion

        #region 获取字段类型
        /// <summary>
        /// 获取字段类型
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static string GetFieldType(Cache_Column item, ConfigModel config, Data_Source source)
        {
            if (config.DbType == DataDbType.Oracle && source.Type.ToLower() == DataDbType.SqlServer.ToLower())
            {
                #region sqlserver to oracle
                if (string.IsNullOrEmpty(item.Type))
                    return "nvarchar2(255)";

                switch (item.Type.ToLower())
                {
                    case "bit":
                    case "int":
                    case "smallint":
                    case "tinyint":
                        return "integer";
                    case "datetime":
                    case "smalldatetime":
                        return "date";
                    case "decimal":
                    case "numeric":
                        return string.Format("decimal({0},{1})", item.Precision, item.Scale);
                    case "money":
                    case "smallmoney":
                    case "real":
                        return "real";
                    case "uniqueidentifier":
                        return string.Format("char({0})", item.Length);
                    case "nchar":
                    case "nvarchar":
                        {
                            if (item.Length > 4000)
                                return "clob";
                            else
                                return string.Format("nvarchar2({0})", item.Precision);
                        }
                    case "varchar":
                    case "char":
                        {
                            if (item.Length > 4000)
                                return "clob";
                            else
                                return string.Format("varchar2({0})", item.Precision);
                        }
                    case "text":
                    case "ntext":
                        return "clob";
                    case "binary":
                    case "varbinary":
                    case "image":
                        return "blob";
                    default:
                        return item.Type;
                }
                #endregion
            }
            else if (config.DbType == DataDbType.Oracle && source.Type.ToLower() == DataDbType.MySql.ToLower())
            {
                #region mysql to oracle
                if (string.IsNullOrEmpty(item.Type))
                    return "nvarchar2(255)";

                switch (item.Type.ToLower())
                {
                    case "bigint":
                        return "number(19,0)";
                    case "bit":
                    case "tinyblob":
                        return "raw";
                    case "blob":
                        return "blob";
                    case "char":
                        return string.Format("char{0}", item.Length);
                    case "date":
                    case "datetime":
                    case "time":
                    case "timestamp":
                        return "date";
                    case "deciaml":
                    case "double":
                    case "real":
                    case "double precision":
                        return "float(24)";
                    case "float":
                        return "float";
                    case "int":
                    case "integer":
                        return "number(10, 0)";
                    case "longblob":
                    case "mediumblog":
                        return "blob";
                    case "longtext":
                    case "mediumtext":
                    case "text":
                    case "tinytext":
                    case "varchar":
                        return "clob";
                    case "mediumint":
                        return "number(7,0)";
                    case "numeric":
                    case "year":
                        return "number";
                    case "smallint":
                        return "number(5,0)";
                    case "tinyint":
                        return "number(3,0)";
                    default:
                        return item.Type;
                }
                #endregion
            }
            else if (config.DbType == DataDbType.SqlServer && source.Type.ToLower() == DataDbType.Oracle.ToLower())
            {
                #region oracle to sqlserver
                if (string.IsNullOrEmpty(item.Type))
                    return "varchar(255)";
                switch (item.Type.ToLower())
                {
                    case "char":
                    case "varchar2":
                        return string.Format("varchar({0})", item.Length);
                    case "nchar":
                    case "nvarchar2":
                        return string.Format("nvarchar({0})", item.Length);
                    case "date":
                        return "datetime";
                    case "long":
                        return "text";
                    case "bfile":
                    case "blob":
                    case "long raw":
                    case "raw":
                    case "nrowid":
                    case "binary":
                        return "image";
                    case "rowid":
                        return "uniqueidentifier";
                    case "number":
                    case "decimal":
                        return string.Format("decimal({0},{1})", item.Precision, item.Scale);
                    case "integer":
                        return "int";
                    default:
                        return item.Type;
                }
                #endregion
            }
            else if (config.DbType == DataDbType.SqlServer && source.Type.ToLower() == DataDbType.MySql.ToLower())
            {
                #region MySql to sqlserver
                if (string.IsNullOrEmpty(item.Type))
                    return "varchar(255)";
                switch (item.Type.ToLower())
                {
                    case "tinyint":
                        return "bit";
                    case "text":
                        return "ntext";
                    case "varchar":
                        return string.Format("nvarchar({0})", item.Length);
                    case "char":
                        return string.Format("nchar({0})", item.Length);
                    case "decimal":
                        return string.Format("decimal({0},{1})", item.Precision,item.Scale);
                    default:
                        return item.Type;
                }
                #endregion
            }
            else if (config.DbType == DataDbType.MySql && source.Type.ToLower() == DataDbType.Oracle.ToLower())
            {
                #region Oracle to MySql
                if (string.IsNullOrEmpty(item.Type))
                    return "varchar(255)";

                switch (item.Type.ToLower())
                {
                    case "colb":
                        return "text";
                    case "date":
                        return "datetime";
                    case "varchar2":
                        return string.Format("varchar({0})", item.Length);
                    case "number":
                        return string.Format("decimal({0},{1})", item.Precision, item.Scale);
                    default:
                        return item.Type;
                }
                #endregion
            }
            else if (config.DbType == DataDbType.MySql && source.Type.ToLower() == DataDbType.SqlServer.ToLower())
            {
                #region SqlServer to MySql
                if (string.IsNullOrEmpty(item.Type))
                    return "varchar(255)";

                switch (item.Type.ToLower())
                {
                    case "datetime2":
                    case "datetimeoffset":
                    case "smalldatetime":
                        return "datetime";
                    case "uniqueidentifier":
                        return "varchar(40)";
                    case "bit":
                        return "bigint";
                    case "money":
                    case "real":
                    case "smallmoney":
                        return "float";
                    case "xml":
                    case "ntext":
                        return "text";
                    case "decimal":
                    case "numeric":
                        return string.Format("decimal({0},{1})", item.Precision,item.Scale);
                    case "char":
                    case "nchar":
                        return string.Format("char({0})", item.Length);
                    case "varchar":
                    case "nvarchar":
                        return string.Format("varchar({0})", item.Length);
                    default:
                        return item.Type;
                }
                #endregion
            }
            else
            {
                #region default
                switch (item.Type.ToLower())
                {
                    case "char":
                    case "nchar":
                    case "varchar":
                    case "nvarchar":
                    case "varchar2":
                    case "nvarchar2":
                        return string.Format("{0}({1})", item.Type, item.Length == -1 ? "max" : item.Length.ToString());
                    case "decimal":
                    case "numeric":
                    case "number":
                        return string.Format("{0}({1},{2})", item.Type, item.Precision, item.Scale);
                    default:
                        return item.Type;
                }
                #endregion
            }
        }
        #endregion
    }
}
