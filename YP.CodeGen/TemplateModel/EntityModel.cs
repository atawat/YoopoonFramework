namespace YP.CodeGen.TemplateModel
{
    public class EntityModel
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 注释
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否虚类型
        /// </summary>
        public bool IsVirtual { get; set; }
    }
}