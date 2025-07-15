using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Machine_API.Models.MachineCheckList;
using Microsoft.EntityFrameworkCore;
using Aspose.Cells;
using Machine_API.Resources;
using Machine_API._Accessor;
using LinqKit;

namespace Machine_API._Service.service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly LocalizationService _languageService;

        public EmployeeService(
            IMachineRepositoryAccessor repository,
            LocalizationService languageService)
        {
            _repository = repository;
            _languageService = languageService;
        }

        public async Task<OperationResult> AddEmploy(EmployeeDto employee, string userName, string lang)
        {
            var data = await _repository.Employee.FirstOrDefaultAsync(x => x.EmpNumber == employee.EmpNumber);
            if (data != null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = lang == "vi-VN" ? "Số thẻ đã được sử dụng! Vui lòng thử lại" :
                    (lang == "en-US" ? "Manager code has been used!Please try again" :
                    (lang == "zh-TW" ? "這工號已經在使用！ 請您選別的工號。" : "Nomor kartu telah digunakan! Silakan coba lagi"))
                };
            }
            else
            {
                Employee emp = new Employee();
                emp.EmpName = employee.EmpName;
                emp.EmpNumber = employee.EmpNumber;
                emp.Visible = true;
                emp.UpdateBy = userName;
                emp.UpdateTime = DateTime.Now;
                _repository.Employee.Add(emp);

                foreach (var item in employee.ListPlnoEmploy)
                {
                    EmployPlno employPlno = new EmployPlno();
                    employPlno.EmpNumber = employee.EmpNumber;
                    employPlno.Plno = item.PlnoID;
                    employPlno.UpdateBy = userName;
                    employPlno.UpdateTime = DateTime.Now;
                    _repository.EmployPlno.Add(employPlno);

                }
                try
                {
                    await _repository.SaveChangesAsync();

                    return new OperationResult
                    {
                        Success = true,
                        Message = lang == "vi-VN" ? "Bạn đã thêm người quản lý thành công" :
                        (lang == "en-US" ? "You have successfully added a manager" :
                        (lang == "zh-TW" ? "您已經加入管理員成功" : "Anda telah berhasil menambahkan manajer"))
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

        public async Task<object> GetEmployeeScanByNumBer(string employeeNumber)
        {
            var dataEmployee = await _repository.Employee.FindAll(x => x.EmpNumber == employeeNumber)
                .Select(x => new { x.EmpName, x.EmpNumber }).FirstOrDefaultAsync();

            
            var dataPlno = new List<object>();
            await _repository.EmployPlno.FindAll(x => x.EmpNumber == employeeNumber).ForEachAsync(x =>
            {
                dataPlno.Add(new {
                    Plno = x.Plno.Trim(),
                    Place = _repository.hp_a15.FirstOrDefault(z => z.Plno == x.Plno).Place.Trim()
                });
            });

            if (dataPlno.Count == 0)
            {
                return new
                {
                    isData = false
                };
            }
            object result = new
            {
                isData = true,
                dataEmployee,
                dataPlno
            };
            return result;
        }

        public async Task<OperationResult> RemoveEmploy(string empNumber, string lang)
        {
            var dataEmployee = await _repository.Employee.FirstOrDefaultAsync(x => x.EmpNumber == empNumber);

            if (dataEmployee != null)
            {
                try
                {
                    _repository.Employee.Remove(dataEmployee);
                    await _repository.SaveChangesAsync();

                    var dataEmployPlno = await _repository.EmployPlno.FindAll(x => x.EmpNumber == empNumber).ToListAsync();
                    _repository.EmployPlno.RemoveMultiple(dataEmployPlno);
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
                    Message = lang == "vi-VN" ? "Không tìm thấy mã người quản lý! Vui lòng thử lại" :
                    (lang == "en-US" ? "No manager code found!Please try strange" :
                    (lang == "zh-TW" ? "找不到工號！請再試一次" : "Kode pengelola tidak ditemukan! Silakan coba lagi"))
                };
            }
        }

        public async Task<PageListUtility<EmployAdminDto>> SearchEmploy(PaginationParams paginationParams, string keyword)
        {
            paginationParams.PageSize = 5;

            var predicate = PredicateBuilder.New<Employee>(true);
            if (keyword != string.Empty && keyword != null)
                predicate.And(x => x.EmpName.ToLower().Contains(keyword.ToLower()) || x.EmpNumber.Contains(keyword.ToLower()));

            var hp_A15 = _repository.hp_a15.FindAll();
            var employee = _repository.Employee.FindAll(predicate);
            var employPlno = _repository.EmployPlno.FindAll();

            var dataJoin = await employee.Select(x => new EmployAdminDto
            {
                ID = x.ID,
                EmpName = x.EmpName,
                EmpNumber = x.EmpNumber,
                Visible = x.Visible,
                ListPlnoEmploy = employPlno.Where(z => z.EmpNumber == x.EmpNumber).Join(hp_A15, e => e.Plno, hp => hp.Plno, (e, hp) => new PlnoEmployDto
                {
                    PlnoID = hp.Plno,
                    Name = hp.Plno + "-" + hp.Place,
                    Place = hp.Place
                }).ToList()
            }).OrderByDescending(x => x.ID).ToListAsync();

            return PageListUtility<EmployAdminDto>.PageList(dataJoin, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<OperationResult> UpdateEmploy(EmployeeDto employee, string userName, string lang)
        {
            var dataEmployee = _repository.Employee.FindAll();
            var dataEmployPlno = await _repository.EmployPlno.FindAll(x => x.EmpNumber == employee.EmpNumber).ToListAsync();
            var data = await dataEmployee.FirstOrDefaultAsync(x => x.EmpNumber == employee.EmpNumber);

            if (data != null)
            {
                data.EmpName = employee.EmpName.Trim();
                data.UpdateBy = userName;
                data.UpdateTime = DateTime.Now;

                _repository.EmployPlno.RemoveMultiple(dataEmployPlno);
                await _repository.SaveChangesAsync();

                foreach (var item in employee.ListPlnoEmploy)
                {
                    EmployPlno employPlno = new EmployPlno();
                    employPlno.EmpNumber = employee.EmpNumber;
                    employPlno.Plno = item.PlnoID;
                    employPlno.UpdateBy = userName;
                    employPlno.UpdateTime = DateTime.Now;
                    _repository.EmployPlno.Update(employPlno);
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
            else
            {
                return new OperationResult
                {
                    Success = false,
                    Message = lang == "vi-VN" ? "Không tìm thấy mã người quản lý! Vui lòng thử lại" :
                    (lang == "en-US" ? "No manager code found!Please try strange" :
                    (lang == "zh-TW" ? "系統發生錯誤!請再試" : "Sistem bermasalah! Silakan coba lagi"))
                };
            }
        }

        public async Task<List<EmployExportDto>> ExportExcelEmploy()
        {
            var hp_A15 = _repository.hp_a15.FindAll();
            var employee = _repository.Employee.FindAll(x => x.Visible == true);
            var employPlno = _repository.EmployPlno.FindAll();

            var data = employee.Select(x => new
            {
                x.EmpName,
                x.EmpNumber,
                Plnos = employPlno.Where(z => z.EmpNumber == x.EmpNumber).Join(hp_A15, e => e.Plno, hp => hp.Plno, (e, hp) => new
                {
                    name = hp.Plno + "-" + hp.Place
                }).ToList()
            }).AsEnumerable().Select(x => new EmployExportDto
            {
                EmpNumber = x.EmpNumber,
                EmpName = x.EmpName,
                Plnos = string.Join(" || ", x.Plnos.Select(y => y.name.Trim()))
            }).ToList();

            return await Task.FromResult(data);
        }

        public void PutStaticValue(ref Worksheet ws)
        {
            ws.Cells["A1"].PutValue(_languageService.GetLocalizedHtmlString("admin_id"));
            ws.Cells["B1"].PutValue(_languageService.GetLocalizedHtmlString("admin_name"));
            ws.Cells["C1"].PutValue(_languageService.GetLocalizedHtmlString("admin_manager_plno"));
        }
    }
}