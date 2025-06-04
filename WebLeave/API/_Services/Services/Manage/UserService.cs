using API._Repositories;
using API._Services.Interfaces.Manage;
using API.Dtos.Manage.UserManage;
using API.Helpers.Utilities;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Aspose.Cells;
using API.Dtos.Auth;
namespace API._Services.Services.Manage
{
    public class UserService : IUserService
    {
        private readonly IRepositoryAccessor _repoAccessor;
        private readonly IUserRolesService _userRolesService;
        private readonly IFunctionUtility _functionUtility;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        public UserService(
            IRepositoryAccessor repoAccessor,
            IFunctionUtility functionUtility,
            IWebHostEnvironment webHostEnvironment,
            IUserRolesService userRolesService,
            IMapper mapper)
        {
            _repoAccessor = repoAccessor;
            _webHostEnvironment = webHostEnvironment;
            _functionUtility = functionUtility;
            _userRolesService = userRolesService;
            _mapper = mapper;
        }
        public async Task<UserForDetailDto> GetUser(int userId)
        {
            Users user = await _repoAccessor.Users.FindById(userId);
            UserForDetailDto userToReturn = _mapper.Map<UserForDetailDto>(user);

            IQueryable<Roles_User> rolePermit = _repoAccessor.RolesUser.FindAll(q => q.UserID == userId && q.Role.RoleSym.ToLower() == "moderator" && q.Role.GroupIN == 0);
            if (rolePermit.Any())
                userToReturn.RolePermitted = 1; // có quyền khu vực Cpanel, vừa xem vừa chỉnh sửa, thêm, upload ngoại trừ vào phần user + role

            IQueryable<Roles_User> roleReport = _repoAccessor.RolesUser.FindAll(q => q.Role.GroupIN == 1 && q.UserID == userId);
            userToReturn.RoleReport = roleReport.Count() switch
            {
                0 => 0,
                1 => 1,
                _ => (int?)2,
            };
            return userToReturn;
        }
        private async Task<List<UserForDetailDto>> GetData(string keyword, bool excel, string lang)
        {
            IQueryable<Users> listUser = _repoAccessor.Users.FindAll(x => x.Visible == true);

            if (!string.IsNullOrEmpty(keyword))
            {
                string keySearch = _functionUtility.RemoveUnicode(keyword).Trim();
                listUser = listUser.Where(x => x.UserName.Trim().Contains(keySearch)
                                || x.FullName.Trim().ToLower().Contains(keySearch)
                                || x.EmailAddress.Trim().ToLower().Contains(keySearch));
            }

            var data = await listUser
                .Include(x => x.Emp)
                .OrderBy(x => x.UserID)
                .Select(x => new UserForDetailDto
                {
                    UserID = x.UserID,
                    UserName = x.UserName,
                    HashImage = x.HashImage,
                    FullName = x.FullName,
                    EmailAddress = x.EmailAddress,
                    UserRank = x.UserRank,
                    ISPermitted = x.ISPermitted,
                    Visible = x.Visible,
                    EmpID = x.EmpID,
                    Employee = x.Emp,
                }).ToListAsync();

            if (excel == true)
            {
                // Tải trước tất cả dữ liệu cần thiết từ cơ sở dữ liệu
                var allUsers = await _repoAccessor.Users.FindAll(true)
                    .Include(x => x.Roles_User).ThenInclude(x => x.Role)
                    .Include(x => x.SetApproveGroupBase)
                    .ToListAsync();

                var dataBase = await _repoAccessor.GroupBase.FindAll(true)
                    .Select(x => new { x.GBID, x.GroupLangs.FirstOrDefault(y => y.LanguageID.ToLower() == lang).BaseName })
                    .ToListAsync();

                // Ánh xạ kết quả
                var excelData = new List<UserForDetailDto>();

                foreach (var x in data)
                {
                    var user = allUsers.FirstOrDefault(u => u.EmpID == x.EmpID);
                    if (user == null) continue;
                    var userDetail = new UserForDetailDto
                    {
                        UserName = user.UserName,
                        FullName = user.FullName.ToUpper(),
                        EmailAddress = user.EmailAddress,
                        Visible = user.Visible,
                        UserRank = user.UserRank,
                    };

                    // Có thể lấy từ bảng Role_User để kiểm tra
                    if (user.UserRank != 1)
                    {
                        var dataRolesss = await GetAssignRoles(x.UserID, lang);
                        userDetail.PartName = string.Join("\n", dataRolesss.GroupBy(area => area.AreaName).Select(areaGroup =>
                        {
                            // Duyệt qua từng AreaNode
                            var areaName = $"●- {areaGroup.Key}";
                            return $"{areaName}\n" + string.Join("\n", areaGroup.SelectMany(area => area.Departments).GroupBy(department => department.DeptName).Select(departmentGroup =>
                            {
                                // Duyệt qua từng DepartmentNode
                                var departmentName = $"   ✓- {departmentGroup.Key}";
                                var partNames = string.Join("\n", departmentGroup.SelectMany(department => department.Parts).Select(part =>
                                {
                                    // Duyệt qua từng PartNode
                                    return $"         {part.PartName}";
                                }));
                                return $"{departmentName}\n{partNames}";
                            }));
                        }).Where(name => !string.IsNullOrEmpty(name)));
                    };

                    userDetail.BaseName = string.Join(" ||", dataBase.Where(baseGroup => user.SetApproveGroupBase.Any(g => g.GBID == baseGroup.GBID) && !string.IsNullOrEmpty(baseGroup.BaseName))
                                                              .Select(baseGroup => baseGroup.BaseName));

                    excelData.Add(userDetail);
                }

                return excelData;
            }
            else
            {
                return data;
            }
        }

