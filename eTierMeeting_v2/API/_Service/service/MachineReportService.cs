using AutoMapper;
using Machine_API._Service.interfaces;
using Machine_API.Data;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Machine_API._Service.service
{
    public class MachineReportService : IMachineReportService
    {
        private readonly DataContext _context;

        public MachineReportService(DataContext context)
        {
            _context = context;
        }

        public async Task<ReportMachineDto> GetListReportMachine(SearchMachineParams searchMachineParams)
        {
            var data = await (_context.SearchMachineDto
                 .FromSqlRaw("EXEC SP_SearchMachine @machineId,@pdcId,@buildingCode,@cellCode,@positionCode,@category,@page,@pageSize,@sort,@isPaging",
                    new SqlParameter("machineId", searchMachineParams.MachineId ?? (object)DBNull.Value),
                    new SqlParameter("pdcId", searchMachineParams.PdcId ?? (object)DBNull.Value),
                    new SqlParameter("buildingCode", searchMachineParams.BuildingCode ?? (object)DBNull.Value),
                    new SqlParameter("cellCode", searchMachineParams.CellCode ?? (object)DBNull.Value),
                    new SqlParameter("positionCode", searchMachineParams.PositionCode ?? (object)DBNull.Value),
                    new SqlParameter("category", searchMachineParams.Category ?? (object)DBNull.Value),
                    new SqlParameter("page", 1),
                    new SqlParameter("pageSize", 10),
                    new SqlParameter("sort", "AssnoID"),
                    new SqlParameter("isPaging", false)
                 )).ToListAsync();

            var dataGroup = data.OrderBy(x => x.BuildingID).GroupBy(x => new { x.BuildingID, x.BuildingName });
            string[] arrayCheck = { "紅牌區", "暫存區" };
            int? idleTotal = 0, inUse = 0;

            List<ReportMachineItem> dataItem = new List<ReportMachineItem>();
            foreach (var item in dataGroup.ToList())
            {
                if (item.Key.BuildingID != null)
                {
                    ReportMachineItem report = new ReportMachineItem();
                    report.BuildingID = item.Key.BuildingID;
                    report.BuildingName = item.Key.BuildingName;
                    report.Idle = data.Where(x => x.BuildingID == item.Key.BuildingID && ((arrayCheck.Any(z => x.Place.Contains(z))) || x.CellCode.Trim() == "A37")).ToList().Count();
                    report.InUse = data.Where(x => x.BuildingID == item.Key.BuildingID).ToList().Count() - report.Idle;
                    idleTotal += report.Idle;
                    inUse += report.InUse;
                    dataItem.Add(report);
                }
            }
            ReportMachineDto result = new ReportMachineDto();
            result.ListReportMachineItem = dataItem;
            result.TotalIdle = idleTotal;
            result.TotalInuse = inUse;
            return result;
        }
    }
}