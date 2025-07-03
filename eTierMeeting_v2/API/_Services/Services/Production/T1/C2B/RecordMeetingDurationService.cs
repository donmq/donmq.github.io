using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Utilities;
using eTierV2_API.Models;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API._Services.Services.Production.T1.C2B
{
    public class RecordMeetingDurationService : IRecordMeetingDurationService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryAccessor _repoAccessor;
        public RecordMeetingDurationService(
            IMapper mapper,
            IRepositoryAccessor repoAccessor)
        {
            _mapper = mapper;
            _repoAccessor = repoAccessor;
        }

        public async Task<OperationResult> Create(string deptId, string classType, string tierLevel)
        {
            if (string.IsNullOrEmpty(deptId))
                return new OperationResult { Success = false };

            var tuId = _repoAccessor.eTM_Team_Unit
            .FindAll(
                x =>
                    x.Center_Level == "Production" &&
                    x.TU_Code.Trim() == deptId.Trim() &&
                    x.Class1_Level == classType.Trim() &&
                    x.Tier_Level == tierLevel.Trim()
                )
            .AsNoTracking().FirstOrDefault()?.TU_ID;

            if (string.IsNullOrEmpty(tuId))
                return new OperationResult(false, "Can not find Team Unit");

            DateTime now = DateTime.Now;
            var record = new eTM_Meeting_Log
            {
                Record_ID = Guid.NewGuid(),
                Data_Date = now.Date,
                Insert_By = "MES",
                Insert_Time = now,
                Record_Status = "MESOPEN",
                Meeting_Start_Time = now,
                TU_ID = tuId
            };

            _repoAccessor.eTM_Meeting_Log.Add(record);
            try
            {
                await _repoAccessor.eTM_Meeting_Log.SaveAll();
                return new OperationResult { Success = true, Data = record };
            }
            catch (Exception)
            {
                return new OperationResult(false, "An error occurred while processing your request");
            }
        }

        public async Task<eTM_Meeting_LogDTO> GetMeetingLog(Guid record_ID)
        {
            var meetingDto = _mapper.Map<eTM_Meeting_LogDTO>(
               await _repoAccessor.eTM_Meeting_Log.FindAll(x => x.Record_ID == record_ID).FirstOrDefaultAsync());

            return meetingDto;
        }

        public async Task<bool> Update(eTM_Meeting_LogDTO mettingLogDto)
        {
            mettingLogDto.Meeting_End_Time = mettingLogDto.Update_Time = DateTime.Now;
            mettingLogDto.Update_By = "ETM";
            mettingLogDto.Record_Status = "NORCLOSE";
            mettingLogDto.Duration_Sec = Convert.ToInt32((mettingLogDto.Meeting_End_Time.Value - mettingLogDto.Meeting_Start_Time).TotalSeconds);
            var data = _mapper.Map<eTM_Meeting_Log>(mettingLogDto);

            try
            {
                _repoAccessor.eTM_Meeting_Log.Update(data);
                return await _repoAccessor.eTM_Meeting_Log.SaveAll();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}