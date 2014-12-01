using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP.CodeGen.ExcelModel
{
    public class WorkGroupModel
    {
        public string ProjectName { get; set; }

        public List<SheetModel> Sheets { get; set; } 
    }
}
