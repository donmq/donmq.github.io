using System;
using System.Threading.Tasks;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Utilities;

namespace eTierV2_API._Services.Interfaces.Production.T1.C2B
{
    public interface IRecordMeetingDurationService
    {
        Task<OperationResult> Create(string deptId, string classType, string tierLevel);
        Task<bool> Update(eTM_Meeting_LogDTO mettingLogDto);
        Task<eTM_Meeting_LogDTO> GetMeetingLog(Guid record_ID);
    }
}