
using Aspose.Cells;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Machine_API._Accessor;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Machine_API.Models.MachineCheckList;
using Machine_API.Resources;
using Microsoft.EntityFrameworkCore;

namespace Machine_API._Service.service
{
    public class CellPlnoService : ICellPlnoService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IMapper _mapper;
        private readonly LocalizationService _languageService;

        public CellPlnoService(IMachineRepositoryAccessor repository, MapperConfiguration mapperConfiguration, IMapper mapper, LocalizationService languageService)
        {
            _repository = repository;
            _mapperConfiguration = mapperConfiguration;
            _mapper = mapper;
            _languageService = languageService;
        }

        public async Task<List<Hp_a15Dto>> GetAllCellPlno()
        {
            var data = await _repository.hp_a15.FindAll().ProjectTo<Hp_a15Dto>(_mapperConfiguration).ToListAsync();
            return data;
        }

        public async Task<List<InventoryLineDto>> GetListPlnoByBuildingToInventory(string id, string checkGetData = "building")
        {
            if (checkGetData == "building")
            {
                var listDataPlano = await GetListPlnoByBuildingID(id.ToInt());
                return await GetListPlnoToInventory(listDataPlano);
            }
            else
            {
                var listDataPlano = await GetListPlanoByCellIDV2(id);
                return await GetListPlnoToInventory(listDataPlano);
            }

        }

        public async Task<List<Hp_a15Dto>> GetListPlanoByCellIDV2(string cellCode)
        {
            var cell = _repository.Cells.FindAll(x => x.CellCode == cellCode);
            var cellPlno = _repository.Cell_Plno.FindAll();
            var hp_a15 = _repository.hp_a15.FindAll();

            var dataJoin = cell.Join(cellPlno, x => x.CellID, y => y.CellID, (x, y)
            => new
            {
                x.BuildingID,
                y.Plno
            }).Join(hp_a15, x => x.Plno, z => z.Plno, (x, z) => new Hp_a15Dto
            {
                Plno = z.Plno,
                Place = z.Place
            });
            return await dataJoin.ToListAsync();
        }

        public async Task<List<Hp_a15Dto>> GetListPlnoByBuildingAndCellID(int buildingID, string cellCode)
        {
            var cell = _repository.Cells.FindAll(x => x.BuildingID == buildingID && x.CellCode == cellCode);
            var cellPlno = _repository.Cell_Plno.FindAll();
            var hp_a15 = _repository.hp_a15.FindAll();

            var dataJoin = cell.Join(cellPlno, x => x.CellID, y => y.CellID, (x, y)
            => new
            {
                x.BuildingID,
                y.Plno
            }).Join(hp_a15, x => x.Plno, z => z.Plno, (x, z) => new Hp_a15Dto
            {
                Plno = z.Plno,
                Place = z.Place
            });
            return await dataJoin.ToListAsync();
        }

        public async Task<List<Hp_a15Dto>> GetListPlnoByBuildingID(int buildingID)
        {
            var cell = _repository.Cells.FindAll(x => buildingID == -100 ? true : x.BuildingID == buildingID);
            var cellPlno = _repository.Cell_Plno.FindAll();
            var hp_a15 = _repository.hp_a15.FindAll();

            var dataJoin = cell.Join(cellPlno, x => x.CellID, y => y.CellID, (x, y)
            => new
            {
                x.BuildingID,
                y.Plno
            }).Join(hp_a15, x => x.Plno, z => z.Plno, (x, z) => new Hp_a15Dto
            {
                Plno = z.Plno,
                Place = z.Place
            });
            return await dataJoin.ToListAsync();
        }

        public async Task<List<Hp_a15Dto>> GetListPlnoByCellID(int cellID)
        {
            var cell = _repository.Cells.FindAll(x => x.CellID == cellID);
            var cellPlno = _repository.Cell_Plno.FindAll();
            var hp_a15 = _repository.hp_a15.FindAll();

            var dataJoin = cell.Join(cellPlno, x => x.CellID, y => y.CellID, (x, y)
            => new
            {
                x.BuildingID,
                y.Plno
            }).Join(hp_a15, x => x.Plno, z => z.Plno, (x, z) => new Hp_a15Dto
            {
                Plno = z.Plno,
                Place = z.Place
            });
            return await dataJoin.ToListAsync();
        }

