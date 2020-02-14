using FastData.Core.Property;
using System.ComponentModel.DataAnnotations;

namespace FastEtlWeb.DataModel
{
    /// <summary>
    /// 数据源
    /// </summary>
    [Table(Comments = "数据源")]
    public class Data_Source
    {
        /// <summary>
        /// 数据源id
        /// </summary>
        [StringLength(64, ErrorMessage = "{0}最大长度64")]
        [Column(Comments = "数据源id", DataType = "varchar2", Length = 64, IsKey = true)]
        public string Id { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        [StringLength(12, ErrorMessage = "{0}最大长度12")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "类型")]
        [Column(Comments = "数据库类型", DataType = "varchar2", Length = 12, IsNull = false)]
        public string Type { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [StringLength(32, ErrorMessage = "{0}最大长度32")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "用户名")]
        [Column(Comments = "用户名", DataType = "varchar2", Length = 32, IsNull = false)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(32, ErrorMessage = "{0}最大长度32")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "密码")]
        [Column(Comments = "密码", DataType = "varchar2", Length = 32, IsNull = false)]
        public string PassWord { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        [StringLength(12, ErrorMessage = "{0}最大长度12")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "服务器")]
        [Column(Comments = "主机", DataType = "varchar2", Length = 12, IsNull = false)]
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        [StringLength(12, ErrorMessage = "{0}最大长度12")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "端口")]
        [Column(Comments = "端口", DataType = "varchar2", Length = 12, IsNull = false)]
        public string Port { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        [StringLength(12, ErrorMessage = "{0}最大长度12")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "服务名")]
        [Column(Comments = "服务名", DataType = "varchar2", Length = 12, IsNull = false)]
        public string ServerName { get; set; }

        /// <summary>
        /// 连接名
        /// </summary>
        [StringLength(12, ErrorMessage = "{0}最大长度12")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "连接名")]
        [Column(Comments = "连接名", DataType = "varchar2", Length = 12, IsNull = false)]
        public string LinkName { get; set; }
    }
}