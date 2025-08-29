using System.Drawing;
using API.Data;
using API._Services.Interfaces;
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using API.Helper.Constant;
using API.Models;
using Aspose.Cells;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SalaryMaintenance
{
    public class S_7_1_16_SalaryMasterFile : BaseServices, I_7_1_16_SalaryMasterFile
    {
        private readonly I_Common _common;
        public S_7_1_16_SalaryMasterFile(DBContext dbContext,I_Common common) : base(dbContext)
        {
            _common = common;
        }

        public async Task<PaginationUtility<SalaryMasterFile_Main>> GetDataPagination(PaginationParam pagination, SalaryMasterFile_Param param)
        {
            var data = await GetData(param);
            return PaginationUtility<SalaryMasterFile_Main>.Create(data, pagination.PageNumber, pagination.PageSize);
        }

        private async Task<List<SalaryMasterFile_Main>> GetData(SalaryMasterFile_Param param)
        {

            var pred_HRMS_Sal_Master = PredicateBuilder.New<HRMS_Sal_Master>(true);
            var pred_HRMS_Emp_Personal = PredicateBuilder.New<HRMS_Emp_Personal>(true);
            if (!string.IsNullOrWhiteSpace(param.Factory))
            {
                pred_HRMS_Sal_Master.And(x => x.Factory == param.Factory);
            }

            if (!string.IsNullOrWhiteSpace(param.Department))
                pred_HRMS_Sal_Master.And(x => x.Department == param.Department);

            if (!string.IsNullOrWhiteSpace(param.Employee_ID))
            {
                pred_HRMS_Sal_Master.And(x => x.Employee_ID.Contains(param.Employee_ID));
            }

            if (!string.IsNullOrWhiteSpace(param.Position_Title))
                pred_HRMS_Sal_Master.And(x => x.Position_Title == param.Position_Title);
            if (param.Permission_Group.Any())
            {
                pred_HRMS_Sal_Master.And(x => param.Permission_Group.Contains(x.Permission_Group));
            }
            if (!string.IsNullOrWhiteSpace(param.Salary_Type))
                pred_HRMS_Sal_Master.And(x => x.Salary_Type == param.Salary_Type);
            if (!string.IsNullOrWhiteSpace(param.Salary_Grade))
                pred_HRMS_Sal_Master.And(x => x.Salary_Grade == Convert.ToDecimal(param.Salary_Grade));
            if (!string.IsNullOrWhiteSpace(param.Salary_Level))
                pred_HRMS_Sal_Master.And(x => x.Salary_Level == Convert.ToDecimal(param.Salary_Level));
            var Eul = _repositoryAccessor.HRMS_Emp_Unpaid_Leave.FindAll(x => x.Factory == param.Factory && x.Effective_Status == true);
            var sal_History = _repositoryAccessor.HRMS_Sal_History.FindAll(true);
            var basic = _repositoryAccessor.HRMS_Basic_Code.FindAll(true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language
                .FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower(), true),
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },

                    (x, y) => new { HBC = x, HBCL = y })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, HBCL = y })
                .Select(x => new
                {
                    x.HBC.Type_Seq,
                    x.HBC.Code,
                    Name = $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"
                });

            var departments = _repositoryAccessor.HRMS_Org_Department.FindAll(true)
                .Join(_repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(b => b.Kind == "1", true),
                    department => new { department.Division, department.Factory },
                    factoryComparison => new { factoryComparison.Division, factoryComparison.Factory },
                    (department, factoryComparison) => department)
                .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower(), true),
                    department => new { department.Factory, department.Department_Code },
                    language => new { language.Factory, language.Department_Code },
                    (department, language) => new { Department = department, Language = language })
                .SelectMany(
                    x => x.Language.DefaultIfEmpty(),
                    (x, language) => new { x.Department, Language = language })
                .OrderBy(x => x.Department.Department_Code)
                .Select(x => new
                {
                    x.Department.Factory,
                    x.Department.Department_Code,
                    Department_Name = $"{x.Department.Department_Code} - {(x.Language != null ? x.Language.Name : x.Department.Department_Name)}"
                }).Distinct();
            var datas = await _repositoryAccessor.HRMS_Sal_Master.FindAll(pred_HRMS_Sal_Master)
                            .Join(_repositoryAccessor.HRMS_Emp_Personal.FindAll(pred_HRMS_Emp_Personal),
                                x => new { x.USER_GUID, x.Factory, x.Employee_ID },
                                y => new { y.USER_GUID, y.Factory, y.Employee_ID },
                                (x, y) => new { Sal_Master = x, Emp_Personal = y }
                            ).Select(x => new SalaryMasterFile_Main
                            {
                                Factory = x.Sal_Master.Factory,
                                Department = x.Sal_Master.Department,
                                Department_Str = departments.FirstOrDefault(de => de.Factory == x.Sal_Master.Factory && de.Department_Code == x.Sal_Master.Department).Department_Name,
                                Employee_ID = x.Sal_Master.Employee_ID,
                                Local_Full_Name = x.Emp_Personal.Local_Full_Name,
                                Employment_Status = Eul.Any(y => y.Employee_ID == x.Emp_Personal.Employee_ID) ? "U" : x.Emp_Personal.Deletion_Code == "Y" ? "Y" : "N",
                                Position_Title = x.Sal_Master.Position_Title,
                                Position_Title_Str = basic.FirstOrDefault(p => p.Code == x.Sal_Master.Position_Title && p.Type_Seq == BasicCodeTypeConstant.JobTitle).Name,
                                Permission_Group = x.Sal_Master.Permission_Group,
                                Permission_Group_Str = basic.FirstOrDefault(p => p.Code == x.Sal_Master.Permission_Group && p.Type_Seq == BasicCodeTypeConstant.PermissionGroup).Name,
                                Salary_Type = x.Sal_Master.Salary_Type,
                                Salary_Type_Str = basic.FirstOrDefault(p => p.Code == x.Sal_Master.Salary_Type && p.Type_Seq == BasicCodeTypeConstant.SalaryType).Name,
                                Salary_Grade = x.Sal_Master.Salary_Grade,
                                Salary_Level = x.Sal_Master.Salary_Level,
                                Currency = x.Sal_Master.Currency,
                                Effective_Date = sal_History.Where(salHistory => salHistory.USER_GUID == x.Emp_Personal.USER_GUID).Max(i => i.Effective_Date).ToString("yyyy/MM/dd"),
                                Update_By = x.Sal_Master.Update_By,
                                Update_Time = x.Sal_Master.Update_Time.ToString("yyyy/MM/dd HH:mm:ss"),
                                Technical_Type = x.Sal_Master.Technical_Type,
                                Technical_Type_Str = basic.FirstOrDefault(p => p.Code == x.Sal_Master.Technical_Type && p.Type_Seq == BasicCodeTypeConstant.Technical_Type).Name,
                                Expertise_Category = x.Sal_Master.Expertise_Category,
                                Expertise_Category_Str = basic.FirstOrDefault(p => p.Code == x.Sal_Master.Expertise_Category && p.Type_Seq == BasicCodeTypeConstant.Expertise_Category).Name,
                                Onboard_Date = x.Emp_Personal.Onboard_Date.ToString("yyyy/MM/dd"),
                                ActingPosition_Start = x.Sal_Master.ActingPosition_Start.HasValue ? x.Sal_Master.ActingPosition_Start.Value.ToString("yyyy/MM/dd") : null,
                                ActingPosition_End = x.Sal_Master.ActingPosition_End.HasValue ? x.Sal_Master.ActingPosition_End.Value.ToString("yyyy/MM/dd") : null,
                                Position_Grade = x.Sal_Master.Position_Grade
                            }).ToListAsync();

            if (!string.IsNullOrWhiteSpace(param.Employment_Status))
                datas = datas.Where(x => x.Employment_Status == param.Employment_Status).ToList();

            return datas;
        }

        public async Task<List<KeyValuePair<string, string>>> GetFactorys(string userName, string language)
        {
            var factoryAccounts = await Queryt_Factory_AddList(userName);
            var factories = await GetDataBasicCode(BasicCodeTypeConstant.Factory, language);
            return factories.IntersectBy(factoryAccounts, x => x.Key).ToList();
        }

        public async Task<List<KeyValuePair<string, string>>> GetDepartments(string factory, string language)
        {
            return await _common.GetListDepartment(language, factory);
        }

        public async Task<List<KeyValuePair<string, string>>> GetPositionTitles(string language)
        {
            return await GetDataBasicCode(BasicCodeTypeConstant.JobTitle, language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string language)
        {
            return await GetDataBasicCode(BasicCodeTypeConstant.PermissionGroup, language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetSalaryTypes(string language)
        {
            return await GetDataBasicCode(BasicCodeTypeConstant.SalaryType, language);
        }

        public async Task<SalaryMasterFile_Detail> GetDataQueryPage(PaginationParam pagination, string factory, string employee_ID, string language)
        {
            SalaryMasterFile_Detail result = new();
            var basic = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.SalaryItem, true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language
                .FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { HBC = x, HBCL = y })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, HBCL = y })
                .Select(x => new
                {
                    x.HBC.Code,
                    Name = $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"
                });
            var salaryItems = await _repositoryAccessor.HRMS_Sal_Master_Detail.FindAll(x => x.Factory == factory && x.Employee_ID == employee_ID)
                                .Select(x => new SalaryItem
                                {
                                    Salary_Item = basic.FirstOrDefault(p => p.Code == x.Salary_Item).Name,
                                    Amount = x.Amount
                                }).Distinct().ToListAsync();
            var dataPaging = PaginationUtility<SalaryItem>.Create(salaryItems, pagination.PageNumber, pagination.PageSize);
            var total_Salary = salaryItems.Sum(x => x.Amount);
            result.Total_Salary = total_Salary;
            result.SalaryItemsPagination = dataPaging;
            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetTechnicalTypes(string language)
        {
            return await GetDataBasicCode(BasicCodeTypeConstant.Technical_Type, language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetExpertiseCategorys(string language)
        {
            return await GetDataBasicCode(BasicCodeTypeConstant.Expertise_Category, language);
        }

        public async Task<OperationResult> DownloadFileExcel(SalaryMasterFile_Param param, string userName)
        {
            var data = await GetData(param);
            if (!data.Any())
                return new OperationResult(false, "No Data");

            DateTime now = DateTime.Now;
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.SalaryItem, true).ToList();
            var excelData = new List<dynamic>();

            var salaryItems = await _repositoryAccessor.HRMS_Sal_Master_Detail
                .FindAll(x => x.Factory == param.Factory)
                .Select(x => x.Salary_Item)
                .Distinct()
                .ToListAsync();
            if (!salaryItems.Any())
                return new OperationResult(false, "No Data");
            var salaryItemResults = await _repositoryAccessor.HRMS_Sal_Master_Detail.FindAll(x => x.Factory == param.Factory && salaryItems.Contains(x.Salary_Item))
                .Select(x => new SalaryItem
                {
                    Employee_ID = x.Employee_ID,
                    Amount = x.Amount,
                    Salary_Item = x.Salary_Item,
                })
                .ToListAsync();



            foreach (var record in data)
            {
                var row = new Dictionary<string, object>
                {
                    { "Factory", record.Factory },
                    { "Department", record.Department_Str},
                    { "Employee_ID", record.Employee_ID },
                    { "Local_Full_Name", record.Local_Full_Name },
                    { "Employment_Status", param.Language.ToLower() == "en" ? (record.Employment_Status == "U" ? "U.Unpaid" : record.Employment_Status == "Y" ? "Y.On job" : "N.Resigned")
                                                                 : (record.Employment_Status == "U" ? "U.留停" : record.Employment_Status == "Y" ? "Y.在職" : "N.離職") },
                    { "Position_Grade", record.Position_Grade },
                    { "Position_Title", record.Position_Title_Str},
                    { "ActingPosition_Start", string.IsNullOrWhiteSpace(record.ActingPosition_Start) ? null : ParseDateOrNull(record.ActingPosition_Start)},
                    { "ActingPosition_End", string.IsNullOrWhiteSpace(record.ActingPosition_End) ? null : ParseDateOrNull(record.ActingPosition_End)},
                    { "Technical_Type", record.Technical_Type_Str},
                    { "Expertise_Category", record.Expertise_Category_Str},
                    { "Onboard_Date", string.IsNullOrWhiteSpace(record.Onboard_Date) ? null : ParseDateOrNull(record.Onboard_Date)},
                    { "Effective_Date", string.IsNullOrWhiteSpace(record.Effective_Date) ? null : ParseDateOrNull(record.Effective_Date)},
                    { "Permission_Group", record.Permission_Group_Str},
                    { "Salary_Type", record.Salary_Type_Str},
                    { "Salary_Grade", record.Salary_Grade },
                    { "Salary_Level", record.Salary_Level },
                    { "Currency", record.Currency },
                    { "Update_By", record.Update_By },
                    { "Update_Time", record.Update_Time }
                };

                var salaryItemList = salaryItemResults.FindAll(x => x.Employee_ID == record.Employee_ID).Select(item => new SalaryItem
                {
                    Salary_Item = item.Salary_Item,
                    Amount = item.Amount
                }).ToList();

                foreach (var salaryItem in salaryItems)
                {
                    var amount = salaryItemList.FirstOrDefault(s => s.Salary_Item == salaryItem)?.Amount ?? 0;

                    row[$"{salaryItem}"] = amount;
                }
                excelData.Add(row);
            }


            MemoryStream memoryStream = new();
            string file = Path.Combine(
                rootPath, 
                "Resources\\Template\\SalaryMaintenance\\7_1_16_SalaryMasterFile\\Download.xlsx"
            );
            WorkbookDesigner obj = new()
            {
                Workbook = new Workbook(file)
            };
            Worksheet worksheet = obj.Workbook.Worksheets[0];





            Style titleStyle = obj.Workbook.CreateStyle();
            titleStyle.Font.IsBold = true;
            titleStyle.ForegroundColor = Color.FromArgb(221, 235, 247);
            titleStyle.Pattern = BackgroundType.Solid;
            titleStyle = AsposeUtility.SetAllBorders(titleStyle);

            Style dataStyleSalaryItem = obj.Workbook.CreateStyle();
            dataStyleSalaryItem.Custom = "#,##0";
            dataStyleSalaryItem = AsposeUtility.SetAllBorders(dataStyleSalaryItem);

            var salaryItemTitle = salaryItems.Select(x => new SalaryItem
            {
                Salary_Item = x,
                Salary_Item_Name = x + " - " + HBCL.FirstOrDefault(y => y.Language_Code.ToLower() == "en" && y.Code == x)?.Code_Name,
                Salary_Item_NameTW = x + " - " + HBCL.FirstOrDefault(y => y.Language_Code.ToLower() == "tw" && y.Code == x)?.Code_Name,
            }).ToList();

            for (int i = 0; i < salaryItemTitle.Count; i++)
            {
                worksheet.Cells[4, i + 20].PutValue(salaryItemTitle[i].Salary_Item_NameTW);
                worksheet.Cells[5, i + 20].PutValue(salaryItemTitle[i].Salary_Item_Name);

                // Áp dụng style cho các ô vừa ghi
                worksheet.Cells[4, i + 20].SetStyle(titleStyle);
                worksheet.Cells[5, i + 20].SetStyle(titleStyle);

            }


            worksheet.Cells["B2"].PutValue(userName);
            worksheet.Cells["D2"].PutValue(now.ToString("yyyy/MM/dd HH:mm:ss"));

            Style dataStyle = obj.Workbook.CreateStyle();
            dataStyle = AsposeUtility.SetAllBorders(dataStyle);

            // Tạo style cho định dạng ngày tháng
            Style dateStyle = obj.Workbook.CreateStyle();
            dateStyle.Custom = "YYYY/MM/DD";
            dateStyle = AsposeUtility.SetAllBorders(dateStyle);

            // Ghi dữ liệu
            for (int i = 0; i < excelData.Count; i++)
            {
                var row = excelData[i];
                int columnIndex = 0;

                foreach (var key in row.Keys)
                {
                    worksheet.Cells[i + 6, columnIndex].PutValue(row[key]);
                    worksheet.Cells[i + 6, columnIndex].SetStyle(dataStyle);
                    worksheet.Cells[i + 6, 7].SetStyle(dateStyle);
                    worksheet.Cells[i + 6, 8].SetStyle(dateStyle);
                    worksheet.Cells[i + 6, 11].SetStyle(dateStyle);
                    worksheet.Cells[i + 6, 12].SetStyle(dateStyle);
                    columnIndex++;
                }
            }
            worksheet.Cells.CreateRange(6, 20, excelData.Count, salaryItemTitle.Count).SetStyle(dataStyleSalaryItem);
            worksheet.AutoFitColumns();
            obj.Workbook.Save(memoryStream, SaveFormat.Xlsx);
            var excelResult = new ExcelResult(isSuccess: true, memoryStream.ToArray());
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
        private static readonly string rootPath = Directory.GetCurrentDirectory();

        private static DateTime? ParseDateOrNull(string dateString)
        {
            return DateTime.TryParse(dateString, out DateTime result) && result != DateTime.MinValue
                ? result
                : null;
        }

    }
}