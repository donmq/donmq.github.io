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
using API.Dtos;
using API.Dtos.Leave;
using API.Dtos.Manage.EmployeeManage;
using API.Dtos.Manage.UserManage;
using API.Models;
using AutoMapper;

namespace API.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            //Manage
            CreateMap<PositionManageDto, Position>();
            CreateMap<PosLangManageDto, PosLang>();
            CreateMap<CompanyManageDto, Company>();
            CreateMap<GroupBaseDto, GroupBase>();
            CreateMap<GroupLangDto, GroupLang>();
            CreateMap<HolidayDto, Holiday>();
            CreateMap<UserHolidayDto, Holiday>();
            CreateMap<DepartmentDto, Department>();
            CreateMap<AreaDto, Area>();
            CreateMap<BuildingDto, Building>();
            CreateMap<DatepickerDto, DatePickerManager>();
            CreateMap<EmployeeDto, Employee>();
            CreateMap<Dtos.Common.HistoryEmpDto, HistoryEmp>();
            CreateMap<Dtos.Manage.EmployeeManage.LeaveDataDto, LeaveData>();
            CreateMap<UsersDto, Users>();
            CreateMap<CommentArchiveDto, CommentArchive>();
            CreateMap<Dtos.Manage.EmployeeManage.HistoryEmpDto, HistoryEmp>();
            CreateMap<Dtos.Common.LeaveDataDto, LeaveData>();
            CreateMap<Dtos.Leave.LeaveDataDto, LeaveData>();
            CreateMap<PartDto, Part>();
            CreateMap<LeaveDataApproveDto, LeaveData>();
            CreateMap<ReportDataDto, ReportData>();
            CreateMap<LeaveDataDTO, LeaveData>();
            CreateMap<ReportDataDto, ReportData>();
            CreateMap<LeaveDataViewDto, LeaveData>();
            CreateMap<UserForDetailDto, Users>();
        }
    }
}