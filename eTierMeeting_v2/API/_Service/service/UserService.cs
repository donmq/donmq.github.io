using AutoMapper;
using Machine_API.DTO;
using AutoMapper.QueryableExtensions;
using Machine_API.Models.MachineCheckList;
using Machine_API.Helpers.Utilities;
using Machine_API.Helpers.Params;
using Machine_API._Service.interfaces;
using Microsoft.EntityFrameworkCore;
using Aspose.Cells;
using Machine_API._Accessor;
using Machine_API.Resources;

namespace Machine_API._Service.service
{
    public class UserService : IUserService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly MapperConfiguration _configuration;
        private readonly LocalizationService _languageService;

        public UserService(
            IMachineRepositoryAccessor repository,
            MapperConfiguration configuration,
            LocalizationService languageService)
        {
            _repository = repository;
            _configuration = configuration;
            _languageService = languageService;
        }

        public async Task<OperationResult> AddUser(UserDto userDto, string userName, string lang)
        {
            var dataUser = _repository.User.FindAll(x => x.UserName == userDto.UserName);
            if (dataUser.Count() > 0)
            {
                return new OperationResult
                {
                    Success = true,
                    Message = lang == "vi-VN" ? "Mã nhân viên đã được sử dụng! Vui lòng thử lại." :
                    (lang == "en-US" ? "User code has been used! Please try again" :
                    (lang == "zh-TW" ? "這個工號已使用！請重試。" : "Kode karyawan telah digunakan! Silakan coba lagi."))
                };
            }
            else
            {
                User user = new User();
                user.UserName = userDto.UserName;
                user.EmpName = userDto.EmpName;
                user.EmailAddress = userDto.EmailAddress;
                user.UpdateDate = DateTime.Now;
                user.UpdateBy = userName;
                user.Visible = true;
                if (string.IsNullOrEmpty(userDto.HashPass))
                {
                    //Nếu password rỗng trả về mật khẩu nặt định "1234"
                    user.HashPass = "1234".HashPassword();
                }
                else
                {
                    user.HashPass = userDto.HashPass.HashPassword();
                }

                foreach (var item in userDto.Roles)
                {
                    UserRoles ur = new UserRoles();
                    ur.Roles = item.ToInt();
                    ur.EmpNumber = userDto.UserName;
                    ur.CreateBy = userName;
                    ur.CreateTime = DateTime.Now;
                    ur.Active = true;
                    _repository.UserRoles.Add(ur);
                }

                _repository.User.Add(user);

                try
                {
                    await _repository.SaveChangesAsync();

                    return new OperationResult
                    {
                        Success = true,
                        Message = lang == "vi-VN" ? "Bạn đã thêm nhân viên thành công !" :
                        (lang == "en-US" ? "You have added successful user !" :
                        (lang == "zh-TW" ? "你已經加入成功 !" : "Anda telah berhasil menambahkan staf!"))
                    };
                }
                catch (System.Exception)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = lang == "vi-VN" ? "Lỗi hệ thống! Vui lòng thử lại" :
                        (lang == "en-US" ? "System error! Please try again" :
                        (lang == "zh-TW" ? "系統發生錯誤!請再試" : "Sistem bermasalah! Silakan coba lagi"))
                    };
                }
            }

        }

        public async Task<List<UserDto>> GetAllUserPreliminary()
        {
            var listUserRoles = await _repository.UserRoles.FindAll(x => x.Active == true && x.Roles == 3).ToListAsync();
            var listUser = _repository.User.FindAll(x => x.Visible == true);
            var listUserHaveRole = _repository.PreliminaryPlno.FindAll().Select(x => new { x.EmpNumber }).Distinct();
            var dataJoin = listUserRoles.Join(listUser, x => x.EmpNumber, y => y.UserName, (x, y) => new UserDto
            {
                UserName = x.EmpNumber,
                EmpName = y.EmpName,
            });
            dataJoin = dataJoin.Where(x => !listUserHaveRole.Any(c => c.EmpNumber == x.UserName));
            return dataJoin.ToList();
        }
        public async Task<List<UserExportDto>> GetAllUser()
        {
            var listUserRoles = await _repository.UserRoles.FindAll(x => x.Active == true).ToListAsync();
            var listUser = _repository.User.FindAll();
            var listRoles = _repository.Roles.FindAll();
            var dataJoin = listUserRoles.Join(listUser, x => x.EmpNumber, y => y.UserName, (x, y) => new
            {
                UserName = x.EmpNumber,
                EmpName = y.EmpName,
                EmailAddress = y.EmailAddress,
                Roles = listUserRoles.Where(x => x.EmpNumber == y.UserName).Select(y => y.Roles).ToList()
            });
            List<UserExportDto> userExportList = new List<UserExportDto>();
            foreach (var item in dataJoin)
            {
                UserExportDto newItem = new UserExportDto();
                newItem.UserName = item.UserName;
                newItem.EmpName = item.EmpName;
                newItem.EmailAddress = item.EmailAddress;
                newItem.ListRolesName = string.Join("-", item.Roles);
                userExportList.Add(newItem);
            }
            // var dataJoin = listUserRoles.Join(listUser, x => x.EmpNumber, y => y.UserName, (x, y) => new
            // {
            //     UserName = x.EmpNumber,
            //     EmpName = y.EmpName,
            //     EmailAddress = y.EmailAddress,
            //     Roles = listUserRoles.Where(x => x.EmpNumber == y.UserName).Join(listRoles, x => x.Roles, y => y.ID, (x, y) => new
            //     {
            //         y.RoleName
            //     })
            // });
            // List<UserExportDto> userExportList = new List<UserExportDto>();
            // foreach (var item in dataJoin)
            // {
            //     UserExportDto newItem = new UserExportDto();
            //     newItem.UserName = item.UserName;
            //     newItem.EmpName = item.EmpName;
            //     newItem.EmailAddress = item.EmailAddress;
            //     newItem.ListRolesName = string.Join(" || ", item.Roles.Select(y => y.RoleName.Trim()));
            //     userExportList.Add(newItem);
            // }
            return await Task.FromResult(userExportList.ToList());
        }

        public async Task<UserDto> GetUserName(string userName)
        {
            return await _repository.User.FindAll(x => x.UserName == userName).ProjectTo<UserDto>(_configuration).FirstOrDefaultAsync();
        }
        public void PutStaticValue(ref Worksheet ws)
        {
            ws.Cells["A1"].PutValue(_languageService.GetLocalizedHtmlString("admin_user_code"));
            ws.Cells["B1"].PutValue(_languageService.GetLocalizedHtmlString("admin_name"));
            ws.Cells["C1"].PutValue(_languageService.GetLocalizedHtmlString("admin_email_address"));
            ws.Cells["D1"].PutValue(_languageService.GetLocalizedHtmlString("inventory_type"));
        }

        public async Task<OperationResult> RemoveUser(string userName, string sessionUsername, string lang)
        {
            var dataUser = _repository.User.FindAll(x => x.UserName == userName).FirstOrDefault();
            var listUserRoles = _repository.UserRoles.FindAll(x => x.EmpNumber == userName).ToList();
            var listPreliminary = _repository.PreliminaryPlno.FindAll(x => x.EmpNumber == userName).ToList();
            if (dataUser != null && listUserRoles != null)
            {
                if (sessionUsername == userName)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = lang == "vi-VN" ? "Không được xóa tài khoản đang đăng nhập !" :
                        (lang == "en-US" ? "Do not delete the login account!" :
                        (lang == "zh-TW" ? "不可刪除這個帳號因為他在登入!" : "Jangan hapus akun yang sedang login!"))
                    };
                }
                try
                {
                    dataUser.Visible = false;
                    _repository.User.Update(dataUser);
                    await _repository.SaveChangesAsync();

                    return new OperationResult
                    {
                        Success = true,
                        Message = lang == "vi-VN" ? "Đã xóa dữ liệu thành công!" :
                        (lang == "en-US" ? "Data deleted successfully!" :
                        (lang == "zh-TW" ? "已經刪除質料成功" : "Data berhasil dihapus!"))
                    };
                }
                catch (System.Exception)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = lang == "vi-VN" ? "Lỗi hệ thống! Vui lòng thử lại" :
                        (lang == "en-US" ? "System error! Please try again" :
                        (lang == "zh-TW" ? "系統發生錯誤!請再試" : "Sistem bermasalah! Silakan coba lagi"))
                    };
                }
            }
            else
            {
                return new OperationResult
                {
                    Success = false,
                    Message = lang == "vi-VN" ? "Không tìm thấy mã người nhân viên! Vui lòng thử lại." :
                    (lang == "en-US" ? "No user code found! Please try again." :
                    (lang == "zh-TW" ? "沒有找到這個工號！請重試." : "Kode karyawan tidak ditemukan! Silakan coba lagi."))
                };
            }
        }

        public async Task<PageListUtility<UserDto>> SearchUser(PaginationParams paginationParams, string keyword)
        {
            paginationParams.PageSize = 6;
            var data = _repository.User.FindAll();

            if (keyword != string.Empty && keyword != null)
            {
                data = data.Where(x => x.UserName.ToLower().Contains(keyword.ToLower()) ||
                                       x.EmpName.ToLower().Contains(keyword.ToLower())).OrderByDescending(x => x.UpdateDate);
            }

            return await PageListUtility<UserDto>.PageListAsync(data.ProjectTo<UserDto>(_configuration).OrderByDescending(x => x.UpdateDate), paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<OperationResult> UpdateUser(UserDto userDto, string userName, string lang)
        {
            var dataUser = _repository.User.FindAll(x => x.UserName == userDto.UserName).FirstOrDefault();
            var dataUserRoles = _repository.UserRoles.FindAll(x => x.EmpNumber == userDto.UserName).ToList();
            if (dataUser == null && dataUserRoles == null)
            {
                return new OperationResult
                {
                    Success = true,
                    Message = lang == "vi-VN" ? "Không tìm thấy mã người nhân viên! Vui lòng thử lại." :
                    (lang == "en-US" ? "No user code found! Please try again." :
                    (lang == "zh-TW" ? "沒有找到這個工號！請重試" : "Kode karyawan tidak ditemukan! Silakan coba lagi."))
                };
            }
            else
            {
                dataUser.UserName = userDto.UserName;
                dataUser.EmpName = userDto.EmpName;
                dataUser.EmailAddress = userDto.EmailAddress;
                dataUser.UpdateBy = userName;
                if (!string.IsNullOrEmpty(userDto.HashPass))
                {
                    dataUser.HashPass = userDto.HashPass.HashPassword();
                }

                _repository.User.Update(dataUser);

                var roleListUpdate = userDto.Roles.ToList();
                var roleList = _repository.Roles.FindAll();
                foreach (var item in roleList)
                {
                    if (roleListUpdate.FirstOrDefault(x => x == item.ID) != null)
                    {
                        var ur = _repository.UserRoles.FindAll(x => x.Roles == item.ID && x.EmpNumber == userDto.UserName).FirstOrDefault();
                        if (ur != null)
                        {
                            ur.Active = true;
                            ur.UpdateTime = DateTime.Now;
                            ur.UpdateBy = userName;
                            _repository.UserRoles.Update(ur);
                        }
                        else
                        {
                            UserRoles urNew = new UserRoles();
                            urNew.Roles = item.ID;
                            urNew.EmpNumber = userDto.UserName;
                            urNew.CreateBy = userName;
                            urNew.CreateTime = DateTime.Now;
                            urNew.Active = true;
                            _repository.UserRoles.Add(urNew);
                        }
                    }
                    else
                    {
                        var ur = _repository.UserRoles.FindAll(x => x.Roles == item.ID && x.EmpNumber == userDto.UserName).FirstOrDefault();
                        if (ur != null)
                        {
                            ur.Active = false;
                            ur.UpdateTime = DateTime.Now;
                            ur.UpdateBy = userName;
                            _repository.UserRoles.Update(ur);
                        }
                    }
                }

                try
                {
                    await _repository.SaveChangesAsync();

                    return new OperationResult
                    {
                        Success = true,
                        Message = lang == "vi-VN" ? "Đã cập nhật dữ liệu thành công!" :
                        (lang == "en-US" ? "Successfully updated data" :
                        (lang == "zh-TW" ? "已經更新質料成功" : "Data berhasil diperbarui!"))
                    };
                }
                catch (System.Exception)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = lang == "vi-VN" ? "Lỗi hệ thống! Vui lòng thử lại" :
                        (lang == "en-US" ? "System error! Please try again" :
                        (lang == "zh-TW" ? "系統發生錯誤!請再試" : "Sistem bermasalah! Silakan coba lagi"))
                    };
                }
            }
        }

        public async Task<OperationResult> RestoreUser(string userName, string userNameUpdate, string lang)
        {
            var dataUser = _repository.User.FindAll(x => x.UserName == userName).FirstOrDefault();
            if (dataUser == null)
            {
                return new OperationResult
                {
                    Success = true,
                    Message = lang == "vi-VN" ? "Không tìm thấy mã người nhân viên! Vui lòng thử lại." :
                    (lang == "en-US" ? "No user code found! Please try again." :
                    (lang == "zh-TW" ? "沒有找到這個工號！請重試" : "Kode karyawan tidak ditemukan! Silakan coba lagi."))
                };
            }
            else
            {
                dataUser.Visible = true;
                dataUser.UpdateBy = userNameUpdate;
                _repository.User.Update(dataUser);

                try
                {
                    await _repository.SaveChangesAsync();

                    return new OperationResult
                    {
                        Success = true,
                        Message = lang == "vi-VN" ? "Đã cập nhật dữ liệu thành công!" :
                        (lang == "en-US" ? "Successfully updated data" :
                        (lang == "zh-TW" ? "已經更新質料成功" : "Data berhasil diperbarui!"))
                    };
                }
                catch (System.Exception)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = lang == "vi-VN" ? "Lỗi hệ thống! Vui lòng thử lại" :
                        (lang == "en-US" ? "System error! Please try again" :
                        (lang == "zh-TW" ? "系統發生錯誤!請再試" : "Sistem bermasalah! Silakan coba lagi"))
                    };
                }
            }
        }
    }
}