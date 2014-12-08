using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YP.CodeGen.TemplateModel;

namespace YP.CodeGen.Templates
{
    public partial class EnumTemplate
    {
        private readonly List<EnumModel> _models;
        private readonly string _projectName;
        public EnumTemplate(List<EnumModel> models, string projectName)
        {
            _models = models;
            _projectName = projectName;
        }
    }
}