        public async Task<List<AreaNode>> GetAssignRoles(int userId, string langId)
        {
            // Tạo danh sách phân tầng
            List<AreaNode> areaNodes = new();

            // Lấy danh sách vai trò được gán
            (_, List<TreeNode<RoleNode>> AssignedRoles) = await _userRolesService.GetAllRoleUser(userId, langId);

            foreach (var itemArea in AssignedRoles)
            {
                // Tạo node cho Area
                var areaNode = new AreaNode
                {
                    AreaName = itemArea.Label,
                    Departments = new List<DepartmentNode>()
                };

                foreach (var itemDept in itemArea.Children)
                {
                    // Tạo node cho Department
                    var departmentNode = new DepartmentNode
                    {
                        DeptName = itemDept.Label,
                        Parts = new List<PartNode>()
                    };

                    foreach (var itemPart in itemDept.Children)
                    {
                        // Tạo node cho Part
                        var partNode = new PartNode
                        {
                            PartName = itemPart.Label
                        };

                        // Thêm Part vào Department
                        departmentNode.Parts.Add(partNode);
                    }

                    // Thêm Department vào Area
                    areaNode.Departments.Add(departmentNode);
                }

                // Thêm Area vào danh sách phân tầng
                areaNodes.Add(areaNode);
            }

            return areaNodes;
        }

