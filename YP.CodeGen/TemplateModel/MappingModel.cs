namespace YP.CodeGen.TemplateModel
{
    public class MappingModel
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// SQL类型
        /// </summary>
        public string ColumnType { get; set; }

        public string TypeLength { get; set; }

        public EnumKey Key { get; set; }

        public bool IsNull { get; set; }
    }

    public enum EnumKey
    {
        PK,
        FK,
        NK
    }
}