using API._Repositories;
using API._Services.Interfaces.SeaHr;
using API.Dtos.SeaHr;
using API.Helpers.Utilities;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SeaHr
{
    public class HrDeleteEmployeeService : IHrDeleteEmployeeService
    {
        private static readonly SemaphoreSlim semaphore = new(1, 1);
        private readonly IRepositoryAccessor _repositoryAccessor;
        private readonly IFunctionUtility _functionUtility;

        public HrDeleteEmployeeService(
            IRepositoryAccessor repositoryAccessor,
            IFunctionUtility functionUtility)
        {
            _repositoryAccessor = repositoryAccessor;
            _functionUtility = functionUtility;
        }

        public async Task<OperationResult> UploadExcelDelete(IFormFile file)
        {
            await semaphore.WaitAsync();
            using var _transaction = await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                ExcelResult excelResult = ExcelUtility.CheckExcel(file, "Resources\\Template\\SeaHr\\ListDelete.xlsx");
                if (!excelResult.IsSuccess)
                    return new OperationResult(false, excelResult.Error);
                DateTime now = DateTime.Now;
                List<string> deletedEmp = new();
                for (int i = excelResult.wsTemp.Cells.Rows.Count; i < excelResult.ws.Cells.Rows.Count; i++)
                {
                    // Lấy dữ liệu excel theo từng cột
                    var empNumber = excelResult.ws.Cells[i, 0].StringValue;
                    var fullName = excelResult.ws.Cells[i, 1].StringValue;
                    if (string.IsNullOrWhiteSpace(empNumber))
                        continue;
                    var empResult = await DeleteEmployee(empNumber, now);
                    if (empResult.IsSuccess)
                        deletedEmp.Add(empNumber);
                    else
                    {
                        if (empResult.Error == "Exception")
                        {
                            await _transaction.RollbackAsync();
                            return new OperationResult(false, "DeletedFailed", empNumber);
                        }
                    }
                }
                if (deletedEmp.Count == 0)
                {
                    await _transaction.RollbackAsync();
                    return new OperationResult(false, "EmptyList");
                }
                await _transaction.CommitAsync();
                await _functionUtility.SaveFile(file, "uploaded/excels", $"ListDelete_{now:yyyyMMddHHmmss}");
                return new OperationResult(true, "DeletedSuccessfully");
            }
            catch (Exception)
            {
                await _transaction.RollbackAsync();
                return new OperationResult(false, "UploadErrorMsg");
            }
            finally
            {
                semaphore.Release();
            }
        }
        private async Task<OperationResult> DeleteEmployee(string empNumber, DateTime now)
        {
            try
            {
                //Employee related data
                Employee employee = await _repositoryAccessor.Employee.FirstOrDefaultAsync(x => x.EmpNumber == empNumber);
                if (employee == null)
                    return new OperationResult(false);
                List<LeaveData> leaveData = await _repositoryAccessor.LeaveData
                    .FindAll(x => x.EmpID == employee.EmpID).ToListAsync();
                List<HistoryEmp> historyEmp = await _repositoryAccessor.HistoryEmp
                    .FindAll(x => x.EmpID == employee.EmpID).ToListAsync();
                List<ReportData> reportData = await _repositoryAccessor.ReportData
                    .FindAll(x => x.EmpID == employee.EmpID).ToListAsync();
                if (leaveData.Any())
                    _repositoryAccessor.LeaveData.RemoveMultiple(leaveData);
                if (historyEmp.Any())
                    _repositoryAccessor.HistoryEmp.RemoveMultiple(historyEmp);
                if (reportData.Any())
                    _repositoryAccessor.ReportData.RemoveMultiple(reportData);
                _repositoryAccessor.Employee.Remove(employee);

                //User related data
                Users user = await _repositoryAccessor.Users.FirstOrDefaultAsync(i => i.EmpID == employee.EmpID);
                if (user == null)
                    return new OperationResult(await _repositoryAccessor.SaveChangesAsync());
                List<Roles_User> listRoleUser = await _repositoryAccessor.RolesUser
                    .FindAll(x => x.UserID == user.UserID).ToListAsync();
                List<SetApproveGroupBase> listUserApproveGroupBase = await _repositoryAccessor.SetApproveGroupBase
                    .FindAll(x => x.UserID == user.UserID).ToListAsync();
                List<LeaveData> listLeaveDataByUser = await _repositoryAccessor.LeaveData
                    .FindAll(x => x.UserID == user.UserID || x.ApprovedBy == user.UserID).ToListAsync();
                foreach (var itemLeavaDate in listLeaveDataByUser)
                {
                    itemLeavaDate.UserID = null;
                    itemLeavaDate.ApprovedBy = null;
                    itemLeavaDate.Updated = now;
                }
                if (listRoleUser.Any())
                    _repositoryAccessor.RolesUser.RemoveMultiple(listRoleUser);
                if (listUserApproveGroupBase.Any())
                    _repositoryAccessor.SetApproveGroupBase.RemoveMultiple(listUserApproveGroupBase);
                if (listLeaveDataByUser.Any())
                    _repositoryAccessor.LeaveData.UpdateMultiple(listLeaveDataByUser);
                _repositoryAccessor.Users.Remove(user);
                return new OperationResult(await _repositoryAccessor.SaveChangesAsync());
            }
            catch (Exception)
            {
                return new OperationResult(false, "Exception");
            }
        }
    }

}