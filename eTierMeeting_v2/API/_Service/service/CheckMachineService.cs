
using AutoMapper;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Microsoft.EntityFrameworkCore;
using Machine_API.Models.MachineCheckList;
using System.Drawing;
using Aspose.Cells;
using Machine_API.Resources;
using Machine_API._Accessor;

namespace Machine_API._Service.service
{
    public class CheckMachineService : ICheckMachineService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly IMapper _mapper;
        private readonly LocalizationService _languageService;

        public CheckMachineService(
            IMachineRepositoryAccessor repository, 
            IMapper mapper,
            LocalizationService languageService)
        {
            _repository = repository;
            _mapper = mapper;
            _languageService = languageService;
        }

        public async Task<List<CheckMachineDto>> GetAllMachine()
        {
            var hpa04 = _repository.hp_a04.FindAll(x => x.Visible == true);
            var hpa15 = _repository.hp_a15.FindAll();

            var dataJoin = hpa04.Join(hpa15, x => x.Plno, y => y.Plno, (x, y)
                => new CheckMachineDto()
                {
                    MachineID = x.AssnoID,
                    OwnerFty = x.OwnerFty,
                    MachineName = x.MachineName_CN,
                    Status = x.State,
                    Supplier = x.Supplier,
                    PlaceName = y.Place,
                    PlnoName = y.Plno,
                    TrDate = x.Trdate
                }).ToListAsync();
            return await dataJoin;
        }

        public async Task<object> GetMachine(string idMachine, string lang)
        {
            //Kiểm tra nếu có dấu '/' trong mã máy
            if (idMachine.Contains('/'))
            {
                idMachine = idMachine.Replace(@"/", "");
            }
            string ownfty = idMachine[..1];
            string idMachineSplit = idMachine[1..];

            var hpa04 = _repository.hp_a04.FindAll(x => x.Visible == true && x.AssnoID == idMachineSplit && x.OwnerFty == ownfty);
            var hpa15 = _repository.hp_a15.FindAll();

            var dataJoin = await hpa04.Join(hpa15, x => x.Plno, y => y.Plno, (x, y)
                => new CheckMachineDto()
                {
                    MachineID = x.AssnoID,
                    OwnerFty = x.OwnerFty,
                    MachineName = lang == "vi-VN" ? x.MachineName_Local : (lang == "zh-TW" ? x.MachineName_CN : x.MachineName_EN),
                    Status = x.State,
                    Supplier = x.Supplier,
                    PlaceName = y.Place,
                    PlnoName = y.Plno,
                    TrDate = x.Trdate
                }).FirstOrDefaultAsync();

            CheckMachineDto result = new(idMachineSplit, ownfty);
            if (dataJoin != null)
            {
                dataJoin.IsNull = false;
                result = dataJoin;
            }
            return result;
        }

        public async Task<ResultHistoryCheckMachineDto> SubmitCheckMachine(List<CheckMachineDto> listModel, string userName, string empName)
        {
            var listDataMachine = await GetAllMachine();
            List<CheckMachineDto> listCheckMachine = new();

            foreach (var itemMachine in listModel)
            {
                CheckMachineDto item = new()
                {
                    MachineID = itemMachine.MachineID,
                    MachineName = itemMachine.MachineName,
                    OwnerFty = itemMachine.OwnerFty,
                    PlnoName = itemMachine.PlnoName,
                    PlaceName = itemMachine.PlaceName,
                    Status = itemMachine.Status,
                    Supplier = itemMachine.Supplier,
                    TrDate = itemMachine.TrDate
                };

                var checkExistMachine = listDataMachine.FirstOrDefault(x => x.MachineID == itemMachine.MachineID);
                if (checkExistMachine != null)
                {
                    item.StatusCheckMachine = 1;//Trạng thái đúng
                    listDataMachine.Remove(checkExistMachine);
                }
                else
                {
                    item.StatusCheckMachine = -1;//Trạng thái sai
                }
                listCheckMachine.Add(item);//Add lại
            }
            int totalScans = listCheckMachine.Count;
            int totalMachine = listCheckMachine.Count;
            int totalNotExist = listCheckMachine.Count(x => x.StatusCheckMachine == -1);
            int totalNotScan = listCheckMachine.Count(x => x.StatusCheckMachine == 0) + totalNotExist;
            int totalExist = listCheckMachine.Count(x => x.StatusCheckMachine == 1);

            //Save to database HistoryCheckMachine
            HistoryCheckMachineDto historyCheckMachine = new ()
            {
                TotalScans = totalScans,
                TotalMachine = totalMachine,
                TotalNotScan = totalNotScan,
                TotalExist = totalExist,
                TotalNotExist = totalNotExist,
                CreateBy = userName,
                CreateTime = DateTime.Now,
            };

            var result = _mapper.Map<HistoryCheckMachine>(historyCheckMachine);
            _repository.HistoryCheckMachine.Add(result);
            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            };

