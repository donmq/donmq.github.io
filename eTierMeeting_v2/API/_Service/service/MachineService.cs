using AutoMapper;
using Machine_API._Accessor;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Utilities;
using Machine_API.Models.MachineCheckList;
using Machine_API.Resources;
using Microsoft.EntityFrameworkCore;

namespace Machine_API._Service.service
{
    public class MachineService : IMachineService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly LocalizationService _languageService;
        private readonly string _OwnerFty;

        public MachineService(
            IMachineRepositoryAccessor repository, 
            LocalizationService languageService, 
            IConfiguration configuration)
        {
            _repository = repository;
            _languageService = languageService;
            _OwnerFty = configuration.GetSection("AppSettings:OwnerFty").Value;
        }

        public async Task<object> GetMachineByID(string idMachine, string lang)
        {
            if (string.IsNullOrWhiteSpace(idMachine) || idMachine.Length <= 1)
                return null;

            string ownfty = idMachine.Trim()[..1];
            string assnoID = idMachine.Trim()[1..];

            var dataMachine = await _repository.hp_a04.FindAll(x => x.AssnoID.Trim() == assnoID.Trim() && x.OwnerFty.Trim() == ownfty.Trim() && x.Visible == true)
                .Select(x => new MachineScanDto
                {
                    machineID = x.AssnoID.Trim(),
                    machineName = lang == "vi-VN" ? x.MachineName_Local : (lang == "zh-TW" ? x.MachineName_CN : x.MachineName_EN),
                    state = x.State.Trim(),
                    plno = x.Plno.Trim(),
                    ownerFty = x.OwnerFty.Trim(),
                }).FirstOrDefaultAsync();

            if (dataMachine != null)
                dataMachine.place = _repository.hp_a15.FindAll(z => z.Plno == dataMachine.plno).Select(z => z.Place.Trim()).FirstOrDefault();

            return dataMachine;
        }

        public async Task<object> MoveMachine(string fromEmploy, string idMachine, string toEmploy, string fromPlno, string toPlno, string userID)
        {
            if (string.IsNullOrWhiteSpace(idMachine) || idMachine.Length <= 1)
                return new { status = false, message = _languageService.GetLocalizedHtmlString("machineid_invalid") };

            string ownerFty = idMachine.Trim()[..1];
            string assnoID = idMachine.Trim()[1..];

            #region Dữ liệu người quản lý máy cũ
            var datafromEmploy = await _repository.EmployPlno.FirstOrDefaultAsync(x => x.EmpNumber == fromEmploy && x.Plno == fromPlno);
            //Cell ID cũ
            var cellOldItem = await _repository.Cell_Plno.FirstOrDefaultAsync(x => x.Plno == datafromEmploy.Plno);

            if (cellOldItem == null)
                return new { status = false, message = _languageService.GetLocalizedHtmlString("management_location_does_not_exist") };

            //Tên Cell cũ
            var nameCellOldItem = await _repository.Cells.FirstOrDefaultAsync(x => x.CellID == cellOldItem.CellID.ToInt());
            string nameCellOld = nameCellOldItem != null ? nameCellOldItem.CellCode : string.Empty;
            #endregion

            #region Dữ liệu người quản lý máy mới
            var dataToEmploy = await _repository.EmployPlno.FirstOrDefaultAsync(x => x.EmpNumber == toEmploy && x.Plno == toPlno);
            //Cell ID mới
            var cellNewItem = await _repository.Cell_Plno.FirstOrDefaultAsync(x => x.Plno == dataToEmploy.Plno);

            if (cellNewItem == null)
                return new { status = false, message = _languageService.GetLocalizedHtmlString("dept_does_not_exist") };

            //Tên Cell mới
            var nameCellNewItem = await _repository.Cells.FirstOrDefaultAsync(x => x.CellID == cellNewItem.CellID.ToInt());
            string nameCellNew = nameCellNewItem != null ? nameCellNewItem.CellCode : string.Empty;
            #endregion

            var itemMachine = await _repository.hp_a04.FirstOrDefaultAsync(x => x.AssnoID == assnoID && x.OwnerFty == ownerFty && x.Visible == true && x.Plno != toPlno);
            if (itemMachine == null)
                return new { status = false, message = _languageService.GetLocalizedHtmlString("management_machine_does_not_exist") };

            itemMachine.Plno = toPlno;
            if (ownerFty == _OwnerFty)
            {
                var data_hp_a15 = await _repository.hp_a15.FirstOrDefaultAsync(x => x.Plno.Trim() == toPlno.Trim());
                if (data_hp_a15 != null && !string.IsNullOrWhiteSpace(data_hp_a15.State))
                    itemMachine.State = data_hp_a15.State;
            }
            itemMachine.Is_Update_To_SAP = false;

            _repository.hp_a04.Update(itemMachine);

            _repository.History.Add(new History()
            {
                assnoID = assnoID, // Mã máy

                Position_Old = fromPlno, // vị trí cũ
                Position_New = toPlno,// Tới vị trí mới

                Cell_Old = nameCellOld, // ô cũ
                Cell_New = nameCellNew, // ô mới

                EmpNumber_Old = fromEmploy, // nhân viên quản lí máy (cũ)
                EmpNumber_New = toEmploy, // nhân viên quản lí máy (mới)

                UserID = userID,
                OwnerFty = ownerFty,
                Update_Date = DateTime.Now
            });

            try
            {
                await _repository.SaveChangesAsync();
                return new { status = true };
            }
            catch (Exception ex)
            {
                ErrorLog err = new()
                {
                    LogType = "move_machine",
                    Content = ex.Message,
                    DateLog = DateTime.Now
                };

                _repository.ErrorLog.Add(err);
                await _repository.SaveChangesAsync();
                return new { status = false };
            }
        }

        public string GetMachineName(string idMachine, string lang)
        {
            if (string.IsNullOrWhiteSpace(idMachine) || idMachine.Length <= 1)
                return string.Empty;

            string ownfty = idMachine.Trim()[..1];
            string assnoID = idMachine.Trim()[1..];

            var machine = _repository.hp_a04.FindAll(x => x.AssnoID == assnoID && x.OwnerFty == ownfty && x.Visible == true)
                                .Select(x => new
                                {
                                    machineName = lang == "vi-VN" ? x.MachineName_Local : (lang == "zh-TW" ? x.MachineName_CN : x.MachineName_EN)
                                })
                                .FirstOrDefault();

            return machine == null ? "" : machine.machineName;
        }
    }
}