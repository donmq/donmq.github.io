using System;
using API._Repositories;
using API._Services.Interfaces.SeaHr;
using API.Dtos.SeaHr;
using API.Helpers.Utilities;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SeaHr
{
    public class PermissionRightsService : IPermissionRightsService
    {
        private readonly IRepositoryAccessor _repositoryAccessor;
        public PermissionRightsService(IRepositoryAccessor repositoryAccessor)
        {
            _repositoryAccessor = repositoryAccessor;
        }

        public async Task<List<PermissionRightsDTO>> GetData(PermissionParam param)
        {
            var data = new List<PermissionRightsDTO>();
            var ApprovalUser = new List<string>();
            var pred = PredicateBuilder.New<Employee>(x => x.Visible == true);
            if (!string.IsNullOrEmpty(param.EmpNumber))
                pred = pred.And(x => x.EmpNumber.ToLower().Contains(param.EmpNumber.ToLower().Trim()) || x.EmpName.ToLower().Contains(param.EmpNumber.ToLower().Trim()));

            if (param.PartID != 0)
                pred = pred.And(x => x.PartID == param.PartID);
            else
                pred = pred.And(x => x.Part != null);


            var dataEmployees = await _repositoryAccessor.Employee.FindAll(pred)
                    .Include(x => x.Part)
                    .ThenInclude(x => x.Dept)
                        .ThenInclude(x => x.Building)
                    .Include(x => x.Part.Dept.Area)
                    .Include(x => x.Position)
                    .AsNoTracking().ToListAsync();

            var users = await _repositoryAccessor.Users.FindAll(x => (x.UserRank.Value == 3 || x.UserRank.Value == 5)
                        && x.Visible == true && x.UserID != 44 && x.UserID != 123)
                        .Include(x => x.Roles_User)
                            .ThenInclude(x => x.Role)
                        .Include(x => x.SetApproveGroupBase)
                        .AsNoTracking().ToListAsync();

            var permissionRightsList = dataEmployees
                .Select((item, i) => new
                {
                    Index = i + 1,
                    Employee = item,
                    Approval = GetPersonApproval(item, users)
                })
                .Select(data => new PermissionRightsDTO
                {
                    STT = data.Index,
                    EmpNumber = data.Employee.EmpNumber,
                    EmpName = data.Employee.EmpName,
                    Part = data.Employee.Part.PartName,
                    PositionName = data.Employee.Position.PositionName,
                    ApprovalUsers = string.Join(" || ", data.Approval)
                })
                .ToList();

            return permissionRightsList;
        }

        public List<string> GetPersonApproval(Employee data, List<Users> users)
        {
            var validRoles = new List<string>
                {
                    data.Part.PartSym,
                    data.Part.Dept.DeptSym,
                    data.Part.Dept.Building.BuildingSym,
                    data.Part.Dept.Area.AreaSym
                };

            var result = users
                .Where(x => x.Roles_User.Any(ro => validRoles.Contains(ro.Role.RoleSym)) && x.SetApproveGroupBase.Any(gb => gb.GBID == data.GBID && gb.UserID != null))
                .Select(u => u.FullName?.ToUpper().Replace(".", " ") ?? u.UserName)
                .ToList();

            if (result.Count == 0)
                result.Add("Không Tìm Được");

            return result;
        }

        public async Task<PaginationUtility<PermissionRightsDTO>> GetDataPagination(PaginationParam pagination, PermissionParam param)
        {
            var data = await GetData(param);
            return PaginationUtility<PermissionRightsDTO>.Create(data, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<List<KeyValuePair<int, string>>> GetListParts()
        {
            // var result = await _repositoryAccessor.Part.FindAll(x => x.Visible == true).OrderBy(x => x.PartName.Trim()).ToListAsync();
            var data = await _repositoryAccessor.Part.FindAll(x => x.Visible == true).Select(x => new KeyValuePair<int, string>(x.PartID, ($"{x.PartName}"))).Distinct().ToListAsync();
            return data;
        }
        public async Task<OperationResult> ExportExcel(PermissionParam param)
        {
            var data = await GetData(param);
            List<Table> dataTable = new List<Table>
            {
                new Table("result", data)
            };

            List<Cell> dataTitle = new List<Cell>
            {
                new Cell("A1", param.Label_Stt),
                new Cell("B1", param.Label_EmpNumber),
                new Cell("C1", param.Label_EmpName),
                new Cell("D1", param.Label_PositionName),
                new Cell("E1", param.Label_Part),
                new Cell("F1", param.Label_ApprovalUsers),
            };
            ExcelResult result = ExcelUtility.DownloadExcel(dataTable, dataTitle, "Resources\\Template\\SeaHr\\PermissionRights.xlsx");
            return new OperationResult(result.IsSuccess, result.Error, result.Result);
        }
    }
}