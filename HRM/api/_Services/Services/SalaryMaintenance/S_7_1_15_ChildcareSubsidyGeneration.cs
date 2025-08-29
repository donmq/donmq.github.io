using System.Globalization;
using API.Data;
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SalaryMaintenance
{
  public class S_7_1_15_ChildcareSubsidyGeneration : BaseServices, I_7_1_15_ChildcareSubsidyGeneration
  {
    public S_7_1_15_ChildcareSubsidyGeneration(DBContext dbContext) : base(dbContext)
    {
    }


    public async Task<OperationResult> CheckParam(ChildcareSubsidyGenerationParam param)
    {

      if (
        string.IsNullOrWhiteSpace(param.Factory) ||
        string.IsNullOrWhiteSpace(param.YearMonth) ||

        string.IsNullOrWhiteSpace(param.Kind_Tab1) ||
        param.PermissionGroupMultiple.Count == 0 ||
        !DateTime.TryParseExact(param.YearMonth, "yyyy/MM", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime YearMonth)
      )
        return new OperationResult(false, "Invalid Input");

      var att_Month = DateTime.Parse(param.YearMonth);


      // Kiểm tra dữ liệu chấm công tháng
      if (param.Kind_Tab1 == "R")
      {
        var wk_count = await _repositoryAccessor.HRMS_Att_Resign_Monthly.AnyAsync(x => x.Factory == param.Factory && x.Att_Month == att_Month);
        if (!wk_count)
          return new OperationResult(false, "本月離職出勤資料未產生，不可執行!");
      }
      else
      {
        var wk_count = await _repositoryAccessor.HRMS_Att_Monthly.AnyAsync(x => x.Factory == param.Factory && x.Att_Month == att_Month);
        if (!wk_count)
          return new OperationResult(false, "本月出勤資料未產生，不可執行!");
      }

      // Lấy danh sách USER_GUID cần xử lý
      var predHEP = PredicateBuilder.New<HRMS_Emp_Personal>(x => x.Factory == param.Factory
              && param.PermissionGroupMultiple.Contains(x.Permission_Group));

      if (param.Kind_Tab1 == "R")
      {
        if (!string.IsNullOrEmpty(param.ResignedDate_Start) && !string.IsNullOrEmpty(param.ResignedDate_End))
        {
          var date_Start = DateTime.Parse(param.ResignedDate_Start);
          var date_End = DateTime.Parse(param.ResignedDate_End);
          predHEP.And(x => x.Resign_Date >= date_Start
                   && x.Resign_Date <= date_End);
        }
      }

      var userGuids = _repositoryAccessor.HRMS_Emp_Personal
          .FindAll(predHEP)
          .Select(x => x.USER_GUID);

      var ad_count = await _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly
          .AnyAsync(x =>
              x.Factory == param.Factory &&
              x.Sal_Month == att_Month &&
              x.AddDed_Type == "A" &&
              x.AddDed_Item == "A13" &&
              userGuids.Contains(x.USER_GUID));

      return new OperationResult(true, ad_count ? "DeleteData" : null);
    }

    #region ExcuteTab1
    public async Task<OperationResult> ExcuteTab1(ChildcareSubsidyGenerationParam param, string userName)
    {
      await _repositoryAccessor.BeginTransactionAsync();
      try
      {
        List<HRMS_Sal_AddDedItem_Monthly> list_Sal_AddDedItem_Monthly;

        if (param.Is_Delete)
        {
          var delete = await DeleteData(param);
          if (!delete.IsSuccess)
          {
            await _repositoryAccessor.RollbackAsync();
            return new OperationResult(false, "Delete failed!");
          }
        }

        if (param.Kind_Tab1 == "O")
        {
          //1.1 Input Kind ='On Job'
          list_Sal_AddDedItem_Monthly = await GetDataKindO(param, userName);
        }
        else
        {
          //2.1 Input Kind ='Resigned
          list_Sal_AddDedItem_Monthly = await GetDataKindR(param, userName);
        }

        _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly.AddMultiple(list_Sal_AddDedItem_Monthly);
        await _repositoryAccessor.Save();

        await _repositoryAccessor.CommitAsync();
        return new OperationResult(true, list_Sal_AddDedItem_Monthly.Count);

      }
      catch (Exception)
      {
        await _repositoryAccessor.RollbackAsync();
        return new OperationResult(false);
      }
    }
    #endregion

    #region GetDataKindO
    private async Task<List<HRMS_Sal_AddDedItem_Monthly>> GetDataKindO(ChildcareSubsidyGenerationParam param, string userName)
    {
      //1.2	
      //1.2.1	
      var att_Month = DateTime.Parse(param.YearMonth);
      var month_First_day = att_Month;
      var month_Last_day = att_Month.AddMonths(1).AddDays(-1);
      List<HRMS_Sal_AddDedItem_Monthly> list_Sal_AddDedItem_Monthly = new();

      var predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(x =>
          x.Factory == param.Factory &&
          param.PermissionGroupMultiple.Contains(x.Permission_Group) &&
          (!x.Resign_Date.HasValue || (x.Resign_Date.HasValue && x.Resign_Date.Value > month_Last_day))
      );

      var predSalChildcareSubsidy = PredicateBuilder.New<HRMS_Sal_Childcare_Subsidy>(x => x.Factory == param.Factory);

      var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(predEmpPersonal);
      var HSCS = _repositoryAccessor.HRMS_Sal_Childcare_Subsidy.FindAll(predSalChildcareSubsidy);
      var dataList = await HSCS
          .Join(HEP,
              x => new { x.USER_GUID, x.Factory },
              y => new { y.USER_GUID, y.Factory },
              (x, y) => new { subsidy = x, personal = y })
          .Select(x => new
          {
            x.subsidy.USER_GUID,
            x.subsidy.Employee_ID,
            x.personal.Permission_Group
          }).Distinct().ToListAsync();

      var HALM = _repositoryAccessor.HRMS_Att_Leave_Maintain
          .FindAll(x =>
              x.Factory == param.Factory &&
              x.Leave_Date >= month_First_day &&
              x.Leave_Date <= month_Last_day
          ).ToList();
      var HAM = _repositoryAccessor.HRMS_Att_Monthly.FindAll(x => x.Factory == param.Factory && x.Att_Month == att_Month).ToList();
      var HSCS_num = HSCS.Where(s => s.Month_Start <= att_Month && s.Month_End >= att_Month).ToList();
      var now = DateTime.Now;
      //1.2.2
      foreach (var item in dataList)
      {

        var att_Monthly_Values = HAM.FirstOrDefault(a => a.USER_GUID == item.USER_GUID);
        if (att_Monthly_Values == null) continue;

        // J3 leave days (無薪假)
        decimal J3_Values = HALM.Where(x => x.USER_GUID == item.USER_GUID && x.Leave_code == "J3")?.Sum(x => x.Days) ?? 0;
        if (J3_Values == att_Monthly_Values.Salary_Days) continue;

        // J4 leave days (暫緩合同)
        decimal J4_Values = HALM.Where(x => x.USER_GUID == item.USER_GUID && x.Leave_code == "J4")?.Sum(x => x.Days) ?? 0;
        if (J4_Values == att_Monthly_Values.Salary_Days) continue;

        //1.2.3
        decimal Num = HSCS_num.Where(s => s.USER_GUID == item.USER_GUID)?.Sum(s => s.Num_Children) ?? 0;
        if (Num == 0) continue;

        var amount = await Query_HRMS_Sal_AddDedItem_Values(
            param.Factory,
            att_Month,
            att_Monthly_Values.Permission_Group,
            att_Monthly_Values.Salary_Type,
            "A",
            "A13"
        );

        // Tính total
        var totalLeavedays = J3_Values + J4_Values;
        var deduction = amount / att_Monthly_Values.Salary_Days * totalLeavedays * Num;
        var total = Num * amount - deduction;

        var sal_AddDedItem_Monthly = new HRMS_Sal_AddDedItem_Monthly
        {
          USER_GUID = item.USER_GUID,
          Factory = param.Factory,
          Sal_Month = att_Month,
          Employee_ID = item.Employee_ID,
          AddDed_Type = "A",
          AddDed_Item = "A13",
          Currency = GetCurrency(item.Permission_Group).Result,
          Amount = (int)total,
          Update_By = userName,
          Update_Time = now
        };
        list_Sal_AddDedItem_Monthly.Add(sal_AddDedItem_Monthly);
      }
      return list_Sal_AddDedItem_Monthly;
    }
    #endregion

    #region GetDataKindR
    private async Task<List<HRMS_Sal_AddDedItem_Monthly>> GetDataKindR(ChildcareSubsidyGenerationParam param, string userName)
    {
      //2.2	
      //2.2.1		
      var att_Month = DateTime.Parse(param.YearMonth);

      var date_Start = string.IsNullOrWhiteSpace(param.ResignedDate_Start)
          ? DateTime.MinValue
          : DateTime.Parse(param.ResignedDate_Start);

      var date_End = string.IsNullOrWhiteSpace(param.ResignedDate_End)
          ? DateTime.MaxValue
          : DateTime.Parse(param.ResignedDate_End);
      var leave_Codes = new List<string>()
                {
                    "I0","I1","N0","D0",
                    "E0","F0","H0","K0",
                    "J0","J1","J2","J5"
                };
      List<HRMS_Sal_AddDedItem_Monthly> list_Sal_AddDedItem_Monthly = new();

      var predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(x => x.Factory == param.Factory
          && param.PermissionGroupMultiple.Contains(x.Permission_Group)
          && x.Resign_Date >= date_Start
          && x.Resign_Date <= date_End);

      var predSalChildcareSubsidy = PredicateBuilder.New<HRMS_Sal_Childcare_Subsidy>(x => x.Factory == param.Factory);
      var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(predEmpPersonal);
      var HSCS = _repositoryAccessor.HRMS_Sal_Childcare_Subsidy.FindAll(predSalChildcareSubsidy);
      var HARM = _repositoryAccessor.HRMS_Att_Resign_Monthly.FindAll(x => x.Factory == param.Factory);

      var dataList = await HSCS
          .Join(HARM,
              x => new { x.USER_GUID, x.Factory },
              y => new { y.USER_GUID, y.Factory },
              (x, y) => new { subsidy = x, resign = y })
          .Join(HEP,
              x => new { x.subsidy.USER_GUID, x.subsidy.Factory },
              y => new { y.USER_GUID, y.Factory },
              (x, y) => new { x.subsidy, x.resign, personal = y })
          .Select(x => new
          {
            x.subsidy.USER_GUID,
            x.subsidy.Employee_ID,
            x.personal.Permission_Group,
          }).Distinct().ToListAsync();
      var HARMD = _repositoryAccessor.HRMS_Att_Resign_Monthly_Detail
          .FindAll(d =>
              d.Factory == param.Factory &&
              d.Att_Month == att_Month &&
              d.Leave_Type == "1" &&
              leave_Codes.Contains(d.Leave_Code))
          .ToList();
      var HACR = _repositoryAccessor.HRMS_Att_Change_Record
          .FindAll(c =>
              c.Factory == param.Factory &&
              c.Leave_Code == "00" &&
              c.Holiday == "C00" &&
              c.Att_Date >= date_Start &&
              c.Att_Date <= date_End)
          .ToList();
      var HSCS_num = HSCS.Where(s => s.Month_Start <= att_Month && s.Month_End >= att_Month).ToList();
      var now = DateTime.Now;
      //2.2.2	
      foreach (var item in dataList)
      {
        var Att_Resign_Monthly_Values = HARM.FirstOrDefault(r => r.Att_Month == att_Month && r.USER_GUID == item.USER_GUID);
        if (Att_Resign_Monthly_Values == null) continue;

        var leaveCount = HARMD.FindAll(d => d.Employee_ID == Att_Resign_Monthly_Values.Employee_ID)?.Sum(d => d.Days) ?? 0;
        var nHoliday = HACR.FindAll(c => c.Employee_ID == Att_Resign_Monthly_Values.Employee_ID)?.Sum(c => c.Days) ?? 0;
        var saldayCount = Att_Resign_Monthly_Values.Actual_Days + leaveCount + nHoliday;
        if (saldayCount == 0) continue;

        //2.2.3
        decimal Num = HSCS_num.Where(s => s.USER_GUID == item.USER_GUID)?.Sum(s => s.Num_Children) ?? 0;
        if (Num == 0) continue;

        // Điều chỉnh ngày lương nếu cần
        var salaryDays = Att_Resign_Monthly_Values.Salary_Days;

        if (salaryDays == 0) continue;

        if (salaryDays > 27) salaryDays = 26;

        // Lấy Amount và tính total
        var amount = await Query_HRMS_Sal_AddDedItem_Values(
            param.Factory,
            att_Month,
            Att_Resign_Monthly_Values.Permission_Group,
            Att_Resign_Monthly_Values.Salary_Type,
            "A",
            "A13"
        );

        var total = Num * amount / salaryDays * saldayCount;
        var sal_AddDedItem_Monthly = new HRMS_Sal_AddDedItem_Monthly
        {
          USER_GUID = item.USER_GUID,
          Factory = param.Factory,
          Sal_Month = att_Month,
          Employee_ID = item.Employee_ID,
          AddDed_Type = "A",
          AddDed_Item = "A13",
          Currency = GetCurrency(item.Permission_Group).Result,
          Amount = (int)total,
          Update_By = userName,
          Update_Time = now
        };
        list_Sal_AddDedItem_Monthly.Add(sal_AddDedItem_Monthly);
      }
      return list_Sal_AddDedItem_Monthly;
    }
    #endregion

    #region ExcuteTab2
    public async Task<OperationResult> ExcuteTab2(ChildcareSubsidyGenerationParam param, string userName)
    {
      var listSubtotal = await GetDataExcel(param);

      if (!listSubtotal.Any())
        return new OperationResult(false, "No Data");

      List<Table> tables = new()
                        {
                            new Table("result", listSubtotal)
                        };

      List<Cell> cells = new()
                        {
                            new Cell("B2", param.YearMonth),
                            new Cell("D2", userName),
                            new Cell("F2", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
                        };

      bool isLocal = param.Kind_Tab2 == "S";
      ConfigDownload config = new() { IsAutoFitColumn = true };


      string templatePath = isLocal
             ? "Resources\\Template\\SalaryMaintenance\\7_1_15_ChildcareSubsidyGeneration\\MonthlyDepartmentSubtotal.xlsx"
             : "Resources\\Template\\SalaryMaintenance\\7_1_15_ChildcareSubsidyGeneration\\MonthlyChildcareSubsidyDetail.xlsx";
      ExcelResult excelResult = ExcelUtility.DownloadExcel(tables, cells, templatePath, config);
      return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
    }
    #endregion

    #region GetTotalRows
    public async Task<int> GetTotaTab2(ChildcareSubsidyGenerationParam param)
    {
      var data = await GetDataExcel(param);
      return data.Count;
    }
    #endregion

    #region GetDataExcel
    private async Task<List<D_7_15_ChildcareSubsidyGenerationDto>> GetDataExcel(ChildcareSubsidyGenerationParam param)
    {
      var year_Month = DateTime.Parse(param.YearMonth);
      List<D_7_15_ChildcareSubsidyGenerationDto> listSubtotal = new();
      var predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(x => x.Factory == param.Factory && param.PermissionGroupMultiple.Contains(x.Permission_Group));
      var predSalAddDedItemMonthly = PredicateBuilder.New<HRMS_Sal_AddDedItem_Monthly>(x => x.Factory == param.Factory && x.Sal_Month == year_Month && x.AddDed_Type == "A" && x.AddDed_Item == "A13");

      var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(predEmpPersonal);
      var HSAM = _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly.FindAll(predSalAddDedItemMonthly);

      var HSCS = _repositoryAccessor.HRMS_Sal_Childcare_Subsidy.FindAll(x => x.Factory == param.Factory &&
                                                                             x.Month_Start <= year_Month &&
                                                                             x.Month_End >= year_Month);
      
      var qry_cur = await HSAM
          .Join(HEP,
             x => new { x.USER_GUID, x.Factory },
             y => new { y.USER_GUID, y.Factory },
             (x, y) => new { sal = x, personal = y }).
          Select(x => new
          {
            x.sal.USER_GUID,
            x.sal.Amount,
            x.personal.Department,
            x.personal.Employee_ID,
            x.personal.Local_Full_Name
          }).Distinct().ToListAsync();

      if (param.Kind_Tab2 == "S")
      {
        foreach (var value in qry_cur)
        {
          //Monthly Department Subtotal 
          decimal NUM = HSCS.Where(x => x.USER_GUID == value.USER_GUID)?.Sum(x => x.Num_Children) ?? 0;

          var (Department_Code, Department_Name) = await Query_Department_Report(param.Factory, value.Department, param.Lang);

          listSubtotal.Add(new D_7_15_ChildcareSubsidyGenerationDto
          {
            Department = Department_Code,
            Factory = param.Factory,
            DepartmentName = Department_Name,
            NumberOfChildren = NUM
          });
        }
        return listSubtotal;
      }
      else
      {
        // Monthly Childcare Subsidy Detail 
        foreach (var value in qry_cur)
        {
          decimal Num_Children_Value = HSCS.Where(x => x.USER_GUID == value.USER_GUID)?.Sum(x => x.Num_Children) ?? 0;

          var (Department_Code, Department_Name) = await Query_Department_Report(param.Factory, value.Department, param.Lang);

          listSubtotal.Add(new D_7_15_ChildcareSubsidyGenerationDto
          {
            Department = Department_Code,
            Factory = param.Factory,
            DepartmentName = Department_Name,
            EmployeeID = value.Employee_ID,
            LocalFullName = value.Local_Full_Name,
            NumberOfChildren = Num_Children_Value,
            SubsidyAmount = value.Amount
          });
        }
        return listSubtotal;
      }
    }
    #endregion

    private async Task<OperationResult> DeleteData(ChildcareSubsidyGenerationParam param)
    {
      try
      {
        var att_Month = DateTime.Parse(param.YearMonth);

        // Lấy danh sách USER_GUID cần xử lý
        var predHEP = PredicateBuilder.New<HRMS_Emp_Personal>(x => x.Factory == param.Factory
            && param.PermissionGroupMultiple.Contains(x.Permission_Group));

        if (param.Kind_Tab1 == "R")
        {
          if (!string.IsNullOrEmpty(param.ResignedDate_Start) && !string.IsNullOrEmpty(param.ResignedDate_End))
          {
            var date_Start = DateTime.Parse(param.ResignedDate_Start);
            var date_End = DateTime.Parse(param.ResignedDate_End);
            predHEP.And(x => x.Resign_Date >= date_Start
                     && x.Resign_Date <= date_End);
          }
        }

        var userGuids = _repositoryAccessor.HRMS_Emp_Personal
            .FindAll(predHEP)
            .Select(x => x.USER_GUID);

        var ad_count = await _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly
            .FindAll(x =>
                x.Factory == param.Factory &&
                x.Sal_Month == att_Month &&
                x.AddDed_Type == "A" &&
                x.AddDed_Item == "A13" &&
                userGuids.Contains(x.USER_GUID))
            .ToListAsync();

        if (ad_count.Any())
          _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly.RemoveMultiple(ad_count);
        return new OperationResult(await _repositoryAccessor.Save());
      }
      catch (Exception)
      {
        return new OperationResult(false);
      }
    }

    #region GetList    
    public async Task<List<KeyValuePair<string, string>>> GetListFactoryByUser(string userName, string language)
    {
      var factoriesByAccount = await Queryt_Factory_AddList(userName);
      var factories = await GetDataBasicCode(BasicCodeTypeConstant.Factory, language);

      return factories.IntersectBy(factoriesByAccount, x => x.Key).ToList();
    }

    public async Task<List<KeyValuePair<string, string>>> GetListPermissionGroupByFactory(string factory, string language)
    {
      var permissionGroupByFactory = await Query_Permission_Group_List(factory);
      var permissionGroup = await GetDataBasicCode(BasicCodeTypeConstant.PermissionGroup, language);
      return permissionGroup.IntersectBy(permissionGroupByFactory, x => x.Key).ToList();
    }

    private async Task<string> GetCurrency(string Permission_Group)
    {
      return await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "4" && x.Code == Permission_Group).Select(x => x.Char1).FirstOrDefaultAsync();
    }

    private async Task<(string, string)> Query_Department_Report(string factory, string department, string lang)
    {
      var result = await _repositoryAccessor.HRMS_Org_Department
          .FindAll(x => x.Factory == factory && x.Department_Code == department, true)
          .GroupJoin(
              _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower() && x.Factory == factory && x.Department_Code == department, true),
              x => new { x.Department_Code },
              y => new { y.Department_Code },
              (x, y) => new { HOD = x, HODL = y }
          )
          .SelectMany(x => x.HODL.DefaultIfEmpty(), (x, y) => new { x.HOD, HBCL = y })
         .Select(x => new
         {
           x.HOD.Department_Code,
           Department_Name = x.HBCL != null ? x.HBCL.Name : x.HOD.Department_Name
         })
          .FirstOrDefaultAsync();

      return (result?.Department_Code, result?.Department_Name);
    }
    #endregion
  }
}