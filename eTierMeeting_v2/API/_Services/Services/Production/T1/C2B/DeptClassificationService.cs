
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Params;
using eTierV2_API.Models;
using Microsoft.EntityFrameworkCore;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API._Repositories;
using eTierV2_API._Services.Interfaces;

namespace eTierV2_API._Services.Services.Production.T1.C2B
{
    public class DeptClassificationService : IDeptClassificationServcie
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IRepositoryAccessor _repoAccessor;

        private readonly ICommonService _commonService;

        public DeptClassificationService( IMapper mapper,
                                    MapperConfiguration configMapper,
                                    IRepositoryAccessor repoAccessor,
                                    ICommonService commonService)
        {
            _configMapper = configMapper;
            _mapper = mapper;
            _repoAccessor = repoAccessor;
            _commonService = commonService;
        }

        public async Task<bool> CheckDeptClassificationExists(string classkind, string deptid)
        {
            return await _commonService.CheckDeptClassificationExists(classkind, deptid);
        }

        public async Task<string> ChkDeptIDBeforeInsert(string deptid)
        {
            return await _repoAccessor.VW_DeptFromMES.FindAll(x => x.Line_ID.Contains(deptid)).Select(x => x.Dept_ID).FirstOrDefaultAsync();
        }

        // call by spa
        public async Task<PagedList<eTM_Dept_ClassificationDTO>> SearchDeptClassification(PaginationParam paginationParams, DeptClassificationParam deptClassificationParam)
        {
            var query = _repoAccessor.eTM_Dept_Classification.FindAll();
            // if (!String.IsNullOrEmpty(deptClassificationParam.class_Kind) && !String.IsNullOrEmpty(deptClassificationParam.dept_ID))
            // {
            query = query.Where(x => x.Class_Kind.Contains(deptClassificationParam.class_Kind) && x.Dept_ID.Contains(deptClassificationParam.dept_ID) && x.Class_Name.Contains(deptClassificationParam.class_Name));
            // }
            var list = query.ProjectTo<eTM_Dept_ClassificationDTO>(_configMapper).OrderBy(x => x.Class_Kind);
            return await PagedList<eTM_Dept_ClassificationDTO>.CreateAsync(list, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<object> GetDept()
        {
            return await _repoAccessor.VW_DeptFromMES.FindAll().Select(x => new { x.Dept_ID, x.Line_ID_2 }).Distinct().OrderBy(x => x.Dept_ID).ToListAsync();
        }

        // 2021/10/07 線別選擇頁面抓取Classificication線別值(僅限用目前T1,未來確認T2後會再修改)
        public async Task<object> GetDeptInClassification()
        {
            return await  _repoAccessor.eTM_Dept_Classification.FindAll().Select(x => new { x.Class_Name }).Distinct().ToListAsync();
            // return await ctx.eTM_Dept_Classification.Select(x => new { x.Class_Name }).Distinct().ToListAsync();
        }


        public async Task<ProductionT2SelectLineDTO> getFactoryIndex()
        {
            var data = new ProductionT2SelectLineDTO();
            data.ListC2B = await GetListItemByClassLevel("CTB", "T1");
            data.ListSTF = await GetListItemByClassLevel("STF", "T1");
            data.ListUPF = await GetListItemByClassLevel("UPF", "T1");
            return data;
        }
        private async Task<List<eTM_Team_UnitIndexOC>> GetListItemByClassLevel(string classLevel, string tierLevel)
        {
            var data = await _repoAccessor.eTM_Team_Unit
            .FindAll(
                x => x.Center_Level == "Production" &&
                x.Tier_Level == tierLevel &&
                x.Class1_Level == classLevel
            )
            .AsNoTracking().ToListAsync();
            // Cấp cha là CTB, level 1
            var Class = new eTM_Team_UnitIndexOC
            {
                Center_Level = "Production",
                Class1_Level = classLevel,
                Id = 1,
                Level = 1,
                Tier_Level = tierLevel,
                LineNum = 1
            };

            var result = new List<eTM_Team_UnitIndexOC>();
            // Add list item level 2
            int index = 1;
            foreach (var item in data)
            {
                result.Add(new eTM_Team_UnitIndexOC
                {
                    Center_Level = item.Center_Level,
                    Class1_Level = item.Class1_Level,
                    Id = index,
                    Level = 2,
                    Tier_Level = item.Tier_Level.Trim(),
                    TU_Code = item.TU_Code.Trim(),
                    TU_Name = item.TU_Name.Trim(),
                    RowCount = 1,
                    SortSeq = index,
                    TU_ID = item.TU_ID.Trim(),
                    ParentID = Class.Id
                });
                index++;
            }
            index = 1;
            int numItemInLine = 14;
            var groupDept = result.Where(x => x.Level > 1).Select(x => x.TU_Code.Substring(0, 1)).Distinct().OrderBy(x => x).ToList();
            foreach (var dept in groupDept)
            {
                var listUnit = result.Where(x => x.Level == 2 && x.TU_Code.Substring(0, 1) == dept).ToList();
                var range = Enumerable.Range(0, (listUnit.Count() % numItemInLine == 0 ? listUnit.Count() / numItemInLine : listUnit.Count() / numItemInLine +1)).ToList();
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
            Class.RowCount = result.Max(x => x.LineNum);
            result.Add(Class);
            return result.OrderBy(x => x.Level).ToList();
        }

        public async Task<bool> Add(eTM_Dept_ClassificationDTO model)
        {
            var deptclassification = _mapper.Map<eTM_Dept_Classification>(model);
            _repoAccessor.eTM_Dept_Classification.Add(deptclassification);
            return await _repoAccessor.eTM_Dept_Classification.SaveAll();
        }
        public async Task<bool> Update(eTM_Dept_ClassificationDTO model)
        {
            var deptclassification = _mapper.Map<eTM_Dept_Classification>(model);
            _repoAccessor.eTM_Dept_Classification.Update(deptclassification);
            return await _repoAccessor.eTM_Dept_Classification.SaveAll();
        }

        public async Task<bool> Delete(eTM_Dept_ClassificationDTO model)
        {
            var deptclassification = _mapper.Map<eTM_Dept_Classification>(model);
            _repoAccessor.eTM_Dept_Classification.Remove(deptclassification);
            return await _repoAccessor.eTM_Dept_Classification.SaveAll();
        }


    }
}