using System.Collections.Generic;
using YP.CodeGen.TemplateModel;

namespace YP.CodeGen.Templates
{
    public partial class ServiceTemplate
    {
        private readonly string _projectName;
        private readonly string _entityName;
        private readonly List<SearchModel> _sModels;
        public ServiceTemplate(string projectName, string entityName, List<SearchModel> sModels)
        {
            _projectName = projectName;
            _entityName = entityName;
            _sModels = sModels;
        }
    }
}