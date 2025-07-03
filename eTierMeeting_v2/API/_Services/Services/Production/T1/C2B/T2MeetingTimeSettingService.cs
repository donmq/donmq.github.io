using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Params;
using eTierV2_API.Helpers.Utilities;
using eTierV2_API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API._Services.Services.Production.T1.C2B
{
    public class T2MeetingTimeSettingService : IT2MeetingTimeSettingService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IRepositoryAccessor _repoAccessor;


        public T2MeetingTimeSettingService(
            IMapper mapper,
            MapperConfiguration configMapper,
            IRepositoryAccessor repoAccessor)
        {
            _configMapper = configMapper;
            _mapper = mapper;
            _repoAccessor = repoAccessor;
        }

        public async Task<Helpers.Utilities.OperationResult> Add(eTM_T2_Meeting_SeetingDTO eTM_T2_Meeting_SeetingDTO)
        {
            try
            {
                var item = await _repoAccessor.eTM_T2_Meeting_Seeting.FindAll(x => x.Meeting_Date.Date == Convert.ToDateTime(eTM_T2_Meeting_SeetingDTO.Meeting_Date).Date
               && x.TU_ID.Trim() == eTM_T2_Meeting_SeetingDTO.TU_ID.Trim()).FirstOrDefaultAsync();

                if (item == null)
                {
                    var newItem = _mapper.Map<eTM_T2_Meeting_Seeting>(eTM_T2_Meeting_SeetingDTO);
                    _repoAccessor.eTM_T2_Meeting_Seeting.Add(newItem);
                    if (await _repoAccessor.eTM_T2_Meeting_Seeting.SaveAll())
                        return new Helpers.Utilities.OperationResult(true);
                    return new Helpers.Utilities.OperationResult(false);
                }
                return new Helpers.Utilities.OperationResult(false, "Data already exits");

            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<Helpers.Utilities.OperationResult> Delete(eTM_T2_Meeting_SeetingDTO eTM_T2_Meeting_SeetingDTO)
        {
            var item = await _repoAccessor.eTM_T2_Meeting_Seeting.FindAll(x => x.Meeting_Date.Date == Convert.ToDateTime(eTM_T2_Meeting_SeetingDTO.Meeting_Date).Date  && x.TU_ID.Trim() == eTM_T2_Meeting_SeetingDTO.TU_ID.Trim()).FirstOrDefaultAsync();
            if (item != null)
            {
                _repoAccessor.eTM_T2_Meeting_Seeting.Remove(item);
                if (await _repoAccessor.eTM_T2_Meeting_Seeting.SaveAll())
                    return new Helpers.Utilities.OperationResult(true);
                return new Helpers.Utilities.OperationResult(false);
            }
            return new Helpers.Utilities.OperationResult(false);
        }

        public async Task<PaginationUtility<eTM_T2_Meeting_SeetingDTO>> GetAllData(Helpers.Params.PaginationParam pagination, T2MeetingTimeSettingParam param)
        {
            var pred_eTM_T2_Meeting_Seeting = PredicateBuilder.New<eTM_T2_Meeting_Seeting>(true);
            if (param.Start_Date.HasValue)
                pred_eTM_T2_Meeting_Seeting.And(x => x.Meeting_Date.Date >= param.Start_Date.Value.Date);
            if (param.End_Date.HasValue)
                pred_eTM_T2_Meeting_Seeting.And(x => x.Meeting_Date.Date <= param.End_Date.Value.Date);
            if (!string.IsNullOrEmpty(param.Building_Or_Group))
                pred_eTM_T2_Meeting_Seeting.And(x => x.TU_ID == param.Building_Or_Group);
            var data = _repoAccessor.eTM_T2_Meeting_Seeting.FindAll(pred_eTM_T2_Meeting_Seeting).Select(x => new eTM_T2_Meeting_Seeting
            {
                Meeting_Date = x.Meeting_Date.Date,
                TU_ID = x.TU_ID,
                Update_At = x.Update_At,
                Start_Time = x.Start_Time,
                End_Time = x.End_Time,
                Update_By = x.Update_By
            }).ProjectTo<eTM_T2_Meeting_SeetingDTO>(_configMapper);
            return await PaginationUtility<eTM_T2_Meeting_SeetingDTO>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListBuildingOrGroup()
        {
            var data = await _repoAccessor.eTM_Team_Unit.FindAll(x => x.Tier_Level.Trim() == "T2")
                .Select(x => new KeyValuePair<string, string>(x.TU_ID.Trim(), x.TU_ID.Trim() + " - " + x.TU_Name.Trim()))
                .Distinct().ToListAsync();
            return data;
        }
    }
}