using FastData.Core.Context;
using FastData.Core.Model;
using FastEtlWeb.Cache;
using FastEtlWeb.DataModel;
using FastUntility.Core.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FastRedis.Core;
using System.Threading.Tasks;

public static class DataSchema
{
    /// <summary>
    /// 修改表名
    /// </summary>
    /// <returns></returns>
    public static bool UpdateTableName(DataContext db, Data_Business table, string oldTableName)
    {
        var sql = "";
        if (db.config.DbType.ToLower() == AppEtl.DataDbType.Oracle.ToLower())
            sql = string.Format("alter table {0} rename to {1}", oldTableName, table.TableName);

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.MySql.ToLower())
            sql = string.Format("alter table {0} rename{1}", oldTableName, table.TableName);

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.SqlServer.ToLower())
            sql = string.Format("exec sp_rename '{0}', '{1}' ", oldTableName, table.TableName);

        return db.ExecuteSql(sql, null, false).writeReturn.IsSuccess;
    }

    /// <summary>
    /// 创建表
    /// </summary>
    /// <returns></returns>
    public static WriteReturn CreateTable(DataContext db, Data_Business table)
    {
        var result = new WriteReturn();
        if (db.config.DbType.ToLower() == AppEtl.DataDbType.Oracle.ToLower())
        {
            result = db.ExecuteSql(string.Format("create table {0}(Id varchar2(64) primary key,AddTime date,key varchar(255))", table.TableName), null, false).writeReturn;
            if (result.IsSuccess)
            {
                db.ExecuteSql(string.Format("Comment on column {0}.Id is '主键'", table.TableName), null, false);
                db.ExecuteSql(string.Format("Comment on column {0}.AddTime is '抽取时间'", table.TableName), null, false);
                db.ExecuteSql(string.Format("Comment on column {0}.key is '关键主键'", table.TableName), null, false);
            }
        }

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.MySql.ToLower())
        {
            result = db.ExecuteSql(string.Format("create table {0}(Id varchar(64) primary key,AddTime date,key varchar(255))", table.TableName), null, false).writeReturn;
            if (result.IsSuccess)
            {
                db.ExecuteSql(string.Format("alter table {0} modify Id varchar2(64) comment '主键'", table.TableName), null, false);
                db.ExecuteSql(string.Format("alter table {0} modify AddTime date comment '抽取时间'", table.TableName), null, false);
                db.ExecuteSql(string.Format("alter table {0} modify key varchar2(255) comment '关键主键'", table.TableName), null, false);
            }
        }

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.SqlServer.ToLower())
        {
            result = db.ExecuteSql(string.Format("create table {0}(Id varchar(64) primary key,AddTime date,key varchar(255))", table.TableName), null, false).writeReturn;
            if (result.IsSuccess)
            {
                db.ExecuteSql(string.Format("exec sys.sp_addextendedproperty N'MS_Description',N'主键',N'SCHEMA', N'dbo', N'TABLE',N'{0}',N'column',N'Id'", table.TableName), null, false);
                db.ExecuteSql(string.Format("exec sys.sp_addextendedproperty N'MS_Description',N'抽取时间',N'SCHEMA', N'dbo', N'TABLE',N'{0}',N'column',N'AddTime'", table.TableName), null, false);
                db.ExecuteSql(string.Format("exec sys.sp_addextendedproperty N'MS_Description',N'关键主键',N'SCHEMA', N'dbo', N'TABLE',N'{0}',N'column',N'key'", table.TableName), null, false);
            }
        }

        return result;
    }

    /// <summary>
    /// 获取字段类型
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private static string GetFieldType(CacheColumn item, ConfigModel config, Data_Source source)
    {
        if (config.DbType.ToLower() == AppEtl.DataDbType.Oracle.ToLower() && source.Type.ToLower() == AppEtl.DataDbType.SqlServer.ToLower())
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
                    if (item.Precision == 0 && item.Scale == 0)
                        return item.Type;
                    else
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
                            return string.Format("nvarchar2({0})", item.Precision/2);
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
        else if (config.DbType.ToLower() == AppEtl.DataDbType.Oracle.ToLower() && source.Type.ToLower() == AppEtl.DataDbType.MySql.ToLower())
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
        else if (config.DbType.ToLower() == AppEtl.DataDbType.SqlServer.ToLower() && source.Type.ToLower() == AppEtl.DataDbType.Oracle.ToLower())
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
                    return string.Format("nvarchar({0})", item.Length/2);
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
                    if (item.Precision == 0 && item.Scale == 0)
                        return item.Type;
                    else
                        return string.Format("decimal({0},{1})", item.Precision, item.Scale);
                case "integer":
                    return "int";
                default:
                    return item.Type;
            }
            #endregion
        }
        else if (config.DbType.ToLower() == AppEtl.DataDbType.SqlServer.ToLower() && source.Type.ToLower() == AppEtl.DataDbType.MySql.ToLower())
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
                    if (item.Precision == 0 && item.Scale == 0)
                        return item.Type;
                    else
                        return string.Format("decimal({0},{1})", item.Precision, item.Scale);
                default:
                    return item.Type;
            }
            #endregion
        }
        else if (config.DbType.ToLower() == AppEtl.DataDbType.MySql.ToLower() && source.Type.ToLower() == AppEtl.DataDbType.Oracle.ToLower())
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
                    if (item.Precision == 0 && item.Scale == 0)
                        return item.Type;
                    else
                        return string.Format("decimal({0},{1})", item.Precision, item.Scale);
                default:
                    return item.Type;
            }
            #endregion
        }
        else if (config.DbType.ToLower() == AppEtl.DataDbType.MySql.ToLower() && source.Type.ToLower() == AppEtl.DataDbType.SqlServer.ToLower())
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
                    if (item.Precision == 0 && item.Scale == 0)
                        return item.Type;
                    else
                        return string.Format("decimal({0},{1})", item.Precision, item.Scale);
                case "char":
                    return string.Format("char({0})", item.Length);
                case "nchar":
                    return string.Format("char({0})", item.Length/2);
                case "varchar":
                    return string.Format("varchar({0})", item.Length);
                case "nvarchar":
                    return string.Format("varchar({0})", item.Length/2);
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
                case "varchar":
                case "varchar2":
                    return string.Format("{0}({1})", item.Type, item.Length == -1 ? "max" : item.Length.ToString());
                case "nchar":
                case "nvarchar":
                case "nvarchar2":
                    return string.Format("{0}({1})", item.Type, item.Length == -1 ? "max" : (item.Length/2).ToString());
                case "decimal":
                case "numeric":
                case "number":
                    if (item.Precision == 0 && item.Scale == 0)
                        return item.Type;
                    else
                        return string.Format("{0}({1},{2})", item.Type, item.Precision, item.Scale);
                default:
                    return item.Type;
            }
            #endregion
        }
    }

    /// <summary>
    /// 修改列
    /// </summary>
    /// <returns></returns>
    public static bool UpdateColumn(DataContext db, Data_Business table, Data_Business_Details column, CacheColumn columnInfo, Data_Source dataSource)
    {
        var sql = "";

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.Oracle.ToLower())
            sql = string.Format("alter table {0} modify {1} {2}", table.TableName, column.FieldName, GetFieldType(columnInfo, db.config, dataSource));

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.MySql.ToLower())
            sql = string.Format("alter table {0} modify {1} {2}", table.TableName, column.FieldName, GetFieldType(columnInfo, db.config, dataSource));

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.SqlServer.ToLower())
            sql = string.Format("alter table {0} alter column {1} {2}", table.TableName, column.FieldName, GetFieldType(columnInfo, db.config, dataSource));

        return db.ExecuteSql(sql, null, false).writeReturn.IsSuccess;
    }

    /// <summary>
    /// 删除列
    /// </summary>
    /// <returns></returns>
    public static bool DropColumn(DataContext db,Data_Business table,Data_Business_Details column)
    {
        if (IsExistsColumn(db, table.TableName,column.FieldName))
        {
            var sql = string.Format("alter table {0} drop column {1}", table.TableName, column.FieldName);
            return db.ExecuteSql(sql, null, false).writeReturn.IsSuccess;
        }
        return true;
    }

    /// <summary>
    /// 增加列
    /// </summary>
    /// <returns></returns>
    public static WriteReturn AddColumn(DataContext db, Data_Business table, Data_Business_Details column, CacheColumn columnInfo, Data_Source dataSource)
    {
        return db.ExecuteSql(string.Format("alter table {0} add {1} {2}", table.TableName, column.FieldName, GetFieldType(columnInfo, db.config, dataSource)), null, false).writeReturn;
    }

    /// <summary>
    /// 修改表备注
    /// </summary>
    /// <param name="db"></param>
    /// <param name="table"></param>
    /// <returns></returns>
    public static bool UpdateTableComment(DataContext db, Data_Business table)
    {
        var sql = "";
        if (db.config.DbType.ToLower() == AppEtl.DataDbType.MySql.ToLower())
            sql = string.Format("alter table {0} comment '{1}'", table.TableName, table.Name);

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.Oracle.ToLower())
            sql = string.Format("Comment on table {0} is '{1}'", table.TableName, table.Name);

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.SqlServer.ToLower())
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

    /// <summary>
    /// 修改列备注
    /// </summary>
    /// <returns></returns>
    public static bool UpdateColumnComment(DataContext db, Data_Business table, Data_Business_Details column, CacheColumn columnInfo, Data_Source dataSource)
    {
        var sql = "";
        if (db.config.DbType.ToLower() == AppEtl.DataDbType.MySql.ToLower())
            sql = string.Format("alter table {0} modify {1} {2} comment '{3}'", table.TableName, column.FieldName, GetFieldType(columnInfo, db.config, dataSource), columnInfo.Comments);

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.Oracle.ToLower())
            sql = string.Format("Comment on column {0}.{1} is '{2}'", table.TableName, column.FieldName, columnInfo.Comments);

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.SqlServer.ToLower())
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

    /// <summary>
    /// 表不否存在
    /// </summary>
    /// <param name="db"></param>
    /// <param name="tableName"></param>
    public static bool IsExistsTable(DataContext db, string tableName)
    {
        var sql = "";

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.Oracle.ToLower())
            sql = string.Format("select count(0) count from all_tables where table_name='{0}'", tableName.ToUpper());

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.SqlServer.ToLower())
            sql = string.Format("select count(0) count from sysobjects where name='{0}'", tableName);

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.MySql.ToLower())
            sql = string.Format("select table_name form information_schema.tables where table_name ='{0}'", tableName);

        return db.ExecuteSql(sql, null, false).DicList.First().GetValue("count").ToStr().ToInt(0) > 0;
    }

    /// <summary>
    /// 删除表
    /// </summary>
    /// <param name="db"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public static bool DropTable(DataContext db, string tableName)
    {
        if (IsExistsTable(db, tableName))
        {
            var sql = string.Format("drop table {0}", tableName);
            return db.ExecuteSql(sql, null, false).writeReturn.IsSuccess;
        }
        return true;
    }

    /// <summary>
    /// 列是否存在
    /// </summary>
    /// <param name="db"></param>
    /// <param name="tableName"></param>
    /// <param name="columnName"></param>
    public static bool IsExistsColumn(DataContext db, string tableName, string columnName)
    {
        var sql = "";

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.Oracle.ToLower())
            sql = string.Format("select count(0) count from all_tab_columns where table_name='{0}' and column_name='{1}'", tableName.ToUpper(), columnName.ToUpper());

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.SqlServer.ToLower())
            sql = string.Format("select * from syscolumns where id = object_id('{0}') and name = '{1}'", tableName, columnName);

        if (db.config.DbType.ToLower() == AppEtl.DataDbType.MySql.ToLower())
            sql = string.Format("select count(0) count from  information_schema.columns where table_name='{0}' and column_name='{1}'", tableName, columnName);

        return db.ExecuteSql(sql, null, false).DicList.First().GetValue("count").ToStr().ToInt(0) > 0;
    }

    /// <summary>
    /// 获取列语句
    /// </summary>
    /// <param name="item"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    private static string ColumnSql(Data_Source item, string tableName)
    {
        var sql = "";
        if (item.Type.ToLower() == AppEtl.DataDbType.Oracle.ToLower())
        {
            tableName = tableName.ToUpper();
            sql = @"select a.column_name,data_type,data_length,b.comments,
                                            (select count(0) from all_cons_columns aa, all_constraints bb
                                                where aa.constraint_name = bb.constraint_name and bb.constraint_type = 'P' and bb.table_name = '"
                                    + tableName + @"' and aa.column_name=a.column_name),(select count(0) from all_ind_columns t,all_indexes i 
                                            where t.index_name = i.index_name and t.table_name = i.table_name and t.table_name = '"
                                    + tableName + @"' and t.column_name=a.column_name),nullable,data_precision,data_scale
                                            from all_tab_columns a inner join all_col_comments b
                                            on a.table_name='" + tableName +
                                    "' and a.table_name=b.table_name and a.column_name=b.column_name order by a.column_id asc";
        }

        if (item.Type.ToLower() == AppEtl.DataDbType.MySql.ToLower())
        {
            sql = @"select column_name,data_type,character_maximum_length,column_comment,
                                            (select count(0) from INFORMATION_SCHEMA.KEY_COLUMN_USAGE a where TABLE_SCHEMA='" + item.ServerName
                                    + "' and TABLE_NAME='" + tableName + @"' and constraint_name='PRIMARY' and c.column_name=a.column_name),
                                            (SELECT count(0) from information_schema.statistics a where table_schema = '"
                                    + item.ServerName + "' and table_name = '" + tableName + @"' and c.column_name=a.column_name),
                                            is_nullable,numeric_precision,numeric_scale,column_type from information_schema.columns c where table_name='"
                                    + tableName + "'  order by ordinal_position asc";
        }

        if (item.Type.ToLower() == AppEtl.DataDbType.SqlServer.ToLower())
        {
            sql = @"select a.name,(select top 1 name from sys.systypes c where a.xtype=c.xtype) as type ,
                                        length,b.value,(select count(0) from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where TABLE_NAME='"
                                + tableName + @"' and COLUMN_NAME=a.name),
                                        (SELECT count(0) FROM sysindexes aa JOIN sysindexkeys bb ON aa.id=bb.id AND aa.indid=bb.indid 
                                         JOIN sysobjects cc ON bb.id=cc.id  JOIN syscolumns dd ON bb.id=dd.id AND bb.colid=dd.colid 
                                         WHERE aa.indid NOT IN(0,255) AND cc.name='" + tableName + @"' and dd.name=a.name),isnullable,prec,scale
                                        from syscolumns a left join sys.extended_properties b 
                                        on major_id = id and minor_id = colid and b.name ='MS_Description' 
                                        where a.id=object_id('" + tableName + "') order by a.colid asc";
        }

        return sql;
    }

    /// <summary>
    /// 获取列的信息
    /// </summary>
    /// <returns></returns>
    public static void InitColumn(Data_Source item, bool IsLoad, string tableName)
    {
        var key = string.Format(AppEtl.CacheKey.Column, item.Host, tableName);
        if (RedisInfo.Exists(key,AppEtl.CacheDb) && IsLoad)
            return;

        var list = new List<CacheColumn>();
        var dt = new DataTable();

        using (var conn = DbProviderFactories.GetFactory(item.Type).CreateConnection())
        {
            conn.ConnectionString = AppCommon.GetConnStr(item);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = ColumnSql(item, tableName);

            if (cmd.CommandText == "")
                return;

            var rd = cmd.ExecuteReader();
            dt.Load(rd);
            rd.Close();
        }

        foreach (DataRow row in dt.Rows)
        {
            var column = new CacheColumn();
            column.Name = (row.ItemArray[0] == DBNull.Value ? "" : row.ItemArray[0].ToString());
            column.Type = row.ItemArray[1] == DBNull.Value ? "" : row.ItemArray[1].ToString();
            column.Length = row.ItemArray[2] == DBNull.Value ? 0 : int.Parse(row.ItemArray[2].ToString());
            column.Comments = row.ItemArray[3] == DBNull.Value ? "" : row.ItemArray[3].ToString();
            column.IsKey = row.ItemArray[4].ToString() != "0" ? true : false;
            column.Precision = row.ItemArray[7] == DBNull.Value ? 0 : int.Parse(row.ItemArray[7].ToString());
            column.ShowName = string.Format("{0}({1})", column.Name, column.Comments);

            list.Add(column);
        }

        RedisInfo.Set<List<CacheColumn>>(key, list,8640, AppEtl.CacheDb);
    }

    /// <summary>
    /// 获取表语句
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private static string TableSql(Data_Source item)
    {
        var sql = "";
        if (item.Type.ToLower() == AppEtl.DataDbType.Oracle.ToLower())
            sql = "select a.table_name,comments from all_tables a inner join all_tab_comments b on a.TABLE_NAME=b.TABLE_NAME and a.TABLESPACE_NAME!='SYSAUX' and a.TABLESPACE_NAME!='SYSTEM'";
        
        if (item.Type.ToLower() == AppEtl.DataDbType.MySql.ToLower())
            sql = string.Format("select table_name, table_comment from information_schema.TABLES where table_schema='{0}' and table_type='BASE TABLE'", item.ServerName);

        if (item.Type.ToLower() == AppEtl.DataDbType.SqlServer.ToLower())
            sql = "select name,(select top 1 value from sys.extended_properties where major_id=object_id(a.name) and minor_id=0) as value from sys.objects a where type='U'";

        return sql;
    }

    /// <summary>
    /// 表说明
    /// </summary>
    /// <returns></returns>
    public static void InitTable(Data_Source item, bool IsLoad)
    {
        var key = string.Format(AppEtl.CacheKey.Table, item.Host);
        if (RedisInfo.Exists(key, AppEtl.CacheDb) && IsLoad)
            return;

        var list = new List<CacheTable>();
        var dt = new DataTable();

        using (var conn = DbProviderFactories.GetFactory(item.Type).CreateConnection())
        {
            conn.ConnectionString = AppCommon.GetConnStr(item);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = TableSql(item);

            if (cmd.CommandText == "")
                return;

            var rd = cmd.ExecuteReader();
            dt.Load(rd);
            rd.Close();
        }

        foreach (DataRow row in dt.Rows)
        {
            var table = new CacheTable();
            table.Comments = row.ItemArray[1] == DBNull.Value ? "" : row.ItemArray[1].ToString();
            table.Name = row.ItemArray[0] == DBNull.Value ? "" : row.ItemArray[0].ToString();
            list.Add(table);
            Parallel.Invoke(() => {
                InitColumn(item, IsLoad, table.Name);
            });
        }

        RedisInfo.Set<List<CacheTable>>(key, list, 8640, AppEtl.CacheDb);
    }
}

