
using AutoMapper;
using Machine_API.DTO;
using Machine_API._Services.Interface;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Machine_API.Helpers.Utilities;
using Machine_API._Accessor;

namespace Machine_API._Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly MapperConfiguration _mapperConfiguration;

        public AuthService(IMachineRepositoryAccessor repository, MapperConfiguration mapperConfiguration)
        {
            _repository = repository;
            _mapperConfiguration = mapperConfiguration;
        }

        public async Task<OperationResult> ChangePassword(ChangePasswordDto changePassword, string lang)
        {
            changePassword.OldPassword = changePassword.OldPassword.HashPassword();
            changePassword.NewPassword = changePassword.NewPassword.HashPassword();
            var data = _repository.User.FirstOrDefault(x => x.UserID == changePassword.Id);
            if (data != null)
            {
                if (data.HashPass != changePassword.OldPassword)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = lang == "vi-VN" ? "Mật khẩu cũ không khớp! Vui lòng kiểm tra lại." :
                        (lang == "en-US" ? "Old password does not match! Please check again." :
                        (lang == "zh-TW" ? "旧密码不匹配！ 请再次检查" : "Kata sandi lama tidak cocok! Silakan periksa lagi."))
                    };
                }
                else
                {
                    if (!string.IsNullOrEmpty(changePassword.NewPassword))
                        data.HashPass = changePassword.NewPassword;

                    _repository.User.Update(data);
                    try
                    {
                        await _repository.SaveChangesAsync();

                        return new OperationResult
                        {
                            Success = true,
                            Message = lang == "vi-VN" ? "Thay đổi mật khẩu thành công!" :
                            (lang == "en-US" ? "Change password successfully." :
                            (lang == "zh-TW" ? "成功更改密码" : "Perubahan kata sandi berhasil!"))
                        };
                    }
                    catch (System.Exception)
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = lang == "vi-VN" ? "Thay đổi mật khẩu thất bại! Vui lòng thử lại" :
                            (lang == "en-US" ? "Password change failed! Please try again." :
                            (lang == "zh-TW" ? "密码更改失败！请再试一次。" : "Perubahan kata sandi gagal! Silakan coba lagi"))
                        };
                    }
                }

            }
            else
            {
                return new OperationResult
                {
                    Success = false,
                    Message = lang == "vi-VN" ? "Thay đổi mật khẩu thất bại! Vui lòng thử lại" :
                    (lang == "en-US" ? "Password change failed! Please try again." :
                    (lang == "zh-TW" ? "密码更改失败！请再试一次。" : "Perubahan kata sandi gagal! Silakan coba lagi"))
                };
            }
        }

        public async Task<UserDto> Login(UserForLoginDto model)
        {
            model.Password = model.Password.ToLower().HashPassword(); // hash password
            var user = await _repository.User.FindAll(x => x.UserName == model.Username && x.HashPass == model.Password).ProjectTo<UserDto>(_mapperConfiguration).FirstOrDefaultAsync();
            if (user == null || user.Visible == false)
                return null;
            var ListRoles = _repository.UserRoles.FindAll(x => x.EmpNumber.Trim() == model.Username.Trim() && x.Active == true).ToList();
            user.ListRoles = ListRoles;
            return user;
        }
    }
}