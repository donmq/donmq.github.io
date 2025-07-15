using System.Drawing;
using Aspose.Cells;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Machine_API.Models.MachineCheckList;
using Microsoft.EntityFrameworkCore;
using Machine_API.Data;
using Machine_API._Accessor;
using Machine_API.Resources;
using LinqKit;

namespace Machine_API._Service.service
{
    public class HistoryInventoryService : IHistoryInventoryService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly LocalizationService _languageService;
        private readonly DataContext _context;
        private readonly ICellPlnoService _cellPlnoService;

        public HistoryInventoryService(
            IMachineRepositoryAccessor repository,
            MapperConfiguration mapperConfiguration,
            LocalizationService languageService,
            DataContext context,
            ICellPlnoService cellPlnoService)
        {
            _repository = repository;
            _mapperConfiguration = mapperConfiguration;
            _languageService = languageService;
            _context = context;
            _cellPlnoService = cellPlnoService;
        }

        public bool CheckDateHistoryInventory(string checkDate)
        {
            DateTime checkDate1 = Convert.ToDateTime(checkDate + " 00:00:00");
            DateTime checkDate2 = Convert.ToDateTime(checkDate + " 23:59:59");
            var data = _repository.HistoryInventory.FindAll(x => x.CreateTime >= checkDate1 && x.CreateTime <= checkDate2).ToList();
            if (data.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void CustomStyle(ref Cell cellCustom)
        {
            string value = Convert.ToString(cellCustom.Value);
            if (value == "1")
            {
                cellCustom.PutValue(_languageService.GetLocalizedHtmlString("match"));
                Style styleCustom = cellCustom.GetStyle();
                styleCustom.Font.Color = Color.Green;
                styleCustom.Font.IsBold = true;
                cellCustom.SetStyle(styleCustom);
            }
            else if (value == "-1")
            {
                cellCustom.PutValue(_languageService.GetLocalizedHtmlString("wrong_position"));
                Style styleCustom = cellCustom.GetStyle();
                styleCustom.Font.Color = Color.Orange;
                styleCustom.Font.IsBold = true;
                cellCustom.SetStyle(styleCustom);
            }
            else if (value == "0")
            {
                cellCustom.PutValue(_languageService.GetLocalizedHtmlString("not_scan"));
                Style styleCustom = cellCustom.GetStyle();
                styleCustom.Font.Color = Color.Red;
                styleCustom.Font.IsBold = true;
                cellCustom.SetStyle(styleCustom);
            }
        }

        public async Task<List<DataHistoryInventoryDto>> GetDatalHistoryInventoryById(int historyInventoryID)
        {
            var data = await _repository.DataHistoryInventory
                .FindAll(x => x.HistoryInventoryID == historyInventoryID)
                .ProjectTo<DataHistoryInventoryDto>(_mapperConfiguration).ToListAsync();
            return data;
        }

        public async Task<ResultAllInventoryDto> GetListDetailHistoryPlno(string plnoId, string timeSoKiem, string timePhucKiem, string timeRutKiem, string lang)
        {
            int? idPhucKiem = null, idSoKiem = null, idRutKiem = null;

            var userSoKiem = new User();
            var userPhucKiem = new User();
            var userRutKiem = new User();

            var dataSoKiem = new HistoryInventory();
            var dataPhucKiem = new HistoryInventory();
            var dataRutKiem = new HistoryInventory();

            var listDataHistorySoKiem = new List<DataHistoryInventory>();
            var listDataHistoryPhucKiem = new List<DataHistoryInventory>();
            var listDataHistoryRutKiem = new List<DataHistoryInventory>();
            var listAll = new List<DataHistoryInventory>();
            var listDetail = new List<DetailInventoryDto>();

            var listMachineOfHPA04 = await _repository.hp_a04
                .FindAll(x => x.Plno.Trim() == plnoId && x.Visible == true)
                .GroupJoin(
                    _repository.hp_a15.FindAll(),
                    x => x.Plno, y => y.Plno,
                    (x, y) => new { HP_A04 = x, HP_A15s = y })
                .SelectMany(x => x.HP_A15s.DefaultIfEmpty(), (x, z) => new { x.HP_A04, HP_A15 = z })
                .Select(x => new DetaiHistoryInventoryDto
                {
                    MachineID = x.HP_A04.OwnerFty.Trim() + x.HP_A04.AssnoID.Trim(),
                    MachineName = lang == "vi-VN" ? x.HP_A04.MachineName_Local : (lang == "zh-TW" ? x.HP_A04.MachineName_CN : x.HP_A04.MachineName_EN),
                    MachineName_CN = x.HP_A04.MachineName_CN,
                    Supplier = x.HP_A04.Supplier.Trim(),
                    Place = x.HP_A15.Place.Trim(),
                    State = x.HP_A04.State.Trim(),
                    Plno = x.HP_A04.Plno.Trim(),
                }).ToListAsync();

            var machines = listMachineOfHPA04.Select(x => x.MachineID).Distinct().ToList();

            //Sơ kiểm
            if (!string.IsNullOrEmpty(timeSoKiem))
            {
                var soKiemTime1 = Convert.ToDateTime(timeSoKiem + " 00:00:00");
                var soKiemTime2 = Convert.ToDateTime(timeSoKiem + " 23:59:59");
                dataSoKiem = await _repository.HistoryInventory
                    .FindAll(x => x.PlnoID == plnoId && x.InventoryType == 1 && x.CreateTime >= soKiemTime1 && x.CreateTime <= soKiemTime2)
                    .OrderByDescending(x => x.CreateTime).FirstOrDefaultAsync();

                if (dataSoKiem != null)
                {
                    idSoKiem = dataSoKiem.HistoryInventoryID;
                    listDataHistorySoKiem = await _repository.DataHistoryInventory
                        .FindAll(x => x.HistoryInventoryID == idSoKiem && machines.Contains(x.MachineID))
                        .ToListAsync();
                    userSoKiem = await _repository.User.FirstOrDefaultAsync(x => x.UserName == dataSoKiem.CreateBy);
                }
            }

            //Phúc kiểm
            if (!string.IsNullOrEmpty(timePhucKiem))
            {
                DateTime phucKiemTime1 = Convert.ToDateTime(timePhucKiem + " 00:00:00");
                DateTime phucKiemTime2 = Convert.ToDateTime(timePhucKiem + " 23:59:59");
                dataPhucKiem = await _repository.HistoryInventory
                    .FindAll(x => x.PlnoID == plnoId && x.InventoryType == 2 && x.CreateTime >= phucKiemTime1 && x.CreateTime <= phucKiemTime2)
                    .OrderByDescending(x => x.CreateTime).FirstOrDefaultAsync();

                if (dataPhucKiem != null)
                {
                    idPhucKiem = dataPhucKiem.HistoryInventoryID;
                    listDataHistoryPhucKiem = await _repository.DataHistoryInventory
                        .FindAll(x => x.HistoryInventoryID == idPhucKiem && machines.Contains(x.MachineID))
                        .ToListAsync();
                    userPhucKiem = await _repository.User.FirstOrDefaultAsync(x => x.UserName == dataPhucKiem.CreateBy);
                }
            }

            //Rút kiểm
            if (!string.IsNullOrEmpty(timeRutKiem))
            {
                DateTime rutKiemTime1 = Convert.ToDateTime(timeRutKiem + " 00:00:00");
                DateTime rutKiemTime2 = Convert.ToDateTime(timeRutKiem + " 23:59:59");
                dataRutKiem = await _repository.HistoryInventory.FindAll(x => x.PlnoID == plnoId && x.InventoryType == 3 && x.CreateTime >= rutKiemTime1 && x.CreateTime <= rutKiemTime2)
                    .OrderByDescending(x => x.CreateTime).FirstOrDefaultAsync();

                if (dataRutKiem != null)
                {
                    idRutKiem = dataRutKiem.HistoryInventoryID;
                    listDataHistoryRutKiem = await _repository.DataHistoryInventory
                        .FindAll(x => x.HistoryInventoryID == idRutKiem && machines.Contains(x.MachineID))
                        .ToListAsync();
                    userRutKiem = await _repository.User.FirstOrDefaultAsync(x => x.UserName == dataRutKiem.CreateBy);
                }
            }

            listAll.AddRange(listDataHistorySoKiem);
            listAll.AddRange(listDataHistoryPhucKiem);
            listAll.AddRange(listDataHistoryRutKiem);
            listAll = listAll.GroupBy(x => x.MachineID.Trim()).Select(x => x.First()).ToList();

            var listResult = new List<DetaiHistoryInventoryDto>();
            foreach (var item in listAll)
            {
                var itemSoKiem = listDataHistorySoKiem.FirstOrDefault(x => x.MachineID.Trim() == item.MachineID.Trim());
                var itemPhucKiem = listDataHistoryPhucKiem.FirstOrDefault(x => x.MachineID.Trim() == item.MachineID.Trim());
                var itemRutKiem = listDataHistoryRutKiem.FirstOrDefault(x => x.MachineID.Trim() == item.MachineID.Trim());

                var machine = listMachineOfHPA04.FirstOrDefault(x => x.MachineID == item.MachineID.Trim());
                var detail = new DetaiHistoryInventoryDto
                {
                    MachineID = item.MachineID,
                    MachineName = machine?.MachineName?.Trim(),
                    Supplier = item.Supplier,
                    Place = machine?.Place?.Trim(),
                    ScanPlace = item.Place.Trim(),
                    State = item.State,
                };

                if (itemPhucKiem != null)
                {
                    detail.StatusPhucKiem = itemPhucKiem.StatusInventory;
                    detail.StatusNamePhucKiem = GetNameInventory(itemPhucKiem.StatusInventory.Value);
                }

                if (itemSoKiem != null)
                {
                    detail.StatusSoKiem = itemSoKiem.StatusInventory;
                    detail.StatusNameSoKiem = GetNameInventory(itemSoKiem.StatusInventory.Value);
                }

                if (itemRutKiem != null)
                {
                    detail.StatusRutKiem = itemRutKiem.StatusInventory;
                    detail.StatusNameRutKiem = GetNameInventory(itemRutKiem.StatusInventory.Value);
                }

                listResult.Add(detail);
            }

            // Lấy ra danh sách mã máy mà chưa được sơ kiểm
            var listMissingMachineIDOfHPA04 = machines.Except(listDataHistorySoKiem.Select(x => x.MachineID.Trim())).ToList();

            // Nếu tồn tại máy chưa được sơ kiểm
            // Thì tiến hành thêm vào với trạng thái là "Not Scan" (StatusInventory = 0)
            if (listMissingMachineIDOfHPA04.Count > 0)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var listNewResult = new List<DetaiHistoryInventoryDto>();
                    foreach (var machineID in listMissingMachineIDOfHPA04)
                    {
                        // Lấy ra máy thiếu sơ kiểm từ hp_a04
                        var hpA04Item = listMachineOfHPA04.FirstOrDefault(x => x.MachineID == machineID);

                        var newItem = new DataHistoryInventory
                        {
                            MachineID = machineID,
                            MachineName = hpA04Item.MachineName_CN,
                            Place = hpA04Item.Place,
                            State = hpA04Item.State,
                            StatusInventory = 0,
                            Supplier = hpA04Item.Supplier,
                            HistoryInventoryID = dataSoKiem.HistoryInventoryID,
                        };
                        dataSoKiem.CountNotScan += 1;

                        _repository.DataHistoryInventory.Add(newItem);
                        _repository.HistoryInventory.Update(dataSoKiem);
                        await _repository.SaveChangesAsync();

                        var newResult = new DetaiHistoryInventoryDto
                        {
                            MachineID = newItem.MachineID?.Trim(),
                            MachineName = hpA04Item.MachineName,
                            Supplier = newItem.Supplier?.Trim(),
                            Place = newItem.Place.Trim(),
                            ScanPlace = newItem.Place.Trim(),
                            State = newItem.State?.Trim(),
                            DateSoKiem = dataSoKiem.CreateTime,
                            StatusSoKiem = newItem.StatusInventory,
                            StatusNameSoKiem = GetNameInventory(newItem.StatusInventory.Value),
                        };
                        listNewResult.Add(newResult);
                    }

                    listResult.AddRange(listNewResult);
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            listResult = listResult
                .OrderBy(x => x.MachineID)
                .ThenByDescending(x => x.DateSoKiem)
                .ThenByDescending(x => x.DatePhucKiem)
                .ThenByDescending(x => x.DateRutKiem)
                .GroupBy(x => new { MachineID = x.MachineID?.Trim() })
                .Select(x => new DetaiHistoryInventoryDto
                {
                    MachineID = x.Key.MachineID,
                    MachineName = x.FirstOrDefault()?.MachineName?.Trim(),
                    Supplier = x.FirstOrDefault()?.Supplier?.Trim(),
                    Place = x.FirstOrDefault()?.Place,
                    ScanPlace = x.FirstOrDefault()?.ScanPlace,
                    State = x.FirstOrDefault()?.State.Trim(),
                    DateSoKiem = x.FirstOrDefault()?.DateSoKiem,
                    DatePhucKiem = x.FirstOrDefault()?.DatePhucKiem,
                    DateRutKiem = x.FirstOrDefault()?.DateRutKiem,
                    StatusSoKiem = x.FirstOrDefault()?.StatusSoKiem,
                    StatusNameSoKiem = x.FirstOrDefault()?.StatusNameSoKiem?.Trim(),
                    StatusPhucKiem = x.FirstOrDefault()?.StatusPhucKiem,
                    StatusNamePhucKiem = x.FirstOrDefault()?.StatusNamePhucKiem?.Trim(),
                    StatusRutKiem = x.FirstOrDefault()?.StatusRutKiem,
                    StatusNameRutKiem = x.FirstOrDefault()?.StatusNameRutKiem?.Trim(),
                }).ToList();

            var dataMachineSoKiem = listResult.Where(x => x.StatusSoKiem != null).ToList();
            var dataMachinePhucKiem = listResult.Where(x => x.StatusPhucKiem != null).ToList();
            var dataMachineRutKiem = listResult.Where(x => x.StatusRutKiem != null).ToList();

            listDetail = new List<DetailInventoryDto>
            {
                new DetailInventoryDto
                {
                    TypeInventory = 1,
                    CountMachine = dataMachineSoKiem.Count,
                    CountMatch = dataMachineSoKiem.Count(x => x.StatusSoKiem == 1),
                    CountNotScan = dataMachineSoKiem.Count(x => x.StatusSoKiem == 0),
                    CountWrongPosition = dataMachineSoKiem.Count(x => x.StatusSoKiem == -1),
                    EmpName = userSoKiem?.EmpName,
                    EmpNumber = userSoKiem.UserName,
                    PercenMatch = dataMachineSoKiem.Any() ? ((double)dataMachineSoKiem.Count(x => x.StatusSoKiem == 1) / dataMachineSoKiem.Count * 100).ToString("0.##") : "0",
                    DateStartInventory = dataSoKiem?.StartTimeInventory,
                    DateEndInventory = dataSoKiem?.EndTimeInventory,
                    CreateTime = dataSoKiem?.CreateTime,
                },
                new DetailInventoryDto
                {
                    TypeInventory = 2,
                    CountMachine = dataMachinePhucKiem.Count,
                    CountMatch = dataMachinePhucKiem.Count(x => x.StatusPhucKiem == 1),
                    CountNotScan = dataMachinePhucKiem.Count(x => x.StatusPhucKiem == 0),
                    CountWrongPosition = dataMachinePhucKiem.Count(x => x.StatusPhucKiem == -1),
                    EmpName = userPhucKiem?.EmpName,
                    EmpNumber = userPhucKiem.UserName,
                    PercenMatch = dataMachinePhucKiem.Any() ? ((double)dataMachinePhucKiem.Count(x => x.StatusSoKiem == 1) / dataMachinePhucKiem.Count * 100).ToString("0.##") : "0",
                    DateStartInventory = dataPhucKiem?.StartTimeInventory,
                    DateEndInventory = dataPhucKiem?.EndTimeInventory,
                    CreateTime = dataPhucKiem?.CreateTime,
                },
                new DetailInventoryDto
                {
                    TypeInventory = 3,
                    CountMachine = dataMachineRutKiem.Count,
                    CountMatch = dataMachineRutKiem.Count(x => x.StatusRutKiem == 1),
                    CountNotScan = dataMachineRutKiem.Count(x => x.StatusRutKiem == 0),
                    CountWrongPosition = dataMachineRutKiem.Count(x => x.StatusRutKiem == -1),
                    EmpName = userRutKiem?.EmpName,
                    EmpNumber = userRutKiem.UserName,
                    PercenMatch = dataMachineRutKiem.Any() ? ((double)dataMachineRutKiem.Count(x => x.StatusSoKiem == 1) / dataMachineRutKiem.Count * 100).ToString("0.##") : "0",
                    DateStartInventory = dataRutKiem?.StartTimeInventory,
                    DateEndInventory = dataRutKiem?.EndTimeInventory,
                    CreateTime = dataRutKiem?.CreateTime,
                }
            };

            var result = new ResultAllInventoryDto
            {
                ListResult = listResult,
                ListDetail = listDetail
            };

            return result;
        }

        private string GetNameInventory(int inventory)
        {
            if (inventory == 0)
            {
                return _languageService.GetLocalizedHtmlString("not_scan");
            }
            else if (inventory == 1)
            {
                return _languageService.GetLocalizedHtmlString("match");
            }
            else if (inventory == -1)
            {
                return _languageService.GetLocalizedHtmlString("wrong_position");
            }
            else
            {
                return _languageService.GetLocalizedHtmlString("not_found");
            }
        }

        public async Task<List<HistoryInventoryLineDto>> GetListPlnoHistotry(string dateSearch, int idBuilding, int? isCheck)
        {
            List<Hp_a15Dto> listPlno = new();

            if (isCheck == 2)
            {
                string nameCell = _repository.Cells.FirstOrDefault(x => x.CellID == idBuilding).CellCode;
                listPlno = await _cellPlnoService.GetListPlanoByCellIDV2(nameCell);
            }
            else
            {
                listPlno = await _cellPlnoService.GetListPlnoByBuildingID(idBuilding);
            }

            var plnos = listPlno.Select(x => x.Plno.Trim()).Distinct().ToList();          

            var predicateHis = PredicateBuilder.New<HistoryInventory>(x => plnos.Contains(x.PlnoID));
            if (!string.IsNullOrEmpty(dateSearch))
            {
                var date = Convert.ToDateTime(dateSearch + " 23:59:59");
                predicateHis.And(x => x.CreateTime <= date);
            }

            var historyInventoryQuery = await _repository.HistoryInventory.FindAll(predicateHis).OrderByDescending(x => x.CreateTime).ToListAsync();
            var hisIds = historyInventoryQuery.Select(x => x.HistoryInventoryID).Distinct().ToList();

            var dataHis = await _repository.DataHistoryInventory.FindAll(x => x.HistoryInventoryID.HasValue && hisIds.Contains(x.HistoryInventoryID.Value)).ToListAsync();

            List<HistoryInventoryLineDto> listResult = new();

            foreach (var item in listPlno)
            {
                HistoryInventoryLineDto result = new()
                {
                    PlnoId = item.Plno.Trim(),
                    Place = item.Place.Trim()
                };
                if (!historyInventoryQuery.Any(x => x.InventoryType == 1 && x.PlnoID.Trim() == result.PlnoId) &&
                    !historyInventoryQuery.Any(x => x.InventoryType == 2 && x.PlnoID.Trim() == result.PlnoId) &&
                    !historyInventoryQuery.Any(x => x.InventoryType == 3 && x.PlnoID.Trim() == result.PlnoId))
                    continue;
                else
                {
                    var checkSoKiem = historyInventoryQuery.FirstOrDefault(x => x.InventoryType == 1 && x.PlnoID.Trim() == result.PlnoId);
                    var checkPhucKiem = historyInventoryQuery.FirstOrDefault(x => x.InventoryType == 2 && x.PlnoID.Trim() == result.PlnoId);
                    var checkRutKiem = historyInventoryQuery.FirstOrDefault(x => x.InventoryType == 3 && x.PlnoID.Trim() == result.PlnoId);

                    
                    if (checkSoKiem != null)
                    {
                        result.TimeSoKiem = checkSoKiem.CreateTime;
                        var listDataSokiem = dataHis.Where(x => x.HistoryInventoryID == checkSoKiem.HistoryInventoryID);
                        var CountComplete = listDataSokiem.Count(x => x.StatusInventory == 1);
                        result.PecenMatchSoKiem = listDataSokiem.Any() ? ((double)CountComplete / listDataSokiem.Count() * 100).ToString("0.##") : "0";
                    }
                    if (checkPhucKiem != null)
                    {
                        result.TimePhucKiem = checkPhucKiem.CreateTime;
                        var listDataPhucKiem = dataHis.Where(x => x.HistoryInventoryID == checkPhucKiem.HistoryInventoryID);
                        var CountComplete = listDataPhucKiem.Count(x => x.StatusInventory == 1);
                        result.PecenMatchPhucKiem = listDataPhucKiem.Any() ? ((double)CountComplete / listDataPhucKiem.Count() * 100).ToString("0.##") : "0";
                    }
                    if (checkRutKiem != null)
                    {
                        result.TimeRutKiem = checkRutKiem.CreateTime;
                        var listDataRutKiem = dataHis.Where(x => x.HistoryInventoryID == checkRutKiem.HistoryInventoryID);
                        var CountComplete = listDataRutKiem.Count(x => x.StatusInventory == 1);
                        result.PecenMatchRutKiem = listDataRutKiem.Any() ? ((double)CountComplete / listDataRutKiem.Count() * 100).ToString("0.##") : "0";
                    }
                    listResult.Add(result);
                }
            }
            return listResult;
        }

        public void PutStaticValue(ref Worksheet ws, HistoryInventoryDto dataHistory)
        {
            ws.Cells["A2"].PutValue(_languageService.GetLocalizedHtmlString("inventory_result"));

            //-title table
            ws.Cells["A9"].PutValue(_languageService.GetLocalizedHtmlString("machine_code"));
            ws.Cells["B9"].PutValue(_languageService.GetLocalizedHtmlString("machine_name"));
            ws.Cells["D9"].PutValue(_languageService.GetLocalizedHtmlString("suppplier"));
            ws.Cells["F9"].PutValue(_languageService.GetLocalizedHtmlString("location"));
            ws.Cells["H9"].PutValue(_languageService.GetLocalizedHtmlString("state"));
            ws.Cells["J9"].PutValue(_languageService.GetLocalizedHtmlString("inventory_status"));

            string InventoryType = dataHistory.InventoryType == 1 ? _languageService.GetLocalizedHtmlString("sokiem") : dataHistory.InventoryType == 2 ? _languageService.GetLocalizedHtmlString("phuc_kiem") : _languageService.GetLocalizedHtmlString("rut_kiem");

            //Set lang header
            ws.Cells["A4"].PutValue(_languageService.GetLocalizedHtmlString("inventory_type") + " : " + InventoryType);
            ws.Cells["A5"].PutValue(_languageService.GetLocalizedHtmlString("employee_code") + " : " + dataHistory.UserName);
            ws.Cells["A6"].PutValue(_languageService.GetLocalizedHtmlString("match") + " : " + dataHistory.CountComplete);
            ws.Cells["A7"].PutValue(_languageService.GetLocalizedHtmlString("startTimeInventory") + " : " + dataHistory.StartTimeInventory != null ? dataHistory.StartTimeInventory.Value.ToString("HH:mm:ss") + "s" : null);
            ws.Cells["D4"].PutValue(_languageService.GetLocalizedHtmlString("inventory_location") + " : " + dataHistory.Place);
            ws.Cells["D5"].PutValue(_languageService.GetLocalizedHtmlString("employee_name") + " : " + dataHistory.EmpName);
            ws.Cells["D6"].PutValue(_languageService.GetLocalizedHtmlString("wrong_position") + " : " + dataHistory.CountWrongPosition);
            ws.Cells["D7"].PutValue(_languageService.GetLocalizedHtmlString("endTimeInventory") + " : " + dataHistory.EndTimeInventory != null ? dataHistory.EndTimeInventory.Value.ToString("HH:mm:ss") + "s" : null);
            ws.Cells["H4"].PutValue(_languageService.GetLocalizedHtmlString("date") + " : " + dataHistory.DateTime != null ? dataHistory.DateTime.Value.ToString("yyyy/MM/dd") : null);
            ws.Cells["H5"].PutValue(_languageService.GetLocalizedHtmlString("total") + " : " + (dataHistory.CountComplete + dataHistory.CountNotScan));
            ws.Cells["H6"].PutValue(_languageService.GetLocalizedHtmlString("not_scan") + " : " + dataHistory.CountNotScan);
        }

        public async Task<PageListUtility<HistoryInventoryDto>> SearchHistoryInventory(HistoryInventoryParams historyInventoryParams, PaginationParams paginationParams)
        {
            var predicateHis = PredicateBuilder.New<HistoryInventory>(true);
            //Search by Inventory type
            if (historyInventoryParams.IdInventory != 0)
                predicateHis.And(x => x.InventoryType == historyInventoryParams.IdInventory);

            //Search by place
            if (!string.IsNullOrEmpty(historyInventoryParams.IdPlno))
                predicateHis.And(x => x.PlnoID == historyInventoryParams.IdPlno);

            DateTime date1 = Convert.ToDateTime(historyInventoryParams.FromDateTime + " 00:00");
            DateTime date2 = Convert.ToDateTime(historyInventoryParams.ToDateTime + " 23:59");
            if (!string.IsNullOrEmpty(historyInventoryParams.FromDateTime) && string.IsNullOrEmpty(historyInventoryParams.ToDateTime))
                predicateHis.And(x => x.CreateTime >= date1);
            else if (string.IsNullOrEmpty(historyInventoryParams.FromDateTime) && !string.IsNullOrEmpty(historyInventoryParams.ToDateTime))
                predicateHis.And(x => x.CreateTime <= date2);
            else if (!string.IsNullOrEmpty(historyInventoryParams.FromDateTime) && !string.IsNullOrEmpty(historyInventoryParams.ToDateTime))
                predicateHis.And(x => x.CreateTime >= date1 && x.CreateTime <= date2);

            var historyInventorry = _repository.HistoryInventory.FindAll(predicateHis);
            var users = _repository.User.FindAll();
            var hp_A15 = _repository.hp_a15.FindAll();
            var dataJoin = (from a in historyInventorry
                            join b in hp_A15 on a.PlnoID equals b.Plno
                            join c in users on a.CreateBy equals c.UserName
                            where a.PlnoID == b.Plno && a.CreateBy == c.UserName
                            select new HistoryInventoryDto
                            {
                                HistoryInventoryID = a.HistoryInventoryID,
                                InventoryType = a.InventoryType,
                                IdPlno = a.PlnoID,
                                Place = b.Place,
                                CountComplete = a.CountComplete,
                                CountWrongPosition = a.CountWrongPosition,
                                CountNotScan = a.CountNotScan,
                                StartTimeInventory = a.StartTimeInventory,
                                EndTimeInventory = a.EndTimeInventory,
                                UserName = c.UserName,
                                EmpName = c.EmpName,
                                DateTime = a.CreateTime,
                            });

            var dataResult = await dataJoin.OrderByDescending(x => x.HistoryInventoryID).ToListAsync();


            return PageListUtility<HistoryInventoryDto>.PageList(dataResult, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<List<HistoryInventoryExportPdfDto>> GetDataPdfByDay(string date)
        {
            // DateTime datePdf = Convert.ToDateTime(date + " 00:00");
            DateTime datePdf1 = Convert.ToDateTime(date + " 00:00:00");
            DateTime datePdf2 = Convert.ToDateTime(date + " 23:59:59");

            List<HistoryInventoryExportPdfDto> result = new();

            var data = await _repository.HistoryInventory.FindAll(x => x.CreateTime >= datePdf1 && x.CreateTime <= datePdf2).ToListAsync();
            var countComplete = data.Sum(x => x.CountComplete);
            var countWrongPosition = data.Sum(x => x.CountWrongPosition);
            var countNotScan = data.Sum(x => x.CountNotScan);
            foreach (var item in data)
            {
                var dataHistory = _repository.DataHistoryInventory.FindAll();
                var hp_A15 = _repository.hp_a15.FindAll();

                IEnumerable<HistoryInventoryExportPdfDto> model = (from d in dataHistory
                                                                   join hp in hp_A15 on item.PlnoID equals hp.Plno
                                                                   where item.HistoryInventoryID == d.HistoryInventoryID
                                                                   select new HistoryInventoryExportPdfDto()
                                                                   {
                                                                       CountComplete = countComplete,
                                                                       CountWrongPosition = countWrongPosition,
                                                                       CountNotScan = countNotScan,
                                                                       MachineID = d.MachineID,
                                                                       MachineName = d.MachineName,
                                                                       Supplier = d.Supplier,
                                                                       Place = d.Place,
                                                                       PlaceInventory = hp.Place,
                                                                       State = d.State,
                                                                       StatusInventory = d.StatusInventory,
                                                                       DateTime = item.CreateTime,
                                                                       InventoryType = item.InventoryType == 1 ? _languageService.GetLocalizedHtmlString("sokiem") : item.InventoryType == 2 ? _languageService.GetLocalizedHtmlString("phuc_kiem") :
                                                                       _languageService.GetLocalizedHtmlString("rut_kiem")
                                                                   });
                result.AddRange(model);
            }
            return result;
        }

        public void PutStaticValue1(ref Worksheet ws, HistoryInventoryExportPdfDto history)
        {
            ws.Cells["A1"].PutValue(_languageService.GetLocalizedHtmlString("inventory_result"));

            //-title table
            ws.Cells["A6"].PutValue(_languageService.GetLocalizedHtmlString("machine_code"));
            ws.Cells["B6"].PutValue(_languageService.GetLocalizedHtmlString("machine_name"));
            ws.Cells["D6"].PutValue(_languageService.GetLocalizedHtmlString("suppplier"));
            ws.Cells["F6"].PutValue(_languageService.GetLocalizedHtmlString("inventory_type"));
            ws.Cells["H6"].PutValue(_languageService.GetLocalizedHtmlString("state"));
            ws.Cells["J6"].PutValue(_languageService.GetLocalizedHtmlString("inventory_type"));
            ws.Cells["M6"].PutValue(_languageService.GetLocalizedHtmlString("inventory_location"));
            ws.Cells["N6"].PutValue(_languageService.GetLocalizedHtmlString("inventory_status"));


            ws.Cells["A4"].PutValue(_languageService.GetLocalizedHtmlString("match"));
            ws.Cells["F4"].PutValue(_languageService.GetLocalizedHtmlString("wrong_position"));
            ws.Cells["K4"].PutValue(_languageService.GetLocalizedHtmlString("not_scan"));
            ws.Cells["B4"].PutValue(history.CountComplete);
            ws.Cells["G4"].PutValue(history.CountWrongPosition);
            ws.Cells["M4"].PutValue(history.CountNotScan);
        }

        public void CustomStyle1(ref Cell cellCustom)
        {
            string value = Convert.ToString(cellCustom.Value);
            if (value == "1")
            {
                cellCustom.PutValue(_languageService.GetLocalizedHtmlString("match"));
                Style styleCustom = cellCustom.GetStyle();
                styleCustom.Font.Color = Color.Green;
                styleCustom.Font.IsBold = true;
                cellCustom.SetStyle(styleCustom);
            }
            else if (value == "-1")
            {
                cellCustom.PutValue(_languageService.GetLocalizedHtmlString("wrong_position"));
                Style styleCustom = cellCustom.GetStyle();
                styleCustom.Font.Color = Color.Orange;
                styleCustom.Font.IsBold = true;
                cellCustom.SetStyle(styleCustom);
            }
            else if (value == "0")
            {
                cellCustom.PutValue(_languageService.GetLocalizedHtmlString("not_scan"));
                Style styleCustom = cellCustom.GetStyle();
                styleCustom.Font.Color = Color.Red;
                styleCustom.Font.IsBold = true;
                cellCustom.SetStyle(styleCustom);
            }
        }

        public async Task<ResultAllInventoryDto> GetAllDetailHistoryPlno(ReportKiemKeParam param)
        {
            var listResult = await _repository.hp_a04
                .FindAll(x => x.Visible.Value).GroupJoin(
                    _repository.hp_a15.FindAll(),
                    x => new { Plno = x.Plno.Trim() }, y => new { Plno = y.Plno.Trim() },
                    (x, y) => new { HP_A04 = x, HP_A15s = y })
                .SelectMany(x => x.HP_A15s.DefaultIfEmpty(), (x, z) => new { x.HP_A04, HP_A15 = z })
                .Select(x => new DetaiHistoryInventoryDto
                {
                    MachineID = x.HP_A04.OwnerFty.Trim() + x.HP_A04.AssnoID.Trim(),
                    MachineName = param.Lang == "vi-VN" ? x.HP_A04.MachineName_Local : (param.Lang == "zh-TW" ? x.HP_A04.MachineName_CN : x.HP_A04.MachineName_EN),
                    Supplier = x.HP_A04.Supplier.Trim(),
                    Place = x.HP_A15.Place.Trim(),
                    State = x.HP_A04.State.Trim(),
                    Plno = x.HP_A04.Plno.Trim(),
                }).ToListAsync();

            var machineList = listResult.Select(x => x.MachineID.Trim()).Distinct().ToList();

            var predicateHis = PredicateBuilder.New<HistoryInventory>(true);
            // date range
            if (!string.IsNullOrEmpty(param.FromDate) && !string.IsNullOrEmpty(param.ToDate))
            {
                var startTime = Convert.ToDateTime(param.FromDate + " 00:00:00");
                var endTime = Convert.ToDateTime(param.ToDate + " 23:59:59");
                predicateHis.And(x => x.CreateTime >= startTime && x.CreateTime <= endTime);
            }
            var inventoryQuery = await _repository.HistoryInventory.FindAll(predicateHis)
                .GroupJoin(_repository.DataHistoryInventory.FindAll(x => machineList.Contains(x.MachineID)),
                    x => x.HistoryInventoryID, y => y.HistoryInventoryID,
                    (x, y) => new { T1 = x, T2 = y }
                ).SelectMany(x => x.T2.DefaultIfEmpty(), (x, y) => new { x.T1, T2 = y })
                .OrderByDescending(x => x.T1.CreateTime)
                .ToListAsync();

            foreach (var item in listResult)
            {
                var itemSoKiem = inventoryQuery.FirstOrDefault(x => x.T2?.MachineID.Trim() == item.MachineID.Trim() && x.T1.InventoryType == 1);
                var itemPhucKiem = inventoryQuery.FirstOrDefault(x => x.T2?.MachineID.Trim() == item.MachineID.Trim() && x.T1.InventoryType == 2);
                var itemRutKiem = inventoryQuery.FirstOrDefault(x => x.T2?.MachineID.Trim() == item.MachineID.Trim() && x.T1.InventoryType == 3);

                if (itemSoKiem != null)
                {
                    item.StatusSoKiem = itemSoKiem.T2.StatusInventory.Value;
                    item.ScanPlace = itemSoKiem.T2.Place;
                }

                if (itemPhucKiem != null)
                {
                    item.StatusPhucKiem = itemPhucKiem.T2.StatusInventory.Value;
                    item.ScanPlace = itemPhucKiem.T2.Place;
                }

                if (itemRutKiem != null)
                {
                    item.StatusRutKiem = itemRutKiem.T2.StatusInventory.Value;
                    item.ScanPlace = itemRutKiem.T2.Place;
                }
            }

            var dataMachineSoKiem = listResult.Where(x => x.StatusSoKiem != null).ToList();
            var dataMachinePhucKiem = listResult.Where(x => x.StatusPhucKiem != null).ToList();
            var dataMachineRutKiem = listResult.Where(x => x.StatusRutKiem != null).ToList();

            var listDetail = new List<DetailInventoryDto>
            {
                new DetailInventoryDto
                {
                    TypeInventory = 1,
                    CountMachine = dataMachineSoKiem.Count,
                    CountMatch = dataMachineSoKiem.Count(x => x.StatusSoKiem == 1),
                    CountNotScan = dataMachineSoKiem.Count(x => x.StatusSoKiem == 0),
                    CountWrongPosition = dataMachineSoKiem.Count(x => x.StatusSoKiem == -1),
                },
                new DetailInventoryDto
                {
                    TypeInventory = 2,
                    CountMachine = dataMachinePhucKiem.Count,
                    CountMatch = dataMachinePhucKiem.Count(x => x.StatusPhucKiem == 1),
                    CountNotScan = dataMachinePhucKiem.Count(x => x.StatusPhucKiem == 0),
                    CountWrongPosition = dataMachinePhucKiem.Count(x => x.StatusPhucKiem == -1),
                },
                new DetailInventoryDto
                {
                    TypeInventory = 3,
                    CountMachine = dataMachineRutKiem.Count,
                    CountMatch = dataMachineRutKiem.Count(x => x.StatusRutKiem == 1),
                    CountNotScan = dataMachineRutKiem.Count(x => x.StatusRutKiem == 0),
                    CountWrongPosition = dataMachineRutKiem.Count(x => x.StatusRutKiem == -1),
                }
            };

            var result = new ResultAllInventoryDto
            {
                ListResult = listResult,
                ListDetail = listDetail
            };

            return result;
        }
    }
}
