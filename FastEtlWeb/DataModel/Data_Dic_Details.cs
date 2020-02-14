using FastData.Core.Property;
using System.ComponentModel.DataAnnotations;

namespace FastEtlWeb.DataModel
{
    /// <summary>
    /// 字典明细表
    /// </summary>
    [Table(Comments = "字典对照明细")]
    public class Data_Dic_Details
    {
        /// <summary>
        /// 日字典明细id
        /// </summary>
        [StringLength(64, ErrorMessage = "{0}最大长度64")]
        [Display(Name = "字典明细id")]
        [Column(Comments = "字典明细id", DataType = "varchar2", Length = 64, IsKey = true)]
        public string Id { get; set; }

        /// <summary>
        /// 日字典明细id
        /// </summary>
        [StringLength(64, ErrorMessage = "{0}最大长度64")]
        [Display(Name = "字典id")]
        [Column(Comments = "字典id", DataType = "varchar2", Length = 64, IsKey = true)]
        public string DicId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(16, ErrorMessage = "{0}最大长度16")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "名称")]
        [Column(Comments = "名称", DataType = "varchar2", Length = 16, IsNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [StringLength(64, ErrorMessage = "{0}最大长度64")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "值")]
        [Column(Comments = "值", DataType = "varchar2", Length = 64, IsNull = false)]
        public string Value { get; set; }

        /// <summary>
        /// 对照值
        /// </summary>
        [StringLength(64, ErrorMessage = "{0}最大长度64")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "对照值")]
        [Column(Comments = "对照值", DataType = "varchar2", Length = 64, IsNull = false)]
        public string ContrastValue { get; set; }
    }
}
