using AutoMapper;
using AutoMapper.QueryableExtensions;
using Machine_API._Accessor;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Machine_API.Models.MachineCheckList;

namespace Machine_API._Service.service
{
    public class DateInventoryServive : IDateInventoryServive
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configuration;

        public DateInventoryServive(
            IMachineRepositoryAccessor repository,
            IMapper mapper,
            MapperConfiguration configuration)
        {
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<OperationResult> AddDateInventory(AddDateDto addDate, string userName, string empName, string lang)
        {
            DateTime startTime = Convert.ToDateTime(addDate.FromDate);
            DateTime endTime = Convert.ToDateTime(addDate.ToDate);

            DateInventory dateInventory = new DateInventory();
            dateInventory.FromDate = startTime;
            dateInventory.ToDate = endTime;
            dateInventory.Content = addDate.Note;
            dateInventory.CreateBy = userName;
            dateInventory.EmpName = empName;
            dateInventory.CreateTime = DateTime.Now;
            _repository.DateInventory.Add(dateInventory);
            try
            {
                await _repository.SaveChangesAsync();

                return new OperationResult
                {
                    Success = true,
                    Message = lang == "vi-VN" ? "Thêm mới ngày kiểm kê thành công !" :
                    (lang == "en-US" ? "Add new inventory day successfully !" :
                    (lang == "zh-TW" ? "添加盤點日期成功 !" : "Tambahkan tanggal inventaris baru yang berhasil!"))
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

        public async Task<OperationResult> RemoveDateInventory(int id, string lang)
        {
            var data = _repository.DateInventory.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return new OperationResult { Success = false, Message = "Không tìm thấy lịch kiểm kê! Vui lòng thử lại." };
            }
            else
            {
                try
                {
                    _repository.DateInventory.Remove(data);
                    await _repository.SaveChangesAsync();

                    return new OperationResult
                    {
                        Success = true,
                        Message = lang == "vi-VN" ? "Xóa ngày kiểm kê thành công !" :
                        (lang == "en-US" ? "Successfully deleted the inventory date !" :
                        (lang == "zh-TW" ? "刪除盤點日期成功 !" : "Hapus tanggal inventaris berhasil!"))
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

        public async Task<PageListUtility<DateInventoryDto>> GetAllDateInventories(PaginationParams paginationParams)
        {
            var data = _repository.DateInventory.FindAll().ProjectTo<DateInventoryDto>(_configuration).OrderByDescending(x => x.CreateTime);
            return await PageListUtility<DateInventoryDto>.PageListAsync(data, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<DateInventoryDto> GetDateInventory(int id) => _mapper.Map<DateInventory, DateInventoryDto>(await _repository.DateInventory.FindById(id));

        public Tuple<bool, DateTime?> CheckScheduleInventory()
        {
            var data = _repository.DateInventory
                .FirstOrDefault(x =>
                    DateTime.Compare(DateTime.Now, x.FromDate.Value) >= 0
                    && DateTime.Compare(DateTime.Now, x.ToDate.Value) <= 0
                );

            if (data != null)
            {
                return new Tuple<bool, DateTime?>(false, Convert.ToDateTime(data.ToDate));
            }
            else
            {
                return new Tuple<bool, DateTime?>(true, null);
            }
        }
    }
}