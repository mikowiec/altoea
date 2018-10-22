

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection;
//
using DocumentFormat.OpenXml
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
//
using Altoea.Framework.MetaData;
using Altoea.Framework.ViewPort.Descriptor;
using Altoea.Framework.Reflection;

namespace Altoea.Framework.Services.Converters
{
    public class ExcelGenerator : IDisposable
    {
        private MemoryStream memoryStream;
        private SheetData sheetData;
        private WorkbookPart workbookpart;
        private SpreadsheetDocument spreadsheetDocument;
        public ExcelGenerator()
        {
            memoryStream = new MemoryStream();
            spreadsheetDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);
            workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
            Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
            sheets.Append(sheet);
            sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
        }

        public void Dispose()
        {
            spreadsheetDocument.Dispose();
            memoryStream.Close();
            memoryStream.Dispose();
        }
        public void AddRow(Action<Row> newrow)
        {
            Row row = new Row();
            sheetData.Append(row);
            newrow(row);
        }
        public MemoryStream ToMemoryStream()
        {
            workbookpart.Workbook.Save();
            spreadsheetDocument.Close();
            memoryStream.Position = 0;
            return new MemoryStream(memoryStream.ToArray());
        }
    }
    public static class OpenXmlExt
    {
        public static void AppendCell(this Row row, string value)
        {
            Cell dataCell = new Cell();
            row.AppendChild(dataCell);
            dataCell.CellValue = new CellValue(value);
            dataCell.DataType = new EnumValue<CellValues>(CellValues.String);
        }
    }




    public class ExcelReader<T> where T : new()
    {
        private readonly Stream _excelStream;
        private WorkbookPart _workBookPart;
        private ViewConfigure _viewConfigure;
        private PropertyInfo[] _entryProperites;

        public ExcelReader(Stream stream)
        {
            _excelStream = stream;
            ErrorMessages = new List<string>();
            Converts = new List<IValueType>();
        }
        public ExcelReader(Stream stream, List<IValueType> converts)
        {
            _excelStream = stream;
            ErrorMessages = new List<string>();
            Converts = converts;
        }
        public Type EntryType { get { return typeof(T); } }
        public List<IValueType> Converts { get; set; }
        public List<string> ErrorMessages { get; set; }
        public virtual T CellConvert(T item, string header, Cell cell)
        {
            Type type = typeof(T);

            string propertyName = header;
            string value = ReadCellValue(cell);
            if (_viewConfigure != null)
            {
                var descriptor = _viewConfigure.GetViewPortDescriptors(true).FirstOrDefault(m => m.DisplayName == header);
                if (descriptor != null)
                {
                    propertyName = descriptor.Name;
                    var dropdown = descriptor as DropDownListDescriptor;
                    if (dropdown != null)
                    {
                        foreach (var op in dropdown.OptionItems)
                        {
                            if (op.Value == value)
                            {
                                value = op.Key;
                            }
                        }
                    }
                }
            }
            var property = _entryProperites.FirstOrDefault(m => m.Name == propertyName);
            if (property != null && value != null)
            {
                var convert = Converts.FirstOrDefault(m => m.SupportType == property.PropertyType);
                object convertedValue = null;
                if (convert != null)
                {
                    convertedValue = convert.Convert(value);
                }
                else
                {
                    convertedValue = ClassAction.ValueConvert(property, value);
                }
                property.SetValue(item, convertedValue);
            }

            return item;
        }
        public virtual string ReadCellValue(Cell cell)
        {
            string cellValue = null;
            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                var stringItem = _workBookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(int.Parse(cell.CellValue.InnerText));
                if (stringItem.Text != null)
                {
                    cellValue = stringItem.Text.Text;
                }
                else if (stringItem.InnerText != null)
                {
                    cellValue = stringItem.InnerText;
                }
                else if (stringItem.InnerXml != null)
                {
                    cellValue = stringItem.InnerXml;
                }
            }
            else if (cell.CellValue != null)
            {
                cellValue = cell.CellValue.InnerText;
            }
            return cellValue;
        }
        public IEnumerable<T> ToList()
        {
            _viewConfigure = ServiceLocator.GetViewConfigure(EntryType);
            _entryProperites = EntryType.GetProperties();
            SpreadsheetDocument doc = null;
            List<T> results = new List<T>();
            try
            {
                doc = SpreadsheetDocument.Open(_excelStream, false);
            }
            catch
            {
                ErrorMessages.Add("The uploaded file is incorrect. It only supports the format of office 2007 (.xlsx)");
            }
            if (doc != null)
            {
                using (doc)
                {
                    _workBookPart = doc.WorkbookPart;
                    Sheet mysheet = (Sheet)doc.WorkbookPart.Workbook.Sheets.ChildElements.FirstOrDefault();
                    Worksheet worksheet = ((WorksheetPart)_workBookPart.GetPartById(mysheet.Id)).Worksheet;
                    SheetData sheetData = null;
                    foreach (var item in worksheet.ChildElements)
                    {
                        if (item is SheetData)
                        {
                            sheetData = (SheetData)item;
                            break;
                        }
                    }
                    if (sheetData != null && sheetData.ChildElements.Any())
                    {
                        List<string> header = new List<string>();
                        var headerRow = sheetData.ChildElements.First() as Row;
                        foreach (var cell in headerRow.ChildElements)
                        {
                            var cellItem = cell as Cell;
                            header.Add(ReadCellValue(cellItem));
                        }

                        for (int i = 1; i < sheetData.ChildElements.Count; i++)
                        {
                            T item = new T();
                            try
                            {
                                for (int j = 0; j < sheetData.ChildElements[i].ChildElements.Count; j++)
                                {
                                    if (j < header.Count)
                                    {
                                        CellConvert(item, header[j], sheetData.ChildElements[i].ChildElements[j] as Cell);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ErrorMessages.Add(ex.Message);
                                break;
                            }
                            yield return item;
                        }
                    }

                }
            }
        }

    }
}





