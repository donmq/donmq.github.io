using API._Repositories;
using API._Services.Interfaces.SeaHr;
using API.Dtos.SeaHr;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SeaHr
{
    public class AllowLeaveSundayService : IAllowLeaveSundayService
    {
        private readonly IRepositoryAccessor _repository;

        public AllowLeaveSundayService(IRepositoryAccessor repository)
        {
            _repository = repository;
        }

        public async Task<PaginationUtility<AllowLeaveSundayDto>> GetPagination(PaginationParam pagination, AllowLeaveSundayParam param)
        {
            var pred = PredicateBuilder.New<Employee>(x => x.IsSun.HasValue && x.IsSun.Value && x.Visible == true);

            if (!string.IsNullOrWhiteSpace(param.Keyword))
                pred.And(x => x.EmpName.ToLower().Contains(param.Keyword.ToLower().Trim()) || x.EmpNumber.ToLower().Contains(param.Keyword.ToLower().Trim()));
            if (param.PartId.HasValue)
                pred.And(x => x.PartID == param.PartId);

            var data = await _repository.Employee
                .FindAll(pred)
                    .Include(x => x.Part)
                        .ThenInclude(x => x.Dept)
                .Select(x => new AllowLeaveSundayDto
                {
                    EmpID = x.EmpID,
                    EmpName = x.EmpName,
                    EmpNumber = x.EmpNumber,
                    IsSun = x.IsSun,
                    PartName = x.Part.PartName,
                    DeptCode = x.Part.Dept.DeptCode,
                    DeptName = x.Part.Dept.DeptName,
                })
                .ToListAsync();

            return PaginationUtility<AllowLeaveSundayDto>.Create(data, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<List<AllowLeaveSundayDto>> GetEmployee(AllowLeaveSundayParam param)
        {
            var pred = PredicateBuilder.New<Employee>(x => (!x.IsSun.HasValue || !x.IsSun.Value) && x.Visible == true);

            if (!string.IsNullOrWhiteSpace(param.Keyword))
                pred.And(x => x.EmpName.ToLower().Contains(param.Keyword.ToLower().Trim()) || x.EmpNumber.ToLower().Contains(param.Keyword.ToLower().Trim()));
            if (param.PartId.HasValue)
                pred.And(x => x.PartID == param.PartId);

            var data = await _repository.Employee
                .FindAll(pred)
                    .Include(x => x.Part)
                        .ThenInclude(x => x.Dept)
                .Select(x => new AllowLeaveSundayDto
                {
                    EmpID = x.EmpID,
                    EmpName = x.EmpName,
                    EmpNumber = x.EmpNumber,
                    IsSun = x.IsSun,
                    PartName = x.Part.PartName,
                    DeptCode = x.Part.Dept.DeptCode,
                    DeptName = x.Part.Dept.DeptName,
                })
                .ToListAsync();

            return data;
        }

        public async Task<OperationResult> AllowLeave(List<int> EmpSelected)
        {
            var employees = await _repository.Employee
                .FindAll(x => EmpSelected.Contains(x.EmpID))
                .ToListAsync();

            var employeesAllow = employees
                .Select(x => { x.IsSun = true; return x; })
                .ToList();

            try
            {
                _repository.Employee.UpdateMultiple(employeesAllow);
                await _repository.SaveChangesAsync();

                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }

        public async Task<OperationResult> DisallowLeave(int EmpID)
        {
            var employee = await _repository.Employee.FirstOrDefaultAsync(x => x.EmpID == EmpID);
            if (employee is null)
                return new OperationResult(false, "System.Message.DataNotFound");

            employee.IsSun = false;

            try
            {
                _repository.Employee.Update(employee);

                await _repository.SaveChangesAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }

        public async Task<List<KeyValuePair<int, string>>> GetParts()
        {
            return await _repository.Part
                .FindAll(x => x.Visible == true)
                .Select(x => new KeyValuePair<int, string>(x.PartID, x.PartName))
                .ToListAsync();
        }
    }
}