using System.Collections.Generic;
using YP.CodeGen.TemplateModel;

namespace YP.CodeGen.Templates
{
    public partial class ConditionTemplate
    {
        private readonly List<SearchModel> _models;
        private readonly string _entityName;
        private readonly string _projectName;

        public ConditionTemplate(List<SearchModel> models, string entityName, string projectName)
        {
            _models = models;
            _entityName = entityName;
            _projectName = projectName;
        }
    }
}