        public async Task<List<Hp_a15Dto>> GetListPlnoByPDCID(int pdcID)
        {
            var cell = _repository.Cells.FindAll(x => x.PDCID == pdcID);
            var cellPlno = _repository.Cell_Plno.FindAll();
            var hp_a15 = _repository.hp_a15.FindAll();

            var dataJoin = cell.Join(cellPlno, x => x.CellID, y => y.CellID, (x, y)
            => new
            {
                x.BuildingID,
                y.Plno
            }).Join(hp_a15, x => x.Plno, z => z.Plno, (x, z) => new Hp_a15Dto
            {
                Plno = z.Plno,
                Place = z.Place
            });
            return await dataJoin.ToListAsync();
        }

        public async Task<List<InventoryLineDto>> GetListPlnoToInventory(List<Hp_a15Dto> listPlano)
        {
            var queryHistoryInven = _repository.HistoryInventory
                .FindAll(x => x.CreateTime.Value.Month == DateTime.Now.Month &&
                         x.CreateTime.Value.Year == DateTime.Now.Year)
                .OrderByDescending(x => x.CreateTime);

            List<InventoryLineDto> listResult = new List<InventoryLineDto>();
            foreach (var item in listPlano)
            {
                InventoryLineDto result = new InventoryLineDto();

                result.PlnoId = item.Plno;
                result.PlnoName = item.Place.Trim();

                var check = await queryHistoryInven.Where(x => x.PlnoID.Trim() == item.Plno.Trim()).ToListAsync();

                var checkSoKiem = check.FirstOrDefault(x => x.InventoryType == 1);
                var checkPhucKiem = check.FirstOrDefault(x => x.InventoryType == 2);
                var checkRutKiem = check.FirstOrDefault(x => x.InventoryType == 3);

                if (checkSoKiem == null)
                {
                    result.TimeSoKiem = null;
                }
                else
                {
                    result.TimeSoKiem = checkSoKiem.CreateTime;
                    result.PecenMatchSoKiem = (((double)checkSoKiem.CountComplete / (checkSoKiem.CountComplete + checkSoKiem.CountNotScan)) * 100).ToString("0.##");
                }
                if (checkPhucKiem == null)
                {
                    result.TimePhucKiem = null;
                }
                else
                {
                    result.TimePhucKiem = checkPhucKiem.CreateTime;
                    result.PecenMatchPhucKiem = (((double)checkPhucKiem.CountComplete / (checkPhucKiem.CountComplete + checkPhucKiem.CountNotScan)) * 100).ToString("0.##");
                }

                if (checkRutKiem == null)
                {
                    result.TimeRutKiem = null;
                }
                else
                {
                    result.TimeRutKiem = checkRutKiem.CreateTime;
                    result.PecenMatchRutKiem = (((double)checkRutKiem.CountComplete / (checkRutKiem.CountComplete + checkRutKiem.CountNotScan)) * 100).ToString("0.##");
                }

                listResult.Add(result);
            }
            return listResult;
        }

        public async Task<Hp_a15Dto> GetPlace(string plno)
        {
            return await _repository.hp_a15.FindAll(x => x.Plno == plno).ProjectTo<Hp_a15Dto>(_mapperConfiguration).FirstOrDefaultAsync();
        }

        public async Task<PageListUtility<Cell_PlnoDto>> GetListCellPlno(PaginationParams pagination)
        {
            var cellPlno = _repository.Cell_Plno.FindAll();
            var cell = _repository.Cells.FindAll();
            var hp_a15 = _repository.hp_a15.FindAll();

            var dataJoin = cellPlno.GroupJoin(cell,
                                        x => x.CellID,
                                        y => y.CellID,
                                        (x, y) => new { x, y })
                                        .SelectMany(x =>


                                            x.y.DefaultIfEmpty(),
                                            (x, y) => new { x.x, y })
                                            .GroupJoin(hp_a15,
                                                x => x.x.Plno,
                                                y => y.Plno,
                                                (x, y) => new { x, y })
                                                .SelectMany(x =>
                                                    x.y.DefaultIfEmpty(),
                                                    (x, y) => new Cell_PlnoDto
                                                    {
                                                        CellID = x.x.x.CellID,
                                                        CellName = x.x.y.CellName,
                                                        CellCode = x.x.y.CellCode,
                                                        ID = x.x.x.ID,
                                                        Place = y.Place,
                                                        Plno = y.Plno
                                                    }
                                                );
            return await PageListUtility<Cell_PlnoDto>.PageListAsync(dataJoin.OrderByDescending(x => x.ID), pagination.PageNumber, pagination.PageSize);
        }

