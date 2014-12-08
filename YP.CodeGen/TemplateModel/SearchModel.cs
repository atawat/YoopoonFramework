namespace YP.CodeGen.TemplateModel
{
    public class SearchModel
    {
        public string SearchName { get; set; }

        public string SearchType { get; set; }

        public EnumSearchType Type { get; set; }
    }

    public enum EnumSearchType
    {
        Order,
        In,
        Like,
        Equal,
        Range
    }
}