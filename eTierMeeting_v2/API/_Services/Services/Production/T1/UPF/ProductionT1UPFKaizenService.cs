using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.UPF;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Enums;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API._Services.Services.Production.T1.UPF
{
    public class ProductionT1UPFKaizenService : IProductionT1UPFKaizenService
    {
        private readonly IRepositoryAccessor _repoAccessor;
        public ProductionT1UPFKaizenService(IRepositoryAccessor repoAccessor)
        {
            _repoAccessor = repoAccessor;
        }

        public async Task<List<eTM_VideoDTO>> GetListVideo(string deptId)
        {
            var today = DateTime.Now.Date;
            var videoQuery = _repoAccessor.eTM_Video.FindAll().AsNoTracking();
            var teamUnitQuery = _repoAccessor.eTM_Team_Unit.FindAll().AsNoTracking();

            var data = await videoQuery.Join(teamUnitQuery,
                                       x => x.TU_ID,
                                       y => y.TU_ID,
                                       (x, y) => new { video = x, teamUnit = y })
                                .Where(x => x.video.Video_Kind == Common.KIND_KAIZEN &&
                                            x.video.Play_Date == today &&
                                            x.teamUnit.Center_Level == "Production" &&
                                            x.teamUnit.Tier_Level == "T1" &&
                                            x.teamUnit.Class1_Level == Common.CLASS_LEVEL_UPF &&
                                            x.teamUnit.TU_Code == deptId)
                                .Select(x => new eTM_VideoDTO
                                {
                                    Video_Kind = x.video.Video_Kind,
                                    TU_ID = x.video.TU_ID,
                                    Play_Date = x.video.Play_Date,
                                    Seq = x.video.Seq,
                                    Video_Title_ENG = x.video.Video_Title_ENG,
                                    Video_Title_CHT = x.video.Video_Title_CHT,
                                    VIdeo_Title_LCL = x.video.VIdeo_Title_LCL,
                                    Video_Path = x.video.Video_Path,
                                    Video_Icon_Path = x.video.Video_Icon_Path,
                                    Video_Remark = x.video.Video_Remark,
                                    Insert_By = x.video.Insert_By,
                                    Insert_At = x.video.Insert_At,
                                    Update_By = x.video.Update_By,
                                    Update_At = x.video.Update_At
                                }).ToListAsync();

            return data;

        }
    }
}