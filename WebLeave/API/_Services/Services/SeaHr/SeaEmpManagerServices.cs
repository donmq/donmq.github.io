using API._Repositories;
using API._Services.Interfaces.SeaHr;
using API.Dtos.Common;
using API.Helpers.Params.SeaHr.EmployeeManager;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
namespace API._Services.Services.SeaHr
{
    public class SeaEmpManagerServices : ISeaEmpManagerServices
    {
        private readonly IRepositoryAccessor _accessorRepo;

        public SeaEmpManagerServices(IRepositoryAccessor accessorRepo)
        {
            _accessorRepo = accessorRepo;
        }

        /// <summary>
        /// Lấy dánh sách khu vực
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValuePair<int, string>>> GetAreas()
        {
            return await _accessorRepo.Area.FindAll(x => x.Visible == true)
            .Select(x => new KeyValuePair<int, string>(x.AreaID, x.AreaName)).ToListAsync();
        }

        /// <summary>
        /// Lấy danh sách Phòng Ban theo mã khu vực
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<int, string>>> GetDepartments(int areaId)
        {
            return await _accessorRepo.Department.FindAll(x => x.AreaID == areaId && x.Visible == true)
            .Select(x => new KeyValuePair<int, string>(x.DeptID, x.DeptName)).ToListAsync();
        }

        public async Task<List<KeyValuePair<int, string>>> GetParts(int departmentId)
        {
            return await _accessorRepo.Part.FindAll(x => x.DeptID == departmentId && x.Visible == true)
            .Select(x => new KeyValuePair<int, string>(x.PartID, x.PartName)).ToListAsync();
        }

        /// <summary>
        /// Tìm kiếm danh sách nhân viên theo Khu vực, bộ phận , phòng ban
        /// </summary>
        /// <param name="param"> Phân trang </param>
        /// <param name="filter"> Điều kiện lọc</param>
        /// <returns></returns>
        public async Task<PaginationUtility<HistoryEmpDto>> Search(PaginationParam param, SeaEmployeeFilter filter)
        {
            var predicate = PredicateBuilder.New<HistoryEmp>(x => x.YearIn == DateTime.Now.Year && x.Emp.Visible == true);

            if (!string.IsNullOrEmpty(filter.EmployeeId))
            {
                filter.EmployeeId = filter.EmployeeId.ToLower();
                predicate = predicate.And(x => x.Emp.EmpNumber.ToLower().Contains(filter.EmployeeId) || x.Emp.EmpName.ToLower().Contains(filter.EmployeeId));
            }

            // Nếu filter có mã khu vực thì filter theo mã khu vực
            if (filter.AreaId != null)
            {
                // Nếu có DepartmentId thì lấy theo Department
                if (filter.DepartmentId == null)
                    predicate.And(x => x.Emp.Part.Dept.Area.AreaID == filter.AreaId);
                else
                {
                    predicate.And(x => x.Emp.Part.Dept.Area.AreaID == filter.AreaId && x.Emp.Part.Dept.DeptID == filter.DepartmentId);

                    if (filter.PartId == null)
                        predicate = predicate.And(x => x.Emp.Part.Dept.DeptID == filter.DepartmentId);
                    else
                        predicate = predicate.And(x => x.Emp.Part.Dept.DeptID == filter.DepartmentId && x.Emp.Part.PartID == filter.PartId);
                }
            }

            // Lấy danh sách history theo năm
            List<HistoryEmpDto> data = await _accessorRepo.HistoryEmp.FindAll(predicate)
            .Include(x => x.Emp)
                .ThenInclude(x => x.Part)
                    .ThenInclude(x => x.Dept)
            .Select(x => new HistoryEmpDto
            {
                HisrotyID = x.HisrotyID,
                EmpID = x.EmpID,
                YearIn = x.YearIn,
                TotalDay = x.TotalDay,
                Arrange = x.Arrange,
                Agent = x.Agent,
                CountArran = x.CountArran,
                CountAgent = x.CountAgent,
                CountTotal = x.CountTotal,
                CountLeave = x.CountLeave,
                Updated = x.Updated,
                Visible = x.Emp.Visible,
                EmpName = x.Emp.EmpName, //- Họ ten
                EmpNumber = x.Emp.EmpNumber,//- Mã nhân vien
                AreaID = x.Emp.Part.Dept.AreaID,
                DeptID = x.Emp.Part.DeptID,
                DeptName = x.Emp.Part.Dept.DeptName,
                DeptCode = x.Emp.Part.Dept.DeptCode,
                PartId = x.Emp.Part.PartID,
                PartName = x.Emp.Part.PartName
            }).OrderBy(x => x.EmpNumber).ToListAsync();

            return PaginationUtility<HistoryEmpDto>.Create(data, param.PageNumber, param.PageSize);
        }
    }
}