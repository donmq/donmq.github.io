using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Utilities;

namespace eTierV2_API._Services.Interfaces
{
    public interface ICommonService
    {
        Task<OperationResult> GetLineID(string deptID);
        Task<OperationResult> AddVideoLog(eTM_Video_Play_LogDTO videoLogDto);
        Task<Guid> AddMeetingLogPage(eTM_Meeting_Log_PageDTO logpageDto);
        Task<OperationResult> UpdateMeetingLogPage(Guid record_ID, bool clickLinkKaizenSystem = false);
        Task<OperationResult> GetRouteT5();
        Task<bool> CheckDeptClassificationExists(string classkind, string deptid);
    }
}