        public async Task<PageListUtility<Cell_PlnoDto>> searchCellPlno(PaginationParams pagination, string keyword)
        {
            var cellPlno = _repository.Cell_Plno.FindAll();
            var cell = _repository.Cells.FindAll();
            var hp_a15 = _repository.hp_a15.FindAll();

            var dataJoin = cellPlno.GroupJoin(cell,
                                        x => x.CellID,
                                        y => y.CellID,
                                        (x, y) => new { x, y })
                                        .SelectMany(x =>
                                            x.y.DefaultIfEmpty(),
                                            (x, y) => new { x.x, y })
                                            .GroupJoin(hp_a15,
                                                x => x.x.Plno,
                                                y => y.Plno,
                                                (x, y) => new { x, y })
                                                .SelectMany(x =>
                                                    x.y.DefaultIfEmpty(),
                                                    (x, y) => new Cell_PlnoDto
                                                    {
                                                        CellID = x.x.x.CellID,
                                                        CellName = x.x.y.CellName,
                                                        CellCode = x.x.y.CellCode,
                                                        ID = x.x.x.ID,
                                                        Place = y.Place,
                                                        Plno = y.Plno
                                                    }
                                                );
            if (!String.IsNullOrEmpty(keyword))
            {
                dataJoin = dataJoin.Where(x => x.CellName.ToLower().Contains(keyword.ToLower())
                    || x.CellCode.ToLower().Contains(keyword.ToLower())
                    || x.Place.ToLower().Contains(keyword.ToLower())
                    || x.Plno.ToLower().Contains(keyword.ToLower()))
                    .OrderByDescending(x => x.CellID);
            };
            return await PageListUtility<Cell_PlnoDto>.PageListAsync(dataJoin.OrderByDescending(x => x.ID), pagination.PageNumber, pagination.PageSize);

        }

