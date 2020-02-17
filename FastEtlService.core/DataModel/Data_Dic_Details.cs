
namespace FastEtlService.core.DataModel
{
    /// <summary>
    /// 字典明细表
    /// </summary>
    public class Data_Dic_Details
    {
        /// <summary>
        /// 日字典明细id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 日字典明细id
        /// </summary>
        public string DicId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 对照值
        /// </summary>
        public string ContrastValue { get; set; }
    }
}
