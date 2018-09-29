using FastData.Property;
namespace FastEtlModel.DataModel
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
        [Column(Comments = "字典明细id", DataType = "varchar2", Length = 64, IsKey = true)]
        public string Id { get; set; }

        /// <summary>
        /// 日字典明细id
        /// </summary>
        [Column(Comments = "字典id", DataType = "varchar2", Length = 64, IsKey = true)]
        public string DicId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(Comments = "名称", DataType = "varchar2", Length = 16, IsNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Column(Comments = "值", DataType = "varchar2", Length = 64, IsNull = false)]
        public string Value { get; set; }

        /// <summary>
        /// 对照值
        /// </summary>
        [Column(Comments = "对照值", DataType = "varchar2", Length = 64, IsNull = false)]
        public string ContrastValue { get; set; }
    }
}
