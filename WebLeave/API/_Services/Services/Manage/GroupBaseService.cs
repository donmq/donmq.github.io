using API._Repositories;
using API._Services.Interfaces.Manage;
using API.Dtos.Manage;
using API.Dtos.Manage.GroupBaseManage;
using API.Helpers.Enums;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
namespace API._Services.Services.Manage
{
    public class GroupBaseService : IGroupBaseService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IRepositoryAccessor _repositoryAccessor;

        public GroupBaseService(IMapper mapper, MapperConfiguration configMapper, IRepositoryAccessor repositoryAccessor)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _repositoryAccessor = repositoryAccessor;
        }

        public async Task<OperationResult> AddGroup(GroupBaseAndGroupLangDto groupBaseAndGroupLang)
        {
            if (groupBaseAndGroupLang == null)
            {
                return new OperationResult(false, MessageConstants.ADD_ERROR, MessageConstants.ERROR);
            }
            //Add Group
            GroupBaseDto groupManage = new()
            {
                BaseName = groupBaseAndGroupLang.GBVN != groupBaseAndGroupLang.GBTW
                    ? groupBaseAndGroupLang.GBVN + " - " + groupBaseAndGroupLang.GBTW
                    : groupBaseAndGroupLang.GBVN,
                BaseSym = groupBaseAndGroupLang.BaseSym
            };
            GroupBase addGroup = _mapper.Map<GroupBase>(groupManage);
            _repositoryAccessor.GroupBase.Add(addGroup);

            try
            {
                //Add List GroupLang
                var ID = _repositoryAccessor.GroupBase.FindAll().OrderByDescending(x => x.GBID).Select(x => x.GBID).FirstOrDefault();

                var listGroupLangManage = new List<GroupLangDto>
                {
                    new GroupLangDto { BaseName = groupBaseAndGroupLang.GBVN, LanguageID = "vi", GBID = ID },
                    new GroupLangDto { BaseName = groupBaseAndGroupLang.GBEN, LanguageID = "en", GBID = ID },
                    new GroupLangDto { BaseName = groupBaseAndGroupLang.GBTW, LanguageID = "zh-TW", GBID = ID }
                };
                var addGroupLang = _mapper.Map<List<GroupLang>>(listGroupLangManage);

                _repositoryAccessor.GroupLang.AddMultiple(addGroupLang);
                await _repositoryAccessor.SaveChangesAsync();

                return new OperationResult(true, MessageConstants.ADD_SUCCESS, MessageConstants.SUCCESS);
            }
            catch (System.Exception)
            {
                return new OperationResult(false, MessageConstants.ADD_ERROR, MessageConstants.ERROR);
            }
        }

        public async Task<OperationResult> EditGroup(GroupBaseAndGroupLangDto groupBaseAndGroupLang)
        {
            GroupBase checkID = _repositoryAccessor.GroupBase.FindAll(x => x.GBID == groupBaseAndGroupLang.GBID, true).FirstOrDefault();
            if (checkID == null)
                return new OperationResult(false, MessageConstants.UPDATE_ERROR, MessageConstants.ERROR);

            int groupBaseLangID = checkID.GBID;
            //Edit GroupBase
            GroupBaseDto groupBaseManageEdit = new()
            {
                GBID = groupBaseAndGroupLang.GBID,
                BaseName = groupBaseAndGroupLang.GBVN != groupBaseAndGroupLang.GBTW
                    ? groupBaseAndGroupLang.GBVN + " - " + groupBaseAndGroupLang.GBTW
                    : groupBaseAndGroupLang.GBVN,
                BaseSym = groupBaseAndGroupLang.BaseSym
            };

            GroupBase addGroupBase = _mapper.Map<GroupBase>(groupBaseManageEdit);
            _repositoryAccessor.GroupBase.Update(addGroupBase);

            try
            {
                //Edit List GroupLang
                List<GroupLang> checkListGroupLang = _repositoryAccessor.GroupLang.FindAll(x => x.GBID == groupBaseLangID, true).ToList();

                foreach (var groupLang in checkListGroupLang)
                {
                    if (groupLang.LanguageID == "vi")
                        groupLang.BaseName = groupBaseAndGroupLang.GBVN;
                    else if (groupLang.LanguageID == "en")
                        groupLang.BaseName = groupBaseAndGroupLang.GBEN;
                    else if (groupLang.LanguageID == "zh-TW")
                        groupLang.BaseName = groupBaseAndGroupLang.GBTW;
                }

                List<GroupLang> editGroupLang = _mapper.Map<List<GroupLang>>(checkListGroupLang);
                _repositoryAccessor.GroupLang.UpdateMultiple(editGroupLang);

                await _repositoryAccessor.SaveChangesAsync();
                return new OperationResult(true, MessageConstants.UPDATE_SUCCESS, MessageConstants.SUCCESS);
            }
            catch (System.Exception)
            {
                return new OperationResult(false, MessageConstants.UPDATE_ERROR, MessageConstants.ERROR);
            }
        }

        public async Task<OperationResult> ExportExcel(GroupBaseTitleExcel title)
        {
            var data = await GetGroupBaseData();
            List<Table> dataTable = new List<Table>
            {
                new Table("result", data)
            };

            List<Cell> dataTitle = new List<Cell>
            {
                new Cell("A1", title.Label_BaseName),
                new Cell("B1", title.Label_BaseSym),
            };
            ExcelResult excelResult = ExcelUtility.DownloadExcel(dataTable, dataTitle, "Resources\\Template\\Manage\\GroupBase\\GroupBase.xlsx");
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

        public async Task<GroupBaseAndGroupLangDto> GetDetailGroupBase(int IDGroupBase)
        {
            GroupBaseAndGroupLangDto data = await _repositoryAccessor.GroupBase.FindAll(x => x.GBID == IDGroupBase)
            .Include(x => x.GroupLangs)
            .Select(x => new GroupBaseAndGroupLangDto
            {
                GBID = x.GBID,
                BaseName = x.BaseName,
                BaseSym = x.BaseSym,
                GBVN = x.GroupLangs.FirstOrDefault(y => y.GBID == x.GBID && y.LanguageID == LangConstants.VN).BaseName,
                GBEN = x.GroupLangs.FirstOrDefault(y => y.GBID == x.GBID && y.LanguageID == LangConstants.EN).BaseName,
                GBTW = x.GroupLangs.FirstOrDefault(y => y.GBID == x.GBID && y.LanguageID == LangConstants.ZH_TW).BaseName
            }).FirstOrDefaultAsync();

            return data;
        }

        public async Task<List<GroupBaseDto>> GetGroupBaseData()
        {
            var data = await _repositoryAccessor.GroupBase.FindAll().ProjectTo<GroupBaseDto>(_configMapper).ToListAsync();
            return data;
        }

        public async Task<OperationResult> RemoveGroup(int GBID)
        {
            GroupBase checkID = await _repositoryAccessor.GroupBase.FirstOrDefaultAsync(x => x.GBID == GBID);

            if (checkID == null)
            {
                return new OperationResult(false, MessageConstants.REMOVE_ERROR, MessageConstants.ERROR);
            }
            else
            {
                List<GroupLang> groupLang = await _repositoryAccessor.GroupLang.FindAll(x => x.GBID == checkID.GBID).ToListAsync();
                List<Employee> employes = await _repositoryAccessor.Employee.FindAll(x => x.GBID == checkID.GBID).ToListAsync();

                if (employes.Any() || groupLang.Any())
                    return new OperationResult(false, MessageConstants.NO_REMOVE, MessageConstants.ERROR);

                _repositoryAccessor.GroupBase.Remove(checkID);

                try
                {
                    await _repositoryAccessor.SaveChangesAsync();
                    return new OperationResult(true, MessageConstants.REMOVE_SUCCESS, MessageConstants.SUCCESS);
                }
                catch (System.Exception)
                {
                    return new OperationResult(false, MessageConstants.REMOVE_ERROR, MessageConstants.ERROR);
                }

            }
        }
    }
}