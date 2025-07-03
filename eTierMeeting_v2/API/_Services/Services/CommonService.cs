using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using eTierV2_API._Repositories;
using eTierV2_API._Services.Interfaces;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Utilities;
using eTierV2_API.Models;

namespace eTierV2_API._Services.Services
{
    public class CommonService : ICommonService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryAccessor _repoAccessor;

        public CommonService(
            IMapper mapper,
            IRepositoryAccessor repoAccessor
        )
        {
            _mapper = mapper;
            _repoAccessor = repoAccessor;
        }
        public async Task<bool> CheckDeptClassificationExists(string classkind, string deptid)
        {
            // 重複主Key
            if (await _repoAccessor.eTM_Dept_Classification.AnyAsync(x => x.Class_Kind.Contains(classkind) && x.Dept_ID.Contains(deptid)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<OperationResult> AddVideoLog(eTM_Video_Play_LogDTO videoLogDto)
        {
            if (!string.IsNullOrWhiteSpace(videoLogDto.Center_Level) && !string.IsNullOrWhiteSpace(videoLogDto.Class_Level) &&
               !string.IsNullOrWhiteSpace(videoLogDto.Tier_Level) && !string.IsNullOrWhiteSpace(videoLogDto.deptId))
            {
                var data = _mapper.Map<eTM_Video_Play_Log>(videoLogDto);
                data.Play_Date_Time = DateTime.Now;
                data.Record_ID = Guid.NewGuid();
                data.TU_ID = _repoAccessor.eTM_Team_Unit.FirstOrDefault(x => x.Center_Level == videoLogDto.Center_Level &&
                                                                        x.TU_Code.Trim() == videoLogDto.deptId.Trim() &&
                                                                        x.Class1_Level == videoLogDto.Class_Level &&
                                                                        x.Tier_Level == videoLogDto.Tier_Level)?.TU_ID.Trim();
                _repoAccessor.eTM_Video_Play_Log.Add(data);
                if (await _repoAccessor.eTM_Video_Play_Log.SaveAll())
                {
                    return new OperationResult(true);
                }
                return new OperationResult(false, "Failed to save");
            }
            return new OperationResult(false, "DTO is either null or empty.");
        }

        public async Task<OperationResult> GetLineID(string deptID)
        {
            if (string.IsNullOrWhiteSpace(deptID))
                return new OperationResult(false, "Invalid depID");
            var data = await _repoAccessor.VW_DeptFromMES.FirstOrDefaultAsync(x => x.Dept_ID.Trim() == deptID.Trim());
            if (data == null)
                return new OperationResult(false, "Dept not found");
            return new OperationResult { Success = true, Data = new KeyValuePair<string, string>("LineID", data.Line_ID) };
        }

        public async Task<Guid> AddMeetingLogPage(eTM_Meeting_Log_PageDTO meetingLogPageDto)
        {
            if (string.IsNullOrWhiteSpace(meetingLogPageDto.Center_Level) ||
            string.IsNullOrWhiteSpace(meetingLogPageDto.Class_Level) ||
            string.IsNullOrWhiteSpace(meetingLogPageDto.Tier_Level) ||
            string.IsNullOrWhiteSpace(meetingLogPageDto.Page_Name))
                return Guid.Empty;
            var data = _repoAccessor.eTM_Team_Unit.FirstOrDefault(x =>
                x.Center_Level == meetingLogPageDto.Center_Level &&
                x.TU_Code.Trim() == meetingLogPageDto.deptId.Trim() &&
                x.Class1_Level == meetingLogPageDto.Class_Level &&
                x.Tier_Level == meetingLogPageDto.Tier_Level);
            if (data == null)
                return Guid.Empty;
            eTM_Meeting_Log_Page dataMapped = _mapper.Map<eTM_Meeting_Log_Page>(meetingLogPageDto);
            dataMapped.Record_ID = Guid.NewGuid();
            dataMapped.TU_ID = data.TU_ID.Trim();
            if (meetingLogPageDto.Start_Time.HasValue)
                dataMapped.Start_Time = (DateTime)meetingLogPageDto.Start_Time;
            else
                dataMapped.Start_Time = DateTime.Now;
            dataMapped.End_Time = DateTime.Now;
            _repoAccessor.eTM_Meeting_Log_Page.Add(dataMapped);
            if (await _repoAccessor.eTM_Meeting_Log_Page.SaveAll())
                return dataMapped.Record_ID;
            return Guid.Empty;
        }

        public async Task<OperationResult> UpdateMeetingLogPage(Guid record_ID, bool clickLinkKaizenSystem = false)
        {
            if (record_ID != Guid.Empty)
            {
                eTM_Meeting_Log_Page item = _repoAccessor.eTM_Meeting_Log_Page.FirstOrDefault(x => x.Record_ID == record_ID);
                if (item == null)
                    return new OperationResult(false, "MeetingLogPage not found.");
                if (clickLinkKaizenSystem) // when click button kaizen system
                    item.Click_Link = true;
                item.End_Time = DateTime.Now; // for each page
                _repoAccessor.eTM_Meeting_Log_Page.Update(item);
                if (await _repoAccessor.eTM_Meeting_Log_Page.SaveAll())
                    return new OperationResult(true, "Successfully updated MeetingLogPage", null, item.End_Time);
                return new OperationResult(false, "Failed to update");
            }
            return new OperationResult(false, "Record_ID is empty.");
        }

        public async Task<OperationResult> GetRouteT5()
        {
            var data = await _repoAccessor.eTM_Page_Settings.FirstOrDefaultAsync(x => x.Tier_Level == "T5" && x.Is_Active);
            if (data == null)
                return new OperationResult(false, "Page setting not found");
            return new OperationResult { Success = true, Data = new KeyValuePair<string, string>("LinkT5", data.Link) };
        }
    }
}
