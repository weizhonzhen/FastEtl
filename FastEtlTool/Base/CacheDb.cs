using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Aoite.LevelDB;

namespace FastEtlTool.Base
{
    public static class CacheDb
    {
        private readonly static string path= string.Format("{0}Db", AppDomain.CurrentDomain.BaseDirectory);
        private readonly static Options options = new Options { CreateIfMissing = true };
        
        #region 转成字节流
        /// <summary>
        /// 转成字节流
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToByte(this object value)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, value);
                return stream.GetBuffer();
            }
        }
        #endregion

        #region 流转成对象
        /// <summary>
        /// 流转成对象
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T ToModel<T>(this byte[] value) where T : class, new()
        {
            using (var stream = new MemoryStream(value))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as T;
            }
        }

        public static object ToModel(this byte[] value)
        {
            using (var stream = new MemoryStream(value))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
        }
        #endregion

        /// <summary>
        /// set value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key,string value)
        {
            CheckPath();
            using (var db = new LDB(path))
            {
                db.Set(key, value);
            }
        }

        /// <summary>
        /// get value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static string Get(string key)
        {
            CheckPath();
            using (var db = new LDB(path))
            {
                return db.Get(key);
            }
        }

        /// <summary>
        /// set value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set<T>(string key, T value)
        {
            CheckPath();
            using (var db = new LDB(path))
            {
                db.Set(key, value.ToByte());
            }
        }

        /// <summary>
        /// get value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static T Get<T>(string key) where T : class, new()
        {
            CheckPath();
            using (var db = new LDB(path))
            {
                var item = db.Get(key);

                if (item == null)
                    return new T();
                else
                    return db.Get(key).ByteArray.ToModel<T>() ?? new T();
            }
        }

        /// <summary>
        /// set value
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            CheckPath();
            using (var db = new LDB(path))
            {
                db.Remove(key);
            }
        }

        /// <summary>
        /// exist value
        /// </summary>
        /// <param name="key"></param>
        public static bool Exists(string key)
        {
            CheckPath();
            using (var db = new LDB(path))
            {
                return db.Get(key) != null;
            }
        }

        /// <summary>
        /// check path
        /// </summary>
        private static void CheckPath()
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