        public async Task<OperationResult> AddCellPlno(Cell_PlnoDto model_Dto, string lang)
        {
            var itemCellPlno = _repository.Cell_Plno.FirstOrDefault(x => x.CellID == model_Dto.CellID && x.Plno == model_Dto.Plno);
            if (itemCellPlno != null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = lang == "vi-VN" ? "Cell Plno đã tồn tại! Vui lòng thử lại" :
                    (lang == "en-US" ? "Cell Plno already exists! Please try again" :
                    (lang == "zh-TW" ? "Cell Plno 已經有了！ 請再試" : "Cell_Plno sudah ada! silakan coba lagi"))
                };
            }
            else
            {
                var model = _mapper.Map<Cell_Plno>(model_Dto);
                _repository.Cell_Plno.Add(model);
                try
                {
                    await _repository.SaveChangesAsync();
                    return new OperationResult
                    {
                        Success = true,
                        Message = lang == "vi-VN" ? "Bạn đã Thêm Cell Plno thành công" :
                        (lang == "en-US" ? "You have successfully added Cell Plno?" :
                        (lang == "zh-TW" ? "您已經加入 Cell Plno 成功" : "Anda telah berhasil menambahkan Cell Plno"))
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

        public async Task<OperationResult> UpdateCellPlno(Cell_PlnoDto model_Dto, string lang)
        {
            if (!await IsExists(model_Dto))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = lang == "vi-VN" ? "Cell Plno không được tìm thấy! Vui lòng chọn cell Plno khác" :
                    (lang == "en-US" ? "Cell_Plno not found!Please select another Cell_Plno" :
                    (lang == "zh-TW" ? "找不到Cell Plno! 請您選別的Cell Plno" : "Cell_Plno tidak ditemukan! Silakan pilih Cell_Plno lain"))
                };
            }


            var itemCellPlno = _repository.Cell_Plno.FirstOrDefault(x => x.CellID == model_Dto.CellID && x.Plno == model_Dto.Plno);
            if (itemCellPlno != null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = lang == "vi-VN" ? "Cell Plno đã tồn tại! Vui lòng thử lại" :
                    (lang == "en-US" ? "Cell Plno already exists! Please try again" :
                    (lang == "zh-TW" ? "Cell Plno 已經有了！ 請再試" : "Cell_Plno sudah ada! silakan coba lagi"))
                };
            }

            var model = await _repository.Cell_Plno.FirstOrDefaultAsync(x => x.ID == model_Dto.ID);

            model.CellID = model_Dto.CellID;
            model.Plno = model_Dto.Plno;
            model.UpdateBy = model_Dto.UpdateBy;
            model.UpdateTime = DateTime.Now;
            _repository.Cell_Plno.Update(model);
            try
            {
                await _repository.SaveChangesAsync();
                return new OperationResult
                {
                    Success = true,
                    Message = lang == "vi-VN" ? "Bạn đã cập nhật Cell Plno thành công!" :
                    (lang == "en-US" ? "You have successfully updated Cell Plno" :
                    (lang == "zh-TW" ? "您已經更新Cell Plno 成功！" : "Anda telah berhasil memperbarui Cell Plno!"))
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

        public async Task<OperationResult> RemoveCellPlno(Cell_PlnoDto model_Dto, string lang)
        {
            var itemCellPlno = _repository.Cell_Plno.FirstOrDefault(x => x.CellID == model_Dto.CellID && x.Plno == model_Dto.Plno);
            if (itemCellPlno != null)
            {
                _repository.Cell_Plno.Remove(itemCellPlno);
                try
                {
                    await _repository.SaveChangesAsync();
                    return new OperationResult
                    {
                        Success = true,
                        Message = lang == "vi-VN" ? "Bạn đã xóa Cell Plno thành công!" :
                        (lang == "en-US" ? "You have successfully deleted Cell Plno!" :
                        (lang == "zh-TW" ? "您已經刪除Cell Plno 成功" : "Anda telah berhasil menghapus Cell Plno!"))
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
                    Message = lang == "vi-VN" ? "Cell Plno không được tìm thấy! Vui lòng chọn cell Plno khác" :
                    (lang == "en-US" ? "Cell_Plno not found!Please select another Cell_Plno" :
                    (lang == "zh-TW" ? "找不到Cell Plno! 請您選別的Cell Plno" : "Cell Plno tidak ditemukan! Silakan pilih sel Plno lain"))
                };
            }
        }

        public async Task<List<CellPlnoExportDto>> ExportExcelCellPlno()
        {
            var cellPlno = _repository.Cell_Plno.FindAll();
            var cell = _repository.Cells.FindAll();
            var hp_a15 = _repository.hp_a15.FindAll();

            var dataJoin = cellPlno.GroupJoin(cell,
                                        x => x.CellID,
                                        y => y.CellID,
                                        (x, y) => new { x, y })
                                        .SelectMany(x =>
                                            x.y.DefaultIfEmpty(),
                                            (x, y) => new { x.x, y })
                                            .GroupJoin(hp_a15,
                                                x => x.x.Plno,
                                                y => y.Plno,
                                                (x, y) => new { x, y })
                                                .SelectMany(x =>
                                                    x.y.DefaultIfEmpty(),
                                                    (x, y) => new CellPlnoExportDto
                                                    {
                                                        CellName = x.x.y.CellName.Trim(),
                                                        CellCode = x.x.y.CellCode.Trim(),
                                                        Place = y.Place.Trim(),
                                                        Plno = y.Plno.Trim()
                                                    }
                                                );

            return await dataJoin.ToListAsync();
        }

        public async Task<bool> IsExists(Cell_PlnoDto model_Dto)
        {
            return await Task.FromResult(_repository.Cell_Plno.FindAll(x => x.ID == model_Dto.ID).Any());
        }

        public void PutStaticValue(ref Worksheet ws)
        {
            ws.Cells["A1"].PutValue(_languageService.GetLocalizedHtmlString("cellplno_cell_name"));
            ws.Cells["B1"].PutValue(_languageService.GetLocalizedHtmlString("cellplno_cell_code"));
            ws.Cells["C1"].PutValue(_languageService.GetLocalizedHtmlString("cellplno_plno"));
            ws.Cells["D1"].PutValue(_languageService.GetLocalizedHtmlString("cellplno_place"));
        }

        public async Task<List<Hp_a15Dto>> GetListPlnoByMultipleBuildingID(string[] listBuildingID)
        {
            List<Hp_a15Dto> hp_a15List = new List<Hp_a15Dto>();
            var list = listBuildingID.Where(x => x.ToString() != "100");

            foreach (var item in list)
            {
                hp_a15List.AddRange(GetListCellPlno(item.ToInt()));
            }
            return await Task.FromResult(hp_a15List.ToList());
        }

        public List<Hp_a15Dto> GetListCellPlno(int buildingID)
        {
            var cell = _repository.Cells.FindAll(x => x.BuildingID == buildingID);
            var cellPlno = _repository.Cell_Plno.FindAll();
            var hp_a15 = _repository.hp_a15.FindAll();
            var building = _repository.Building.FindAll(x => x.BuildingID == buildingID);

            var dataJoin = cell.Join(building, x => x.BuildingID, y => y.BuildingID, (x, y)
            => new
            {
                x.CellID,
                y.BuildingID,
                y.BuildingCode
            }).Join(cellPlno, x => x.CellID, y => y.CellID, (x, y)
            => new
            {
                x.BuildingID,
                y.Plno,
                x.BuildingCode
            }).Join(hp_a15, x => x.Plno, z => z.Plno, (x, z) => new Hp_a15Dto
            {
                Plno = z.Plno + "_" + x.BuildingID,
                BuildingCode = x.BuildingCode,
                PlnoCode = z.Plno,
                Place = z.Plno.Trim() + '-' + z.Place.Trim() + " (" + x.BuildingCode + ")"
            });
            return dataJoin.ToList();
        }

        public async Task<List<Hp_a15Dto>> GetListPlnoByMultipleCellID(string[] listCellID)
        {
            List<Hp_a15Dto> hp_a15List = new List<Hp_a15Dto>();
            foreach (var item in listCellID)
            {
                hp_a15List.AddRange(GetListCellPlnoWithCellId(item));
            }
            return await Task.FromResult(hp_a15List.ToList());
        }

        public List<Hp_a15Dto> GetListCellPlnoWithCellId(string cellCode)
        {
            var buiding = _repository.Building.FirstOrDefault(x => x.BuildingID == 100);
            var cellList = _repository.Cells.FindAll();
            var cellPlnoList = _repository.Cell_Plno.FindAll();
            var hp_a15List = _repository.hp_a15.FindAll();
            var buildingList = _repository.Building.FindAll();

            var dataJoin = cellList.Join(buildingList, x => x.BuildingID, y => y.BuildingID, (x, y)
            => new
            {
                x.CellID,
                x.CellCode,
                y.BuildingID,
                y.BuildingCode
            }).Join(cellPlnoList, x => x.CellID, y => y.CellID, (x, y)
            => new
            {
                x.CellID,
                x.CellCode,
                x.BuildingID,
                y.Plno,
                x.BuildingCode
            }).Where(x => x.CellCode == cellCode).Join(hp_a15List, x => x.Plno, z => z.Plno, (x, z) => new Hp_a15Dto
            {
                Plno = z.Plno + "_" + buiding.BuildingID + "_" + x.CellID + "_" + x.CellCode,
                BuildingCode = buiding.BuildingCode,
                CellName = x.CellCode,
                PlnoCode = z.Plno,
                Place = z.Plno.Trim() + '-' + z.Place.Trim() + " (" + buiding.BuildingCode + " | " + x.CellCode + ")"
            }).OrderBy(x => x.PlnoCode);
            return dataJoin.ToList();
        }
        public async Task<List<Hp_a15Dto>> GetListPlnoByMultipleID(FilterListDTO listAll)
        {
            List<Hp_a15Dto> hp_a15List = new List<Hp_a15Dto>();
            foreach (var item in listAll.ListCell)
            {
                hp_a15List.AddRange(GetListCellPlnoWithCellId(item));
            }
            foreach (var item in listAll.ListBuilding.Where(x => x.ToString() != "100"))
            {
                hp_a15List.AddRange(GetListCellPlno(item.ToInt()));
            }
            return await Task.FromResult(hp_a15List.OrderBy(x => x.BuildingCode).ToList());
        }
    }
}