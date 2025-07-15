using System.Drawing;
using Aspose.Cells;
using Machine_API._Accessor;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Machine_API.Models.MachineCheckList;
using Machine_API.Resources;
using Microsoft.EntityFrameworkCore;

namespace Machine_API._Service.service
{
    public class InventoryService : IInventoryService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly LocalizationService _languageService;

        public InventoryService(IMachineRepositoryAccessor repository, LocalizationService languageService)
        {
            _repository = repository;
            _languageService = languageService;
        }

        public async Task<List<SearchMachineInventoryDto>> GetAllMachineByPlno(string plnoID)
        {
            var hp_a04 = _repository.hp_a04.FindAll(x => x.Plno == plnoID && x.Visible == true);
            var hp_a15 = _repository.hp_a15.FindAll();

            var dataJoin = await hp_a04.GroupJoin(hp_a15, x => x.Plno, y => y.Plno, (x, y) => new
            {
                hpa04 = x,
                hpa15 = y
            }).SelectMany(x => x.hpa15.DefaultIfEmpty(), (x, z) => new SearchMachineInventoryDto
            {
                MachineID = x.hpa04.AssnoID,
                OwnerFty = x.hpa04.OwnerFty,
                MachineName = x.hpa04.MachineName_CN,
                Status = x.hpa04.State,
                Supplier = x.hpa04.Supplier,
                PlaceName = z.Place,
                PlnoName = z.Plno,
                TrDate = x.hpa04.Trdate
            }).ToListAsync();
            return dataJoin;
        }

        public async Task<SearchMachineInventoryDto> GetMachine(string idMachine, string lang)
        {
            string ownfty = idMachine.Substring(0, 1);
            string idMachineSplit = idMachine.Substring(1, idMachine.Length - 1);

            var hp_a04 = _repository.hp_a04.FindAll(x => x.OwnerFty == ownfty && x.AssnoID == idMachineSplit && x.Visible == true);
            var hp_a15 = _repository.hp_a15.FindAll();

            var dataJoin = await hp_a04.GroupJoin(hp_a15, x => x.Plno, y => y.Plno, (x, y) => new
            {
                hpa04 = x,
                hpa15 = y
            }).SelectMany(x => x.hpa15.DefaultIfEmpty(), (x, z) => new SearchMachineInventoryDto
            {
                MachineID = x.hpa04.AssnoID,
                OwnerFty = x.hpa04.OwnerFty,
                MachineName = lang == "vi-VN" || lang == "id-ID" ? x.hpa04.MachineName_Local : (lang == "zh-TW" ? x.hpa04.MachineName_CN : x.hpa04.MachineName_EN),
                Status = x.hpa04.State,
                Supplier = x.hpa04.Supplier,
                PlaceName = z.Place,
                Plno = z.Plno,
                TrDate = x.hpa04.Trdate
            }).FirstOrDefaultAsync();

            SearchMachineInventoryDto result = new SearchMachineInventoryDto(idMachineSplit, ownfty);
            if (dataJoin != null)
            {
                dataJoin.IsNull = false;
                result = dataJoin;
            }
            return result;
        }

        public async Task<ResultHistoryInventoryDto> SubmitInventory(InventoryParams inventoryParams, string userName, string empName)
        {
            int error = 0;
            var listDataMachine = await GetAllMachineByPlno(inventoryParams.IdPlno);
            List<SearchMachineInventoryDto> listInventory = new List<SearchMachineInventoryDto>();
            List<ErrorLog> listError = new List<ErrorLog>();

            foreach (var itemMachine in inventoryParams.listMachineInventory)
            {
                SearchMachineInventoryDto item = new SearchMachineInventoryDto();
                item.MachineID = itemMachine.MachineID;
                item.MachineName = itemMachine.MachineName;
                item.OwnerFty = itemMachine.OwnerFty;
                item.PlnoName = itemMachine.Plno;
                item.PlaceName = itemMachine.PlaceName;
                item.Status = itemMachine.Status;
                item.Supplier = itemMachine.Supplier;
                item.TrDate = itemMachine.TrDate;

                var checkExistMachine = listDataMachine.FirstOrDefault(x => x.MachineID == itemMachine.MachineID);
                if (checkExistMachine != null)
                {
                    item.StatusIventory = 1;//Trạng thái đúng kiểm kê không sai vị trí
                    listDataMachine.Remove(checkExistMachine);
                }
                else
                {
                    item.StatusIventory = -1;//Trạng thái sai vị trí
                }
                listInventory.Add(item);//Add lại
            }

            if (inventoryParams.IdInventory != 3)
            {
                foreach (var itemMachine in listDataMachine)
                {
                    SearchMachineInventoryDto item = new SearchMachineInventoryDto();
                    item.MachineID = itemMachine.MachineID;
                    item.MachineName = itemMachine.MachineName;
                    item.OwnerFty = itemMachine.OwnerFty;
                    item.PlnoName = itemMachine.PlnoName;
                    item.PlaceName = itemMachine.PlaceName.Trim();
                    item.Status = itemMachine.Status;
                    item.Supplier = itemMachine.Supplier;
                    item.TrDate = itemMachine.TrDate;
                    item.StatusIventory = 0; //Trạng thái không được quét

                    listInventory.Add(item);
                }
            }

            int countSuccess = listInventory.Count(x => x.StatusIventory == 1);
            int countNotScan = listInventory.Count(x => x.StatusIventory == 0);
            int countWrongPosition = listInventory.Count(x => x.StatusIventory == -1);

            HistoryInventory historyInventory = new HistoryInventory();
            historyInventory.InventoryType = inventoryParams.IdInventory;
            historyInventory.PlnoID = inventoryParams.IdPlno;
            historyInventory.CountComplete = countSuccess;
            historyInventory.CountNotScan = countNotScan;
            historyInventory.CountWrongPosition = countWrongPosition;
            historyInventory.CreateBy = userName;
            historyInventory.StartTimeInventory = Convert.ToDateTime(inventoryParams.FromDateTime);
            historyInventory.EndTimeInventory = Convert.ToDateTime(inventoryParams.ToDateTime);
            historyInventory.CreateTime = DateTime.Now;
            _repository.HistoryInventory.Add(historyInventory);

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorLog err = new ErrorLog
                {
                    LogType = "inventory",
                    Content = ex.Message,
                    DateLog = DateTime.Now
                };
                listError.Add(err);

                error = 1;
            }

