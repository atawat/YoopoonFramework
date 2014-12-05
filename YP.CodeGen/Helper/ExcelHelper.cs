﻿using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Drawing.Charts;
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
                var model = new SheetModel
                {
                    TabbleEnName = sheet.Name,
                    TableDescription = descriptionCell == null?"":GetValue(descriptionCell,sharedStringTablePart),
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
                            mapping[index].ColumnType = GetValue(cell, stringTablePart);
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
                                enums.Add(new EnumModel
                                {
                                    EnumName = type
                                });
                            }
                            break;
                        case "H":
                            var value = GetValue(cell, stringTablePart);
                            if (!string.IsNullOrEmpty(value))
                            {
                                search.Add(new SearchModel
                                {
                                    SearchName = entity[index].FieldName,
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
                                    SearchName = entity[index].FieldName,
                                    Type = EnumSearchType.Order
                                });
                            }
                            break;
                        case "K":
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