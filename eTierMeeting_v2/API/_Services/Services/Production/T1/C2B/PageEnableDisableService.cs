using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Params;
using eTierV2_API.Helpers.Utilities;
using eTierV2_API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API._Services.Services.Production.T1.C2B
{
    public class PageEnableDisableService : IPageEnableDisableService
    {
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IMapper _mapper;
        private readonly IRepositoryAccessor _repoAccessor;

        public PageEnableDisableService(
            IRepositoryAccessor repoAccessor,
            MapperConfiguration mapperConfiguration,
            IMapper mapper)
        {
            _mapperConfiguration = mapperConfiguration;
            _mapper = mapper;
            _repoAccessor = repoAccessor;
        }

        public async Task<List<KeyValuePair<string, string>>> GetCenters()
        {
            var centers = await _repoAccessor.eTM_Page_Settings
                .FindAll()
                .GroupBy(x => x.Center_Level.Trim())
                .Select(x => new KeyValuePair<string, string>(x.Key.Trim(), x.Key.Trim()))
                .ToListAsync();

            return centers;
        }

        public async Task<List<eTM_Page_SettingsDTO>> GetPages(PageEnableDisableParam param)
        {
            var pages = await _repoAccessor.eTM_Page_Settings
                .FindAll(x =>
                    x.Center_Level.Trim() == param.Center_Level.Trim() &&
                    x.Tier_Level.Trim() == param.Tier_Level.Trim() &&
                    x.Class_Level.Trim() == param.Class_Level.Trim())
                .OrderBy(x => x.Seq)
                .ProjectTo<eTM_Page_SettingsDTO>(_mapperConfiguration)
                .ToListAsync();

            return pages;
        }

        public async Task<List<KeyValuePair<string, string>>> GetSections(string center, string tier)
        {
            var pageSettingsPred = PredicateBuilder.New<eTM_Page_Settings>(true);

            if (!string.IsNullOrEmpty(center))
                pageSettingsPred = pageSettingsPred.And(x => x.Center_Level.Trim() == center.Trim());

            if (!string.IsNullOrEmpty(tier))
                pageSettingsPred = pageSettingsPred.And(x => x.Tier_Level.Trim() == tier.Trim());

            var sections = await _repoAccessor.eTM_Page_Settings
                .FindAll(pageSettingsPred)
                .GroupBy(x => x.Class_Level.Trim())
                .Select(x => new KeyValuePair<string, string>(x.Key.Trim(), x.Key.Trim()))
                .ToListAsync();

            return sections;
        }

        public async Task<List<KeyValuePair<string, string>>> GetTiers(string center)
        {
            var pageSettingsPred = PredicateBuilder.New<eTM_Page_Settings>(true);

            if (!string.IsNullOrEmpty(center?.Trim()))
                pageSettingsPred = pageSettingsPred.And(x => x.Center_Level.Trim() == center.Trim());

            var tiers = await _repoAccessor.eTM_Page_Settings
                .FindAll(pageSettingsPred)
                .GroupBy(x => x.Tier_Level.Trim())
                .Select(x => new KeyValuePair<string, string>(x.Key.Trim(), x.Key.Trim()))
                .ToListAsync();

            return tiers;
        }

        public async Task<OperationResult> UpdatePages(List<eTM_Page_SettingsDTO> pagesDto)
        {
            var pages = _mapper.Map<List<eTM_Page_Settings>>(pagesDto);

            try
            {
                _repoAccessor.eTM_Page_Settings.UpdateMultiple(pages);
                await _repoAccessor.eTM_Page_Settings.SaveAll();
                return new OperationResult(true);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}