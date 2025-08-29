using System.Security.Claims;
using API.Data;
using API.Dtos.Auth;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API._Services.Interfaces.Auth;

namespace API._Services.Services.Auth

{
    public class S_Auth : BaseServices, I_Auth
    {
        private readonly IConfiguration _configuration;
        public S_Auth(IConfiguration configuration, DBContext dbContext) : base(dbContext)
        {
            _configuration = configuration;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListFactory()
        {
            return await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2").Select(x => new KeyValuePair<string, string>(x.Code, x.Code_Name))
            .Distinct().ToListAsync();
        }
        public async Task<List<KeyValuePair<string, string>>> GetDirection()
        {
            var data = await _repositoryAccessor.HRMS_SYS_Directory
                .FindAll()
                .Select(x => new KeyValuePair<string, string>(
                    x.Directory_Code,
                    x.Directory_Name
                )).Distinct().ToListAsync();
            return data;
        }
        public async Task<List<KeyValuePair<string, string>>> GetProgram(string direction)
        {
            List<string> filterProgram = new() { "4.1.2", "4.1.3", "4.1.4", "4.1.5" };
            var data = await _repositoryAccessor.HRMS_SYS_Program
                .FindAll(x => x.Parent_Directory_Code == direction && !filterProgram.Contains(x.Program_Code))
                .Select(x => new KeyValuePair<string, string>(
                    x.Program_Code,
                    x.Program_Name
            )).Distinct().ToListAsync();
            return data;
        }
        public async Task<List<string>> GetListLangs()
        {
            var data = await _repositoryAccessor.HRMS_SYS_Language.FindAll(x => x.IsActive).Select(x => x.Language_Code.ToLower()).ToListAsync();
            return data;
        }
        public async Task<ResultResponse> Login(UserLoginParam userForLogin)
        {
            var user = await _repositoryAccessor.HRMS_Basic_Account.FirstOrDefaultAsync(x => x.Account.Trim() == userForLogin.Username.Trim() && x.IsActive && x.Factory.Trim() == userForLogin.Factory.Trim());
            if (user == null)
                return null;
            if (user.Password != userForLogin.Password)
                return null;

            var userLogged = new UserForLoggedDTO
            {
                Id = user.Account,
                Factory = user.Factory,
                Account = user.Account,
                Name = user.Name,
            };
            var dir_Lang = await _repositoryAccessor.HRMS_SYS_Directory.FindAll()
                .GroupJoin(_repositoryAccessor.HRMS_SYS_Program_Language.FindAll(x => x.Kind == "D"),
                   x => x.Directory_Code,
                   y => y.Code,
                   (x, y) => new { sys_Code = x, sys_Lang = y })
                .SelectMany(x => x.sys_Lang.DefaultIfEmpty(),
                   (x, y) => new { x.sys_Code, sys_Lang = y })
                .GroupBy(x => x.sys_Code)
                .Select(x => new CodeInformation
                {
                    Code = x.Key.Directory_Code,
                    Name = x.Key.Directory_Name,
                    Kind = x.FirstOrDefault().sys_Lang.Kind,
                    Translations = x.Where(y => y.sys_Lang != null)
                        .Select(y => new CodeLang
                        {
                            Lang = y.sys_Lang.Language_Code.ToLower(),
                            Name = y.sys_Lang.Name
                        })
                }).ToListAsync();
            var pro_Lang = await _repositoryAccessor.HRMS_SYS_Program.FindAll()
                .GroupJoin(_repositoryAccessor.HRMS_SYS_Program_Language.FindAll(x => x.Kind == "P"),
                    x => x.Program_Code,
                    y => y.Code,
                    (x, y) => new { sys_Code = x, sys_Lang = y })
                .SelectMany(x => x.sys_Lang.DefaultIfEmpty(),
                    (x, y) => new { x.sys_Code, sys_Lang = y })
                .GroupBy(x => x.sys_Code)
                .Select(x => new CodeInformation
                {
                    Code = x.Key.Program_Code,
                    Name = x.Key.Program_Name,
                    Kind = x.FirstOrDefault().sys_Lang.Kind,
                    Translations = x.Where(y => y.sys_Lang != null)
                        .Select(y => new CodeLang
                        {
                            Lang = y.sys_Lang.Language_Code.ToLower(),
                            Name = y.sys_Lang.Name
                        })
                }).ToListAsync();
            var code_Information = dir_Lang.Union(pro_Lang).ToList();
            var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userLogged.Account), };
            _repositoryAccessor.HRMS_Basic_Account_Role.FindAll(x => x.Account.Trim() == user.Account.Trim())
                .Select(x => x.Role).ToList().ForEach(role => { claims.Add(new Claim(ClaimTypes.Role, role)); });
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMonths(2),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = new DataResponseDTO
            {
                User = userLogged,
                Code_Information = code_Information
            };
            return new ResultResponse() { Token = tokenHandler.WriteToken(token), Data = result };
        }

    }
}