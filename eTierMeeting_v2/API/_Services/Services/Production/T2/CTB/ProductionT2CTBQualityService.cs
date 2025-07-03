using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T2.C2B;
using eTierV2_API.Data;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eTierV2_API._Services.Services.Production.T2.C2B
{
    public class ProductionT2CTBQualityService : IProductionT2CTBQualityService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepositoryAccessor _repoAccessor;
        public ProductionT2CTBQualityService( 
            IConfiguration configuration,
            IRepositoryAccessor repoAccessor)
        {
            _configuration = configuration;
            _repoAccessor =  repoAccessor;
        }

        private int GetLineNameOrder(string lineSname)
        {
            string numStr = Regex.Replace(lineSname, @"[^0-9]", "");
            if (numStr.Length > 0)
                return Convert.ToInt32(numStr);
            else
                return 999;
        }

        public async Task<List<FRI_BA_DefectDTO>> GetBADefectTop3Chart(string tuCode, bool switchDate)
        {
            if (string.IsNullOrEmpty(tuCode?.Trim()))
                return null;

            tuCode = tuCode.Trim();
            var dataDate = await GetMaxDate(tuCode);

            if (dataDate == null)
                return null;

            var factory = _configuration.GetSection("Appsettings:FactoryId").Value;
            var data = new FRI_BA_DefectDTO();
            data.Data_Date = dataDate.Value;
            var firstDayOfMonth = new DateTime(data.Data_Date.Year, data.Data_Date.Month, 1);
            var etmMESQualityDefectData = await _repoAccessor.eTM_MES_Quality_Defect_Data
                .FindAll(x =>
                    (x.Data_Kind.Trim() == Common.KIND_BADEF) &&
                    (!switchDate ?
                        (x.Data_Date == data.Data_Date) :
                        (x.Data_Date <= data.Data_Date && x.Data_Date >= firstDayOfMonth)))
                .ToListAsync();
            var friBADefect = await _repoAccessor.FRI_BA_Defect.FindAll(x => x.Factory_ID == factory).ToListAsync();
            var vmDeptFromMES = await _repoAccessor.VW_DeptFromMES.FindAll().ToListAsync();
            var vwLineGroup = await _repoAccessor.VW_LineGroup.FindAll().ToListAsync();

            var result = etmMESQualityDefectData
                .Join(friBADefect,
                    x => new { Reason_ID = x.Reason_ID.Trim() },
                    y => new { Reason_ID = y.BA_Defect_ID.Trim() },
                    (x, y) => new { T1 = x, T2 = y })
                .Join(vmDeptFromMES,
                    x => new { Dept_ID = x.T1.Dept_ID.Trim() },
                    y => new { Dept_ID = y.Dept_ID.Trim() },
                    (x, y) => new { x.T1, x.T2, T3 = y })
                .Join(vwLineGroup,
                    x => new { Dept_ID = x.T1.Dept_ID.Trim() },
                    y => new { Dept_ID = y.Dept_ID.Trim() },
                    (x, y) => new { x.T1, x.T2, x.T3, T4 = y })
                .Where(x => x.T4.Line_Group.Trim() == tuCode || x.T3.Building.Trim() == tuCode)
                .GroupBy(x => new
                {
                    Reason_ID = x.T1.Reason_ID.Trim(),
                    Building = x.T3.Building.Trim()
                }).Select(x => new FRI_BA_DefectDTO
                {
                    Reason_ID = x.Key.Reason_ID?.Trim(),
                    BA_Defect_Desc = x.FirstOrDefault()?.T2.BA_Defect_Desc,
                    Finding_Qty = x.Sum(m => m.T1.Finding_Qty)
                }).OrderByDescending(x => x.Finding_Qty).ThenBy(x => x.Reason_ID).Take(3).ToList();

            return result;
        }

        public async Task<T2CTBQualityDTO> GetData(string tuCode, bool switchDate)
        {
            if (string.IsNullOrEmpty(tuCode?.Trim()))
                return null;

            tuCode = tuCode.Trim();
            var dataDate = await GetMaxDate(tuCode);

            if (dataDate == null)
                return null;

            var factory = _configuration.GetSection("Appsettings:FactoryId").Value;
            var data = new T2CTBQualityDTO();
            data.Data_Date = dataDate.Value;
            var firstDayOfMonth = new DateTime(data.Data_Date.Year, data.Data_Date.Month, 1);
            var hasData = false;

            while (!hasData)
            {
                /* -------------------------------- RFT Chart ------------------------------- */
                data.RFT_Chart = _repoAccessor.eTM_Dept_Score_Data
                    .FindAll(x =>
                        (x.Output_FGIN > 0 && x.RFT > 0) &&
                        (!switchDate ?
                            (x.Insert_Time.Date == data.Data_Date) :
                            (x.Insert_Time.Date >= firstDayOfMonth && x.Insert_Time.Date <= data.Data_Date)))
                    .Join(_repoAccessor.VW_DeptFromMES.FindAll(),
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(_repoAccessor.VW_LineGroup.FindAll(),
                        x => new { Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x =>
                        (x.T3.Factory_ID.Trim() == factory) &&
                        (x.T3.Line_Group.Trim() == tuCode || x.T2.Building.Trim() == tuCode))
                    .GroupBy(x => x.T2.Line_Sname)
                    .Select(x => new RFTChart
                    {
                        Line_Sname = x.Key,
                        RFT = Math.Round((x.Sum(y => y.T1.Output_FGIN) / (x.Sum(y => y.T1.Output_FGIN / y.T1.RFT))) ?? 0, 1)
                    }).ToList().OrderByDescending(m => GetLineNameOrder(m.Line_Sname)).ThenByDescending(m => m.Line_Sname).ToList();

                /* -------------------------------- BA Chart -------------------------------- */
                data.BA_Chart = _repoAccessor.eTM_Dept_Score_Data
                    .FindAll(x =>
                        (x.Output_FGIN > 0) &&
                        (!switchDate ?
                            (x.Insert_Time.Date == data.Data_Date) :
                            (x.Insert_Time.Date >= firstDayOfMonth && x.Insert_Time.Date <= data.Data_Date)))
                    .Join(_repoAccessor.VW_DeptFromMES.FindAll(),
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(_repoAccessor.VW_LineGroup.FindAll(),
                        x => new { Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x => x.T3.Line_Group.Trim() == tuCode || x.T2.Building.Trim() == tuCode)
                    .GroupBy(x => x.T2.Line_Sname)
                    .Select(x => new BAChart
                    {
                        Line_Sname = x.Key,
                        BA = x.Sum(y => y.T1.BA * y.T1.Output_FGIN) / x.Sum(y => y.T1.Output_FGIN)
                    }).ToList().OrderByDescending(m => GetLineNameOrder(m.Line_Sname)).ThenByDescending(m => m.Line_Sname).ToList();

                /* ------------------------------- RFT Target ------------------------------- */
                data.RFT_Target = _repoAccessor.MES_Dept_Target
                    .FindAll(x => x.Year_Target == data.Data_Date.Year && x.Month_Target == data.Data_Date.Month).ToList()
                    .Join(_repoAccessor.VW_DeptFromMES.FindAll(x => x.PS_ID == Common.PS_ID_ASY).ToList(),
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(_repoAccessor.VW_LineGroup.FindAll(x => x.Factory_ID == factory).ToList(),
                        x => new { Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x => x.T3.Line_Group.Trim() == tuCode || x.T2.Building.Trim() == tuCode)
                    .GroupBy(x => x.T1.Dept_ID)
                    .Select(x => new T2CTBQualityDTO
                    {
                        RFT_Target = x.Sum(y => y.T1.Output_Target) / x.Sum(y => y.T1.Output_Target / y.T1.RFT_Target)
                    }).FirstOrDefault()?.RFT_Target ?? 0;

                /* -------------------------------- BA Target ------------------------------- */
                data.BA_Target = _repoAccessor.eTM_Dept_Score_Data
                    .FindAll(x =>
                        (x.Dept_Output_Target > 0) &&
                        (!switchDate ?
                            (x.Data_Date == data.Data_Date) :
                            (x.Data_Date >= firstDayOfMonth && x.Data_Date <= data.Data_Date))).ToList()
                    .Join(_repoAccessor.MES_Dept_Target.FindAll().ToList(),
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(_repoAccessor.VW_DeptFromMES.FindAll().ToList(),
                        x => new { Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Join(_repoAccessor.VW_LineGroup.FindAll().ToList(),
                        x => new { Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, x.T3, T4 = y })
                    .Where(x =>
                        (x.T2.Year_Target == x.T1.Data_Date.Year) &&
                        (x.T2.Month_Target == x.T1.Data_Date.Month) &&
                        (x.T4.Line_Group.Trim() == tuCode || x.T3.Building.Trim() == tuCode))
                    .GroupBy(x => x.T1.Dept_ID)
                    .Select(x => new T2CTBQualityDTO
                    {
                        BA_Target =
                            x.Sum(x => x.T2.Star_Target * x.T1.Dept_Output_Target) /
                            x.Sum(x => x.T1.Dept_Output_Target)
                    }).FirstOrDefault()?.BA_Target ?? 0;

                /* -------------------------- Check if value exists ------------------------- */
                if (data.RFT_Chart.Any() || data.BA_Chart.Any())
                {
                    hasData = true;
                }
                else
                {
                    data.Data_Date = data.Data_Date.AddDays(-1);
                    firstDayOfMonth = new DateTime(data.Data_Date.Year, data.Data_Date.Month, 1);
                }
            }

            return data;
        }

        public async Task<List<DefectTop3DTO>> GetDefectTop3Photos(string tuCode, bool switchDate)
        {
            if (string.IsNullOrEmpty(tuCode?.Trim()))
                return null;

            tuCode = tuCode.Trim();
            var dataDate = await GetMaxDate(tuCode);

            if (dataDate == null)
                return null;

            var factory = _configuration.GetSection("Appsettings:FactoryId").Value;
            var data = new DefectTop3DTO();
            data.Data_Date = dataDate.Value;
            var firstDayOfMonth = new DateTime(data.Data_Date.Year, data.Data_Date.Month, 1);

            var result = _repoAccessor.eTM_MES_Quality_Defect_Data
                .FindAll(x =>
                    (x.Data_Kind.Trim() == Common.KIND_TQCDEF) &&
                    (!switchDate ?
                        (x.Data_Date == data.Data_Date) :
                        (x.Data_Date <= data.Data_Date && x.Data_Date >= firstDayOfMonth))).ToList()
                .Join(_repoAccessor.MES_Defect.FindAll(x => x.Factory_ID.Trim() == factory).ToList(),
                    x => new { Reason_ID = x.Reason_ID.Trim() },
                    y => new { Reason_ID = y.Def_ID.Trim() },
                    (x, y) => new { T1 = x, T2 = y })
                .Join(_repoAccessor.VW_DeptFromMES.FindAll().ToList(),
                    x => new { Dept_ID = x.T1.Dept_ID.Trim() },
                    y => new { Dept_ID = y.Dept_ID.Trim() },
                    (x, y) => new { x.T1, x.T2, T3 = y })
                .Join(_repoAccessor.VW_LineGroup.FindAll().ToList(),
                    x => new { Dept_ID = x.T1.Dept_ID.Trim() },
                    y => new { Dept_ID = y.Dept_ID.Trim() },
                    (x, y) => new { x.T1, x.T2, x.T3, T4 = y })
                .Where(x =>
                    (x.T3.PS_ID == Common.PS_ID_ASY) &&
                    (x.T4.Line_Group.Trim() == tuCode || x.T3.Building.Trim() == tuCode))
                .GroupBy(x => new
                {
                    Reason_ID = x.T1.Reason_ID.Trim(),
                    Def_DescVN = x.T2.Def_DescVN.Trim()
                }).Select(x => new DefectTop3DTO
                {
                    Reason_ID = x.Key.Reason_ID?.Trim(),
                    Def_DescVN = x.Key.Def_DescVN?.Trim(),
                    Finding_Qty = x.Sum(x => x.T1.Finding_Qty),
                    Image_Path = x.Max(x => x.T1.Image_Path)
                }).OrderByDescending(x => x.Finding_Qty).ThenBy(x => x.Reason_ID).Take(3).ToList();

            return result;
        }

        private async Task<DateTime?> GetMaxDate(string tuCode)
        {
            if (string.IsNullOrEmpty(tuCode?.Trim()))
                return null;

            var maxDateQualityDefect = await _repoAccessor.eTM_MES_Quality_Defect_Data.FindAll()
                .Join(_repoAccessor.VW_DeptFromMES.FindAll(),
                    x => new { Dept_ID = x.Dept_ID.Trim() },
                    y => new { Dept_ID = y.Dept_ID.Trim() },
                    (x, y) => new { Defect = x, Dept = y })
                .Join(
                    _repoAccessor.VW_LineGroup.FindAll(),
                    x => x.Dept.Dept_ID,
                    y => y.Dept_ID,
                    (x, y) => new { Building = x.Dept.Building, LineGroup = y.Line_Group, Date = x.Defect.Data_Date }
                )
                .Where(x => x.LineGroup.Trim() == tuCode || x.Building.Trim() == tuCode)
                .ToListAsync();

            DateTime? result = maxDateQualityDefect.Count > 0 ? maxDateQualityDefect.Max(m => m.Date) : null;
            return result;
        }
    }
}
