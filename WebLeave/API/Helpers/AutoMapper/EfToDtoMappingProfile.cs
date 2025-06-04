using API.Dtos.Manage.CompanyManage;
using API.Dtos.Manage.PositionManage;
using API.Dtos.Manage;
using API.Dtos.Manage.GroupBaseManage;
using API.Dtos.Manage.HolidayManage;
using API.Dtos.Manage.DatepickerManagement;
using API.Dtos.Auth;
using API.Dtos.Report;
using API.Dtos.Common;
using API.Dtos.Manage.TeamManagement;
using API.Dtos.Leave.LeaveApprove;
using API.Dtos.Manage.EmployeeManage;
using API.Dtos.Manage.UserManage;
using API.Models;
using AutoMapper;
using API.Dtos;
using API.Dtos.Leave;

namespace API.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            //Manage
            CreateMap<Position, PositionManageDto>();
            CreateMap<PosLang, PosLangManageDto>();
            CreateMap<Company, CompanyManageDto>();
            CreateMap<GroupBase, GroupBaseDto>();
            CreateMap<GroupLang, GroupLangDto>();
            CreateMap<Holiday, HolidayDto>();
            CreateMap<Holiday, UserHolidayDto>();
            CreateMap<Department, DepartmentDto>();
            CreateMap<Area, AreaDto>();
            CreateMap<Building, BuildingDto>();
            CreateMap<DatePickerManager, DatepickerDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<HistoryEmp, Dtos.Common.HistoryEmpDto>();
            CreateMap<HistoryEmp, Dtos.Manage.EmployeeManage.HistoryEmpDto>();
            CreateMap<LeaveData, Dtos.Manage.EmployeeManage.LeaveDataDto>();
            CreateMap<Users, UsersDto>();
            CreateMap<CommentArchive, CommentArchiveDto>();
            CreateMap<LeaveData, Dtos.Common.LeaveDataDto>();
            CreateMap<LeaveData, Dtos.Leave.LeaveDataDto>();
            CreateMap<Part, PartDto>();
            CreateMap<LeaveData, LeaveDataApproveDto>();
            CreateMap<ReportData, ReportDataDto>();
            CreateMap<LeaveData, LeaveDataViewDto>();
            CreateMap<LeaveData, LeaveDataViewModel>();
            CreateMap<Users, UserForDetailDto>();
        }
    }
}