using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
                var worksheetpart = (WorksheetPart)_document.WorkbookPart.GetPartById(sheet.Id);
                var worksheet = worksheetpart.Worksheet;
                var sharedStringTablePart = _document.WorkbookPart.SharedStringTablePart;
                var descriptionCell = worksheet.Descendants<Cell>().FirstOrDefault(c => c.CellReference.Value == "A1");
                if (descriptionCell == null || string.IsNullOrWhiteSpace(GetValue(descriptionCell, sharedStringTablePart)))
                    continue;
                var model = new SheetModel
                {
                    TabbleEnName = sheet.Name,
                    TableDescription = GetValue(descriptionCell,sharedStringTablePart),
                    Entity = new List<EntityModel>(),
                    Mapping = new List<MappingModel>(),
                    Search = new List<SearchModel>(),
                    Enums = new List<EnumModel>()
                };
                CreateSubModels(worksheet, model.Entity, model.Mapping, model.Search, model.Enums, sharedStringTablePart);
                models.Add(model);
            }
            return models;
        }

        private void CreateSubModels(Worksheet sheet, List<EntityModel> entity, List<MappingModel> mapping, List<SearchModel> search, List<EnumModel> enums, SharedStringTablePart stringTablePart)
        {
            int index = 0;
            foreach (var row in sheet.Descendants<Row>().Where(r => r.RowIndex > 3))
            {
                foreach (var cell in row.Descendants<Cell>())
                {
                    switch (cell.CellReference.Value.Substring(0,1))
                    {
                        case "B":
                            mapping.Add(new MappingModel { FieldName = GetValue(cell, stringTablePart) });
                            break;
                        case "C":
                            var colType = GetValue(cell, stringTablePart);
                            if (string.IsNullOrEmpty(colType))
                            {
                                mapping[index].ColumnType = "";
                                mapping[index].TypeLength = "";
                                break;
                            }
                            var typeValue = Regex.Match(colType, "[a-zA-Z]+").Captures[0];
                            var length = Regex.Match(colType, @"\((\d+)\)").Groups[1];
                            mapping[index].ColumnType = typeValue.Value;
                            mapping[index].TypeLength = length.Value;
                            break;
                        case "D":
                            mapping[index].Key = GetEnumKey(GetValue(cell, stringTablePart));
                            break;
                        case "E":
                            mapping[index].IsNull = GetValue(cell, stringTablePart) == "1";
                            break;
                        case "F":
                            entity.Add(new EntityModel{FieldName = GetValue(cell, stringTablePart) });
                            break;
                        case "G":
                            var type = GetValue(cell, stringTablePart);
                            entity[index].Type = type;
                            if (type.StartsWith("Enum"))
                            {
                                var description = GetValue(row.Descendants<Cell>().First(c => c.CellReference.Value.StartsWith("K")), stringTablePart);
                                if (!string.IsNullOrEmpty(description))
                                {
                                    enums.Add(new EnumModel
                                    {
                                        EnumName = type,
                                        Values = description.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(v =>{ 
                                            var splitValue = v.Split(':');
                                            return new EnumValueModel
                                            {
                                                Attribute = splitValue[0],
                                                Description = splitValue[1]
                                            };
                                        }).ToList()
                                    });
                                }
                            }
                            break;
                        case "H":
                            var value = GetValue(cell, stringTablePart);
                            if (!string.IsNullOrEmpty(value))
                            {
                                search.Add(new SearchModel
                                {
                                    SearchName = entity[index].FieldName.Replace("_",""),
                                    SearchType = entity[index].Type,
                                    Type = GetEnumSearchType(value)
                                });
                            }
                            break;
                        case "I":
                            var isOrder = GetValue(cell, stringTablePart);
                            if (isOrder == "1")
                            {
                                search.Add(new SearchModel
                                {
                                    SearchName = entity[index].FieldName.Replace("_", ""),
                                    Type = EnumSearchType.Order
                                });
                            }
                            break;
                        case "J":
                            entity[index].Description = GetValue(cell, stringTablePart);
                            break;
                        default:
                            continue;
                    }
                }
                index++;
            }
        }

        private EnumKey GetEnumKey(string keyValue)
        {
            switch (keyValue)
            {
                case "PK":
                    return EnumKey.PK;
                case "FK":
                    return EnumKey.FK;
                default:
                    return EnumKey.NK;
            }
        }

        private EnumSearchType GetEnumSearchType(string type)
        {
            switch (type.ToLower())
            {
                case "in":
                    return EnumSearchType.In;
                case "like":
                    return EnumSearchType.Like;
                case "equal":
                    return EnumSearchType.Equal;
                case "range":
                    return EnumSearchType.Range;
                default:
                    return EnumSearchType.Order;
            }
        }

        private String GetValue(Cell cell, SharedStringTablePart stringTablePart)
        {
            if (cell.ChildElements.Count == 0)
                return null;
            String value = cell.CellValue.InnerText;
            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                value = stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            return value;
        }
    }
}