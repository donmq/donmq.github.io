using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T2.CTB;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Params.Production.T2.CTB;
using eTierV2_API.Helpers.Utilities;
using eTierV2_API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using eTierV2_API._Repositories;

namespace eTierV2_API._Services.Services.Production.T2.CTB
{
    public class PageItemSettingService : IPageItemSettingService
    {
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IMapper _mapper;
        private readonly IRepositoryAccessor _repoAccessor;
        public PageItemSettingService(
            IMapper mapper,
            MapperConfiguration mapperConfiguration,
            IRepositoryAccessor repoAccessor)
        {
            _repoAccessor = repoAccessor;
            _mapper = mapper;
            _mapperConfiguration = mapperConfiguration;
        }

        public async Task<OperationResult> Add(eTM_Page_Item_SettingsDTO settingDTO, string update_By)
        {
            if (string.IsNullOrEmpty(settingDTO.Center_Level?.Trim()) ||
                string.IsNullOrEmpty(settingDTO.Class_Level?.Trim()) ||
                string.IsNullOrEmpty(settingDTO.Tier_Level?.Trim()) ||
                string.IsNullOrEmpty(settingDTO.Page_Name?.Trim()))
                return new OperationResult(false);

            var checkExists = await CheckExists(settingDTO);
            if (checkExists)
                return new OperationResult(false);

            settingDTO.Item_ID = Guid.NewGuid().ToString();
            settingDTO.Update_By = update_By;
            settingDTO.Update_Time = DateTime.Now;

            var setting = _mapper.Map<eTM_Page_Item_Settings>(settingDTO);

            _repoAccessor.eTM_Page_Item_Settings.Add(setting);
            await _repoAccessor.eTM_Page_Item_Settings.SaveAll();

            return new OperationResult(true);
        }

        public async Task<PaginationUtility<eTM_Page_Item_SettingsDTO>> GetAll(PageItemSettingParam param, PaginationParam pagination)
        {
            var pred = PredicateBuilder.New<eTM_Page_Item_Settings>(true);

            if (!string.IsNullOrEmpty(param.Center_Level?.Trim()))
                pred = pred.And(x => x.Center_Level.Trim() == param.Center_Level.Trim());

            if (!string.IsNullOrEmpty(param.Tier_Level?.Trim()))
                pred = pred.And(x => x.Tier_Level.Trim() == param.Tier_Level.Trim());

            if (!string.IsNullOrEmpty(param.Class_Level?.Trim()))
                pred = pred.And(x => x.Class_Level.Trim() == param.Class_Level.Trim());

            var query = _repoAccessor.eTM_Page_Item_Settings.FindAll(pred).ProjectTo<eTM_Page_Item_SettingsDTO>(_mapperConfiguration);
            var result = await PaginationUtility<eTM_Page_Item_SettingsDTO>.CreateAsync(query, pagination.PageNumber, pagination.PageSize);

            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetCenterLevels()
        {
            var result = await _repoAccessor.eTM_Team_Unit
                .FindAll().Select(x => new KeyValuePair<string, string>(x.Center_Level.Trim(), x.Center_Level.Trim()))
                .Distinct().ToListAsync();
            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetSections(string center_Level, string tier_Level)
        {
            var pred = PredicateBuilder.New<eTM_Team_Unit>(true);

            if (!string.IsNullOrEmpty(center_Level?.Trim()))
                pred = pred.And(x => x.Center_Level.Trim() == center_Level.Trim());

            if (!string.IsNullOrEmpty(tier_Level?.Trim()))
                pred = pred.And(x => x.Tier_Level.Trim() == tier_Level.Trim());

            var result = await _repoAccessor.eTM_Team_Unit
                .FindAll(pred).Select(x => new KeyValuePair<string, string>(x.Class1_Level.Trim(), x.Class1_Level.Trim()))
                .Distinct().ToListAsync();
            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetPages()
        {
            var result = await _repoAccessor.eTM_Page_Settings
                .FindAll().Select(x => new KeyValuePair<string, string>(x.Page_Name.Trim(), x.Page_Name.Trim()))
                .Distinct().ToListAsync();
            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetTierLevels(string center_Level)
        {
            var pred1 = PredicateBuilder.New<eTM_Team_Unit>(true);
            var pred2 = PredicateBuilder.New<eTM_Page_Item_Settings>(true);

            if (!string.IsNullOrEmpty(center_Level?.Trim()))
                pred1 = pred1.And(x => x.Center_Level.Trim() == center_Level.Trim());

            if (!string.IsNullOrEmpty(center_Level?.Trim()))
                pred2 = pred2.And(x => x.Center_Level.Trim() == center_Level.Trim());

            var result1 = await _repoAccessor.eTM_Team_Unit
                .FindAll(pred1).Select(m => m.Tier_Level.Trim()).ToListAsync();

            var result2 = await _repoAccessor.eTM_Page_Item_Settings
                .FindAll(pred2).Select(m => m.Tier_Level.Trim()).ToListAsync();

            var result = result1.Union(result2)
                .Distinct()
                .Select(x => new KeyValuePair<string, string>(x, x))
                .ToList();

            return result;
        }

        public async Task<OperationResult> Update(eTM_Page_Item_SettingsDTO settingDTO, string update_By)
        {
            var checkExists = await CheckExists(settingDTO);
            if (!checkExists)
                return new OperationResult(false);

            settingDTO.Update_By = update_By;
            settingDTO.Update_Time = DateTime.Now;

            var setting = _mapper.Map<eTM_Page_Item_Settings>(settingDTO);

            _repoAccessor.eTM_Page_Item_Settings.Update(setting);
            await _repoAccessor.eTM_Page_Item_Settings.SaveAll();

            return new OperationResult(true);
        }

        private async Task<bool> CheckExists(eTM_Page_Item_SettingsDTO settingDTO)
        {
            if (string.IsNullOrEmpty(settingDTO.Center_Level?.Trim()) ||
                string.IsNullOrEmpty(settingDTO.Class_Level?.Trim()) ||
                string.IsNullOrEmpty(settingDTO.Tier_Level?.Trim()) ||
                string.IsNullOrEmpty(settingDTO.Page_Name?.Trim()) ||
                string.IsNullOrEmpty(settingDTO.Item_ID?.Trim()))
                return false;

            var result = await _repoAccessor.eTM_Page_Item_Settings
                .FindAll(x =>
                    x.Center_Level.Trim() == settingDTO.Center_Level.Trim() &&
                    x.Class_Level.Trim() == settingDTO.Class_Level.Trim() &&
                    x.Tier_Level.Trim() == settingDTO.Tier_Level.Trim() &&
                    x.Page_Name.Trim() == settingDTO.Page_Name.Trim() &&
                    x.Item_ID.Trim() == settingDTO.Item_ID.Trim())
                .AnyAsync();
            return result;
        }
    }
}