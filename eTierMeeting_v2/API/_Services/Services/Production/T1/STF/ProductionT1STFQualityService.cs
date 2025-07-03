using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.STF;
using eTierV2_API.Data;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eTierV2_API._Services.Services.Production.T1.STF
{
    public class ProductionT1STFQualityService : IProductionT1STFQualityService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepositoryAccessor _repoAccessor;

        public ProductionT1STFQualityService(
            IConfiguration configuration,
            IRepositoryAccessor repoAccessor)
        {
            _configuration = configuration;
            _repoAccessor = repoAccessor;
        }

        public async Task<QualityDTO> GetData(string deptId)
        {
            var factory = _configuration.GetSection("Appsettings:Factory").Value;
            var factoryId = _configuration.GetSection("Appsettings:FactoryId").Value;

            QualityDTO data = new QualityDTO();

            var queryDeptName = _repoAccessor.eTM_Dept_Classification.FirstOrDefault(x => x.Dept_ID == deptId);

            if (queryDeptName == null)
            {
                return null;
            }

            data.Dept_Name = queryDeptName.Dept_Name;
            data.Data_Date = await GetMaxDate();

            var mesDeptTargets = await _repoAccessor.MES_Dept_Target
                .FindAll(x => x.Factory_ID == factoryId && x.Year_Target == data.Data_Date.Year && x.Month_Target == data.Data_Date.Month)
                .ToListAsync();
            var mesOrgs = await _repoAccessor.MES_Org.FindAll(x => x.Building == deptId).ToListAsync();
            var etmDeptCoreData = await _repoAccessor.eTM_Dept_Score_Data.FindAll(x => x.Data_Date == data.Data_Date).ToListAsync();
            var mesDepts = await _repoAccessor.MES_Dept.FindAll(x => x.PS_ID == Common.CLASS_LEVEL_STF).ToListAsync();

            var eTMSetting = await _repoAccessor.eTM_Settings
                .FindAll(x => x.Key == "Factory_ID")
                .Select(x => x.Value).FirstOrDefaultAsync();

            var basicData = await _repoAccessor.SM_Basic_Data
                .FindAll(x => x.Basic_Class == "Factory.Yearly.Target" && x.Column_01 == eTMSetting)
                .FirstOrDefaultAsync();

            var rftActuals = etmDeptCoreData
                .Join(mesOrgs, x => x.Dept_ID, y => y.Dept_ID, (x, y) => new { T1 = x, T2 = y })
                .Join(mesDepts, x => x.T2.Dept_ID.Trim(), y => y.Dept_ID.Trim(), (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = y })
                .Select(x => new
                {
                    x.T1.Dept_ID,
                    x.T1.Output_FGIN,
                    x.T1.Defect_Qty
                }).ToList();

            decimal? rft = 0;
            if (rftActuals.Any() && (rftActuals.Sum(x => x.Output_FGIN) + rftActuals.Sum(x => x.Defect_Qty) > 0))
            {
                rft = Math.Round((decimal)
                    ((rftActuals.Sum(x => x.Output_FGIN) * 100) /
                    (rftActuals.Sum(x => x.Output_FGIN) + rftActuals.Sum(x => x.Defect_Qty))), 1);
            }

            data.RFT_Target = basicData != null && !string.IsNullOrEmpty(basicData.Column_04)
                ? (decimal.TryParse(basicData.Column_04, out var column04Value) ? Math.Round(column04Value, 1) : 0) : 0; ;
            data.RFT = rft;
            data.ColoRft = rft >= 90 ? "green" : (90 > rft && rft >= 85) ? "yellow" : "red";

            return data;
        }

        public async Task<List<DefectTop3DTO>> GetDefectTop3(string deptId)
        {
            var factory = _configuration.GetSection("Appsettings:FactoryId").Value;
            var data_Date = await GetMaxDate();
            var mesOrgs = await _repoAccessor.MES_Org.FindAll().ToListAsync();
            var mesDepts = await _repoAccessor.MES_Dept.FindAll().ToListAsync();

            var mesQualityDefectData = await _repoAccessor.eTM_MES_Quality_Defect_Data
                .FindAll(x => x.Data_Kind.Trim() == Common.KIND_TQCDEF && x.Data_Date == data_Date)
                .ToListAsync();

            var mesDefects = await _repoAccessor.MES_Defect.FindAll()
                .Select(x => new
                {
                    Def_ID = x.Def_ID.Trim(),
                    Def_DescVN = x.Def_DescVN
                }).ToListAsync();

            var mesIPQCDefects = await _repoAccessor.MES_IPQC_Defect.FindAll()
                .Select(x => new
                {
                    Def_ID = x.Def_Id.ToString(),
                    Def_DescVN = x.Def_Name_Location,
                }).ToListAsync();

            var mesUnionDefects = mesDefects.Union(mesIPQCDefects).ToList();

            var dataQueryTop3 = mesQualityDefectData
                .Join(mesUnionDefects, x => x.Reason_ID.Trim(), y => y.Def_ID, (x, y) => new { T1 = x, T2 = y })
                .Join(mesOrgs, x => x.T1.Dept_ID.Trim(), y => y.Dept_ID, (x, y) => new { x.T1, x.T2, T3 = y })
                .Join(mesDepts, x => x.T3.Dept_ID.Trim(), y => y.Dept_ID.Trim(), (x, y) => new { T1 = x.T1, T2 = x.T2, x.T3, T4 = y })
                .Where(x => x.T3.Building.Trim() == deptId && x.T4.PS_ID.Trim() == Common.CLASS_LEVEL_STF)
                .GroupBy(x => new { x.T1.Reason_ID, x.T2.Def_DescVN })
                .Select(x => new DefectTop3DTO
                {
                    Reason_ID = x.Key.Reason_ID,
                    Def_DescVN = x.Key.Def_DescVN,
                    Finding_Qty = x.Sum(y => y.T1.Finding_Qty),
                    Image_Path = x.Max(y => y.T1.Image_Path)
                }).OrderByDescending(x => x.Finding_Qty).Take(3).ToList();

            return dataQueryTop3;
        }

        private async Task<DateTime> GetMaxDate()
        {
            DateTime? maxDateDeptScore = _repoAccessor.eTM_Dept_Score_Data.FindAll().Any() ? _repoAccessor.eTM_Dept_Score_Data.FindAll().Max(x => x.Data_Date) : null;
            DateTime? maxDateQualityDefect = _repoAccessor.eTM_MES_Quality_Defect_Data.FindAll().Any() ? _repoAccessor.eTM_MES_Quality_Defect_Data.FindAll().Max(x => x.Data_Date) : null;
            DateTime? maxDateMORecord = _repoAccessor.eTM_MES_MO_Record.FindAll().Any() ? _repoAccessor.eTM_MES_MO_Record.FindAll().Max(x => x.Data_Date) : null;

            List<DateTime> listDate = new List<DateTime>();
            if (maxDateDeptScore.HasValue)
                listDate.Add(maxDateDeptScore.Value);
            if (maxDateQualityDefect.HasValue)
                listDate.Add(maxDateQualityDefect.Value);
            if (maxDateMORecord.HasValue)
                listDate.Add(maxDateMORecord.Value);
            return await Task.FromResult(listDate.Max(x => x));
        }
    }
}