using AutoMapper;
using AutoMapper.QueryableExtensions;
using Machine_API._Accessor;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Machine_API.Models.MachineCheckList;
using Microsoft.EntityFrameworkCore;

namespace Machine_API._Service.service
{
    public class PDCService : IPDCService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;

        public PDCService(
            IMachineRepositoryAccessor repository,
            IMapper mapper,
            MapperConfiguration mapperConfiguration)
        {
            _repository = repository;
            _mapper = mapper;
            _mapperConfiguration = mapperConfiguration;
        }

        public async Task<OperationResult> AddPDC(PdcDto model_Dto, string lang)
        {
            var itemPDC = _repository.PDC.FirstOrDefault(x => x.PDCCode == model_Dto.PDCCode);
            if (itemPDC != null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = lang == "vi-VN" ? "PDC đã tồn tại! Vui lòng chọn PDC khác" :
                    (lang == "en-US" ? "PDC already exists !. Please select another PDC" :
                    (lang == "zh-TW" ? "單位已經有了！請您選別的單位" : "PDC sudah ada! Silakan pilih PDC lain"))
                };
            }
            else
            {
                var model = _mapper.Map<PDC>(model_Dto);
                _repository.PDC.Add(model);
                try
                {
                    await _repository.SaveChangesAsync();
                    return new OperationResult
                    {
                        Success = true,
                        Message = lang == "vi-VN" ? "Bạn đã thêm  PDC thành công" :
                        (lang == "en-US" ? "You have successfully added PDC" :
                        (lang == "zh-TW" ? "您已經加入單位成功" : "Anda telah berhasil menambahkan PDC"))
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

        public async Task<List<PdcDto>> GetAllPDC()
        {
            var data = await _repository.PDC.FindAll(x => x.Visible == true)
                .ProjectTo<PdcDto>(_mapperConfiguration).ToListAsync();
            return data;
        }

        public async Task<PageListUtility<PdcDto>> GetListAllPDC(PaginationParams paginationParams)
        {
            var data = _repository.PDC.FindAll(x => x.Visible == true).OrderByDescending(x => x.PDCID).ProjectTo<PdcDto>(_mapperConfiguration);
            return await PageListUtility<PdcDto>.PageListAsync(data, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<OperationResult> RemovePDC(PdcDto model_Dto, string lang)
        {
            var itemPDC = _repository.PDC.FindAll(x => x.PDCID == model_Dto.PDCID).FirstOrDefault();
            if (itemPDC != null)
            {
                _repository.PDC.Remove(itemPDC);
                try
                {
                    await _repository.SaveChangesAsync();
                    return new OperationResult
                    {
                        Success = true,
                        Message = lang == "vi-VN" ? "Bạn đã xóa PDC thành công" :
                        (lang == "en-US" ? "You have deleted the PDC successfully" :
                        (lang == "zh-TW" ? "您已經刪除單位成功" : "Anda telah berhasil menghapus PDC"))
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
                    Message = lang == "vi-VN" ? "PDC không được tìm thấy! Vui lòng chọn Pdc khác" :
                    (lang == "en-US" ? "PDC not found! Please choose another PDC" :
                    (lang == "zh-TW" ? "找不到單位！請您選別的單位 " : "PDC tidak ditemukan! Silakan pilih Pdc lain"))
                };
            }
        }

        public async Task<PageListUtility<PdcDto>> SearchPDC(PaginationParams paginationParams, string keyword)
        {
            var pdcQuery = _repository.PDC.FindAll();
            if (!String.IsNullOrEmpty(keyword))
            {
                pdcQuery = pdcQuery.Where(x => x.PDCCode.ToLower().Contains(keyword.ToLower())
                    || x.PDCName.ToLower().Contains(keyword.ToLower()));
            }
            var test = pdcQuery.OrderByDescending(x => x.PDCID).ToList();
            return await PageListUtility<PdcDto>.PageListAsync(pdcQuery.OrderByDescending(x => x.PDCID).ProjectTo<PdcDto>(_mapperConfiguration), paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<OperationResult> UpdatePDC(PdcDto model_Dto, string lang)
        {
            var model = await _repository.PDC.FindAll(x => x.PDCID == model_Dto.PDCID).FirstOrDefaultAsync();
            model.PDCCode = model_Dto.PDCCode.Trim();
            model.PDCName = model_Dto.PDCName.Trim();

            _repository.PDC.Update(model);
            try
            {
                await _repository.SaveChangesAsync();
                return new OperationResult
                {
                    Success = true,
                    Message = lang == "vi-VN" ? "Bạn đã cập nhật Pdc thành công" :
                    (lang == "en-US" ? "You have updated the pdc successfully" :
                   (lang == "zh-TW" ? "您已經更新單位成功" : "Anda telah berhasil memperbarui Pdc"))
                };
            }
            catch (System.Exception)
            {
                return new OperationResult
                {
                    Success = true,
                    Message = lang == "vi-VN" ? "Lỗi hệ thống! Vui lòng thử lại" :
                    (lang == "en-US" ? "System error! Please try again" :
                    (lang == "zh-TW" ? "系統發生錯誤!請再試" : "Sistem bermasalah! Silakan coba lagi"))
                };
            }

        }
    }
}