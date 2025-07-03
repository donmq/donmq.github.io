using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Cells;
using AutoMapper;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Report;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API._Services.Services.Report
{
    public class S_2_3_1_Meeting_Audit_Report : I_2_3_1_Meeting_Audit_Report
    {

        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IRepositoryAccessor _repoAccessor;

        public S_2_3_1_Meeting_Audit_Report(IRepositoryAccessor repoAccessor, IMapper mapper, IWebHostEnvironment environment)
        {
            _repoAccessor = repoAccessor;
            _mapper = mapper;
            _environment = environment;
        }
        public async Task<byte[]> DownloadExcel(VW_T2_Meeting_LogParam param)
        {
            List<Models.VW_T2_Meeting_Log> listData = await _repoAccessor.VW_T2_Meeting_Log.FindAll(x => x.Meeting_Date >= param.startDate && x.Meeting_Date <= param.endDate).ToListAsync();
            var dataMapped = _mapper.Map<List<VW_T2_Meeting_LogDTO>>(listData);
            VW_T2_Meeting_LogSum dataSum = GetMeetingLogSum(listData);

            MemoryStream stream = new();
            if (dataMapped.Any())
            {
                var path = Path.Combine(_environment.ContentRootPath, "Resources\\Template\\MeetingAuditReport.xlsx");
                WorkbookDesigner designer = new()
                {
                    Workbook = new Workbook(path)
                };
                Worksheet ws = designer.Workbook.Worksheets[0];
                ws.Cells["A1"].PutValue("ETM T2 Meeting Audit Report");
                ws.Cells["J2"].PutValue(dataSum.Perform_Total);
                ws.Cells["J3"].PutValue(dataSum.Perform_Total_Lines);
                ws.Cells["J4"].PutValue(dataSum.Perform_Score + "%");
                ws.Cells["K2"].PutValue(dataSum.Effective_Total);
                ws.Cells["K3"].PutValue(dataSum.Effective_Total_Lines);
                ws.Cells["K4"].PutValue(dataSum.Effective_Score + "%");
                foreach (var item in dataMapped)
                {
                    item.Duration_Sec = Math.Round(item.Duration_Sec / 60, 2);
                    item.Safety_Duration = Math.Round(item.Safety_Duration / 60, 2);
                    item.Quality_Duration = Math.Round(item.Quality_Duration / 60, 2);
                    item.Efficiency_Duration = Math.Round(item.Efficiency_Duration / 60, 2);
                    item.Kaizen_Duration = Math.Round(item.Kaizen_Duration / 60, 2);
                }
                designer.SetDataSource("result", dataMapped);
                designer.Process();
                designer.Workbook.Save(stream, SaveFormat.Xlsx);
            }

            return stream.ToArray();
        }

        public VW_T2_Meeting_LogSum GetMeetingLogSum(List<Models.VW_T2_Meeting_Log> data)
        {
            if (data.Any())
            {
                var result = data.Select(x => new VW_T2_Meeting_LogSum
                {
                    Perform_Total = data.Sum(y => y.Perform),
                    Perform_Total_Lines = data.Count,
                    Perform_Score = data.Sum(y => y.Perform) / data.Count * 100,
                    Effective_Total = data.Sum(y => y.Effective),
                    Effective_Total_Lines = data.Count,
                    Effective_Score = data.Sum(y => y.Effective) / data.Count * 100,
                }).First();
                return result;
            }
            return null;

        }
    }
}