        public async Task<PaginationUtility<UserForDetailDto>> GetAll(PaginationParam pagination, string keyword)
        {
            var data = await GetData(keyword, false, "");
            return PaginationUtility<UserForDetailDto>.Create(data, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<OperationResult> Add(UserForDetailDto userDto)
        {
            if (await _repoAccessor.Users.AnyAsync(x => x.UserName == userDto.UserName))
                return new OperationResult(false, "AccountExist");

            if (string.IsNullOrWhiteSpace(userDto.HashPass))
                userDto.HashPass = SettingsConfigUtility.GetCurrentSettings("AppSettings:Factory").ToLower() + "@1234";

            Employee employee = await _repoAccessor.Employee.FirstOrDefaultAsync(k => k.EmpNumber == userDto.EmpNumber);

            userDto.EmpID = employee?.EmpID;
            userDto.HashPass = _functionUtility.HashPasswordUser(userDto.HashPass);
            userDto.Visible = true;
            userDto.FullName = userDto.FullName.ToLower();

            Users model = _mapper.Map<Users>(userDto);
            _repoAccessor.Users.Add(model);

            await _repoAccessor.SaveChangesAsync();
            return new OperationResult(true, "CreateOKMsg");
        }

        public async Task<OperationResult> Edit(UserForDetailDto userDto)
        {
            Users user = await _repoAccessor.Users.FindById(userDto.UserID);

            Employee employee = await _repoAccessor.Employee.FirstOrDefaultAsync(k => k.EmpNumber == userDto.EmpNumber);
            userDto.EmpID = employee?.EmpID;

            if (string.IsNullOrEmpty(userDto.HashPass))
                userDto.HashPass = user.HashPass;
            else
                userDto.HashPass = _functionUtility.HashPasswordUser(userDto.HashPass);

            userDto.FullName = userDto.FullName.ToLower();

            _mapper.Map(userDto, user);

            await _repoAccessor.SaveChangesAsync();
            return new OperationResult(true, "UpdateOKMsg");
        }

        public async Task<OperationResult> UploadExcel(IFormFile file)
        {
            if (file == null)
                return new OperationResult(false, "FileNotFound");
            string extension = Path.GetExtension(file.FileName).ToLower();
            string uploadFile = $"ListUserDelete{extension}";
            string uploadPath = @"uploaded/excels";
            string folder = Path.Combine(_webHostEnvironment.WebRootPath, uploadPath);
            string filePath = Path.Combine(folder, uploadFile);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (File.Exists(filePath))
                File.Delete(filePath);

            try
            {
                using FileStream fs = File.Create(filePath);
                await file.CopyToAsync(fs);
                await fs.FlushAsync();
            }
            catch (System.Exception)
            {
                return new OperationResult(false, "DeleteErrorMsg");
            }

            //read file
            WorkbookDesigner designer = new()
            {
                Workbook = new Workbook(filePath)
            };

            List<UserForDeleteDto> usersForDel = new();

            Worksheet ws = designer.Workbook.Worksheets[0];
            int rows = ws.Cells.MaxDataRow;

            for (int i = 1; i <= rows; i++)
            {
                UserForDeleteDto user = new()
                {
                    UserName = ws.Cells[i, 0].StringValue?.Trim().ToUpper(),
                    FullName = ws.Cells[i, 1].StringValue?.Trim().ToUpper()
                };

                // User Expert - Tài khoản chuyên gia
                Users itemUser = await _repoAccessor.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);

                if (itemUser.EmpID.HasValue)
                {
                    try
                    {
                        List<Roles_User> listRoleUser = await _repoAccessor.RolesUser.FindAll(x => x.UserID == itemUser.UserID).ToListAsync();
                        if (listRoleUser.Count > 0)
                        {
                            _repoAccessor.RolesUser.RemoveMultiple(listRoleUser);
                        }

                        List<SetApproveGroupBase> listUserApproveGroupBase = await _repoAccessor.SetApproveGroupBase.FindAll(x => x.UserID == itemUser.UserID).ToListAsync();
                        if (listUserApproveGroupBase.Count > 0)
                        {
                            _repoAccessor.SetApproveGroupBase.RemoveMultiple(listUserApproveGroupBase);
                        }

                        List<LeaveData> listLeaveDataByUser = await _repoAccessor.LeaveData.FindAll(x => x.UserID == itemUser.UserID || x.ApprovedBy == itemUser.UserID).ToListAsync();
                        foreach (LeaveData itemLeavaDate in listLeaveDataByUser)
                        {
                            itemLeavaDate.UserID = null;
                            itemLeavaDate.ApprovedBy = null;
                            await _repoAccessor.SaveChangesAsync();
                        }

                        _repoAccessor.Users.Remove(itemUser);
                        user.Status = 1;

                        await _repoAccessor.SaveChangesAsync();
                    }
                    catch (System.Exception)
                    {
                        user.Status = 0;
                    }
                    usersForDel.Add(user);
                }
                else if (!itemUser.EmpID.HasValue)
                {
                    user.Status = 0;
                    usersForDel.Add(user);
                }
                else
                {
                    user.Status = -1;
                    usersForDel.Add(user);
                }
            }
            return new OperationResult(true, "DeleteOKMsg", usersForDel);
        }

        public async Task<bool> CheckLeavePermission(int? userID, int? group)
        {
            // Kiểm tra quyền theo 3 khu vực: Apply (1) - Approve (2)
            Users user = await _repoAccessor.Users.FirstOrDefaultAsync(x => x.UserID == userID);
            List<Roles_User> roles = await _repoAccessor.RolesUser.FindAll(q => q.UserID == userID).ToListAsync();

            return group switch
            {
                1 => ((user.UserRank <= 1 || roles.Count <= 0) ? 0 : user.UserRank.Value) >= 4,
                2 => ((user.UserRank <= 2 || roles.Count <= 0) ? 0 : user.UserRank.Value) >= 4,
                _ => (user.UserRank < 3 ? 0 : user.UserRank.Value) >= 4,
            };
        }

        public async Task<OperationResult> DownloadExcel(UserManageTitleExcel title, string keyword, string lang)
        {
            var data = await GetData(keyword, true, lang);

            foreach (var item in data)
            {
                // Lấy data cho cột Vai Trò E
                switch (item.UserRank)
                {
                    case 1:
                        item.UserRankString = title.label_Rank1;
                        break;
                    case 2:
                        item.UserRankString = title.label_Rank2;
                        break;
                    case 3:
                        item.UserRankString = title.label_Rank3;
                        break;
                    case 6:
                        item.UserRankString = title.label_Rank6;
                        break;
                    case 4:
                        item.UserRankString = "SEA/HR";
                        break;
                    case 5:
                        item.UserRankString = title.label_Rank5;
                        break;
                    default:
                        item.UserRankString = string.Empty;
                        break;
                }
                // Chỉnh sửa dữ liệu trong cột F và G
                if (item.PartName != null)
                {
                    item.PartName = item.PartName.Replace("||", "\n");
                }
                if (item.BaseName != null)
                {
                    item.BaseName = item.BaseName.Replace("||", "\n");
                }
            }

            List<Table> dataTable = new List<Table>
            {
                new Table("result", data)
            };

            List<SDCores.Cell> dataTitle = new List<SDCores.Cell>
            {
                new SDCores.Cell("A1", title.label_Username),
                new SDCores.Cell("B1", title.label_Fullname),
                new SDCores.Cell("C1", title.label_Email),
                new SDCores.Cell("D1", title.label_Visible),
                new SDCores.Cell("E1", title.label_RankTitle),
                new SDCores.Cell("F1", title.label_CurrentRole),
                new SDCores.Cell("G1", title.label_CurrentRole),
            };

            var excelResult = ExcelUtility.DownloadExcel(dataTable, dataTitle, "Resources\\Template\\Manage\\ListUsers.xlsx");

            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

    }
}