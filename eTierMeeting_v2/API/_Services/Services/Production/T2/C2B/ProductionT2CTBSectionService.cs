using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API.DTO;
using Microsoft.EntityFrameworkCore;
using eTierV2_API._Services.Interfaces.Production.T2.CTB;
using eTierV2_API.Helpers.Enums;
using eTierV2_API.Models;
using eTierV2_API._Repositories;

namespace eTierV2_API._Services.Services.Production.T2.C2B
{
    public class ProductionT2CTBSectionService : IProductionT2CTBSectionService
    {
        private readonly IRepositoryAccessor _repoAccessor;
        public ProductionT2CTBSectionService(
            IRepositoryAccessor repoAccessor)
        {
            _repoAccessor = repoAccessor;
        }

        public async Task<ProductionT2SelectLineDTO> getFactoryIndex()
        {
            var data = new ProductionT2SelectLineDTO();
            data.ListC2B = await GetListItemByClassLevel(Common.CLASS_LEVEL_CTB, Common.TIER_LEVEL_T2);
            return data;
        }


        private async Task<List<eTM_Team_UnitIndexOC>> GetListItemByClassLevel(string classLevel, string tierLevel)
        {
            var result = new List<eTM_Team_UnitIndexOC>();
            var index = 1;
            var activeBuildings = await _repoAccessor.eTM_Team_Unit
                .FindAll(x =>
                    x.Class1_Level == classLevel &&
                    x.Tier_Level == tierLevel &&
                    x.Class2_Level == Common.CLASS2_LEVEL_BUILDING)
                .Select(x => new eTM_Team_UnitIndexOC
                {
                    Building = x.TU_Code.Trim(),
                    IsActive = true,
                    Center_Level = x.Center_Level.Trim(),
                    Class1_Level = x.Class1_Level.Trim(),
                    Class2_Level = x.Class2_Level.Trim(),
                    TU_Code = x.TU_Code.Trim(),
                    TU_ID = x.TU_ID.Trim(),
                    TU_Name = x.TU_Name.Trim(),
                    Tier_Level = x.Tier_Level.Trim(),
                }).ToListAsync();

            var data = await _repoAccessor.eTM_Team_Unit.FindAll()
                .Join(_repoAccessor.VW_LineGroup.FindAll(),
                    x => new { TU_Code = x.TU_Code.Trim() },
                    y => new { TU_Code = y.Line_Group.Trim() },
                    (x, y) => new { T1 = x, T2 = y })
                .Where(x =>
                    x.T1.Class1_Level == classLevel &&
                    x.T1.Tier_Level == tierLevel &&
                    x.T1.Class2_Level == Common.CLASS2_LEVEL_GROUP &&
                    x.T2.Kind == "5")
                .Select(x => new eTM_Team_UnitIndexOC
                {
                    Building = x.T2.Building.Trim(),
                    IsActive = true,
                    Center_Level = x.T1.Center_Level.Trim(),
                    Class1_Level = x.T1.Class1_Level.Trim(),
                    Class2_Level = x.T1.Class2_Level.Trim(),
                    TU_Code = x.T1.TU_Code.Trim(),
                    TU_ID = x.T1.TU_ID.Trim(),
                    TU_Name = x.T1.TU_Name.Trim(),
                    Tier_Level = x.T1.Tier_Level.Trim(),
                }).Distinct().ToListAsync();

            var dataBuildings = await _repoAccessor.eTM_Team_Unit.FindAll()
                .Where(x =>
                    x.Class1_Level == classLevel &&
                    x.Tier_Level == tierLevel &&
                    x.Class2_Level == Common.CLASS2_LEVEL_BUILDING)
                .Select(x => new eTM_Team_UnitIndexOC
                {
                    Building = x.TU_Code.Trim(),
                    IsActive = true,
                    Center_Level = x.Center_Level.Trim(),
                    Class1_Level = x.Class1_Level.Trim(),
                    Class2_Level = x.Class2_Level.Trim(),
                    TU_Code = x.TU_Code.Trim(),
                    TU_ID = x.TU_ID.Trim(),
                    TU_Name = x.TU_Name.Trim(),
                    Tier_Level = x.Tier_Level.Trim(),
                }).Distinct().ToListAsync();

            var allBuildings = data.Union(dataBuildings)
                .GroupBy(x => x.Building)
                .Select(x => new eTM_Team_UnitIndexOC
                {
                    Building = x.Key,
                    TU_Code = x.Key,
                    TU_Name = x.Key,
                    IsActive = false,
                }).ToList();

            foreach (var item in activeBuildings)
            {
                var buildingIndex = allBuildings.FindIndex(x => x.Building == item.Building);
                if (buildingIndex != -1)
                    allBuildings[buildingIndex] = item;
            }

            data.AddRange(allBuildings);
            data = data.OrderBy(x => x.Building).ThenBy(x => x.TU_Code).ToList();

            // Level 1
            var parent = new eTM_Team_UnitIndexOC
            {
                Center_Level = Common.CENTER_LEVEL_PRODUCTION,
                Class1_Level = classLevel,
                Id = 1,
                Level = 1,
                LineNum = 1,
                Tier_Level = tierLevel,
                IsActive = true,
            };

            // Level 2
            foreach (var item in data)
            {
                item.Id = index;
                item.SortSeq = index;
                item.Level = 2;
                item.RowCount = 1;
                item.ParentID = parent.Id;
                index++;
            }

            result.AddRange(data);
            index = 1;
            int numItemInLine = 14;
            var groupDept = result.Where(x => x.Level > 1).Select(x => x.TU_Code.Substring(0, 1)).Distinct().OrderBy(x => x).ToList();

            foreach (var dept in groupDept)
            {
                var listUnit = result.Where(x => x.Level == 2 && x.TU_Code.Substring(0, 1) == dept).ToList();
                var range = Enumerable.Range(0, (listUnit.Count() % numItemInLine == 0 ? listUnit.Count() / numItemInLine : listUnit.Count() / numItemInLine + 1)).ToList();
                foreach (var i in range)
                {
                    var listUnitChild = listUnit.Skip(i * numItemInLine).Take(numItemInLine).ToList();
                    foreach (var unit in listUnitChild)
                    {
                        unit.LineNum = index;
                    }
                    index++;
                }
            }
            parent.RowCount = result.Max(x => x.LineNum);
            result.Add(parent);
            return result.OrderBy(x => x.Level).ToList();
        }
    }
}