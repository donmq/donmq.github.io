using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T2.CTB;
using eTierV2_API.DTO.Production.T2.CTB;
using eTierV2_API.Helpers.Enums;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API._Services.Services.Production.T2.CTB
{
    public class ProductionT2SafetyService : IProductionT2SafetyService
    {
        private readonly MapperConfiguration _mapperConfig;
        private readonly IRepositoryAccessor _repoAccessor;


        public ProductionT2SafetyService(
            MapperConfiguration mapperConfig,
            IRepositoryAccessor repoAccessor)
        {
            _mapperConfig = mapperConfig;
            _repoAccessor = repoAccessor;
        }

        private int GetLineNameOrder(string lineSname)
        {
            string numStr = Regex.Replace(lineSname, @"[^0-9]", "");
            if (numStr.Length > 0)
                return Convert.ToInt32(numStr);
            else
                return 999;
        }

        public async Task<SefetyViewModel> GetData(string building)
        {
            if (string.IsNullOrEmpty(building?.Trim()))
                return null;

            building = building.Trim();
            var data = new SefetyViewModel();

            // Query Script 1
            var script1Query = _repoAccessor.eTM_HSE_Score_Data
                .FindAll(x =>
                    x.Center_Level.Trim() == Common.CENTER_LEVEL_PRODUCTION &&
                    x.Tier_Level.Trim() == Common.TIER_LEVEL_T2 &&
                    x.Class_Level.Trim() == Common.CLASS_LEVEL_CTB)
                .Join(_repoAccessor.VW_DeptFromMES.FindAll(),
                    x => new { Dept_ID = x.TU_Code.Trim() },
                    y => new { Dept_ID = y.Dept_ID.Trim() },
                    (x, y) => new { T1 = x, T2 = y })
                .Join(_repoAccessor.VW_LineGroup.FindAll(),
                    x => new { Dept_ID = x.T2.Dept_ID.Trim() },
                    y => new { Dept_ID = y.Dept_ID.Trim() },
                    (x, y) => new { x.T1, x.T2, T3 = y })
                .Where(x => x.T2.PS_ID == "ASY" && (x.T2.Building.Trim() == building || x.T3.Line_Group.Trim() == building));

            // Get max Month, Max Year
            var lastDateOfThePerviousMonth = DateTime.Now.AddDays(-DateTime.Now.Day).Date;
            data.Month = lastDateOfThePerviousMonth.Month;
            data.Year = lastDateOfThePerviousMonth.Year;
            var VW_DeptFromMES = _repoAccessor.VW_DeptFromMES.FindAll().ToList();
            var MES_Dept_Plan = _repoAccessor.MES_Dept_Plan.FindAll(x => x.Plan_Date.Year == data.Year && x.Plan_Date.Month == data.Month).ToList();
            var VW_LineGroup =_repoAccessor.VW_LineGroup.FindAll().ToList();
            // Get list Line of VWDeptFromMES - MES_Dept_Plan
            data.Lines = VW_DeptFromMES
                .Join(MES_Dept_Plan,
                    x => new { Dept_ID = x.Dept_ID.Trim() },
                    y => new { Dept_ID = y.Dept_ID.Trim() },
                    (x, y) => new { T1 = x, T2 = y })
                .Join(VW_LineGroup,
                    x => new { Dept_ID = x.T2.Dept_ID.Trim() },
                    y => new { Dept_ID = y.Dept_ID.Trim() },
                    (x, y) => new { x.T1, x.T2, T3 = y })
                .Where(x => x.T1.PS_ID == "ASY" && (x.T1.Building.Trim() == building || x.T3.Line_Group.Trim() == building))
                .GroupBy(x => x.T1.Line_Sname).Where(x => x.Sum(y => y.T2.Working_Hour) > 0)
                .Select(x => x.Key).OrderBy(m => GetLineNameOrder(m)).ThenBy(m => m)
                .ToList();

            // Get list Evaluetion Category Safety
            data.Evaluetions = await _repoAccessor.eTM_Page_Item_Settings
                .FindAll(
                    x => x.Center_Level.Trim() == Common.CENTER_LEVEL_PRODUCTION &&
                    x.Tier_Level.Trim() == Common.TIER_LEVEL_T2 &&
                    x.Class_Level.Trim() == Common.CLASS_LEVEL_CTB &&
                    x.Page_Name.Trim() == Common.PAGE_NAME_SAFETY &&
                    x.Is_Active)
                .Select(x => new EvaluetionCategory
                {
                    Item_ID = x.Item_ID,
                    Item_Name = x.Item_Name,
                    Item_Name_LL = x.Item_Name_LL,
                    Target = x.Target.HasValue ? x.Target.Value.ToString("0.##") : null
                }).ToListAsync();

            // Get list Achievement by Item_ID of Evaluetion Category
            foreach (var item in data.Evaluetions)
            {
                var achievements = data.Lines
                    .GroupJoin(
                        script1Query.Where(x =>
                            x.T1.Year == data.Year &&
                            x.T1.Month == data.Month &&
                            x.T1.Item_ID == item.Item_ID
                        ).ToList(),
                        x => x,
                        y => y.T2.Line_Sname,
                        (x, y) => new { Line_ID = x, T = y }
                    )
                    .SelectMany(
                        x => x.T.DefaultIfEmpty(),
                        (x, y) => new { Line_ID = x.Line_ID, T = y }
                    )
                    .Select(x => new Achievement
                    {
                        HSE_Score_ID = x.T?.T1.HSE_Score_ID??0,
                        Item_ID = item.Item_ID,
                        Line_Sname = x.Line_ID,
                        Score = x.T?.T1.Score.ToString("0.##")??"--",
                        IsPass = (x.T is null)? true : x.T.T1.Score >= Convert.ToDecimal(item.Target)
                    })
                    .ToList()
                    .OrderBy(m => GetLineNameOrder(m.Line_Sname))
                    .ToList();

                int count = data.Lines.Count() - achievements.Count();
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        achievements.Add(new Achievement
                        {
                            Score = "--",
                            IsPass = true
                        });
                    }
                }
                item.Achievements = achievements;
            }

            return data;
        }

        public async Task<List<eTM_HSE_Score_ImageDTO>> GetDetailScoreUnPass(int hseScoreID)
        {
            return await _repoAccessor.eTM_HSE_Score_Image.FindAll(x => x.HSE_Score_ID == hseScoreID).ProjectTo<eTM_HSE_Score_ImageDTO>(_mapperConfig).AsNoTracking().ToListAsync();
        }
    }
}