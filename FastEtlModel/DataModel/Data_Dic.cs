using FastData.Property;

namespace FastEtlModel.DataModel
{
    /// <summary>
    /// 字典表
    /// </summary>
    [Table(Comments = "字典对照表")]
    public class Data_Dic
    {
        /// <summary>
        /// 字典id
        /// </summary>
        [Column(Comments = "字典id", DataType = "varchar2", Length = 64, IsKey = true)]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(Comments = "名称", DataType = "varchar2", Length = 16, IsNull = true)]
        public string Name { get; set; }
    }
}
