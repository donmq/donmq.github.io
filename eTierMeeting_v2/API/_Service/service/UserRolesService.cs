using AutoMapper;
using Machine_API._Accessor;
using Machine_API._Service.interfaces;
using Machine_API.Models.MachineCheckList;
using Microsoft.EntityFrameworkCore;

namespace Machine_API._Service.service
{
    public class UserRolesService : IUserRolesService
    {
        private readonly IMachineRepositoryAccessor _repository;

        public UserRolesService(IMachineRepositoryAccessor repository)
        {
            _repository = repository;
        }

        public async Task<List<UserRoles>> GetRoleByUser(string user)
        {
            return await _repository.UserRoles.FindAll(x => x.EmpNumber.Trim() == user.Trim()).ToListAsync();
        }

    }
}