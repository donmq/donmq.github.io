using Machine_API.Models.MachineCheckList;
using AutoMapper;
using Machine_API.DTO;

namespace Machine_API.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<DataHistoryCheckMachineDto, DataHistoryCheckMachine>();
            CreateMap<DataHistoryInventoryDto, DataHistoryInventory>();
            CreateMap<Hp_a15Dto, hp_a15>();
            CreateMap<HistoryCheckMachineDto, HistoryCheckMachine>();
            CreateMap<HistoryDto, History>();
            CreateMap<PdcDto, PDC>();
            CreateMap<BuildingDto, Building>();
            CreateMap<CellDto, Cells>();
            CreateMap<EmployeeDto, Employee>();
            CreateMap<Cell_PlnoDto, Cell_Plno>();
            CreateMap<DateInventoryDto, DateInventory>();
            CreateMap<RolesDto, Roles>();
        }
    }
}