using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YP.CodeGen.TemplateModel;

namespace YP.CodeGen.Templates
{
    public partial class ControllerTemplate
    {
        private  List<EntityModel> _entityModels;
        private List<SearchModel> _searchModels;
        private string _tableName;
        private string _projectName;

        public ControllerTemplate(string projectName,string tableName,List<SearchModel> searchModels,List<EntityModel> entityModels)
        {
            _projectName = projectName;
            _tableName = tableName;
            _searchModels = searchModels;
            _entityModels = entityModels;
        }
    }
}
