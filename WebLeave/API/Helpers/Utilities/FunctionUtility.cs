
using System.Text.RegularExpressions;
using API._Repositories;
using API.Models;
using Microsoft.EntityFrameworkCore;
namespace API.Helpers.Utilities
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IFunctionUtility
    {
        Task<string> UploadAsync(IFormFile file, string subfolder = "upload", string rawFileName = null);
        Task<string> UploadAsync(string file, string subfolder = "upload", string rawFileName = null);
        Task<List<string>> CheckAllowData(int userID);
        Task<List<int>> CheckGroupBase(int userID);
        string HashPasswordUser(string password);
        string RemoveUnicode(string str);
        Task<List<string>> GetPersonApproval(Employee employee);
        Task SaveFile(IFormFile file, string subfolder = "upload", string rawFileName = null);
        Task<string> GetCateName(string lang, int cateID);
        Task<string> GetPartName(string lang, int partID);
        Task<string> GetDeptName(string lang, int deptID);
        string ConvertLeaveDay(string day);
        Task<string> GetUsername(int? userID);
    }

    public class FunctionUtility : IFunctionUtility
    {

        private readonly string _webRootPath;
        private readonly IRepositoryAccessor _repositoryAccessor;

        public FunctionUtility(IRepositoryAccessor repositoryAccessor)
        {
            _repositoryAccessor = repositoryAccessor;
            _webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        /// <summary>
        /// Upload a file to server folder.
        /// </summary>
        /// <param name="file">Uploaded file.</param>
        /// <param name="subfolder">Subfolder. Default: "upload"</param>
        /// <param name="rawFileName">Raw file name. Default: uploaded file name.</param>
        /// <returns>File name.</returns>

        public async Task SaveFile(IFormFile file, string subfolder = "upload", string rawFileName = null)
        {
            string folderPath = Path.Combine(_webRootPath, subfolder);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = file.FileName;
            string extension = Path.GetExtension(file.FileName);
            if (!string.IsNullOrEmpty(rawFileName))
                fileName = $"{rawFileName}{extension}";

            string filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            using FileStream fs = File.Create(filePath);
            await file.CopyToAsync(fs);
            await fs.FlushAsync();
        }
        public async Task<string> UploadAsync(IFormFile file, string subfolder = "upload", string rawFileName = null)
        {
            if (file == null)
                return null;

            var folderPath = Path.Combine(_webRootPath, subfolder);
            var fileName = file.FileName;
            var extension = Path.GetExtension(file.FileName);

            if (string.IsNullOrEmpty(extension))
                return null;

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            if (!string.IsNullOrEmpty(rawFileName))
                fileName = $"{rawFileName}{extension}";

            var filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            try
            {
                using (FileStream fs = File.Create(filePath))
                {
                    await file.CopyToAsync(fs);
                    await fs.FlushAsync();
                }

                return fileName;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Upload a base64 string file to server folder.
        /// </summary>
        /// <param name="file">Uploaded file.</param>
        /// <param name="subfolder">Subfolder. Default: "upload"</param>
        /// <param name="rawFileName">Raw file name. Default: uploaded file name.</param>
        /// <returns>File name.</returns>
        public async Task<string> UploadAsync(string file, string subfolder = "upload", string rawFileName = null)
        {
            if (string.IsNullOrEmpty(file))
                return null;

            var folderPath = Path.Combine(_webRootPath, subfolder);
            var extension = $".{file.Split(';')[0].Split('/')[1]}";

            if (string.IsNullOrEmpty(extension))
                return null;

            var fileName = $"{Guid.NewGuid()}{extension}";

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            if (!string.IsNullOrEmpty(rawFileName))
                fileName = $"{rawFileName}{extension}";

            var filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            var base64String = file[(file.IndexOf(',') + 1)..];
            var fileData = Convert.FromBase64String(base64String);

            try
            {
                await File.WriteAllBytesAsync(filePath, fileData);
                return fileName;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        private static string GetSHA512(string text)
        {
            System.Text.UnicodeEncoding UE = new();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);
            string hex = "";

            hashValue = System.Security.Cryptography.SHA512.HashData(message);

            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        public string HashPasswordUser(string password)
        {
            string hashpass = GetSHA512(password)[..75].ToString();
            return hashpass;
        }

        private static readonly string[] VietNamChar = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public string RemoveUnicode(string str)
        {
            //Regex re = new Regex("[;\\/:*?\"<>|&']");
            //Regex re = new Regex("[;\\\\/:*?\"<>|&']");
            //Thay thế và lọc dấu từng char
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            //str = re.Replace(str, "-");

            str = Regex.Replace(str, "[^0-9a-zA-Z]+", " ").ToLower();
            return str;
        }

        public async Task<List<string>> CheckAllowData(int userID)
        {
            var result = new List<string>();

            var roles = await _repositoryAccessor.RolesUser.FindAll(x => x.UserID == userID).AsNoTracking()
            .Include(x => x.Role).ToListAsync();

            if (roles.Count > 0)
            {
                foreach (var item in roles)
                {
                    switch (item.Role.Ranked.Value)
                    {
                        case 1:
                            result.Add($"A-{item.Role.RoleSym}");
                            break;

                        case 2:
                            result.Add($"B-{item.Role.RoleSym}");
                            break;

                        case 3:
                            result.Add($"D-{item.Role.RoleSym}");
                            break;

                        default:
                            result.Add($"P-{item.Role.RoleSym}");
                            break;
                    }
                }
            }

            return result;
        }

        public async Task<List<int>> CheckGroupBase(int userID)
        {
            var role_group = await _repositoryAccessor.SetApproveGroupBase.FindAll(q => q.UserID == userID, true).ToListAsync();
            List<int> result = new();
            foreach (var q in role_group)
            {
                if (q.GBID.HasValue)
                    result.Add(q.GBID.Value);
            }

            return result;
        }

        public async Task<List<string>> GetPersonApproval(Employee employee)
        {
            var data = await _repositoryAccessor.Employee.FindAll(x => x.EmpID == employee.EmpID)
                .Join(_repositoryAccessor.Part.FindAll(),
                    x => x.PartID,
                    y => y.PartID,
                    (x, y) => new { Employee = x, Part = y })
                .Join(_repositoryAccessor.Department.FindAll(),
                    x => x.Part.DeptID,
                    y => y.DeptID,
                    (x, y) => new { x.Employee, x.Part, Department = y })
                .Join(_repositoryAccessor.Building.FindAll(),
                    x => x.Department.BuildingID,
                    y => y.BuildingID,
                    (x, y) => new { x.Employee, x.Part, x.Department, Building = y })
                .Join(_repositoryAccessor.Area.FindAll(),
                    x => x.Department.AreaID,
                    y => y.AreaID,
                    (x, y) => new { x.Employee, x.Part, x.Department, x.Building, Area = y })
                .FirstOrDefaultAsync();
            var groupID = data.Employee.GBID.Value;
            var partSym = data.Part.PartSym;
            var deptSym = data.Department.DeptSym;
            var buildingSym = data.Building.BuildingSym;
            var areaSym = data.Area.AreaSym;
            var users = await _repositoryAccessor.Users.FindAll(x => (x.UserRank.Value == 3 || x.UserRank.Value == 5) && x.Visible == true).ToListAsync();

            var validUsers = new List<Users>();
            var result = new List<string>();

            foreach (var user in users)
            {
                var checkRole = await _repositoryAccessor.RolesUser
                    .FindAll(x => x.UserID == user.UserID)
                    .Join(_repositoryAccessor.Roles.FindAll(),
                        x => x.RoleID,
                        y => y.RoleID,
                        (x, y) => new { RolesUser = x, Roles = y })
                    .Where(x => x.Roles.RoleSym == partSym || x.Roles.RoleSym == deptSym || x.Roles.RoleSym == buildingSym || x.Roles.RoleSym == areaSym)
                    .AnyAsync();

                var checkGroupBase = await _repositoryAccessor.SetApproveGroupBase
                    .FindAll(x => x.GBID == groupID && x.UserID == user.UserID)
                    .AnyAsync();

                if (checkRole && checkGroupBase)
                {
                    validUsers.Add(user);
                }
            }

            foreach (var user in validUsers)
            {
                result.Add(user.FullName?.ToUpper().Replace(".", " ") ?? user.UserName);
            }

            if (result.Count == 0)
            {
                result.Add("Không Tìm Được");
            }

            return result;
        }

        public async Task<string> GetCateName(string lang, int cateID)
        {
            var name = await _repositoryAccessor.CatLang
                .FindAll(y => y.LanguageID == lang && y.CateID.Value == cateID)
                .Select(y => y.CateName)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return name;
        }

        public async Task<string> GetPartName(string lang, int partID)
        {
            var name = await _repositoryAccessor.PartLang
                .FindAll(y => y.LanguageID == lang && y.PartID.Value == partID)
                .Select(y => y.PartName)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return name;
        }

        public async Task<string> GetDeptName(string lang, int deptID)
        {
            var name = await _repositoryAccessor.DetpLang
                 .FindAll(x => x.LanguageID == lang && x.DeptID.Value == deptID)
                 .Select(x => x.DeptName)
                 .AsNoTracking()
                 .FirstOrDefaultAsync();
            return name;
        }

        public string ConvertLeaveDay(string day)
        {
            try
            {
                return Math.Round(decimal.Parse(day), 5) + "d" + " - " + Math.Round(decimal.Parse((Convert.ToDouble(day) * 8).ToString()), 5) + "h";
            }
            catch
            {
                return "0";
            }
        }

        public async Task<string> GetUsername(int? userID)
        {
            if (userID != null && userID > 0)
            {
                var data = await _repositoryAccessor.Users
                    .FindAll(x => x.UserID == userID)
                    .GroupJoin(_repositoryAccessor.Employee.FindAll(),
                        x => x.EmpID,
                        y => y.EmpID,
                        (x, y) => new { Users = x, Employees = y })
                    .SelectMany(
                        x => x.Employees.DefaultIfEmpty(),
                        (x, y) => new { x.Users, Employee = y })
                    .Select(x => new { x.Employee.EmpName, x.Users.UserName })
                    .FirstOrDefaultAsync();
                return data.EmpName ?? data.UserName;
            }
            else
            {
                return "Đang chờ duyệt";
            }
        }
    }
}