using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Utilities;

namespace eTierV2_API._Services.Interfaces.Report
{
    public interface I_2_3_1_Meeting_Audit_Report
    {
        Task<byte[]> DownloadExcel(VW_T2_Meeting_LogParam param);
    }
}