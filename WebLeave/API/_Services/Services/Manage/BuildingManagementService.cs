using API._Repositories;
using API._Services.Interfaces.Manage;
using API.Dtos.Common;
using API.Helpers.Enums;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.Manage
{
    public class BuildingManagementService : IBuildingManagementService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryAccessor _repoAccessor;
        public BuildingManagementService(IRepositoryAccessor repoAccessor, IMapper mapper)
        {
            _repoAccessor = repoAccessor;
            _mapper = mapper;
        }

        public async Task<bool> Add(BuildingInformation buildingInformation)
        {
            using var _transaction = await _repoAccessor.BeginTransactionAsync();
            try
            {
                // Create Building
                Building building = new()
                {
                    BuildingName = buildingInformation.BuildingName.Trim(),
                    AreaID = buildingInformation.AreaID,
                    Number = buildingInformation.Number,
                    Visible = buildingInformation.Visible
                };
                _repoAccessor.Building.Add(building);

                // Save Building trước để lấy BuildingID mới tạo tự động tăng.
                await _repoAccessor.SaveChangesAsync();

                // Update lại trường buildingSym dựa theo id mới
                Building buildingNewAdd = await _repoAccessor.Building.FirstOrDefaultAsync(x => x.BuildingName.Trim() == building.BuildingName);
                int buildingNewId = buildingNewAdd.BuildingID;
                buildingNewAdd.BuildingSym = "B" + buildingNewId;
                _repoAccessor.Building.Update(buildingNewAdd);

                var idBuilding = _repoAccessor.Building.FindAll().OrderByDescending(x => x.BuildingID).Select(x => x.BuildingID).FirstOrDefault();

                var listBuildLang = new List<BuildLang>
                {
                    new() { BuildingName = buildingInformation.BuildingNameVi, LanguageID = "vi", BuildingID = idBuilding },
                    new() { BuildingName = buildingInformation.BuildingNameEn, LanguageID = "en", BuildingID = idBuilding },
                    new() { BuildingName = buildingInformation.BuildingNameZh, LanguageID = "zh-TW", BuildingID = idBuilding }
                };

                var addBuildLang = _mapper.Map<List<BuildLang>>(listBuildLang);
                _repoAccessor.BuildLang.AddMultiple(addBuildLang);
                await _repoAccessor.SaveChangesAsync();

                Roles r = new()
                {
                    Ranked = 2,
                    RoleName = building.BuildingName,
                    RoleSym = buildingNewAdd.BuildingSym,
                    GroupIN = 3
                };
                _repoAccessor.Roles.Add(r);

                await _repoAccessor.SaveChangesAsync();
                await _transaction.CommitAsync();

                return true;
            }
            catch
            {
                await _transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> Edit(BuildingInformation buildingInformation)
        {
            Building building = await _repoAccessor.Building.FirstOrDefaultAsync(x => x.BuildingID == buildingInformation.BuildingID);

            if (building == null)
                return false;

            using var _transaction = await _repoAccessor.BeginTransactionAsync();
            try
            {
                // Update Building
                building.AreaID = buildingInformation.AreaID;
                building.BuildingName = buildingInformation.BuildingName;
                building.Number = buildingInformation.Number;
                building.Visible = buildingInformation.Visible;

                List<BuildLang> buildingLangs = await _repoAccessor.BuildLang.FindAll(x => x.BuildingID == buildingInformation.BuildingID).ToListAsync();
                foreach (var item in buildingLangs)
                {
                    item.BuildingName = buildingInformation.BuildingNameVi;
                    item.BuildingName = buildingInformation.BuildingNameEn;
                    item.BuildingName = buildingInformation.BuildingNameZh;
                }

                var updateBuildLang = _mapper.Map<List<BuildLang>>(buildingLangs);
                _repoAccessor.BuildLang.UpdateMultiple(updateBuildLang);
                await _repoAccessor.SaveChangesAsync();

                await _transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await _transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<List<BuildingInformation>> GetAll()
        {
            List<BuildingInformation> result = await _repoAccessor.Building.FindAll(true)
            .Include(x => x.BuildLangs)
            .Include(x => x.Area)
            .Select(x => new BuildingInformation
            {
                BuildingID = x.BuildingID,
                BuildingName = x.BuildingName,
                BuildingSym = x.BuildingSym,
                BuildingCode = x.BuildingCode,
                Number = x.Number,
                Visible = x.Visible,
                AreaID = x.AreaID,
                AreaName = x.Area.AreaName,
                BuildingNameVi = x.BuildLangs.FirstOrDefault(a => a.BuildingID == x.BuildingID && a.LanguageID == LangConstants.VN).BuildingName,
                BuildingNameEn = x.BuildLangs.FirstOrDefault(a => a.BuildingID == x.BuildingID && a.LanguageID == LangConstants.EN).BuildingName,
                BuildingNameZh = x.BuildLangs.FirstOrDefault(a => a.BuildingID == x.BuildingID && a.LanguageID == LangConstants.ZH_TW).BuildingName,
            }).ToListAsync();

            return result;
        }

        public async Task<List<KeyValuePair<int, string>>> GetListArea()
        {
            return await _repoAccessor.Area
                .FindAll(x => x.Visible == true)
                .Select(x => new KeyValuePair<int, string>
                (
                    x.AreaID,
                    x.AreaName
                )).Distinct().ToListAsync();
        }
    }
}