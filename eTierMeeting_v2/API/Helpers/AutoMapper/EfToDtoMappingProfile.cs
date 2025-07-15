using Machine_API.Models.MachineCheckList;
using AutoMapper;
using Machine_API.DTO;

namespace Machine_API.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<DataHistoryCheckMachine, DataHistoryCheckMachineDto>();
            CreateMap<DataHistoryInventory, DataHistoryInventoryDto>();
            CreateMap<hp_a03, CategoryDto>();
            CreateMap<PDC, PdcDto>();
            CreateMap<Building, BuildingDto>();
            CreateMap<Cells, CellDto>();
            CreateMap<Cell_Plno, Cell_PlnoDto>();
            CreateMap<hp_a15, Hp_a15Dto>();
            CreateMap<HistoryCheckMachine, HistoryCheckMachineDto>();
            CreateMap<HistoryInventory, HistoryInventoryDto>();
            CreateMap<History, HistoryDto>();
            CreateMap<Cell_Plno, Hp_a15Dto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<DateInventory, DateInventoryDto>();
            CreateMap<Roles, RolesDto>();
        }
    }
}