using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using YP.CodeGen.ExcelModel;
using YP.CodeGen.TemplateModel;

namespace YP.CodeGen.Helper
{
    public class ExcelHelper : IDisposable
    {
        private readonly SpreadsheetDocument _document;
        private readonly List<Sheet> _sheets;

        public ExcelHelper(string excelPath)
        {
            _document = SpreadsheetDocument.Open(excelPath, false);//读取excel到对象
            _sheets = _document.WorkbookPart.Workbook.Descendants<Sheet>().ToList();
        }


        public void Dispose()
        {
            _document.Dispose();
        }

        public List<SheetModel> CreateModels()
        {
            var models = new List<SheetModel>();
            foreach (var sheet in _sheets)
            {
                var cells = sheet.Descendants<Cell>();
                var model = new SheetModel
                {
                    TabbleEnName = sheet.Name,
                    TableDescription = cells.FirstOrDefault(c => c.CellReference.Value == "A1").InnerText
                };
                var entity = new List<EntityModel>();
                var mapping = new List<MappingModel>();
                var search = new List<SearchModel>();
                var enums = new List<EnumModel>();
                CreateSubModels(sheet,entity,mapping,search,enums);
            }
            return models;
        }

        private void CreateSubModels(Sheet sheet, List<EntityModel> entity, List<MappingModel> mapping, List<SearchModel> search, List<EnumModel> enums)
        {
            foreach (var row in sheet.Descendants<Row>().Where(r => r.RowIndex > 2))
            {
                foreach (var cell in row.Descendants<Cell>())
                {

                }
            }
        }
    }
}