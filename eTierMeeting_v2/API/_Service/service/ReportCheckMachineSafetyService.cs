using System.Data.SqlClient;
using API.Helpers.Utilities;
using Aspose.Cells;
using Aspose.Cells.Drawing;
using Machine_API._Accessor;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Enums;
using Machine_API.Helpers.Utilities;
using Microsoft.EntityFrameworkCore;
namespace Machine_API._Service.service
{
    public class ReportCheckMachineSafetyService : IReportCheckMachineSafetyService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly IConfiguration _configuration;
        private readonly string _factory;
        private readonly string _ownerFty;
        private readonly string _sqlConnection;

        public ReportCheckMachineSafetyService(
            IMachineRepositoryAccessor repository,
            IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
            _factory = _configuration.GetSection("AppSettings:Factory").Value;
            _ownerFty = configuration.GetSection("AppSettings:OwnerFty").Value;
            string area = _configuration.GetSection("AppSettings:Area").Value;
            _sqlConnection = _configuration.GetConnectionString($"{_factory}_{area}_DefaultConnection");
        }

        private async Task<List<Machine_Safe_Check_ReportDto>> GetData(string dateFrom, string dateTo, string lang)
        {
            List<Machine_Safe_Check_ReportDto> result = new();
            using SqlConnection connection = new(_sqlConnection);
            await connection.OpenAsync();
            string sql = @$"
            SELECT 
                msc.AssnoID,
                FORMAT(msc.CheckDate, 'MM/dd/yyyy') AS CheckDate,
                CONCAT(RTRIM(a15.Place), '-',RTRIM(a15.Plno)) AS Location,
                CASE 
                    WHEN @Lang = 'vi-VN' THEN a04.MachineName_Local
                    WHEN @Lang = 'zh-TW' THEN a04.MachineName_CN
                    ELSE a04.MachineName_EN
                END AS MachineName,
                    msc.Check_Item as QuestionCode,
                    msc.Resault AS Answer,
                msc.Pic_Path
            FROM Machine_Safe_Check msc
            INNER JOIN Machine_Safe_Checklist mscl
                ON msc.Check_Item = mscl.Id
            INNER JOIN hp_a04 a04
                ON msc.AssnoID = a04.AssnoID
                AND msc.Dept_ID = a04.Dept_ID
                AND a04.Visible = 1
            INNER JOIN hp_a15 a15
                ON a04.Plno = a15.Plno
            ";

            if (!string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
            {
                sql += " WHERE msc.CheckDate >= @DateFrom AND msc.CheckDate <= DATEADD(DAY, 1, @DateTo)";
            }

            using SqlCommand command = new(sql, connection);
            command.CommandTimeout = SystemConstains.CommandTimeout;
            if (!string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
            {
                command.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(dateFrom));
                command.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(dateTo));
            }
            command.Parameters.AddWithValue("@Lang", lang ?? "vi-VN");

            // execute result
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                string assnoID = reader["AssnoID"]?.ToString() ?? string.Empty;
                Machine_Safe_Check_ReportDto model = new()
                {
                    MachineCode = string.IsNullOrEmpty(assnoID) ? string.Empty : $"{_ownerFty}{assnoID}",
                    MachineName = reader["MachineName"]?.ToString(),
                    Location = reader["Location"]?.ToString(),
                    CheckDate = reader["CheckDate"]?.ToString(),
                    QuestionCode = reader["QuestionCode"].ToInt(),
                    Answer = reader["Answer"]?.ToString(),
                    ImagePath = reader["Pic_Path"]?.ToString() ?? string.Empty
                };
                result.Add(model);
            }
            return result;
        }

        public async Task<OperationResult> ExportExcel(ReportCheckMachineSafetyParam param)
        {
            List<Machine_Safe_Check_ReportDto> data = await GetData(param.FromDate, param.ToDate, param.Lang);
            if (!data.Any())
                return new OperationResult { Success = false, Message = "No data" };
            try
            {
                MemoryStream stream = new();
                string template = @"Resources\Template\CheckMachineSafetyReportTemplate.xlsx";
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", template);
                WorkbookDesigner designer = new() { Workbook = new Workbook(fullPath) };

                Worksheet ws = designer.Workbook.Worksheets[0];
                ws.Cells["A1"].PutValue(param.Lang == "vi-VN" ? "Mã số" :
                    (param.Lang == "zh-TW" ? "編碼" : "Machine Code"));
                ws.Cells["B1"].PutValue(param.Lang == "vi-VN" ? "Tên máy" :
                    (param.Lang == "zh-TW" ? "機器名稱" : "Machine Name"));
                ws.Cells["C1"].PutValue(param.Lang == "vi-VN" ? "Đơn vị đang sử dụng" :
                    (param.Lang == "zh-TW" ? "正在使用的單位" : "Curent Location"));
                ws.Cells["D1"].PutValue(param.Lang == "vi-VN" ? "Ngày kiểm tra" :
                    (param.Lang == "zh-TW" ? "檢查日期" : "Check Date"));

                // Lấy danh sách các trường động từ database
                List<AnswerListDto> dynamicFields = await _repository.Machine_Safe_Checklist.FindAll().AsNoTracking().Select(x =>
                new AnswerListDto()
                {
                    Id = x.Id,
                    Name = param.Lang == "vi-VN" ? x.ChecklistName_Local : (param.Lang == "zh-TW" ? x.ChecklistName_CN : x.ChecklistName_EN)
                }).OrderBy(x => x.Id).ToListAsync();

                // Thêm xen kẽ trường "image"
                List<string> interleavedFields = AddInterleavedImageField(dynamicFields, param.Lang);

                // (các trường câu hỏi trong Machine_Safe_Checklist theo hàng ngang trong file excel)
                int startColumnIndex = 4; // E1
                for (int i = 0; i < interleavedFields.Count; i++)
                {
                    string columnName = CellsHelper.ColumnIndexToName(startColumnIndex + i); // E, F, G,...
                    ws.Cells[$"{columnName}1"].PutValue(interleavedFields[i]);
                }

                // Group data by MachineCode, MachineName, Location, and CheckDate
                var groupedData = data.GroupBy(d => new { d.MachineCode, d.MachineName, d.Location, d.CheckDate });

                int rowIndex = 1;
                foreach (var group in groupedData)
                {
                    var firstItem = group.FirstOrDefault();
                    ws.Cells[$"A{rowIndex + 1}"].PutValue(firstItem?.MachineCode);
                    ws.Cells[$"B{rowIndex + 1}"].PutValue(firstItem?.MachineName);
                    ws.Cells[$"C{rowIndex + 1}"].PutValue(firstItem?.Location);
                    ws.Cells[$"D{rowIndex + 1}"].PutValue(firstItem?.CheckDate);

                    int columnIndex = startColumnIndex;
                    foreach (var item in group)
                    {
                        ws.Cells[rowIndex, columnIndex].PutValue(item.Answer);

                        // Add image
                        if (!string.IsNullOrEmpty(item.ImagePath))
                        {
                            string picPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", item.ImagePath.Replace("/", "\\"));
                            if (File.Exists(picPath))
                            {
                                Picture picture = ws.Pictures[ws.Pictures.Add(rowIndex, columnIndex + 1, picPath)];
                                picture.SetPictureSize();

                                ws.Cells.SetRowHeight(rowIndex, picture.Height);
                                ws.Cells.SetColumnWidth(columnIndex + 1, 20); // Adjust column width based on image width
                            }
                        }
                        columnIndex += 2; // Move to the next pair of columns (answer and image)
                    }
                    rowIndex++;
                }

                designer.Process();
                AsposeUtility.ApplyBorders(ws);
                ws.AutoFitColumns(0, startColumnIndex - 1); // ignore AutoFit cột chứa hình ảnh
                designer.Workbook.Save(stream, SaveFormat.Xlsx);
                return new OperationResult { Success = true, Data = stream.ToArray() };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        // Hàm xen kẽ trường "image"
        private static List<string> AddInterleavedImageField(List<AnswerListDto> fields, string lang)
        {
            List<string> result = new();
            foreach (var field in fields)
            {
                result.Add(field.Name);
                result.Add(lang == "vi-VN" ? "Hình ảnh không tuân thủ" : (lang == "zh-TW" ? "不合規圖片" : "Non-compliance pic"));
            }
            return result;
        }

    }

}