// using Aspose.Cells;
// namespace SD3_API.Helpers.Utilities
// {
//     public static class ExcelUtility
//     {
//         private static string rootPath = Directory.GetCurrentDirectory();

//         public static ExcelResult CheckExcel(IFormFile file, string subPath)
//         {
//             if (file == null)
//                 return new ExcelResult(false, "File not found");
//             using Stream stream = file.OpenReadStream();
//             WorkbookDesigner designer = new WorkbookDesigner();
//             designer.Workbook = new Workbook(stream);
//             Worksheet ws = designer.Workbook.Worksheets[0];
//             if (designer.Workbook.Worksheets.Count() > 1)
//                 return new ExcelResult(false, "More than one sheet");
//             string pathTemp = Path.Combine(rootPath, subPath);
//             designer.Workbook = new Workbook(pathTemp);
//             Worksheet wsTemp = designer.Workbook.Worksheets[0];
//             ws.Cells.DeleteBlankColumns();
//             ws.Cells.DeleteBlankRows();
//             if (ws.Cells.Columns.Count < wsTemp.Cells.Columns.Count)
//                 return new ExcelResult(false, "Not enough column for data import");
//             if (ws.Cells.Columns.Count > wsTemp.Cells.Columns.Count)
//                 return new ExcelResult(false, "Higher column quantity than required");
//             if (ws.Cells.Rows.Count <= wsTemp.Cells.Rows.Count)
//                 return new ExcelResult(false, "No data in excel file");
//             string firstCellTemp = wsTemp.Cells[0, 0].Name;
//             string lastCellTemp = wsTemp.Cells[wsTemp.Cells.MaxRow, wsTemp.Cells.MaxColumn].Name;
//             Aspose.Cells.Range rangeTemp = wsTemp.Cells.CreateRange(firstCellTemp, lastCellTemp);
//             Aspose.Cells.Range range = ws.Cells.CreateRange(firstCellTemp, lastCellTemp);
//             for (int r = 0; r < rangeTemp.RowCount; r++)
//             {
//                 for (int c = 0; c < rangeTemp.ColumnCount; c++)
//                 {
//                     string val = range[r, c].Value != null ? range[r, c].StringValue.Trim() : "";
//                     string valTmp = rangeTemp[r, c].Value != null ? rangeTemp[r, c].StringValue.Trim() : "";
//                     if (val != valTmp)
//                         return new ExcelResult(false, $"Header in cell {CellsHelper.CellIndexToName(r, c)} : {val}\nMust be : {valTmp}");
//                 }
//             }
//             return new ExcelResult(true, ws, wsTemp);
//         }

//         public static byte[] DownloadExcel<T>(List<T> data, string subPath)
//         {
//             MemoryStream stream = new MemoryStream();
//             if (data.Any())
//             {
//                 var path = Path.Combine(rootPath, subPath);
//                 WorkbookDesigner designer = new WorkbookDesigner();
//                 designer.Workbook = new Workbook(path);
//                 Worksheet ws = designer.Workbook.Worksheets[0];
//                 designer.SetDataSource("result", data);
//                 designer.Process();
//                 designer.Workbook.Save(stream, SaveFormat.Xlsx);
//             }
//             return stream.ToArray();
//         }
//     }
//     public class ExcelResult
//     {
//         public string Error { set; get; }
//         public bool IsSuccess { set; get; }
//         public Worksheet ws { set; get; }
//         public Worksheet wsTemp { set; get; }

//         public ExcelResult(bool isSuccess, Worksheet worksheet, Worksheet worksheetTemp)
//         {
//             this.IsSuccess = isSuccess;
//             this.ws = worksheet;
//             this.wsTemp = worksheetTemp;
//         }
//         public ExcelResult(bool isSuccess, string error)
//         {
//             this.Error = error;
//             this.IsSuccess = isSuccess;
//         }
//     }
// }