using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.UPF;
using eTierV2_API.Data;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eTierV2_API._Services.Services.Production.T1.UPF
{
    public class ProductionT1UPFQualityService : IProductionT1UPFQualityService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepositoryAccessor _repoAccessor;

        public ProductionT1UPFQualityService( 
            IConfiguration configuration,
            IRepositoryAccessor repoAccessor
        )
        {
            _configuration = configuration;
            _repoAccessor = repoAccessor;
        }
        public async Task<QualityDTO> GetData(string deptId)
        {
            var factory = _configuration.GetSection("Appsettings:Factory").Value;
            var factoryId = _configuration.GetSection("Appsettings:FactoryId").Value;
            QualityDTO data = new QualityDTO();

            var queryDeptName = _repoAccessor.eTM_Team_Unit.FirstOrDefault(x => x.TU_Code == deptId);
            if (queryDeptName == null)
            {
                return null;
            }
            data.Dept_Name = queryDeptName.TU_Name;

            data.Data_Date = await GetMaxDate();
            var mESDeptTarget = await _repoAccessor.MES_Dept_Target
                .FindAll(x => x.Year_Target == data.Data_Date.Year && x.Month_Target == data.Data_Date.Month).ToListAsync(); 
            var vwDeptFromMES = await _repoAccessor.VW_DeptFromMES.FindAll(x => x.Building.Trim() == deptId && x.PS_ID.Trim() == "STI").ToListAsync();       
            var eTMDeptScoreData = await _repoAccessor.eTM_Dept_Score_Data
                .FindAll(x => x.Data_Date == data.Data_Date).ToListAsync();                  
            var rftTargets = mESDeptTarget.Join(vwDeptFromMES,
                    x => new { Dept_ID = x.Dept_ID.Trim()},
                    y => new { Dept_ID = y.Dept_ID.Trim()}, 
                    (x,y) => new { mESDeptTarget = x, vwDeptFromMES = y}
                )
                .Select(x => new {
                    x.mESDeptTarget.Dept_ID,
                    x.mESDeptTarget.Year_Target,
                    x.mESDeptTarget.Month_Target,
                    x.mESDeptTarget.RFT_Target,
                    x.mESDeptTarget.Star_Target,
                    x.mESDeptTarget.Output_Target
                }).ToList();

            var rftActuals = eTMDeptScoreData.Join(vwDeptFromMES,
                    x => new { x.Dept_ID},
                    y => new { y.Dept_ID},
                    (x,y) => new { eTMDeptScoreData = x, vwDeptFromMES = y}
                ).Select(x => new { 
                    x.eTMDeptScoreData.Dept_ID, 
                    x.eTMDeptScoreData.Output_FGIN,
                    x.eTMDeptScoreData.Defect_Qty
                }).ToList();
            
            decimal? rftTarget = 0;
            if (rftTargets.Any() && rftTargets.All(x => x.RFT_Target > 0) && rftTargets.Sum(x => x.Output_Target / x.RFT_Target) > 0)
            {
                rftTarget = Math.Round((decimal)
                    (rftTargets.Sum(x => x.Output_Target) /
                    rftTargets.Sum(x => x.Output_Target / x.RFT_Target)), 1);
            }

            decimal? rft = 0;
            if (rftActuals.Any() && (rftActuals.Sum(x => x.Output_FGIN) + rftActuals.Sum(x => x.Defect_Qty) > 0))
            {
                rft = Math.Round((decimal)
                    ((rftActuals.Sum(x => x.Output_FGIN) * 100) /
                    (rftActuals.Sum(x => x.Output_FGIN) + rftActuals.Sum(x => x.Defect_Qty))), 1);
            }
            data.RFT_Target = rftTarget;
            data.RFT = rft;
            data.ColoRft = rft >= 90 ? "green" : (90 > rft && rft >= 85) ? "yellow" : "red";
            return data;
        }

        public async Task<List<DefectTop3DTO>> GetDefectTop3(string deptId)
        {
            var factory = _configuration.GetSection("Appsettings:Factory").Value;
            var factoryId = _configuration.GetSection("Appsettings:FactoryId").Value;
            var data_Date = await GetMaxDate();
            
            var mesQualityDefectData = await _repoAccessor.eTM_MES_Quality_Defect_Data
                .FindAll(x => x.Data_Kind.Trim() == Common.KIND_TQCDEF && x.Data_Date == data_Date)
                .ToListAsync();

            var mesDefects = await _repoAccessor.MES_Defect.FindAll(x => x.Factory_ID.Trim() == factoryId).ToListAsync();

            var vwDeptFromMES = await _repoAccessor.VW_DeptFromMES
                .FindAll(x => x.Building.Trim() == deptId && x.PS_ID.Trim() == "STI").ToListAsync();

            var dataQueryTop3 = mesQualityDefectData.Join(mesDefects,
                    x => new { Reason_ID = x.Reason_ID.Trim() } , 
                    y => new { Reason_ID = y.Def_ID.Trim()}, 
                    (x,y) => new { mesQualityDefectData = x, mesDefects = y}
                ).Join(vwDeptFromMES,
                    x => new { x.mesQualityDefectData.Dept_ID },
                    y => new { y.Dept_ID},
                    (x,y) => new { x.mesQualityDefectData, x.mesDefects, vwDeptFromMES = y}
                ).GroupBy(
                    x => new { x.mesQualityDefectData.Reason_ID, x.mesDefects.Def_DescVN}
                )
                .Select(x => new DefectTop3DTO{
                    Reason_ID = x.Key.Reason_ID, 
                    Def_DescVN = x.Key.Def_DescVN,
                    Finding_Qty = x.Sum(y => y.mesQualityDefectData.Finding_Qty),
                    Image_Path = x.Max(y => y.mesQualityDefectData.Image_Path)
                }).OrderByDescending(x => x.Finding_Qty).ThenBy(x => x.Reason_ID).Take(3).ToList();
            return dataQueryTop3;
        }

        private async Task<DateTime>GetMaxDate(){
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