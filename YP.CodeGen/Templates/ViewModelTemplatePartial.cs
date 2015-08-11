using System.Collections.Generic;
using YP.CodeGen.TemplateModel;

namespace YP.CodeGen.Templates
{
    public partial class ViewModelTemplate
    {
        private List<EntityModel> _entityModels;
        private List<EnumModel> _enumModels;
        private string _tableName;
        private string _projectName;

        public ViewModelTemplate(string projectName, string tableName, List<EnumModel> enumModels, List<EntityModel> entityModels)
        {
            _projectName = projectName;
            _tableName = tableName;
            _enumModels = enumModels;
            _entityModels = entityModels;
        }
    }
}