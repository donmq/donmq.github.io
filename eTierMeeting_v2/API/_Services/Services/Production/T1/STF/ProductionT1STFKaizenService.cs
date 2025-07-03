using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API._Services.Interfaces.Production.T1.STF;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Enums;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API._Services.Services.Production.T1.STF
{
    public class ProductionT1STFKaizenService : IProductionT1STFKaizenService
    {
        private readonly IRepositoryAccessor _repoAccessor;

        public ProductionT1STFKaizenService(
            IRepositoryAccessor repoAccessor)
        {
            _repoAccessor = repoAccessor;
        }

        public async Task<List<eTM_VideoDTO>> GetListVideo(string deptId, string date)
        {
            var today = DateTime.Now.Date;
            var eTMVideoQuery = _repoAccessor.eTM_Video.FindAll();
            var eTMTeamUnitQuery = _repoAccessor.eTM_Team_Unit.FindAll();
            var data = await eTMVideoQuery.Join(eTMTeamUnitQuery,
                        x => x.TU_ID,
                        y => y.TU_ID,
                        (x, y) => new { T1 = x, T2 = y })
                        .Where(x => x.T1.Video_Kind == Common.KIND_KAIZEN &&
                                  x.T1.Play_Date == Convert.ToDateTime(date) &&
                                  x.T2.TU_Code == deptId &&
                                  x.T2.Tier_Level == "T1" &&
                                  x.T2.Center_Level == "Production" &&
                                  x.T2.Class1_Level == "STF")
                        .Select(x => new eTM_VideoDTO
                        {
                            Insert_At = x.T1.Insert_At,
                            Insert_By = x.T1.Insert_By,
                            Play_Date = x.T1.Play_Date,
                            Seq = x.T1.Seq,
                            TU_ID = x.T1.TU_ID,
                            Video_Icon_Path = x.T1.Video_Icon_Path,
                            Video_Kind = x.T1.Video_Kind,
                            Video_Path = x.T1.Video_Path,
                            Video_Remark = x.T1.Video_Remark,
                            Video_Title_CHT = x.T1.Video_Title_CHT,
                            Video_Title_ENG = x.T1.Video_Title_ENG,
                            Update_At = x.T1.Update_At,
                            Update_By = x.T1.Update_By,
                            VIdeo_Title_LCL = x.T1.VIdeo_Title_LCL,
                        }).ToListAsync();
            return data;
        }
    }
}