using System.Collections.Generic;
using YP.CodeGen.TemplateModel;

namespace YP.CodeGen.Templates
{
    public partial class ModelTemplate
    {
        private readonly List<EntityModel> _models;

        private readonly string _entityName;

        private readonly string _projectName;
        public ModelTemplate(List<EntityModel> models, string ntityName, string projectName)
        {
            _models = models;
            _entityName = ntityName;
            _projectName = projectName;
        }
    }
}