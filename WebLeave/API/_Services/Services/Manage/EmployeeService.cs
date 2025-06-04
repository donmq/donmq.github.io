using AutoMapper;
using Microsoft.EntityFrameworkCore;
using API._Repositories;
using API.Helpers.Utilities;
using API.Models;
using API.Helpers.Enums;
using LinqKit;
using System.Globalization;
using API._Services.Interfaces.Manage;
using API.Dtos.Manage.EmployeeManage;
namespace API._Services.Services.Manage
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryAccessor _repository;
        private readonly IMapper _mapper;


        public EmployeeService(IRepositoryAccessor repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }
        public async Task<PaginationUtility<ListEmployeeDto>> Search(PaginationParam paginationParams, string keyword, string lang)
        {
            var pred = PredicateBuilder.New<Employee>(true);
            //Check giá trị thanh search
            if (!string.IsNullOrEmpty(keyword))
            {
                pred.And(x => x.EmpNumber.Contains(keyword.Trim()) || x.EmpName.EndsWith(keyword.Trim()));

            }
            //Get Data
            List<ListEmployeeDto> data = await _repository.Employee.FindAll(pred, true)// Nhân viên
                .Include(x => x.Part)
                    .ThenInclude(x => x.Dept)
                .Include(x => x.Part.PartLangs.Where(x => x.LanguageID == lang))
                .Include(x => x.Position)

                .Select(x => new ListEmployeeDto
                {
                    //Cột 1
                    DeptCode = x.Part.Dept.DeptCode + " - " + x.Part.Dept.DeptName, // Mã phòng ban - Tên phòng ban
                    //Cột 2
                    PositionSym = x.Position.PositionSym,
                    //Cột 3 + 4 
                    EmpID = x.EmpID,
                    PartID = x.PartID,
                    PartName = x.Part.PartLangs.FirstOrDefault(y => y.LanguageID == lang).PartName,
                    EmpName = x.EmpName,
                    EmpNumber = x.EmpNumber,
                    //Cột 5
                    Visible = x.Visible,
                }).OrderBy(i => i.EmpID).ToListAsync();
            return PaginationUtility<ListEmployeeDto>.Create(data, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<List<KeyValuePair<string, string>>> ListDeptID()
        {
            List<KeyValuePair<string, string>> result = await _repository.Department.FindAll()
            .OrderBy(x => x.Visible)
            .Select(x => new KeyValuePair<string, string>(
                x.DeptID.ToString(),
                x.DeptCode + "-" + x.DeptName
            )).ToListAsync();
            return result;
        }
        public async Task<List<KeyValuePair<string, string>>> ListPartID(int DeptID)
        {
            List<KeyValuePair<string, string>> result = await _repository.Part.FindAll(x => x.DeptID == DeptID)
            .OrderBy(x => x.Visible)
            .Select(x => new KeyValuePair<string, string>(
                x.PartID.ToString(),
                x.PartName
            )).ToListAsync();
            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> ListPositionID()
        {
            List<KeyValuePair<string, string>> result = await _repository.Position.FindAll()
            .Select(x => new KeyValuePair<string, string>(
                x.PositionID.ToString(),
                x.PositionName
            )).ToListAsync();
            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> ListGroupBase()
        {
            List<KeyValuePair<string, string>> result = await _repository.GroupBase.FindAll()
            .Select(x => new KeyValuePair<string, string>(
                x.GBID.ToString(),
                x.BaseName
            )).ToListAsync();
            return result;
        }


        public async Task<OperationResult> UpdateEmploy(EmployeeDto employee)
        {
            //cách 1:
            //var employee_test = await _repository.Employee.FindById(employee.EmpID);
            //end cách 1
            //cách 2 - Không cần findID , map luôn - đối với dữ liệu 1 bảng có đủ thuộc tính 
            Employee item = _mapper.Map<Employee>(employee);
            try
            {
                _repository.Employee.Update(item);
                await _repository.SaveChangesAsync();
                return new OperationResult(true, MessageConstants.UPDATE_SUCCESS, MessageConstants.SUCCESS);
            }
            catch (System.Exception)
            {
                return new OperationResult(false, MessageConstants.UPDATE_ERROR, MessageConstants.ERROR);
                throw;
            }
        }

        public async Task<OperationResult> UpdateInDetail(EmployExportDto employee)
        {
            int currentYear = DateTime.Now.Year;

            Employee item_employ = await _repository.Employee.FindById(employee.EmpID);
            item_employ.EmpNumber = employee.NumberID;
            item_employ.EmpName = employee.Fullname;
            item_employ.DateIn = Convert.ToDateTime(employee.DateIn);
            item_employ.PartID = employee.PartID;
            item_employ.GBID = employee.GBID;
            item_employ.PositionID = employee.PositionID;
            item_employ.Descript = employee.Descript;

            HistoryEmp item_history = await _repository.HistoryEmp.FirstOrDefaultAsync(x => x.EmpID == employee.EmpID && x.YearIn == currentYear);
            if(item_history == null)
                return null;
            item_history.TotalDay = employee.PhepNam;
            item_history.CountArran = employee.PNSapXep_DaNghi;
            item_history.CountAgent = employee.PNCaNhan_DaNghi_HeThong;
            item_history.CountLeave = employee.TongPhep_DaNghi;
            item_history.CountRestArran = employee.PNSapXep_ChuaNghi;
            item_history.CountRestAgent = employee.PNCaNhan_ChuaNghi;

            using var _transaction = await _repository.BeginTransactionAsync();
            try
            {
                _repository.Employee.Update(item_employ);
                _repository.HistoryEmp.Update(item_history);
                await _repository.SaveChangesAsync();
                await _transaction.CommitAsync();
                return new OperationResult(true, MessageConstants.UPDATE_SUCCESS, MessageConstants.SUCCESS);
            }
            catch (System.Exception)
            {
                await _transaction.RollbackAsync();
                return new OperationResult(false, MessageConstants.UPDATE_ERROR, MessageConstants.ERROR);
            }
        }

        public async Task<OperationResult> ExportExcelEmploy()
        {
            int year = DateTime.Now.Year;
            var data = await _repository.Employee.FindAll()
                    .OrderBy(o => o.PartID)
                    .Include(x => x.HistoryEmps)
                    .Include(x => x.Users)
                    .Include(x => x.Part)
                    .Include(x => x.GB)
                    .Include(x => x.Position)
                    .Select(x => new EmployExportDto
                    {
                        EmpID = x.EmpID,
                        PartCode = x.Part.PartCode ?? string.Empty,
                        NumberID = x.EmpNumber.Length < 4 ? "00" + x.EmpNumber : x.EmpNumber.Length < 5 ? "0" + x.EmpNumber : x.EmpNumber,
                        Fullname = x.EmpName,
                        DateIn = x.DateIn.HasValue ? x.DateIn.Value.ToString("MM/dd/yyyy") : "",
                        EmpPosition = x.Position.PositionSym,
                        EmpGroup = x.GB.BaseSym ?? string.Empty,
                        VoHieu = x.Visible == true ? "1" : "0",
                        CreateAccount = "1",
                        Year = year,
                        Email = x.Users.FirstOrDefault().EmailAddress ?? string.Empty,
                        PhepNam = x.HistoryEmps.FirstOrDefault(y => y.YearIn == year).TotalDay ?? 0,
                        PNSapXep_DaNghi = x.HistoryEmps.FirstOrDefault(y => y.YearIn == year).CountArran ?? 0,
                        PNSapXep_ChuaNghi = x.HistoryEmps.FirstOrDefault(y => y.YearIn == year).CountRestArran ?? 0,
                        PNCaNhan_DaNghi_HeThong = x.HistoryEmps.FirstOrDefault(y => y.YearIn == year).CountAgent ?? 0,
                        PNCaNhan_ChuaNghi = x.HistoryEmps.FirstOrDefault(y => y.YearIn == year).CountRestAgent ?? 0,
                        // PNCaNhan_ChuaNghi = x.HistoryEmps.FirstOrDefault(y => y.EmpID == x.EmpID && y.YearIn == year).CountRestAgent.HasValue
                        //                 ? Math.Round((double)x.HistoryEmps.FirstOrDefault(y => y.EmpID == x.EmpID && y.YearIn == year)
                        //                 .CountRestAgent.Value, 6, MidpointRounding.AwayFromZero)
                        //                 : 0,
                        TongPhep_DaNghi = x.HistoryEmps.FirstOrDefault(y => y.YearIn == year).CountLeave ?? 0,
                        PNCaNhan_DaNghi_HP = 0,
                    })
                    .AsNoTracking()
                    .ToListAsync();

            var res = await Task.FromResult(data);
            ExcelResult excelResult = ExcelUtility.DownloadExcel(res, "Resources\\Template\\Manage\\EmployeeExport.xlsx");

            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

        public async Task<EmployExportDto> getDataDetail(int EmpID, string lang)
        {
            int year = DateTime.Now.Year;

            EmployExportDto data = _repository.Employee.FindAll(x => x.EmpID == EmpID, true).OrderBy(o => o.PartID)
                .Include(x => x.Part)
                    .ThenInclude(x => x.Dept)
                .Include(x => x.Part.PartLangs.Where(x => x.LanguageID == lang))
                .Include(x => x.GB)
                    .ThenInclude(x => x.GroupLangs.Where(x => x.LanguageID == lang))
                .Include(x => x.Position)
                .Select(x => new EmployExportDto
                {
                    EmpID = x.EmpID,
                    PartID = x.PartID,
                    DeptID = x.Part.Dept.DeptID,
                    DeptName = x.Part.Dept.DeptName,
                    PartName = x.Part.PartName,
                    DeptCode = x.Part.Dept.DeptCode + " - " + x.Part.PartLangs.FirstOrDefault(y => y.LanguageID == lang).PartName,
                    PartCode = x.Part.PartCode,
                    NumberID = x.EmpNumber.Length < 4 ? "00" + x.EmpNumber : x.EmpNumber.Length < 5 ? "0" + x.EmpNumber : x.EmpNumber,
                    Fullname = x.EmpName,
                    DateIn = x.DateIn.HasValue ? x.DateIn.Value.ToString("dd/MM/yyyy") : "",
                    DateIn_By_Lang = x.DateIn.HasValue ? lang == "en" ? x.DateIn.Value.ToString("d - MMMM - yyyy") : x.DateIn.Value.ToString("d - MMMM - yyyy", new CultureInfo(lang)) : "",
                    Date_In_Edit = x.DateIn.HasValue ? x.DateIn.Value.ToString("dd/MM/yyyy hh:mm:ss") : "",
                    EmpPosition = x.Position.PositionSym,
                    PositionID = x.Position.PositionID,
                    PositionName = x.Position.PositionID + "-" + x.Position.PositionName,
                    GBID = x.GB.GBID,
                    EmpGroup = x.GB.GroupLangs.FirstOrDefault(y => y.LanguageID == lang).BaseName,
                    VoHieuBoolean = x.Visible,
                    VoHieu = x.Visible == true ? "0" : "1",
                    Descript = x.Descript,
                    Year = year
                }).ToList().FirstOrDefault();

            HistoryEmp history = await _repository.HistoryEmp.FirstOrDefaultAsync(x => x.EmpID == data.EmpID && x.YearIn == data.Year);
            if (history != null)
            {
                data.PhepNam = (double?)Math.Round((decimal)history.TotalDay, 6);
                data.PNSapXep_DaNghi =  history.CountArran;
                data.PNSapXep_ChuaNghi = (double?)Math.Round((decimal)history.CountRestArran, 6);
                data.PNCty = history.CountLeave;
                data.PNCaNhan_DaNghi_HeThong = history.CountAgent;
                data.PNCaNhan_ChuaNghi = (double?)Math.Round((decimal)history.CountRestAgent, 6);
                data.TongPhep_DaNghi = history.CountTotal;
                data.Phep_Nam_Ca_Nhan = Math.Round((decimal)history.CountRestAgent, 6).ToString() + "/" + Math.Round((decimal)history.Agent, 6).ToString();
                data.Phep_Nam_Cty = Math.Round((decimal)history.CountRestArran, 6).ToString() + "/" + Math.Round((decimal)history.Arrange, 6).ToString();

            }
            else
            {
                data.PhepNam = data.PNSapXep_DaNghi = data.PNSapXep_ChuaNghi
                = data.PNCaNhan_DaNghi_HeThong = data.PNCaNhan_ChuaNghi
                = data.TongPhep_DaNghi = 0;
            }
            data.PNCaNhan_DaNghi_HP = 0;
            data.Email = _repository.Users.FirstOrDefault(x => x.EmpID == data.EmpID)?.EmailAddress;

            return data;
        }

        public async Task<PaginationUtility<LeaveDataDto>> SearchDetail(PaginationParam param, int EmployeeId, int CategoryId, int Year, string lang)
        {
            var predicate = PredicateBuilder.New<LeaveData>(x => x.Status_Line == true);

            if (EmployeeId != 0)
                predicate.And(x => x.EmpID == EmployeeId);

            if (Year != 0)
                predicate.And(x => x.DateLeave.Value.Year >= Year);

            if (CategoryId != 0)
                predicate.And(x => x.CateID >= CategoryId);

            // Lấy  danh sách nghỉ của nhân viên theo năm bắt đầu đến năm hiện tại, với trạng thái là true
            List<LeaveDataDto> data = await _repository.LeaveData.FindAll(predicate, true)
                .Include(x => x.Cate)
                    .ThenInclude(x => x.CatLangs.Where(x => x.LanguageID == lang))
                .Select(x => new LeaveDataDto()
                {
                    LeaveID = x.LeaveID,
                    EmpID = x.EmpID,
                    CateID = x.CateID,
                    Created = x.Created,
                    Created_day = x.Created.Value.ToString("HH:mm dd/MM/yyyy"),
                    Category = $"{x.Cate.CateSym} - {x.Cate.CatLangs.FirstOrDefault(x => x.LanguageID == lang).CateName}",
                    TimeLine = x.TimeLine,
                    NumberDay = x.LeaveDay.ToString().ToDetailDay(),
                    StatusString = x.Approved.CheckStatus()
                }).OrderByDescending(x => x.Created).ToListAsync();

            return PaginationUtility<LeaveDataDto>.Create(data, param.PageNumber, param.PageSize);

        }

        public async Task<List<KeyValuePair<string, string>>> ListCataLog(string lang)
        {
            List<KeyValuePair<string, string>> data = await _repository.Category.FindAll(true)
                .Include(x => x.CatLangs.Where(x => x.LanguageID == lang))
                .Select(x => new KeyValuePair<string, string>(
                    x.CatLangs.FirstOrDefault(x => x.LanguageID == lang).CateID.ToString(),
                    x.CateSym + "-" + x.CatLangs.FirstOrDefault(x => x.LanguageID == lang).CateName
                )).ToListAsync();
            return data;
        }

        public async Task<OperationResult> RemoveEmploy(int empID)
        {
            var check = false;
            List<LeaveData> leaveData = await _repository.LeaveData.FindAll(x => x.EmpID == empID).ToListAsync();
            if (leaveData.Any())
            {
                _repository.LeaveData.RemoveMultiple(leaveData);
                check = await _repository.SaveChangesAsync();

            }

            Users user = await _repository.Users.FirstOrDefaultAsync(i => i.EmpID == empID);
            if (user != null)
            {
                _repository.Users.Remove(user);
                check = await _repository.SaveChangesAsync();

            }

            List<HistoryEmp> historyEmp = await _repository.HistoryEmp.FindAll(x => x.EmpID == empID).ToListAsync();
            if (historyEmp.Any())
            {
                _repository.HistoryEmp.RemoveMultiple(historyEmp);
                check = await _repository.SaveChangesAsync();

            }

            List<ReportData> reportData = await _repository.ReportData.FindAll(x => x.EmpID == empID).ToListAsync();
            if (reportData.Any())
            {
                _repository.ReportData.RemoveMultiple(reportData);
                check = await _repository.SaveChangesAsync();

            }
            Employee employee = await _repository.Employee.FindById(empID);
            if (employee != null)
            {
                _repository.Employee.Remove(employee);
                check = await _repository.SaveChangesAsync();

            }
            if (check)
            {
                return new OperationResult(true, MessageConstants.REMOVE_SUCCESS, MessageConstants.SUCCESS);
            }
            else
            {
                return new OperationResult(false, MessageConstants.REMOVE_ERROR, MessageConstants.ERROR);
            }

        }
        public async Task<OperationResult> changeVisible(int empID)
        {
            Employee model = await _repository.Employee.FindById(empID);
            model.Visible = !model.Visible;

            Users user = await _repository.Users.FirstOrDefaultAsync(x => x.EmpID == empID);
            if (user is not null)
            {
                user.Visible = model.Visible;
                _repository.Users.Update(user);
            }

            _repository.Employee.Update(model);
            await _repository.SaveChangesAsync();
            return new OperationResult(true, MessageConstants.UPDATE_SUCCESS, MessageConstants.SUCCESS);

        }
    }

}