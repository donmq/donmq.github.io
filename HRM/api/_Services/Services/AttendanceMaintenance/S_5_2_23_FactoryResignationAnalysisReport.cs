using System.Globalization;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public partial class S_5_2_23_FactoryResignationAnalysisReport : BaseServices, I_5_2_23_FactoryResignationAnalysisReport
    {

        public S_5_2_23_FactoryResignationAnalysisReport(DBContext dbContext) : base(dbContext) { }

        public async Task<List<KeyValuePair<string, string>>> GetDropDownList(FactoryResignationAnalysisReport_Param param, List<string> roleList)
        {
            var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll();
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower());
            var result = new List<KeyValuePair<string, string>>();
            var data = HBC.GroupJoin(HBCL,
                x => new { x.Type_Seq, x.Code },
                y => new { y.Type_Seq, y.Code },
                (x, y) => new { hbc = x, hbcl = y })
                .SelectMany(x => x.hbcl.DefaultIfEmpty(),
                (x, y) => new { x.hbc, hbcl = y });
            var authFactories = await Queryt_Factory_AddList(roleList);
            result.AddRange(data.Where(x => x.hbc.Type_Seq == BasicCodeTypeConstant.Factory && authFactories.Contains(x.hbc.Code)).Select(x => new KeyValuePair<string, string>("FA", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            if (!string.IsNullOrWhiteSpace(param.Factory))
            {
                var authPermission = await Query_Permission_Group_List(param.Factory);
                result.AddRange(data.Where(x => x.hbc.Type_Seq == BasicCodeTypeConstant.PermissionGroup && authPermission.Contains(x.hbc.Code)).Select(x => new KeyValuePair<string, string>("PE", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            }
            return result;
        }

        public async Task<OperationResult> Process(FactoryResignationAnalysisReport_Param param, string userName)
        {
            if (
                !DateTime.TryParseExact(param.Date_From_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Date_From) ||
                !DateTime.TryParseExact(param.Date_To_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Date_To)
              )
                return new OperationResult(false, "InvalidInput");
            param.Date_From_Date = Date_From;
            param.Date_To_Date = Date_To;
            OperationResult result = param.Function_Type switch
            {
                "search" => await Search(param),
                "excel" => await Excel(param, userName),
                _ => new OperationResult(false, "InvalidFunc")
            };
            return result;
        }
        private async Task<OperationResult> Search(FactoryResignationAnalysisReport_Param param)
        {
            var res = await GetData(param);
            return new OperationResult(true, res.Data);
        }
        private async Task<OperationResult> Excel(FactoryResignationAnalysisReport_Param param, string userName)
        {
            var res = await GetData(param, false);
            if (res.Data is List<FactoryResignationAnalysisReport_Detail> data && data.Count > 0)
            {
                var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x =>
                    x.Type_Seq == BasicCodeTypeConstant.PermissionGroup &&
                    param.Permission_Group.Contains(x.Code)
                );
                var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower());
                var Permission_Group = await HBC
                    .GroupJoin(HBCL,
                        x => new { x.Type_Seq, x.Code },
                        y => new { y.Type_Seq, y.Code },
                        (x, y) => new { hbc = x, hbcl = y })
                    .SelectMany(x => x.hbcl.DefaultIfEmpty(),
                        (x, y) => new { x.hbc, hbcl = y })
                    .Select(x => $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}").ToListAsync();
                FactoryResignationAnalysisReport_Excel result = new()
                {
                    Factory = param.Factory,
                    Date_From = param.Date_From_Str,
                    Date_To = param.Date_To_Str,
                    Permission_Group = string.Join(" / ", Permission_Group),
                    Print_By = userName,
                    Print_Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    Detail = data
                };
                ExcelResult excelResult = ExcelUtility.DownloadExcel(
                    new List<FactoryResignationAnalysisReport_Excel>() { result },
                    "Resources\\Template\\AttendanceMaintenance\\5_2_23_FactoryResignationAnalysisReport\\Download.xlsx",
                    new ConfigDownload(false)
                );
                return new OperationResult(excelResult.IsSuccess, excelResult.Error, new { data.Count, excelResult.Result });
            }
            return new OperationResult(true, new { Count = 0 });
        }
        private async Task<OperationResult> GetData(FactoryResignationAnalysisReport_Param param, bool isSearch = true)
        {
            List<FactoryResignationAnalysisReport_Detail> result = new();
            var _HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(x =>
               x.Factory == param.Factory &&
               param.Permission_Group.Contains(x.Permission_Group)
            );
            List<string> otherLeaveList = new() { "E0", "F0", "H0", "K0", "G1" };
            List<string> personalLeaveList = new() { "A0", "O0", "G3" };
            List<string> unpaidLeaveList = new() { "J3", "J4" };
            List<string> sickLeaveList = new() { "B0", "G2" };
            var _HACR = _repositoryAccessor.HRMS_Att_Change_Record.FindAll(x => x.Factory == param.Factory);
            var cur0_w = await _HACR
                .Where(x => x.Att_Date.Date >= param.Date_From_Date.Date && x.Att_Date.Date <= param.Date_To_Date.Date)
                .Select(x => x.Att_Date)
                .Distinct()
                .ToListAsync();
            if (isSearch)
                return new OperationResult(true, cur0_w.Count);
            var HEP_HACR = _HEP.Join(_HACR,
                x => x.Employee_ID,
                y => y.Employee_ID,
                (x, y) => new { HEP = x, HACR = y }).ToList();
            var HEP = _HEP.ToList();
            foreach (var Att_Date in cur0_w)
            {
                FactoryResignationAnalysisReport_Detail PH = new()
                {
                    Att_Date = Att_Date
                };
                var temp_HEP_HACR = HEP_HACR.FindAll(x => x.HACR.Att_Date.Date == PH.Att_Date.Date);
                var resign_HEP_HACR = temp_HEP_HACR.FindAll(x =>
                    !x.HEP.Resign_Date.HasValue || (x.HEP.Resign_Date.HasValue && x.HEP.Resign_Date.Value.Date >= PH.Att_Date.Date));
                // 2. Number of people taking maternity leave
                PH.pregn = resign_HEP_HACR.Where(x => x.HACR.Leave_Code == "G0")
                    .GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();

                // 3. New employees
                PH.newop = HEP.Where(x => x.Onboard_Date.Date == PH.Att_Date.Date)
                    .GroupBy(x => x.Employee_ID).Select(i => i.FirstOrDefault()).Count();

                // 4. Total number of employees to be on-board (excluding maternity leave)               
                PH.totemp = HEP.Where(x => (!x.Resign_Date.HasValue || (x.Resign_Date.HasValue && x.Resign_Date.Value.Date >= PH.Att_Date.Date)) &&
                                           (x.Onboard_Date.Date < PH.Att_Date.Date) &&
                                           (!x.Resign_Date.HasValue || (x.Resign_Reason != null && x.Resign_Reason != "B202")))
                    .GroupBy(x => x.Employee_ID).Select(i => i.FirstOrDefault()).Count();
                PH.totemp -= PH.pregn;

                // 5.	旷职(不含连续旷职5天以上旷职)
                PH.absent = resign_HEP_HACR.Where(x => x.HACR.Leave_Code == "C0" && x.HEP.Onboard_Date.Date < PH.Att_Date.Date)
                    .GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();
                PH.absentr = PH.absent / PH.totemp;

                // 6. T.Suspension of work (full pay)
                PH.wkoff_t = resign_HEP_HACR.Where(x => x.HACR.Leave_Code == "J2" && x.HEP.Onboard_Date.Date < PH.Att_Date.Date)
                    .GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();
                PH.wkoffr_t = PH.wkoff_t / PH.totemp;

                // 7. L. Work stoppage (basic salary + allowance)
                PH.wkoff_l = resign_HEP_HACR.Where(x => x.HACR.Leave_Code == "J1" && x.HEP.Onboard_Date.Date < PH.Att_Date.Date)
                    .GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();
                PH.wkoffr_l = PH.wkoff_l / PH.totemp;

                // 8. I. Work stoppage (regional minimum basic salary)
                PH.wkoff_i = resign_HEP_HACR.Where(x => x.HACR.Leave_Code == "J0" && x.HEP.Onboard_Date.Date < PH.Att_Date.Date)
                    .GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();
                PH.wkoffr_i = PH.wkoff_i / PH.totemp;

                // 9. K. Work stoppage (negotiated salary)
                PH.wkoff_k = resign_HEP_HACR.Where(x => x.HACR.Leave_Code == "J5" && x.HEP.Onboard_Date.Date < PH.Att_Date.Date)
                    .GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();
                PH.wkoffr_k = PH.wkoff_k / PH.totemp;

                // 10. Number of other leave
                PH.leave = resign_HEP_HACR.Where(x => otherLeaveList.Contains(x.HACR.Leave_Code) && x.HEP.Onboard_Date.Date < PH.Att_Date.Date)
                    .GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();
                PH.leaver = PH.leave / PH.totemp;

                // 11. Number of annual leave (company)
                PH.vacu = resign_HEP_HACR.Where(x => x.HACR.Leave_Code == "I1" && x.HEP.Onboard_Date.Date < PH.Att_Date.Date)
                    .GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();
                PH.vacuer = PH.vacu / PH.totemp;

                // 12. Number of annual leave (employees)
                PH.vacj = resign_HEP_HACR.Where(x => x.HACR.Leave_Code == "I0" && x.HEP.Onboard_Date.Date < PH.Att_Date.Date)
                    .GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();
                PH.vacjer = PH.vacj / PH.totemp;

                // 13. Number of personal leave
                PH.aff = resign_HEP_HACR
                    .Where(x =>
                        personalLeaveList.Contains(x.HACR.Leave_Code) &&
                        x.HEP.Onboard_Date.Date < PH.Att_Date.Date)
                    .GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();
                PH.affer = PH.aff / PH.totemp;

                // 14. Number of unpaid leave
                PH.aff_q = resign_HEP_HACR
                    .Where(x =>
                        unpaidLeaveList.Contains(x.HACR.Leave_Code) &&
                        x.HEP.Onboard_Date.Date < PH.Att_Date.Date)
                    .GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();
                PH.affer_q = PH.aff_q / PH.totemp;

                // 15. Number of sick leave
                PH.dis = resign_HEP_HACR
                    .Where(x =>
                        sickLeaveList.Contains(x.HACR.Leave_Code) &&
                        x.HEP.Onboard_Date.Date < PH.Att_Date.Date)
                    .GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();
                PH.diser = PH.dis / PH.totemp;

                // 16. Number of resignations
                PH.resign = HEP.Where(x =>
                        x.Resign_Date.HasValue && x.Resign_Date.Value.Date == PH.Att_Date.Date &&
                        x.Onboard_Date.Date < PH.Att_Date.Date)
                    .GroupBy(x => x.Employee_ID).Select(i => i.FirstOrDefault()).Count();
                PH.resignr = PH.resign / PH.totemp;

                // 17. Total number of employees
                PH.actemp = PH.totemp - PH.absent - PH.vacu - PH.vacj - PH.aff - PH.aff_q - PH.dis - PH.leave + PH.wkoff_t + PH.wkoff_l + PH.wkoff_i + PH.wkoff_k - PH.resign;
                PH.actemp2 = PH.totemp - PH.absent - PH.vacj - PH.aff - PH.dis - PH.resign;

                // 18. Total absenteeism rate
                PH.totabsr = (PH.absent + PH.vacu + PH.vacj + PH.aff + PH.aff_q + PH.dis + PH.leave + PH.resign) / PH.totemp;
                PH.totabsr2 = (PH.totemp - PH.actemp2) / PH.totemp;
                PH.totabsr3 = PH.absentr + PH.vacuer + PH.vacjer + PH.affer + PH.affer_q + PH.diser + PH.leaver + PH.wkoffr_t + PH.wkoffr_l + PH.wkoffr_i + PH.wkoffr_k;

                var week_HEP_HACR = temp_HEP_HACR.FindAll(x => x.HACR.Leave_Code == "C0");

                // 19. Number of non-new employees leaving
                PH.oldres = HEP.Where(x =>
                        x.Resign_Date.HasValue && x.Resign_Date.Value.Date == PH.Att_Date.Date &&
                        x.Onboard_Date.Date < PH.Att_Date.Date)
                    .GroupBy(x => x.Employee_ID).Select(i => i.FirstOrDefault()).Count();

                // 20. Within one week after joining the factory (new absenteeism)
                PH.wc1 = week_HEP_HACR.Where(x =>
                    x.HEP.Onboard_Date.Date >= PH.Att_Date.AddDays(-7).Date &&
                    x.HEP.Onboard_Date.Date < PH.Att_Date.Date
                ).GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();

                // 21. Within two weeks after joining the factory (new absenteeism)
                PH.wc2 = week_HEP_HACR.Where(x =>
                    x.HEP.Onboard_Date.Date >= PH.Att_Date.AddDays(-14).Date &&
                    x.HEP.Onboard_Date.Date < PH.Att_Date.AddDays(-7).Date
                ).GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();

                // 22. Within three weeks after joining the factory (new absenteeism)
                PH.wc3 = week_HEP_HACR.Where(x =>
                    x.HEP.Onboard_Date.Date >= PH.Att_Date.AddDays(-21).Date &&
                    x.HEP.Onboard_Date.Date < PH.Att_Date.AddDays(-14).Date
                ).GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();

                // 23. Within four weeks after joining the factory (new absenteeism)
                PH.wc4 = week_HEP_HACR.Where(x =>
                    x.HEP.Onboard_Date.Date >= PH.Att_Date.AddDays(-28).Date &&
                    x.HEP.Onboard_Date.Date < PH.Att_Date.AddDays(-21).Date
                ).GroupBy(x => x.HACR.Employee_ID).Select(i => i.FirstOrDefault()).Count();

                // 24. Total number of employees (new absenteeism)	
                PH.totalwc = PH.wc1 + PH.wc2 + PH.wc3 + PH.wc4;
                result.Add(PH);
            }
            return new OperationResult(true, result); ;
        }
    }
}
