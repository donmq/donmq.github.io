using API._Repositories;
using API._Services.Interfaces.Manage;
using API.Dtos.Common;
using API.Helpers.Enums;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace API._Services.Services.Manage
{
    public class AreaManagementService : IAreaManagementService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryAccessor _repoAccessor;
        public AreaManagementService(IRepositoryAccessor repoAccessor, IMapper mapper)
        {
            _repoAccessor = repoAccessor;
            _mapper = mapper;
        }

        public async Task<bool> Add(AreaInformation areaInformation)
        {
            using var _transaction = await _repoAccessor.BeginTransactionAsync();
            try
            {
                // Create Area
                Area area = new()
                {
                    AreaName = areaInformation.AreaName.Trim(),
                    CompanyID = areaInformation.CompanyID,
                    Number = areaInformation.Number,
                    Visible = areaInformation.Visible
                };
                _repoAccessor.Area.Add(area);
                await _repoAccessor.SaveChangesAsync();

                // Update lại trường AreaSym
                Area areaAfterAdd = await _repoAccessor.Area.FirstOrDefaultAsync(x => x.AreaName.Trim() == area.AreaName);
                int areaIDNew = areaAfterAdd.AreaID;
                areaAfterAdd.AreaSym = "A" + areaIDNew;

                var idArea = _repoAccessor.Area.FindAll().OrderByDescending(x => x.AreaID).Select(x => x.AreaID).FirstOrDefault();
                var listBuildLang = new List<AreaLang>
                {
                    new() { AreaName = areaInformation.AreaNameVi, LanguageID = "vi", AreaID = idArea },
                    new() { AreaName = areaInformation.AreaNameEn, LanguageID = "en", AreaID = idArea },
                    new() { AreaName = areaInformation.AreaNameZh, LanguageID = "zh-TW", AreaID = idArea }
                };

                var addAreaLang = _mapper.Map<List<AreaLang>>(listBuildLang);
                _repoAccessor.AreaLang.AddMultiple(addAreaLang);
                await _repoAccessor.SaveChangesAsync();

                Roles role = new()
                {
                    Ranked = 1,
                    RoleName = area.AreaName,
                    RoleSym = areaAfterAdd.AreaSym,
                    GroupIN = 2
                };
                _repoAccessor.Roles.Add(role);
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

        public async Task<bool> Edit(AreaInformation areaInformation)
        {
            Area area = await _repoAccessor.Area.FirstOrDefaultAsync(x => x.AreaID == areaInformation.AreaID);
            if (area == null)
                return false;

            using var _transaction = await _repoAccessor.BeginTransactionAsync();
            try
            {
                // Update Area
                area.AreaName = areaInformation.AreaName;
                area.Visible = areaInformation.Visible;
                area.Number = areaInformation.Number;

                List<AreaLang> areaLangs = await _repoAccessor.AreaLang.FindAll(x => x.AreaID == areaInformation.AreaID).ToListAsync();
                foreach (var item in areaLangs)
                {
                    item.AreaName = areaInformation.AreaNameVi;
                    item.AreaName = areaInformation.AreaNameEn;
                    item.AreaName = areaInformation.AreaNameZh;
                }

                var addAreaLang = _mapper.Map<List<AreaLang>>(areaLangs);
                _repoAccessor.AreaLang.UpdateMultiple(addAreaLang);
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

        public async Task<List<AreaInformation>> GetAll()
        {
            List<AreaInformation> areas = await _repoAccessor.Area.FindAll(true)
            .Include(x => x.AreaLangs)
            .Include(x => x.Company)
            .Select(x => new AreaInformation
            {
                AreaID = x.AreaID,
                AreaName = x.AreaName,
                AreaSym = x.AreaSym,
                AreaCode = x.AreaCode,
                CompanyID = x.Company.CompanyID,
                Company = x.Company.CompanyName,
                Number = x.Number,
                Visible = x.Visible,
                AreaNameVi = x.AreaLangs.FirstOrDefault(a => a.AreaID == x.AreaID && a.LanguageID == LangConstants.VN).AreaName,
                AreaNameEn = x.AreaLangs.FirstOrDefault(a => a.AreaID == x.AreaID && a.LanguageID == LangConstants.EN).AreaName,
                AreaNameZh = x.AreaLangs.FirstOrDefault(a => a.AreaID == x.AreaID && a.LanguageID == LangConstants.ZH_TW).AreaName,
            }).ToListAsync();

            return areas;
        }
    }
}