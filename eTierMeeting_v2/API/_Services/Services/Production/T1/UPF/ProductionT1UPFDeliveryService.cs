using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.UPF;
using eTierV2_API.DTO;
using eTierV2_API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API._Services.Services.Production.T1.UPF
{
    public class ProductionT1UPFDeliveryService : IProductionT1UPFDeliveryService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryAccessor _repoAccessor;

        public ProductionT1UPFDeliveryService(IRepositoryAccessor repoAccessor, IMapper mapper)
        {
            _repoAccessor = repoAccessor;
            _mapper = mapper;
        }

        public async Task<List<VW_Production_T1_UPF_Delivery_RecordDTO>> GetData(string deptId)
        {
            var uPFDeliveryRecord = _repoAccessor.VW_Production_T1_UPF_Delivery_Record
                .FindAll(x => x.Building.Trim() == deptId.Trim())
                .OrderBy(x => x.Plan_End_STC);

            var data = await _mapper.ProjectTo<VW_Production_T1_UPF_Delivery_RecordDTO>(uPFDeliveryRecord)
                .ToListAsync();
            
            return data;
        }
    }
}