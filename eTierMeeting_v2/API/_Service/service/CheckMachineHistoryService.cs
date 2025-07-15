
using System.Drawing;
using Aspose.Cells;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinqKit;
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
    public class CheckMachineHistoryService : ICheckMachineHistoryService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly LocalizationService _languageService;

        public CheckMachineHistoryService(
            IMachineRepositoryAccessor repository,
            MapperConfiguration mapperConfiguration,
            LocalizationService languageService)
        {
            _repository = repository;
            _mapperConfiguration = mapperConfiguration;
            _languageService = languageService;
        }

        public async Task<List<DataHistoryCheckMachineDto>> GetDetailHistoryCheckMachine(int historyCheckMachineID)
        {
            return await _repository.DataHistoryCheckMachine.FindAll(x => x.HistoryCheckMachineID == historyCheckMachineID).ProjectTo<DataHistoryCheckMachineDto>(_mapperConfiguration).ToListAsync();
        }

        public async Task<PageListUtility<HistoryCheckMachineDto>> SearchHistoryCheckMachine(CheckMachineHisstoryParams checkMachineHisstoryParams, PaginationParams paginationParams)
        {
            var predicateUser = PredicateBuilder.New<User>(true);
            if (!string.IsNullOrEmpty(checkMachineHisstoryParams.UserName))
                predicateUser.And(x => x.UserName.ToLower().Contains(checkMachineHisstoryParams.UserName.ToLower()));

            //Search by date
            DateTime date1 = Convert.ToDateTime(checkMachineHisstoryParams.FromDateTime + " 00:00");
            DateTime date2 = Convert.ToDateTime(checkMachineHisstoryParams.ToDateTime + " 23:59");

            var predicateHis = PredicateBuilder.New<HistoryCheckMachine>(true);
            if (!string.IsNullOrEmpty(checkMachineHisstoryParams.FromDateTime) && string.IsNullOrEmpty(checkMachineHisstoryParams.ToDateTime))
            {
                predicateHis.And(x => x.CreateTime >= date1);
            }
            else if (string.IsNullOrEmpty(checkMachineHisstoryParams.FromDateTime) && !string.IsNullOrEmpty(checkMachineHisstoryParams.ToDateTime))
            {
                predicateHis.And(x => x.CreateTime <= date2);
            }
            else if (!string.IsNullOrEmpty(checkMachineHisstoryParams.FromDateTime) && !string.IsNullOrEmpty(checkMachineHisstoryParams.ToDateTime))
            {
                predicateHis.And(x => x.CreateTime >= date1 && x.CreateTime <= date2);
            }

            var historyCheckMachine = _repository.HistoryCheckMachine.FindAll(predicateHis);
            var user = _repository.User.FindAll(predicateUser);
            var dataJoin = historyCheckMachine.Join(user, x => x.CreateBy, y => y.UserName, (x, y)
            => new HistoryCheckMachineDto
            {
                HistoryCheckMachineID = x.HistoryCheckMachineID,
                TotalScans = x.TotalScans,
                TotalNotScan = x.TotalNotScan,
                TotalMachine = x.TotalMachine,
                TotalExist = x.TotalExist,
                TotalNotExist = x.TotalNotExist,
                UserName = y.UserName,
                CreateBy = y.EmpName,
                CreateTime = x.CreateTime,
            });

            var dataResult = await dataJoin.OrderByDescending(x => x.HistoryCheckMachineID).ToListAsync();
            return PageListUtility<HistoryCheckMachineDto>.PageList(dataResult, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public void PutStaticValue(ref Worksheet ws, HistoryCheckMachineDto data)
        {
            var historyCheckMachine = data;

            //-title table
            ws.Cells["A7"].PutValue(_languageService.GetLocalizedHtmlString("machine_code"));
            ws.Cells["B7"].PutValue(_languageService.GetLocalizedHtmlString("machine_name"));
            ws.Cells["C7"].PutValue(_languageService.GetLocalizedHtmlString("suppplier"));
            ws.Cells["D7"].PutValue(_languageService.GetLocalizedHtmlString("location"));
            ws.Cells["E7"].PutValue(_languageService.GetLocalizedHtmlString("state"));
            ws.Cells["F7"].PutValue(_languageService.GetLocalizedHtmlString("inventory_status"));

            //Set lang header
            ws.Cells["A3"].PutValue(_languageService.GetLocalizedHtmlString("totalMachine") + " : " + data.TotalMachine);
            ws.Cells["C3"].PutValue(_languageService.GetLocalizedHtmlString("total_scanned_machine") + " : " + data.TotalScans);
            ws.Cells["E3"].PutValue(_languageService.GetLocalizedHtmlString("qtyNotScan") + " : " + data.TotalNotScan);
            ws.Cells["A4"].PutValue(_languageService.GetLocalizedHtmlString("total_machine_found") + " : " + data.TotalExist);
            ws.Cells["C4"].PutValue(_languageService.GetLocalizedHtmlString("total_machine_not_found") + " : " + data.TotalNotExist);
            ws.Cells["E4"].PutValue(_languageService.GetLocalizedHtmlString("date") + " : " + data.CreateTime != null ? data.CreateTime.Value.ToString("yyyy/MM/dd | HH:mm:ss") : null);
            ws.Cells["A5"].PutValue(_languageService.GetLocalizedHtmlString("employee_code") + " : " + data.UserName);
            ws.Cells["C5"].PutValue(_languageService.GetLocalizedHtmlString("employee_name") + " : " + data.CreateBy);
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