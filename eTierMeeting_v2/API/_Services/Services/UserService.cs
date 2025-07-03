using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Params;
using eTierV2_API.Models;
using eTierV2_API.Helpers.Utilities;
using eTierV2_API._Services.Interfaces;
using eTierV2_API._Repositories;

namespace eTierV2_API._Services.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IRepositoryAccessor _repoAccessor;

        public UserService(IMapper mapper,
        MapperConfiguration configMapper,
        IRepositoryAccessor repoAccessor
         )
        {
            _repoAccessor= repoAccessor;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<bool> AddUser(UserDTO user, string updateBy)
        {
            user.update_by = updateBy;
            user.update_time = DateTime.Now;
            var data = _mapper.Map<Users>(user);
            _repoAccessor.Users.Add(data);

            return await _repoAccessor.Users.SaveAll();
        }

        public async Task<OperationResult> ChangePassword(UserForLoginDto user)
        {
            var currentUser = _repoAccessor.Users.FirstOrDefault(x => x.account == user.Account);
            if (currentUser.password != user.OldPassword)
            {
                return new OperationResult { Caption = "Fail", Message = "Current Pasword not match", Success = false };
            }
            else
            {
                currentUser.password = user.Password;
                currentUser.update_time = DateTime.Now;
                _repoAccessor.Users.Update(currentUser);
                await _repoAccessor.Users.SaveAll();
                return new OperationResult { Caption = "Success", Message = "Update Password Success", Success = true };
            }
        }

        public async Task<bool> CheckExistUser(string account)
        {
            var data = _repoAccessor.Users.FindAll(x => x.account == account);
            if (await data.AnyAsync())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<PagedList<UserDTO>> GetListUserPaging(string account, string isActive, int pageNumber = 1, int pageSize = 10)
        {
            var data = _repoAccessor.Users.FindAll();
            if (!string.IsNullOrEmpty(account))
            {
                data = data.Where(x => x.account.Contains(account));
            }
            if (!string.IsNullOrEmpty(isActive))
            {
                var active = isActive == "1" ? true : false;
                data = data.Where(x => x.is_active == active);
            }

            var result = data.ProjectTo<UserDTO>(_configMapper);
            return await PagedList<UserDTO>.CreateAsync(result, pageNumber, pageSize);
        }

        public async Task<List<RoleByUserDTO>> GetRoleByUser(string account)
        {
            var roleByUser = await _repoAccessor.RoleUser.FindAll(x => x.user_account == account).Select(x => x.role_unique).ToListAsync();
            var role = await _repoAccessor.Roles.FindAll().OrderBy(x => x.role_sequence).Select(x => new RoleByUserDTO
            {
                role_name = x.role_name,
                role_unique = x.role_unique,
                status = roleByUser.Contains(x.role_unique) == true ? true : false
            }).ToListAsync();
            return role;
        }

        public async Task<bool> UpdateRoleByUser(string account, List<RoleByUserDTO> roles, string updateBy)
        {
            var timeNow = DateTime.Now;
            var roleByUserHad = await _repoAccessor.RoleUser.FindAll(x => x.user_account == account).ToListAsync();
            _repoAccessor.RoleUser.RemoveMultiple(roleByUserHad);
            var roleByUserNew = roles.Select(x => new RoleUserDTO
            {
                create_by = updateBy,
                create_time = timeNow,
                role_unique = x.role_unique,
                user_account = account
            }).ToList();
            var roleByUserNewMap = _mapper.Map<List<RoleUser>>(roleByUserNew);
            _repoAccessor.RoleUser.AddMultiple(roleByUserNewMap);
            return await _repoAccessor.RoleUser.SaveAll();
        }

        public async Task<bool> UpdateUser(UserDTO user, string updateBy)
        {
            user.update_by = updateBy;
            user.update_time = DateTime.Now;
            var data = _mapper.Map<Users>(user);
            _repoAccessor.Users.Update(data);
            return await _repoAccessor.Users.SaveAll();
        }
    }
}