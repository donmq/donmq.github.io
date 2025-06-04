using API._Repositories;
using API._Services.Interfaces.Manage;
using API.Dtos.Manage.PositionManage;
using API.Helpers.Enums;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace API._Services.Services.Manage
{
    public class PositionManageService : IPositionManageService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryAccessor _repoAccessor;

        public PositionManageService(IMapper mapper, IRepositoryAccessor repoAccessor)
        {
            _mapper = mapper;
            _repoAccessor = repoAccessor;
        }

        public async Task<OperationResult> AddPosition(PositionManageDto positionAndPosLang)
        {
            if (positionAndPosLang == null)
            {
                return new OperationResult(false, MessageConstants.ADD_ERROR, MessageConstants.ERROR);
            }
            //Add Position
            PositionManageDto positionManage = new()
            {
                PositionName = positionAndPosLang.PositionVN + " - " + positionAndPosLang.PositionTW,
                PositionSym = positionAndPosLang.PositionSym == "" ? null : positionAndPosLang.PositionSym
            };
            Position addPosition = _mapper.Map<Position>(positionManage);
            _repoAccessor.Position.Add(addPosition);
            try
            {
                //Add List PosLang
                await _repoAccessor.SaveChangesAsync();
                int id = _repoAccessor.Position.FindAll().OrderByDescending(x => x.PositionID).Select(x => x.PositionID).FirstOrDefault();
                List<PosLangManageDto> listPosLangManage = new();
                PosLangManageDto posLangManageVN = new()
                {
                    PositionName = positionAndPosLang.PositionVN ?? "",
                    LanguageID = LangConstants.VN,
                    PositionID = id
                };
                listPosLangManage.Add(posLangManageVN);
                PosLangManageDto posLangManageEN = new()
                {
                    PositionName = positionAndPosLang.PositionEN ?? "",
                    LanguageID = LangConstants.EN,
                    PositionID = id
                };
                listPosLangManage.Add(posLangManageEN);
                PosLangManageDto posLangManageTW = new()
                {
                    PositionName = positionAndPosLang.PositionTW ?? "",
                    LanguageID = LangConstants.ZH_TW,
                    PositionID = id
                };
                listPosLangManage.Add(posLangManageTW);

                List<PosLang> addPosLang = _mapper.Map<List<PosLang>>(listPosLangManage);
                _repoAccessor.PosLang.AddMultiple(addPosLang);
                try
                {
                    await _repoAccessor.SaveChangesAsync();
                    return new OperationResult(true, MessageConstants.ADD_SUCCESS, MessageConstants.SUCCESS);
                }
                catch (System.Exception)
                {
                    return new OperationResult(false, MessageConstants.ADD_ERROR, MessageConstants.ERROR);
                }
            }
            catch (System.Exception)
            {
                return new OperationResult(false, MessageConstants.ADD_ERROR, MessageConstants.ERROR);
            }
        }

        public async Task<OperationResult> RemovePosition(int IDPosition)
        {
            Position checkID = _repoAccessor.Position.FirstOrDefault(x => x.PositionID == IDPosition);
            if (checkID == null)
            {
                return new OperationResult(false, MessageConstants.REMOVE_ERROR, MessageConstants.ERROR);
            }
            else
            {
                List<PosLang> posLangs = _repoAccessor.PosLang.FindAll(x => x.PositionID == checkID.PositionID).ToList();
                if (posLangs.Any())
                {
                    _repoAccessor.PosLang.RemoveMultiple(posLangs);
                }
                //Remove Position
                _repoAccessor.Position.Remove(checkID);
                try
                {
                    await _repoAccessor.SaveChangesAsync();
                    return new OperationResult(true, MessageConstants.REMOVE_SUCCESS, MessageConstants.SUCCESS);
                }
                catch (System.Exception)
                {
                    return new OperationResult(false, MessageConstants.REMOVE_ERROR, MessageConstants.ERROR);
                }

            }
        }
        public async Task<List<PositionManageDto>> GetPosition()
        {
            return await _repoAccessor.Position.FindAll()
                .Select(x => new PositionManageDto
                {
                    PositionID = x.PositionID,
                    PositionName = x.PositionName,
                    PositionSym = x.PositionSym,
                }).ToListAsync();
        }


        public async Task<PaginationUtility<PositionManageDto>> GetAllPosition(PaginationParam pagination, bool isPaging = true)
        {
            return PaginationUtility<PositionManageDto>.Create(await GetPosition(), pagination.PageNumber, pagination.PageSize, isPaging);
        }

        public async Task<OperationResult> Download(PositionManageDto dto)
        {
            var data = await GetPosition();
            List<Table> dataTable = new List<Table>
            {
                new Table("result", data)
            };
            List<Cell> dataTitle = new List<Cell>
            {
                new Cell("A1", dto.PositionNameExcel),
                new Cell("B1", dto.PositionSymExcel),
            };
            ExcelResult excelResult = ExcelUtility.DownloadExcel(dataTable, dataTitle, "Resources\\Template\\Manage\\PositionManage.xlsx");
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

        public string GetNamePosition(int IDCheck, string lang)
        {
            return _repoAccessor.PosLang.FirstOrDefault(x => x.PositionID == IDCheck && x.LanguageID == lang)?.PositionName;
        }

        public async Task<OperationResult> EditPosition(PositionManageDto positionAndPosLang)
        {
            Position checkID = _repoAccessor.Position.FindAll(x => x.PositionID == positionAndPosLang.PositionID, true).FirstOrDefault();
            if (checkID == null)
            {
                return new OperationResult(false, MessageConstants.UPDATE_ERROR, MessageConstants.ERROR);
            }
            else
            {
                //Edit Position
                PositionManageDto positionManageEdit = new()
                {
                    PositionID = positionAndPosLang.PositionID,
                    PositionName = positionAndPosLang.PositionVN + " - " + positionAndPosLang.PositionTW,
                    PositionSym = positionAndPosLang.PositionSym == "" ? null : positionAndPosLang.PositionSym
                };
                Position addPosition = _mapper.Map<Position>(positionManageEdit);
                _repoAccessor.Position.Update(addPosition);

                //Edit List PosLang
                List<PosLang> checkListPosLang = _repoAccessor.PosLang.FindAll(x => x.PositionID == checkID.PositionID).ToList();

                checkListPosLang[0].PosLangID = checkListPosLang[0].PosLangID;
                checkListPosLang[0].PositionName = positionAndPosLang.PositionVN ?? "";

                checkListPosLang[1].PosLangID = checkListPosLang[1].PosLangID;
                checkListPosLang[1].PositionName = positionAndPosLang.PositionEN ?? "";

                checkListPosLang[2].PosLangID = checkListPosLang[2].PosLangID;
                checkListPosLang[2].PositionName = positionAndPosLang.PositionTW ?? "";

                List<PosLang> editPosLang = _mapper.Map<List<PosLang>>(checkListPosLang);
                _repoAccessor.PosLang.UpdateMultiple(editPosLang);
                try
                {
                    await _repoAccessor.SaveChangesAsync();
                    return new OperationResult(true, MessageConstants.UPDATE_SUCCESS, MessageConstants.SUCCESS);
                }
                catch (System.Exception)
                {
                    return new OperationResult(false, MessageConstants.UPDATE_ERROR, MessageConstants.ERROR);
                }
            }
        }

        public async Task<PositionManageDto> GetDetailPosition(int IDPosition)
        {
            PositionManageDto data = await _repoAccessor.Position.FindAll(x => x.PositionID == IDPosition)
                .Select(x => new PositionManageDto
                {
                    PositionID = x.PositionID,
                    PositionName = x.PositionName,
                    PositionSym = x.PositionSym,
                }).FirstOrDefaultAsync();
            if (data != null)
            {
                data.PositionVN = GetNamePosition(data.PositionID, LangConstants.VN) ?? data.PositionName;
                data.PositionEN = GetNamePosition(data.PositionID, LangConstants.EN) ?? data.PositionName;
                data.PositionTW = GetNamePosition(data.PositionID, LangConstants.ZH_TW) ?? data.PositionName;
            }
            else
            {
                return null;
            }
            return data;
        }
    }
}