            List<DataHistoryCheckMachine> listDataHistoryCheckMachine = new();
            foreach (var item in listCheckMachine)
            {
                //Save data  DataHistoryCheckMachine  to database
                DataHistoryCheckMachine dataCheckMachine = new ()
                {
                    MachineID = item.OwnerFty + item.MachineID,
                    MachineName = item.MachineName,
                    Supplier = item.Supplier,
                    Place = item.PlaceName,
                    State = item.Status,
                    StatusCheckMachine = item.StatusCheckMachine,
                    HistoryCheckMachineID = result.HistoryCheckMachineID
                };
                listDataHistoryCheckMachine.Add(dataCheckMachine);
            }
            _repository.DataHistoryCheckMachine.AddMultiple(listDataHistoryCheckMachine);
            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw ;
            };

            ResultHistoryCheckMachineDto data = new ()
            {
                listCheckMachine = listCheckMachine,
                historyCheckMachine = historyCheckMachine,
            };
            return data;
        }

        public async Task<ResultHistoryCheckMachineDto> SubmitCheckMachineAll(List<CheckMachineDto> listModel, string userName, string empName)
        {
            int error = 0;
            var listDataMachine = await GetAllMachine();
            List<CheckMachineDto> listCheckMachine = new();

            foreach (var itemMachine in listModel)
            {
                CheckMachineDto item = new ()
                {
                    MachineID = itemMachine.MachineID,
                    MachineName = itemMachine.MachineName,
                    OwnerFty = itemMachine.OwnerFty,
                    PlnoName = itemMachine.PlnoName,
                    PlaceName = itemMachine.PlaceName,
                    Status = itemMachine.Status,
                    Supplier = itemMachine.Supplier,
                    TrDate = itemMachine.TrDate
                };

                var checkExistMachine = listDataMachine.FirstOrDefault(x => x.MachineID == itemMachine.MachineID);
                if (checkExistMachine != null)
                {
                    item.StatusCheckMachine = 1;//Trạng thái đúng
                    listDataMachine.Remove(checkExistMachine);
                }
                else
                {
                    item.StatusCheckMachine = -1;//Trạng thái sai
                }
                listCheckMachine.Add(item);//Add lại
            }

            foreach (var itemMachine in listDataMachine)
            {
                CheckMachineDto item = new()
                {
                    MachineID = itemMachine.MachineID,
                    MachineName = itemMachine.MachineName,
                    OwnerFty = itemMachine.OwnerFty,
                    PlnoName = itemMachine.PlnoName,
                    PlaceName = itemMachine.PlaceName,
                    Status = itemMachine.Status,
                    Supplier = itemMachine.Supplier,
                    TrDate = itemMachine.TrDate,
                    StatusCheckMachine = 0//Trạng thái Không được quet
                };

                listCheckMachine.Add(item);//Add lại
            }
            int totalScans = listCheckMachine.Count;
            int totalMachine = listCheckMachine.Count;
            int totalNotExist = listCheckMachine.Count(x => x.StatusCheckMachine == -1);
            int totalNotScan = listCheckMachine.Count(x => x.StatusCheckMachine == 0) + totalNotExist;
            int totalExist = listCheckMachine.Count(x => x.StatusCheckMachine == 1);

            //Save to database HistoryCheckMachine
            HistoryCheckMachineDto historyCheckMachine = new()
            {
                TotalScans = totalScans,
                TotalMachine = totalMachine,
                TotalNotScan = totalNotScan,
                TotalExist = totalExist,
                TotalNotExist = totalNotExist,
                CreateBy = userName,
                CreateTime = DateTime.Now,
            };

            var result = _mapper.Map<HistoryCheckMachine>(historyCheckMachine);
            _repository.HistoryCheckMachine.Add(result);
            try
            {
                await _repository.SaveChangesAsync();
            }
            catch
            {
                error = 1;
            };

            List<DataHistoryCheckMachine> listDataHistoryCheckMachine = new();
            foreach (var item in listCheckMachine)
            {
                //Save DataCheckMachine to database
                DataHistoryCheckMachine dataCheckMachine = new()
                {
                    MachineID = item.OwnerFty + item.MachineID,
                    MachineName = item.MachineName,
                    Supplier = item.Supplier,
                    Place = item.PlaceName,
                    State = item.Status,
                    StatusCheckMachine = item.StatusCheckMachine,
                    HistoryCheckMachineID = result.HistoryCheckMachineID
                };
                listDataHistoryCheckMachine.Add(dataCheckMachine);
            }
            _repository.DataHistoryCheckMachine.AddMultiple(listDataHistoryCheckMachine);
            try
            {
                await _repository.SaveChangesAsync();
            }
            catch
            {
                error = 1;
            };

            ResultHistoryCheckMachineDto data = new()
            {
                listCheckMachine = listCheckMachine,
                historyCheckMachine = historyCheckMachine,
                error = error
            };
            return data;
        }
        public IEnumerable<DataHistoryCheckMachine> GetListDataHistoryById(int id)
        {
            return _repository.DataHistoryCheckMachine.FindAll(x => x.HistoryCheckMachineID == id).ToList();
        }

        public HistoryCheckMachine GetHistoryCheckMachine(int id)
        {
            return _repository.HistoryCheckMachine.FirstOrDefault(x => x.HistoryCheckMachineID == id);
        }

        public void PutStaticValue(ref Worksheet ws, ResultHistoryCheckMachineDto data, string userName, string empName)
        {
            var historyCheckMachine = data.historyCheckMachine;

            //-title table
            ws.Cells["A7"].PutValue(_languageService.GetLocalizedHtmlString("machine_code"));
            ws.Cells["B7"].PutValue(_languageService.GetLocalizedHtmlString("machine_name"));
            ws.Cells["C7"].PutValue(_languageService.GetLocalizedHtmlString("suppplier"));
            ws.Cells["D7"].PutValue(_languageService.GetLocalizedHtmlString("location"));
            ws.Cells["E7"].PutValue(_languageService.GetLocalizedHtmlString("state"));
            ws.Cells["F7"].PutValue(_languageService.GetLocalizedHtmlString("inventory_status"));

            //Set lang header
            ws.Cells["A3"].PutValue(_languageService.GetLocalizedHtmlString("totalMachine") + " : " + historyCheckMachine.TotalMachine);
            ws.Cells["C3"].PutValue(_languageService.GetLocalizedHtmlString("total_scanned_machine") + " : " + historyCheckMachine.TotalScans);
            ws.Cells["E3"].PutValue(_languageService.GetLocalizedHtmlString("qtyNotScan") + " : " + historyCheckMachine.TotalNotScan);
            ws.Cells["A4"].PutValue(_languageService.GetLocalizedHtmlString("total_machine_found") + " : " + historyCheckMachine.TotalExist);
            ws.Cells["C4"].PutValue(_languageService.GetLocalizedHtmlString("total_machine_not_found") + " : " + historyCheckMachine.TotalNotExist);
            ws.Cells["E4"].PutValue(_languageService.GetLocalizedHtmlString("date") + " : " + historyCheckMachine.CreateTime != null ? historyCheckMachine.CreateTime.Value.ToString("yyyy/MM/dd | HH:mm:ss") : null);
            ws.Cells["A5"].PutValue(_languageService.GetLocalizedHtmlString("employee_code") + " : " + userName);
            ws.Cells["C5"].PutValue(_languageService.GetLocalizedHtmlString("employee_name") + " : " + empName);
        }

        public void CustomStyle(ref Cell cellCustom)
        {
            string value = Convert.ToString(cellCustom.Value);
            if (value == "1")
            {
                cellCustom.PutValue(_languageService.GetLocalizedHtmlString("match_machine"));
                Style styleCustom = cellCustom.GetStyle();
                styleCustom.Font.Color = Color.Green;
                styleCustom.Font.IsBold = true;
                cellCustom.SetStyle(styleCustom);
            }
            else if (value == "-1")
            {
                cellCustom.PutValue(_languageService.GetLocalizedHtmlString("not_found"));
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
    }
}