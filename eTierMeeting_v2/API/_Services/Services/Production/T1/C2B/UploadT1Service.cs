using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTierV2_API._Repositories;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.DTO.UploadT1;
using eTierV2_API.Helpers.Params;
using eTierV2_API.Helpers.Utilities;
using eTierV2_API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API._Services.Services.Production.T1.C2B
{
    public class UploadT1Service : IUploadT1Service
    {
         private readonly IRepositoryAccessor _repoAccessor;
        public UploadT1Service(IRepositoryAccessor repoAccessor)
        {
            _repoAccessor = repoAccessor;
        }

        public async Task<bool> DeleteAllBySearch(List<eTM_Video> data)
        {
            _repoAccessor.eTM_Video.RemoveMultiple(data);
            return await _repoAccessor.eTM_Video.SaveAll();
        }

        public async Task<bool> DeleteProductVideoT1(TMVideoDto model)
        {
            var record = await _repoAccessor.eTM_Video.FindAll(x => x.Video_Kind == model.Video_Kind &&
                                                x.TU_ID == model.Unit &&
                                                x.Play_Date == model.Play_Date &&
                                                x.Seq == model.Seq).FirstOrDefaultAsync();
            _repoAccessor.eTM_Video.Remove(record);
            return await _repoAccessor.eTM_Video.SaveAll();
        }

        public async Task<List<string>> GetCenters()
        {
            return await _repoAccessor.eTM_Team_Unit.FindAll().Select(x => x.Center_Level.Trim()).Distinct().ToListAsync();
        }

        public async Task<List<string>> GetListVideoKind()
        {
            var data = await _repoAccessor.eTM_Production_T1_Video.FindAll().Select(x => x.Video_Kind.Trim()).Distinct().ToListAsync();
            return data;
        }

        public async Task<List<string>> GetSections()
        {
            return await _repoAccessor.eTM_Team_Unit.FindAll().Select(x => x.Class1_Level.Trim()).Distinct().ToListAsync();
        }

        public async Task<List<string>> GetTiers()
        {
            return await _repoAccessor.eTM_Team_Unit.FindAll().Select(x => x.Tier_Level.Trim()).Distinct().ToListAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> GetUnits()
        {
            return await _repoAccessor.eTM_Team_Unit.FindAll().Select(x => new KeyValuePair<string, string>(x.TU_ID.Trim(), x.TU_Name.Trim())).Distinct().ToListAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> GetUnits(string center, string tier, string section)
        {
            var pred_eTM_Team_Unit = PredicateBuilder.New<eTM_Team_Unit>(true);
            if (center != "all" && !string.IsNullOrEmpty(center))
            {
                pred_eTM_Team_Unit.And(x => x.Center_Level.Trim() == center.Trim());
            }
            if (tier != "all" && !string.IsNullOrEmpty(tier))
            {
                pred_eTM_Team_Unit.And(x => x.Tier_Level.Trim() == tier.Trim());
            }
            if (section != "all" && !string.IsNullOrEmpty(section))
            {
                pred_eTM_Team_Unit.And(x => x.Class1_Level.Trim() == section.Trim());
            }

            var data = await _repoAccessor.eTM_Team_Unit.FindAll(pred_eTM_Team_Unit).Select(x => new KeyValuePair<string, string>(x.TU_ID.Trim(), x.TU_Name.Trim())).ToListAsync();
            return data;
        }

        public async Task<PagedList<TMVideoDto>> Search(eTierV2_API.Helpers.Params.PaginationParam param, ProductVideoT1Param filterParam)
        {
            var pred_eTMVideo = PredicateBuilder.New<eTM_Video>(true);
            var pred_eTM_Team_Unit = PredicateBuilder.New<eTM_Team_Unit>(true);

            if (filterParam.VideoKind != "all" && !string.IsNullOrEmpty(filterParam.VideoKind))
            {
                pred_eTMVideo.And(x => x.Video_Kind.Trim() == filterParam.VideoKind.Trim());
            }
            if (filterParam.Center != "all" && !string.IsNullOrEmpty(filterParam.Center))
            {
                pred_eTM_Team_Unit.And(x => x.Center_Level.Trim() == filterParam.Center.Trim());
            }
            if (filterParam.Tier != "all" && !string.IsNullOrEmpty(filterParam.Tier))
            {
                pred_eTM_Team_Unit.And(x => x.Tier_Level.Trim() == filterParam.Tier.Trim());
            }
            if (filterParam.Section != "all" && !string.IsNullOrEmpty(filterParam.Section))
            {
                pred_eTM_Team_Unit.And(x => x.Class1_Level.Trim() == filterParam.Section.Trim());
            }
            if (filterParam.Unit != "all" && !string.IsNullOrEmpty(filterParam.Unit))
            {
                pred_eTM_Team_Unit.And(x => x.TU_ID.Trim() == filterParam.Unit.Trim());
            }

            if (!string.IsNullOrEmpty(filterParam.To_Date) && !string.IsNullOrEmpty(filterParam.From_Date))
            {
                var fromDate = Convert.ToDateTime(filterParam.From_Date + " 00:00:00.000");
                var toDate = Convert.ToDateTime(filterParam.To_Date + " 23:59:59.997");
                pred_eTMVideo.And(x => x.Play_Date >= fromDate && x.Play_Date <= toDate);
            }

            var tmVideos = _repoAccessor.eTM_Video.FindAll(pred_eTMVideo);
            var tmTeamUnit = _repoAccessor.eTM_Team_Unit.FindAll(pred_eTM_Team_Unit);
            var data = tmVideos.Join(
                tmTeamUnit, x => x.TU_ID.Trim(), y => y.TU_ID.Trim(),
                (x, y) => new TMVideoDto()
                {
                    Video_Kind = x.Video_Kind,
                    Play_Date = x.Play_Date,
                    Seq = x.Seq,
                    Video_Title_ENG = x.Video_Title_ENG,
                    Video_Title_CHT = x.Video_Title_CHT,
                    VIdeo_Title_LCL = x.VIdeo_Title_LCL,
                    Video_Icon_Path = x.Video_Icon_Path,
                    Video_Remark = x.Video_Remark,
                    Center = y.Center_Level,
                    Tier = y.Tier_Level,
                    Section = y.Class1_Level,
                    Unit = y.TU_ID,
                    Unit_Name = y.TU_Name,
                    Insert_At = x.Insert_At
                }).OrderByDescending(x => x.Insert_At);
            return await PagedList<TMVideoDto>.CreateAsync(data, param.PageNumber, param.PageSize, true);
        }

        public async Task<PagedList<eTM_Video>> SearchOfBatch(eTierV2_API.Helpers.Params.PaginationParam param, BatchDeleteParam filterParam, string user)
        {
            var pred_eTM_Video = PredicateBuilder.New<eTM_Video>(true);
            var pred_eTM_Team_Unit = PredicateBuilder.New<eTM_Team_Unit>(true);
            pred_eTM_Video.And(x => x.Insert_By.Trim() == user.Trim());
            if (!string.IsNullOrEmpty(filterParam.VideoKind)) {
                pred_eTM_Video.And(x => x.Video_Kind.Trim() == filterParam.VideoKind.Trim());
            }
            if (!string.IsNullOrEmpty(filterParam.Center)) {
                pred_eTM_Team_Unit.And(x => x.Center_Level.Trim() == filterParam.Center.Trim());
            }
            if (!string.IsNullOrEmpty(filterParam.Tier)) {
                pred_eTM_Team_Unit.And(x => x.Tier_Level.Trim() == filterParam.Tier.Trim());
            }
            if (!string.IsNullOrEmpty(filterParam.Section)) {
                pred_eTM_Team_Unit.And(x => x.Class1_Level.Trim() == filterParam.Section.Trim());
            }
            if (filterParam.Units.Any()) {
                pred_eTM_Team_Unit.And(x => filterParam.Units.Contains(x.TU_ID));
            }       

            if (!string.IsNullOrEmpty(filterParam.To_Date) && !string.IsNullOrEmpty(filterParam.From_Date)) {
                var fromDate = Convert.ToDateTime(filterParam.From_Date + " 00:00:00.000");
                var toDate = Convert.ToDateTime(filterParam.To_Date + " 23:59:59.997");
                pred_eTM_Video.And(x => x.Play_Date >= fromDate && x.Play_Date <= toDate);
            }

            var tmVideos = _repoAccessor.eTM_Video.FindAll(pred_eTM_Video);
            var tmTeamUnit = _repoAccessor.eTM_Team_Unit.FindAll(pred_eTM_Team_Unit);
            var data = tmVideos.Join(
                tmTeamUnit, x => x.TU_ID.Trim(), y => y.TU_ID.Trim(),
                (x, y) => x).OrderByDescending(x => x.Insert_At);
            return await PagedList<eTM_Video>.CreateAsync(data, param.PageNumber, param.PageSize, false);
        }

        public async Task<bool> UploadVideo(UploadVideoT1Dto data, string insertBy)
        {
            var dataUploads = new List<eTM_Video>();

            var fn = new FunctionUtility();
            var videoFileName = await fn.UploadAsync(data.video, "Production/T1/VideoUpload", Guid.NewGuid().ToString().Substring(17, 19));
                var iconFileName = await fn.UploadAsync(data.video_Icon, "Production/T1/IconUpload", Guid.NewGuid().ToString().Substring(17, 19));
                var video_Path = "Production/T1/VideoUpload/" + videoFileName;
                var video_Icon_Path = "Production/T1/IconUpload/" + iconFileName;
            var fromDate = Convert.ToDateTime(data.From_Date + " 00:00:00.000");
            var toDate = Convert.ToDateTime(data.To_Date + " 23:59:59.997");
            foreach (var unitItem in data.UnitIds)
            {
                for (DateTime dayCurrent = fromDate; dayCurrent < toDate; dayCurrent = dayCurrent.AddDays(+1))
                {
                    var em_VideoItem = new eTM_Video();
                    em_VideoItem.TU_ID = unitItem;
                    em_VideoItem.Video_Kind = data.Video_Kind;

                    var productCheckMoSeqDB = _repoAccessor.eTM_Video.FindAll(x => x.Video_Kind.Trim() == data.Video_Kind.Trim() &&
                                            x.TU_ID.Trim() == unitItem.Trim() &&
                                            x.Play_Date == dayCurrent);
                    if (productCheckMoSeqDB.Any())
                    {
                        var seq = Convert.ToInt32(productCheckMoSeqDB.Max(x => x.Seq)) + 1;
                        em_VideoItem.Seq = Convert.ToInt16(seq);
                    }
                    else
                    {
                        em_VideoItem.Seq = Convert.ToInt16(1);
                    }

                    em_VideoItem.Play_Date = DateTime.Parse(dayCurrent.ToString("yyyy-MM-dd"));
                    em_VideoItem.Video_Title_ENG = data.Video_Title_ENG;
                    em_VideoItem.VIdeo_Title_LCL = data.VIdeo_Title_LCL;
                    em_VideoItem.Video_Title_CHT = data.Video_Title_CHT;
                    em_VideoItem.Video_Remark = data.Video_Remark;

                    em_VideoItem.Video_Path = video_Path;
                    em_VideoItem.Video_Icon_Path = video_Icon_Path;

                    em_VideoItem.Insert_By = insertBy;
                    em_VideoItem.Insert_At = DateTime.Now;
                    em_VideoItem.Update_At = DateTime.Now;
                    em_VideoItem.Update_By = insertBy;

                    dataUploads.Add(em_VideoItem);
                }
            }
            _repoAccessor.eTM_Video.AddMultiple(dataUploads);
            return await _repoAccessor.eTM_Production_T1_Video.SaveAll();
        }
    }
}