            List<DataHistoryInventory> listDataInventory = new List<DataHistoryInventory>();
            foreach (var item in listInventory)
            {
                //Save data inventory to database
                DataHistoryInventory dataInventory = new DataHistoryInventory();
                dataInventory.MachineID = item.OwnerFty + item.MachineID;
                dataInventory.MachineName = item.MachineName;
                dataInventory.Supplier = item.Supplier;
                dataInventory.Place = item.PlaceName;
                dataInventory.State = item.Status;
                dataInventory.StatusInventory = item.StatusIventory;
                dataInventory.HistoryInventoryID = historyInventory.HistoryInventoryID;
                listDataInventory.Add(dataInventory);
            }
            _repository.DataHistoryInventory.AddMultiple(listDataInventory);
            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorLog err = new ErrorLog
                {
                    LogType = "inventory",
                    Content = ex.Message,
                    DateLog = DateTime.Now
                };
                listError.Add(err);

                error = 1;
            }
            // var dataErr = _mapper.Map<ErrorLog>(listError);
            _repository.ErrorLog.AddMultiple(listError);
            await _repository.SaveChangesAsync();

            ResultHistoryInventoryDto result = new ResultHistoryInventoryDto
            {
                HistoryInventoryID = historyInventory.HistoryInventoryID,
                CountSuccess = countSuccess,
                CountNotScan = countNotScan,
                CountWrongPosition = countWrongPosition,
                StartTimeInventory = historyInventory.StartTimeInventory,
                EndTimeInventory = historyInventory.EndTimeInventory,
                ListInventory = listInventory,
                InventoryID = inventoryParams.IdInventory,
                Place = _repository.hp_a15.FindAll(x => x.Plno == inventoryParams.IdPlno).FirstOrDefault().Place,
                Error = error
            };

            return result;
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
                cellCustom.PutValue(_languageService.GetLocalizedHtmlString("do_not_scan"));
                Style styleCustom = cellCustom.GetStyle();
                styleCustom.Font.Color = Color.Red;
                styleCustom.Font.IsBold = true;
                cellCustom.SetStyle(styleCustom);
            }
        }
        public void PutStaticValue(ref Worksheet ws, ResultHistoryInventoryDto data, string userName, string empName)
        {
            var historyCheckMachine = data;
            //-title table
            ws.Cells["A8"].PutValue(_languageService.GetLocalizedHtmlString("machine_code"));
            ws.Cells["B8"].PutValue(_languageService.GetLocalizedHtmlString("machine_name"));
            ws.Cells["C8"].PutValue(_languageService.GetLocalizedHtmlString("suppplier"));
            ws.Cells["D8"].PutValue(_languageService.GetLocalizedHtmlString("location"));
            ws.Cells["E8"].PutValue(_languageService.GetLocalizedHtmlString("state"));
            ws.Cells["F8"].PutValue(_languageService.GetLocalizedHtmlString("inventory_status"));

            string InventoryType = data.InventoryID == 1 ? _languageService.GetLocalizedHtmlString("sokiem") : data.InventoryID == 2 ? _languageService.GetLocalizedHtmlString("phuc_kiem") : _languageService.GetLocalizedHtmlString("rut_kiem");
            //Set lang header
            ws.Cells["A3"].PutValue(_languageService.GetLocalizedHtmlString("inventory_type") + " : " + InventoryType);
            ws.Cells["A4"].PutValue(_languageService.GetLocalizedHtmlString("employee_code") + " : " + userName);
            ws.Cells["A5"].PutValue(_languageService.GetLocalizedHtmlString("match") + " : " + data.CountSuccess);
            ws.Cells["A6"].PutValue(_languageService.GetLocalizedHtmlString("startTimeInventory") + " : " + data.StartTimeInventory != null ? (data.StartTimeInventory).ToString("HH:mm:ss") : null);

            ws.Cells["C3"].PutValue(_languageService.GetLocalizedHtmlString("inventory_location") + " : " + data.Place);
            ws.Cells["C4"].PutValue(_languageService.GetLocalizedHtmlString("employee_name") + " : " + empName);
            ws.Cells["C5"].PutValue(_languageService.GetLocalizedHtmlString("wrong_position") + " : " + data.CountWrongPosition);
            ws.Cells["C6"].PutValue(_languageService.GetLocalizedHtmlString("endTimeInventory") + " : " + data.EndTimeInventory != null ? (data.EndTimeInventory).ToString("HH:mm:ss") : null);

            ws.Cells["E3"].PutValue(_languageService.GetLocalizedHtmlString("date") + " : " + data.StartTimeInventory != null ? (data.StartTimeInventory).ToString("yyyy/MM/dd") : null);
            ws.Cells["E4"].PutValue(_languageService.GetLocalizedHtmlString("total") + " : " + (data.CountSuccess + data.CountNotScan));
            ws.Cells["E5"].PutValue(_languageService.GetLocalizedHtmlString("do_not_scan") + " : " + data.CountNotScan);
        }
    }
}