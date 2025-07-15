using LinqKit;
using Machine_API._Accessor;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Machine_API.Models.MachineCheckList;
using Microsoft.EntityFrameworkCore;

namespace Machine_API._Service.service
{
    public class DataHistoryCheckMachineService : IDataHistoryCheckMachineService
    {

        private readonly IMachineRepositoryAccessor _repoAccessor;

        public DataHistoryCheckMachineService(IMachineRepositoryAccessor repository)
        {
            _repoAccessor = repository;
        }

        public async Task<List<SearchMachineDto>> ExportExcelMachine(PaginationParams paginationParams, SearchMachineParams searchMachineParams)
        {
            return await GetData(searchMachineParams);
        }

        public async Task<PageListUtility<SearchMachineDto>> SearchMachine(PaginationParams paginationParams, SearchMachineParams searchMachineParams)
        {
            List<SearchMachineDto> data = await GetData(searchMachineParams);
            return PageListUtility<SearchMachineDto>.PageList(data, paginationParams.PageNumber, paginationParams.PageSize);
        }

        /// <summary>
        /// Get machine data from hp_a04, hp_a15, cell_plno, cells, building, pdc, history
        /// </summary>
        /// <param name="param">Search machine parameters</param>
        /// <returns>List of machine data</returns>
        public async Task<List<SearchMachineDto>> GetData(SearchMachineParams param)
        {
            IQueryable<hp_a04> tbl_hpa04 = _repoAccessor.hp_a04.FindAll(x => x.Visible == true).AsNoTracking();
            IQueryable<hp_a15> tbl_hpa15 = _repoAccessor.hp_a15.FindAll().AsNoTracking();
            IQueryable<Cell_Plno> tbl_cell_plno = _repoAccessor.Cell_Plno.FindAll().AsNoTracking();
            IQueryable<Cells> tbl_cells = _repoAccessor.Cells.FindAll().AsNoTracking();
            IQueryable<Building> tbl_buildings = _repoAccessor.Building.FindAll().AsNoTracking();
            IQueryable<PDC> tbl_PDC = _repoAccessor.PDC.FindAll().AsNoTracking();
            IQueryable<History> tbl_histories = _repoAccessor.History.FindAll().OrderByDescending(h => h.Update_Date).AsNoTracking();

            var cellPlnoGrouped = tbl_cell_plno
                .GroupBy(c => new { c.Plno, c.CellID })
                .Select(g => new { g.Key.Plno, g.Key.CellID });

            IQueryable<SearchMachineDto> query = tbl_hpa04
                .GroupJoin(tbl_hpa15,
                    a04 => a04.Plno,
                    a15 => a15.Plno,
                    (a04, a15Group) => new { a04, a15Group })
                .SelectMany(
                    x => x.a15Group.DefaultIfEmpty(),
                    (x, a15) => new { x.a04, a15 })
                .GroupJoin(cellPlnoGrouped,
                    x => x.a15.Plno,
                    cellPlno => cellPlno.Plno,
                    (x, tGroup) => new { x.a04, x.a15, tGroup })
                .SelectMany(
                    x => x.tGroup.DefaultIfEmpty(),
                    (x, cellPlno) => new { x.a04, x.a15, cellPlno })
                .GroupJoin(tbl_cells,
                    x => x.cellPlno.CellID,
                    cell => cell.CellID,
                    (x, c0Group) => new { x.a04, x.a15, x.cellPlno, c0Group })
                .SelectMany(
                    x => x.c0Group.DefaultIfEmpty(),
                    (x, cell) => new { x.a04, x.a15, x.cellPlno, cell })
                .GroupJoin(tbl_buildings,
                    x => x.cell.BuildingID,
                    building => building.BuildingID,
                    (x, bGroup) => new { x.a04, x.a15, x.cellPlno, x.cell, bGroup })
                .SelectMany(
                    x => x.bGroup.DefaultIfEmpty(),
                    (x, building) => new { x.a04, x.a15, x.cellPlno, x.cell, building })
                .GroupJoin(tbl_PDC,
                    x => x.cell.PDCID,
                    pdc => pdc.PDCID,
                    (x, pGroup) => new { x.a04, x.a15, x.cellPlno, x.cell, x.building, pGroup })
                .SelectMany(
                    x => x.pGroup.DefaultIfEmpty(),
                    (x, pdc) => new SearchMachineDto
                    {
                        MainAssetNumber = x.a04 != null && x.a04.Main_Asset_Number != null ? x.a04.Main_Asset_Number.TrimEnd() : string.Empty,
                        OwnerFty = x.a04.OwnerFty,
                        Askid = x.a04.Askid,
                        AssnoID = x.a04 != null && x.a04.AssnoID != null ? x.a04.AssnoID.TrimEnd() : string.Empty,
                        MachineName_CN = x.a04.MachineName_CN,
                        MachineName_EN = x.a04.MachineName_EN,
                        MachineName_Local = x.a04.MachineName_Local,
                        Spec = x.a04.Spec,
                        CellCode = x.cell.CellCode ?? string.Empty,
                        PdcId = pdc != null ? pdc.PDCID : 0,
                        BuildingID = x.building != null ? x.building.BuildingID : 0,
                        BuildingName = x.building.BuildingName ?? string.Empty,
                        Place = x.a15.Place ?? string.Empty,
                        PlaceReport = (x.a15.Plno + " " + x.a15.Place) ?? string.Empty,
                        Plno = x.a15.Plno,
                        Supplier = x.a04.Supplier,
                        State = x.a04.State,
                        Trdate = x.a04.Trdate,
                        LastMoveMachineDate = tbl_histories
                            .FirstOrDefault(h => h.assnoID == x.a04.AssnoID && h.OwnerFty == x.a04.OwnerFty)
                            .Update_Date.ToString("yyyyMMdd")
                    });
            ExpressionStarter<SearchMachineDto> predicate = PredicateBuilder.New<SearchMachineDto>(true);
            if (!string.IsNullOrEmpty(param.MachineId))
            {
                string ownerFty = param.MachineId[..1];
                string assnoID = param.MachineId[1..];
                if (!char.IsDigit(ownerFty.ToChar()))
                    predicate = predicate.And(x => x.AssnoID.Contains(assnoID) && x.OwnerFty == ownerFty);
                else
                    predicate = predicate.And(x => x.AssnoID.Contains(param.MachineId));
            }

            if (!string.IsNullOrEmpty(param.Category))
                predicate = predicate.And(x => x.Askid == param.Category);

            if (!string.IsNullOrEmpty(param.PositionCode))
                predicate = predicate.And(x => x.Plno == param.PositionCode);

            if (!string.IsNullOrEmpty(param.CellCode) && param.CellCode != "0")
                predicate = predicate.And(x => x.CellCode == param.CellCode);

            if (param.BuildingId != 0)
                predicate = predicate.And(x => x.BuildingID == param.BuildingId);

            if (param.PdcId != 0)
                predicate = predicate.And(x => x.PdcId == param.PdcId);

            query = query.Where(predicate);

            List<SearchMachineDto> result = await query.ToListAsync();
            return result
                .GroupBy(t => new { t.MainAssetNumber, t.AssnoID, t.OwnerFty })
                .Select(g => new SearchMachineDto
                {
                    MainAssetNumber = g.Max(t => t.MainAssetNumber),
                    AssnoID = g.Max(t => t.AssnoID),
                    OwnerFty = g.Max(t => t.OwnerFty),
                    Askid = g.Max(t => t.Askid),
                    MachineName_CN = g.Max(t => t.MachineName_CN),
                    MachineName_EN = g.Max(t => t.MachineName_EN),
                    MachineName_Local = g.Max(t => t.MachineName_Local),
                    Spec = g.Max(t => t.Spec),
                    CellCode = string.Join(",", g.Select(t => t.CellCode).Distinct()),
                    PdcId = g.Max(t => t.PdcId),
                    BuildingID = g.Max(t => t.BuildingID),
                    BuildingName = g.Max(t => t.BuildingName),
                    PlaceReport = g.Max(t => t.PlaceReport),
                    Place = g.Max(t => t.Place),
                    Plno = g.Max(t => t.Plno),
                    Supplier = g.Max(t => t.Supplier),
                    State = g.Max(t => t.State),
                    Trdate = g.Max(t => t.Trdate),
                    LastMoveMachineDate = g.Max(t => t.LastMoveMachineDate)
                }).ToList();
        }

    }
}