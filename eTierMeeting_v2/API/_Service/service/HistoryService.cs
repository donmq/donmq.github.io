using Machine_API._Service.interfaces;
using Machine_API.Data;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Machine_API._Service.service
{
    public class HistoryService : IHistoryService
    {
        private readonly DataContext _context;

        public HistoryService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<HistoryExelDto>> ExcelHistories(PaginationParams paginationParams, SearchHistoryParams searchHistoryParams)
        {
            var data = await (_context.HistoryDto
                .FromSqlRaw("EXEC SP_SearchHistory @machineId,@pdcId,@buildingCode,@cellCode,@positionCode,@timeStart,@timeEnd,@page,@pageSize,@sort,@isPaging",
                    new SqlParameter("machineId", searchHistoryParams.MachineId ?? (object)DBNull.Value),
                    new SqlParameter("pdcId", searchHistoryParams.PdcId ?? (object)DBNull.Value),
                    new SqlParameter("buildingCode", searchHistoryParams.BuildingCode ?? (object)DBNull.Value),
                    new SqlParameter("cellCode", searchHistoryParams.CellCode ?? (object)DBNull.Value),
                    new SqlParameter("positionCode", searchHistoryParams.PositionCode ?? (object)DBNull.Value),
                    new SqlParameter("timeStart", searchHistoryParams.TimeStart ?? (object)DBNull.Value),
                    new SqlParameter("timeEnd", searchHistoryParams.TimeEnd ?? (object)DBNull.Value),
                    new SqlParameter("page", paginationParams.PageNumber),
                    new SqlParameter("pageSize", paginationParams.PageSize),
                    new SqlParameter("sort", "Update_Date"),
                    new SqlParameter("isPaging", false)
                )).ToListAsync();

            var dataResult = (from a in data
                              select new HistoryExelDto
                              {
                                  MachineID = a.assnoID,
                                  MachineName = a.MachineName_CN,
                                  UserName = a.EmpName,
                                  UserID = a.UserID,
                                  TrDate = a.Update_Date != null ? a.Update_Date.Value.ToString("yyyy/MM/dd HH:mm:ss tt") : "",

                                  OldPlnoCode = a.Position_Old,
                                  NewPlnoCode = a.Position_New,
                                  NewPlnoName = a.Place_New,
                                  OldPlnoName = a.Place_Old,

                                  OldEmpNumber = a.EmpNumber_Old,
                                  NewEmpNumber = a.EmpNumber_New,

                                  OldCell = a.Cell_Old,
                                  NewCell = a.Cell_New,
                                  OldCellCode = a.CellName_Old,
                                  NewCellCode = a.CellName_New,

                                  Ownerfty = a.OwnerFty
                              }).ToList();
            return dataResult;
        }

        public async Task<PageListUtility<HistoryDto>> SearchHistory(PaginationParams paginationParams, SearchHistoryParams searchHistoryParams)
        {
            var data = await (_context.HistoryDto
                .FromSqlRaw("EXEC SP_SearchHistory @machineId,@pdcId,@buildingCode,@cellCode,@positionCode,@timeStart,@timeEnd,@page,@pageSize,@sort,@isPaging",
                    new SqlParameter("machineId", searchHistoryParams.MachineId ?? (object)DBNull.Value),
                    new SqlParameter("pdcId", searchHistoryParams.PdcId ?? (object)DBNull.Value),
                    new SqlParameter("buildingCode", searchHistoryParams.BuildingCode ?? (object)DBNull.Value),
                    new SqlParameter("cellCode", searchHistoryParams.CellCode ?? (object)DBNull.Value),
                    new SqlParameter("positionCode", searchHistoryParams.PositionCode ?? (object)DBNull.Value),
                    new SqlParameter("timeStart", searchHistoryParams.TimeStart ?? (object)DBNull.Value),
                    new SqlParameter("timeEnd", searchHistoryParams.TimeEnd ?? (object)DBNull.Value),
                    new SqlParameter("page", paginationParams.PageNumber),
                    new SqlParameter("pageSize", paginationParams.PageSize),
                    new SqlParameter("sort", "Update_Date"),
                    new SqlParameter("isPaging", false)
                )).ToListAsync();

            var dataOderBy = data.OrderByDescending(x => x.Update_Date).ToList();
            return PageListUtility<HistoryDto>.PageList(dataOderBy, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}