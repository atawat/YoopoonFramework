using System.Collections.Generic;
using YP.CodeGen.TemplateModel;

namespace YP.CodeGen.ExcelModel
{
    public class SheetModel
    {
        public string TableDescription { get; set; }

        public string TabbleEnName { get; set; }

        public List<EntityModel> Entity { get; set; }

        public List<MappingModel> Mapping { get; set; }

        public List<SearchModel> Search { get; set; }

        public List<EnumModel> Enums { get; set; }

        public SheetModel()
        {
            Entity = new List<EntityModel>();
            Enums = new List<EnumModel>();
            Mapping = new List<MappingModel>();
            Search = new List<SearchModel>();
        }
    }
}