using Machine_API._Accessor;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Microsoft.EntityFrameworkCore;

namespace Machine_API._Service.service
{
    public class RolesService : IRolesService
    {
        private readonly IMachineRepositoryAccessor _repository;

        public RolesService(IMachineRepositoryAccessor repository)
        {
            _repository = repository;
        }

        public async Task<List<RolesDto>> GetAllRoles()
        {
            return await _repository.Roles.FindAll().Select(x => new RolesDto()
            {
                ID = x.ID,
                RoleName = x.RoleName,
                UpdateBy = x.UpdateBy,
                UpdateTime = x.UpdateTime,
                Checked = false,
                Visible = false,
                RoleSequence = x.RoleSequence
            }).ToListAsync();
        }
    }
}