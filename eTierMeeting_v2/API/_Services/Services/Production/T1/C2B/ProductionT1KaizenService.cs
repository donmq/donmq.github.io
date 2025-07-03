using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Enums;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API._Services.Services.Production.T1.C2B
{
    public class ProductionT1KaizenService : IProductionT1KaizenService
    {
        private readonly IRepositoryAccessor _repoAccessor;

        public ProductionT1KaizenService(
            IRepositoryAccessor repoAccessor)
        {
            _repoAccessor = repoAccessor;
        }

        public async Task<List<eTM_VideoDTO>> GetListVideo(string deptId)
        {
            var today = DateTime.Now.Date;
            var videoQuery = _repoAccessor.eTM_Video.FindAll();
            var teamUnitQuery = _repoAccessor.eTM_Team_Unit.FindAll();
            var data = await videoQuery.Join(teamUnitQuery,
                x => x.TU_ID,
                y => y.TU_ID,
                (x, y) => new { videoQuery = x, teamUnitQuery = y })
                .Where(x => x.videoQuery.Video_Kind == Common.KIND_KAIZEN
                         && x.videoQuery.Play_Date == today
                         && x.teamUnitQuery.Center_Level == "Production"
                         && x.teamUnitQuery.Tier_Level == "T1"
                         && x.teamUnitQuery.Class1_Level == Common.CLASS_LEVEL_CTB
                         && x.teamUnitQuery.TU_Code == deptId)
                .Select(x => new eTM_VideoDTO
                {
                    Video_Kind = x.videoQuery.Video_Kind,
                    TU_ID = x.videoQuery.TU_ID,
                    Play_Date = x.videoQuery.Play_Date,
                    Seq = x.videoQuery.Seq,
                    Video_Title_ENG = x.videoQuery.Video_Title_ENG,
                    Video_Title_CHT = x.videoQuery.Video_Title_CHT,
                    VIdeo_Title_LCL = x.videoQuery.VIdeo_Title_LCL,
                    Video_Path = x.videoQuery.Video_Path,
                    Video_Icon_Path = x.videoQuery.Video_Icon_Path,
                    Video_Remark = x.videoQuery.Video_Remark,
                    Insert_By = x.videoQuery.Insert_By,
                    Insert_At = x.videoQuery.Insert_At,
                    Update_By = x.videoQuery.Update_By,
                    Update_At = x.videoQuery.Update_At
                }).ToListAsync();

            return data;
            
        }
    }
}