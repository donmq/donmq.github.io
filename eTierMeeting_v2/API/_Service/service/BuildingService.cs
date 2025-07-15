
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Microsoft.EntityFrameworkCore;
using Machine_API.Helpers.Utilities;
using Machine_API.Helpers.Params;
using Machine_API.Models.MachineCheckList;
using Machine_API._Accessor;
using Machine_API.Resources;
using LinqKit;

namespace Machine_API._Service.service
{
    public class BuildingService : IBuildingService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;

        public BuildingService(
            IMachineRepositoryAccessor repository,
            IMapper mapper,
            MapperConfiguration mapperConfiguration)
        {
            _repository = repository;
            _mapper = mapper;
            _mapperConfiguration = mapperConfiguration;
        }

        public async Task<OperationResult> AddBuilding(BuildingDto model_Dto, string lang)
        {
            var itemBuilding = _repository.Building.FirstOrDefault(x => x.BuildingCode == model_Dto.BuildingCode);
            if (itemBuilding != null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = lang == "vi-VN" ? "Tòa nhà đã tồn tại! Vui lòng chọn tòa nhà khác" :
                    (lang == "en-US" ? "Building already exists !. Please select another Building" :
                    (lang == "zh-TW" ? "單位已經有了！請您選別的單位" : "Bangunan itu sudah ada! Silakan pilih bangunan lain"))
                };
            }
            else
            {
                var model = _mapper.Map<Building>(model_Dto);
                _repository.Building.Add(model);
                try
                {
                    await _repository.SaveChangesAsync();
                    return new OperationResult
                    {
                        Success = true,
                        Message = lang == "vi-VN" ? "Bạn đã thêm tòa nhà thành công" :
                        (lang == "en-US" ? "You have successfully added building" :
                        (lang == "zh-TW" ? "您已經加入單位成功" : "Anda telah berhasil menambahkan bangunan"))
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

        public async Task<List<BuildingDto>> GetAllBuilding()
        {
            var data = await _repository.Building.FindAll(x => x.Visible == true)
                .OrderBy(x => x.BuildingCode)
                .ProjectTo<BuildingDto>(_mapperConfiguration).ToListAsync();
            return data;
        }

        public async Task<List<BuildingDto>> GetAllBuildingByID(int idPDC)
        {
            var queryCell = _repository.Cells.FindAll(x => x.PDCID == idPDC);
            var queryBuilding = _repository.Building.FindAll(x => x.Visible == true);
            var dataQuery = await (queryCell.GroupJoin(queryBuilding, x => x.BuildingID, y => y.BuildingID, (x, y) => new
            {
                Building = y,
                Cell = x
            }).SelectMany(x => x.Building.DefaultIfEmpty(), (x, y) => new BuildingDto
            {
                BuildingCode = y.BuildingCode,
                BuildingID = y.BuildingID,
                BuildingName = y.BuildingName,
                Visible = y.Visible
            })).ToListAsync();
            return dataQuery;
        }

        public async Task<List<BuildingDto>> GetBuildingByCellCode(string cellCode)
        {
            var building = _repository.Building.FindAll();
            var cell = _repository.Cells.FindAll(x => x.CellCode == cellCode);

            var dataJoin = cell.Join(building, x => x.BuildingID, y => y.BuildingID, (x, y)
            => new BuildingDto
            {
                BuildingID = y.BuildingID,
                BuildingCode = y.BuildingCode,
                BuildingName = y.BuildingName,
                Visible = y.Visible
            });

            return await dataJoin.Distinct().ToListAsync();
        }

        public async Task<List<BuildingDto>> GetBuildingByCellCodeAndPDC(string cellCode, int? idPDC = 0)
        {
            var building = _repository.Building.FindAll();
            var cell = _repository.Cells.FindAll(x => x.CellCode == cellCode);

            if (idPDC > 0)
                cell = cell.Where(x => x.PDCID == idPDC);
            var dataJoin = cell.Join(building, x => x.BuildingID, y => y.BuildingID, (x, y)
            => new BuildingDto
            {
                BuildingID = y.BuildingID,
                BuildingCode = y.BuildingCode,
                BuildingName = y.BuildingName,
                Visible = y.Visible
            });

            return await dataJoin.Distinct().ToListAsync();
        }

        public async Task<List<BuildingDto>> GetBuildingByPdcID(int idPDC)
        {
            var building = _repository.Building.FindAll();
            var cell = _repository.Cells.FindAll(x => x.PDCID == idPDC);

            var dataJoin = cell.Join(building, x => x.BuildingID, y => y.BuildingID, (x, y)
            => new BuildingDto
            {
                BuildingID = y.BuildingID,
                BuildingCode = y.BuildingCode,
                BuildingName = y.BuildingName,
                Visible = y.Visible
            });

            return await dataJoin.Distinct().ToListAsync();
        }

        public async Task<PageListUtility<BuildingDto>> GetListBuilding(PaginationParams paginationParams)
        {
            var data = _repository.Building.FindAll(x => x.Visible == true)
                .ProjectTo<BuildingDto>(_mapperConfiguration).OrderBy(x => x.BuildingName);
            return await PageListUtility<BuildingDto>.PageListAsync(data, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<OperationResult> RemoveBuilding(BuildingDto model_Dto, string lang)
        {
            var itemBuilding = _repository.Building.FirstOrDefault(x => x.BuildingID == model_Dto.BuildingID);
            if (itemBuilding != null)
            {
                _repository.Building.Remove(itemBuilding);
                try
                {
                    await _repository.SaveChangesAsync();
                    return new OperationResult
                    {
                        Success = true,
                        Message = lang == "vi-VN" ? "Bạn đã xóa tòa nhà thành công" :
                        (lang == "en-US" ? "You have deleted the Building successfully" :
                        (lang == "zh-TW" ? "您已經刪除單位成功" : "Anda telah berhasil membersihkan gedung"))
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
                    Message = lang == "vi-VN" ? "Tòa nhà không được tìm thấy! Vui lòng chọn tòa nhà khác" :
                    (lang == "en-US" ? "Building not found! Please choose another Building" :
                    (lang == "zh-TW" ? "找不到單位！請您選別的單位 " : "Bangunan tidak ditemukan! Silakan pilih bangunan lain"))
                };
            }
        }

        public async Task<PageListUtility<BuildingDto>> SearchBuilding(PaginationParams paginationParams, string keyword)
        {
            var predicate = PredicateBuilder.New<Building>(true);

            if (!String.IsNullOrEmpty(keyword))
                predicate.And(x => x.BuildingCode.ToLower().Contains(keyword.ToLower()) || x.BuildingName.ToLower().Contains(keyword.ToLower()));
                
            var buildingQuery = _repository.Building.FindAll(predicate).OrderByDescending(x => x.BuildingID).ProjectTo<BuildingDto>(_mapperConfiguration);

            return await PageListUtility<BuildingDto>.PageListAsync(buildingQuery, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<OperationResult> UpdateBuilding(BuildingDto model_Dto, string lang)
        {
            var model = await _repository.Building.FirstOrDefaultAsync(x => x.BuildingID == model_Dto.BuildingID);
            model.BuildingCode = model_Dto.BuildingCode.Trim();
            model.BuildingName = model_Dto.BuildingName.Trim();

            _repository.Building.Update(model);
            try
            {
                await _repository.SaveChangesAsync();
                return new OperationResult
                {
                    Success = true,
                    Message = lang == "vi-VN" ? "Bạn đã cập nhật tòa nhà thành công" :
                    (lang == "en-US" ? "You have updated the building successfully" :
                    (lang == "zh-TW" ? "您已經更新單位成功" : "Anda telah berhasil memperbarui gedung"))
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