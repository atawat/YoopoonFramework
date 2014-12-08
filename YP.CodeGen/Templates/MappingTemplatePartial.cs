using System.Collections.Generic;
using YP.CodeGen.TemplateModel;

namespace YP.CodeGen.Templates
{
    public partial class MappingTemplate
    {
        private readonly List<MappingModel> _mModels;
        private readonly List<EntityModel> _eModels;
        private readonly string _projectName;
        private readonly string _entityName;
        public MappingTemplate(List<MappingModel> mModels, List<EntityModel> eModels, string projectName, string entityName)
        {
            _mModels = mModels;
            _eModels = eModels;
            _projectName = projectName;
            _entityName = entityName;
        }
    }
}