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
        private List<EnumModel> _models;
        public EnumTemplate(List<EnumModel> models)
        {
            _models = models;
        }
    }
}
