
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
    public class CellService : ICellService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;

        public CellService(IMachineRepositoryAccessor repository, IMapper mapper, MapperConfiguration mapperConfiguration)
        {
            _repository = repository;
            _mapper = mapper;
            _mapperConfiguration = mapperConfiguration;
        }

        public async Task<OperationResult> AddCell(CellDto model_Dto, string lang)
        {
            var itemCell = _repository.Cells.FirstOrDefault(x => x.CellCode == model_Dto.CellCode && x.BuildingID == model_Dto.BuildingID);
            if (itemCell != null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = lang == "vi-VN" ? "Cell đã tồn tại! Vui lòng chọn cell khác" :
                    (lang == "en-US" ? "Unit already exists !. Please select another Unit" :
                    (lang == "zh-TW" ? "單位已經有了！請您選別的單位" : "Cell sudah ada! Silakan pilih Cell lain"))
                };
            }
            else
            {
                var model = _mapper.Map<Cells>(model_Dto);
                _repository.Cells.Add(model);
                try
                {
                    await _repository.SaveChangesAsync();
                    return new OperationResult
                    {
                        Success = true,
                        Message = lang == "vi-VN" ? "Bạn đã thêm  Cell thành công" :
                        (lang == "en-US" ? "You have successfully added units" :
                        (lang == "zh-TW" ? "您已經加入單位成功" : "Anda telah berhasil menambahkan Cell"))
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

        public async Task<List<CellExportDto>> ExportExcelCell()
        {
            var cell = _repository.Cells.FindAll();
            var building = _repository.Building.FindAll();
            var pdc = _repository.PDC.FindAll();

            var dataJoin = cell.GroupJoin(building,
                                        x => x.BuildingID,
                                        y => y.BuildingID,
                                        (x, y) => new { x, y })
                                        .SelectMany(x =>
                                            x.y.DefaultIfEmpty(),
                                            (x, y) => new { x.x, y })
                                            .GroupJoin(pdc,
                                                x => x.x.PDCID,
                                                y => y.PDCID,
                                                (x, y) => new { x, y })
                                                .SelectMany(x =>
                                                    x.y.DefaultIfEmpty(),
                                                    (x, y) => new CellExportDto
                                                    {
                                                        CellCode = x.x.x.CellCode,
                                                        CellName = x.x.x.CellName,
                                                        BuildingName = x.x.y.BuildingName,
                                                        PDCName = y.PDCName
                                                    }
                                                );

            return await dataJoin.ToListAsync();

        }

        public async Task<List<CellDto>> GetAllCell()
        {
            var data = await _repository.Cells.FindAll().Select(x => new CellDto
            {
                CellName = x.CellName,
                CellCode = x.CellCode,
            }
            ).Distinct().OrderByDescending(x => x.CellCode).ToListAsync();
            return data;
        }

        public async Task<PageListUtility<CellDto>> GetListCell(PaginationParams paginationParams)
        {
            var cell = _repository.Cells.FindAll();
            var building = _repository.Building.FindAll();
            var pdc = _repository.PDC.FindAll();

            var dataJoin = cell.GroupJoin(building,
                                        x => x.BuildingID,
                                        y => y.BuildingID,
                                        (x, y) => new { x, y })
                                        .SelectMany(x =>
                                            x.y.DefaultIfEmpty(),
                                            (x, y) => new { x.x, y })
                                            .GroupJoin(pdc,
                                                x => x.x.PDCID,
                                                y => y.PDCID,
                                                (x, y) => new { x, y })
                                                .SelectMany(x =>
                                                    x.y.DefaultIfEmpty(),
                                                    (x, y) => new CellDto
                                                    {
                                                        CellID = x.x.x.CellID,
                                                        CellCode = x.x.x.CellCode,
                                                        CellName = x.x.x.CellName,
                                                        BuildingName = x.x.y.BuildingName,
                                                        PDCName = y.PDCName,
                                                        PDCID = x.x.x.PDCID,
                                                        BuildingID = x.x.y.BuildingID
                                                    }
                                                );
            return await PageListUtility<CellDto>.PageListAsync(dataJoin.OrderByDescending(x => x.CellID), paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<List<CellDto>> GetListCellByBuildingID(int buildingID)
        {
            return await _repository.Cells.FindAll(x => x.BuildingID == buildingID).ProjectTo<CellDto>(_mapperConfiguration).ToListAsync();
        }
        public async Task<List<CellDto>> GetListCellExistPlnoByBuildingID(int buildingID)
        {
            var listCells = _repository.Cells.FindAll(x => x.BuildingID == buildingID);
            return await listCells.ProjectTo<CellDto>(_mapperConfiguration).ToListAsync();
        }
        public async Task<object> GetListCellByPdcID(int pdcID)
        {
            return await _repository.Cells
                .FindAll(x => x.PDCID == pdcID).Select(x => new { x.CellName, x.CellCode })
                .Distinct().ToListAsync();
        }

        public async Task<OperationResult> RemoveCell(CellDto model_Dto, string lang)
        {
            var itemCell = await _repository.Cells.FirstOrDefaultAsync(x => x.CellID == model_Dto.CellID);
            if (itemCell != null)
            {
                _repository.Cells.Remove(itemCell);
                try
                {
                    await _repository.SaveChangesAsync();
                    return new OperationResult
                    {
                        Success = true,
                        Message = lang == "vi-VN" ? "Bạn đã xóa Cell thành công" :
                        (lang == "en-US" ? "You have deleted the location successfully" :
                        (lang == "zh-TW" ? "您已經刪除單位成功" : "Anda telah berhasil menghapus Cell"))
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
                    Message = lang == "vi-VN" ? "Cell không được tìm thấy! Vui lòng chọn cell khác" :
                    (lang == "en-US" ? "Location not found! Please choose another location" :
                    (lang == "zh-TW" ? "找不到單位！請您選別的單位 " : "Cell tidak ditemukan! Silakan pilih Cell lain"))
                };
            }
        }

        public async Task<PageListUtility<CellDto>> SearchCell(PaginationParams paginationParams, string keyword)
        {
            var cell = _repository.Cells.FindAll();
            var building = _repository.Building.FindAll();
            var pdc = _repository.PDC.FindAll();

            var dataJoin = cell.GroupJoin(building,
                                        x => x.BuildingID,
                                        y => y.BuildingID,
                                        (x, y) => new { x, y })
                                        .SelectMany(x =>
                                            x.y.DefaultIfEmpty(),
                                            (x, y) => new { x.x, y })
                                            .GroupJoin(pdc,
                                                x => x.x.PDCID,
                                                y => y.PDCID,
                                                (x, y) => new { x, y })
                                                .SelectMany(x =>
                                                    x.y.DefaultIfEmpty(),
                                                    (x, y) => new CellDto
                                                    {
                                                        CellID = x.x.x.CellID,
                                                        CellCode = x.x.x.CellCode,
                                                        CellName = x.x.x.CellName,
                                                        BuildingID = x.x.y.BuildingID,
                                                        BuildingName = x.x.y.BuildingName,
                                                        PDCID = y.PDCID,
                                                        PDCName = y.PDCName
                                                    }
                                                );

            if (!String.IsNullOrEmpty(keyword))
            {
                dataJoin = dataJoin.Where(x => x.CellName.ToLower().Contains(keyword.ToLower())
                    || x.CellCode.ToLower().Contains(keyword.ToLower())
                    || x.PDCName.ToLower().Contains(keyword.ToLower())
                    || x.BuildingName.ToLower().Contains(keyword.ToLower()))
                    .OrderByDescending(x => x.CellID);
            };
            return await PageListUtility<CellDto>.PageListAsync(dataJoin.OrderByDescending(x => x.CellID), paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<OperationResult> UpdateCell(CellDto model_Dto, string lang)
        {
            var checkCell = _repository.Cells.FirstOrDefault(x => x.CellCode == model_Dto.CellCode && x.BuildingID == model_Dto.BuildingID);
            var itemCell = _repository.Cells.FirstOrDefault(x => x.CellID == model_Dto.CellID);
            if (itemCell != null)
            {
                itemCell.CellCode = model_Dto.CellCode.Trim();
                itemCell.CellName = model_Dto.CellName.Trim();
                itemCell.PDCID = model_Dto.PDCID;
                itemCell.BuildingID = model_Dto.BuildingID;
                itemCell.UpdateBy = model_Dto.UpdateBy;
                itemCell.UpdateTime = DateTime.Now;
                _repository.Cells.Update(itemCell);
                try
                {
                    await _repository.SaveChangesAsync();
                    return new OperationResult
                    {
                        Success = true,
                        Message = lang == "vi-VN" ? "Bạn đã cập nhật Cell thành công" :
                        (lang == "en-US" ? "You have updated the location successfully" :
                        (lang == "zh-TW" ? "您已經更新單位成功" : "Anda telah berhasil memperbarui Cell"))
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
            else
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

        public async Task<List<CellDto>> GetAllCellAdmin()
        {
            var cell = _repository.Cells.FindAll();
            var building = _repository.Building.FindAll();

            var dataJoin = cell.Join(building, x => x.BuildingID, y => y.BuildingID, (x, y)
            => new CellDto
            {
                BuildingID = y.BuildingID,
                BuildingCode = y.BuildingCode,
                CellID = x.CellID,
                CellName = x.CellName,
                CellCode = x.CellCode
            });
            return await dataJoin.ToListAsync();
        }

        public async Task<Cells> GetDataCell(string cellCode)
        {
            return await _repository.Cells.FirstOrDefaultAsync(x => x.CellCode == cellCode);
        }
    }
}