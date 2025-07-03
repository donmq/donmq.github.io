using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTierV2_API._Repositories;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eTierV2_API._Services.Services.Production.T1.C2B
{
    public class ProductionT1QualityService : IProductionT1QualityService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepositoryAccessor _repoAccessor;

        public ProductionT1QualityService(
            IRepositoryAccessor repoAccessor,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _repoAccessor = repoAccessor;
        }

        public async Task<List<FRI_BA_DefectDTO>> GetBADefectTop3Chart(string deptId)
        {
            var factory = _configuration.GetSection("Appsettings:FactoryId").Value;
            FRI_BA_DefectDTO data = new FRI_BA_DefectDTO();

            data.Data_Date = await GetMaxDate();
            
            var queryMesQualityData = await _repoAccessor.eTM_MES_Quality_Defect_Data
                .FindAll(
                    x => x.Dept_ID == deptId &&
                    x.Data_Kind == "BADEF" &&
                    x.Data_Date == data.Data_Date)
                .Select(x => new
                {
                    x.Finding_Qty,
                    x.Reason_ID,
                    x.Data_Kind
                }).ToListAsync();

            var queryFRIData = await _repoAccessor.FRI_BA_Defect.FindAll(x => x.Factory_ID == factory).ToListAsync();

            var bADefectTop3ChartData = queryMesQualityData
                .Join(queryFRIData, x => x.Reason_ID.Trim(), y => y.BA_Defect_ID, (x, y) => new FRI_BA_DefectDTO
                {
                    BA_Defect_Desc = y.BA_Defect_Desc?.Trim(),
                    Factory_ID = y.Factory_ID?.Trim(),
                    Finding_Qty = x.Finding_Qty,
                    Reason_ID = x.Reason_ID?.Trim(),
                    Update_Time = y.Update_Time,
                    Updated_By = y.Updated_By,
                    Data_Date = data.Data_Date,
                })
                .OrderByDescending(x => x.Finding_Qty)
                .ThenBy(x => x.Reason_ID)
                .Take(3)
                .ToList();

            return bADefectTop3ChartData;
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

            var rptActual = await _repoAccessor.eTM_Dept_Score_Data
                .FindAll(
                    x => x.Data_Date == data.Data_Date &&
                    x.Dept_ID == deptId)
                .Select(x => x.RFT).FirstOrDefaultAsync();

            var baScore = await _repoAccessor.eTM_Dept_Score_Data
                .FindAll(
                    x => x.Data_Date == data.Data_Date &&
                    x.Dept_ID == deptId)
                .Select(x => x.BA).FirstOrDefaultAsync();

            var eTMSetting = await _repoAccessor.eTM_Settings
                .FindAll(x => x.Key == "Factory_ID")
                .Select(x => x.Value).FirstOrDefaultAsync();

            var basicData = await _repoAccessor.SM_Basic_Data
                .FindAll(x => x.Basic_Class == "Factory.Yearly.Target" && x.Column_01 == eTMSetting)
                .ToListAsync();

            data.RFT_Target = basicData
                .Select(x => x.Column_04)
                .Where(value => !string.IsNullOrEmpty(value) && decimal.TryParse(value, out _))
                .Select(decimal.Parse).FirstOrDefault();

            data.Star_Target = basicData
                .Select(x => x.Column_05)
                .Where(value => !string.IsNullOrEmpty(value) && decimal.TryParse(value, out _))
                .Select(decimal.Parse).FirstOrDefault();
            data.RFT = rptActual;
            data.BA = baScore;
            data.ColoRft = rptActual >= 90 ? "green" : (90 > rptActual && rptActual >= 85) ? "yellow" : "red";
            data.ColoBa = baScore >= (decimal)3.8 ? "green" : ((decimal)3.8 > baScore && baScore >= (decimal)3.5) ? "yellow" : "red";

            return data;
        }

        public async Task<List<DefectTop3DTO>> GetDefectTop3(string deptId)
        {
            var factory = _configuration.GetSection("Appsettings:FactoryId").Value;
            DefectTop3DTO data = new DefectTop3DTO();

            data.Data_Date = await GetMaxDate();

            var queryMesQuality = await _repoAccessor.eTM_MES_Quality_Defect_Data
                .FindAll(
                    x => x.Dept_ID == deptId &&
                    x.Data_Kind == "TQCDEF" &&
                    x.Data_Date == data.Data_Date)
                .Select(x => new
                {
                    x.Finding_Qty,
                    x.Image_Path,
                    x.Data_Kind,
                    x.Reason_ID
                }).ToListAsync();

            var queryMesDefect = await _repoAccessor.MES_Defect.FindAll(x => x.Factory_ID == factory).ToListAsync();

            var dataQueryTop3 = queryMesQuality
                    .Join(queryMesDefect, x => x.Reason_ID?.Trim(), y => y.Def_ID, (x, y) => new DefectTop3DTO
                    {
                        Def_DescVN = y.Def_DescVN?.Trim(),
                        Finding_Qty = x.Finding_Qty,
                        Image_Path = x.Image_Path,
                        Reason_ID = x.Reason_ID?.Trim(),
                    })
                    .OrderByDescending(x => x.Finding_Qty)
                    .ThenBy(x => x.Reason_ID)
                    .Take(3)
                    .ToList();

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