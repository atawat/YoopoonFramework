using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace YP.CodeGen.Helper
{
    public class ExcelHelper:IDisposable
    {
        private readonly SpreadsheetDocument _document;
        private readonly List<Sheet> _sheets;

        public ExcelHelper(string excelPath)
        {
            _document = SpreadsheetDocument.Open(excelPath,false);//读取excel到对象
            _sheets = _document.WorkbookPart.Workbook.Descendants<Sheet>().ToList();
        }


        public void Dispose()
        {
            _document.Dispose();
        }
    }
}