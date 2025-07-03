using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API.DTO;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using eTierV2_API._Services.Interfaces.Production.T1.STF;
using eTierV2_API._Repositories;

namespace eTierV2_API._Services.Services.Production.T1.STF
{
    public class ProductionT1STFDeliveryService : IProductionT1STFDeliveryService
    {
        private readonly MapperConfiguration _configuration;
        private readonly IRepositoryAccessor _repoAccessor;
        public ProductionT1STFDeliveryService(IRepositoryAccessor repoAccessor, MapperConfiguration configuration)
        {
            _configuration = configuration;
            _repoAccessor = repoAccessor;
        }

        public async Task<List<VW_Production_T1_STF_Delivery_RecordDTO>> GetData(string deptId)
        {
            var data = await _repoAccessor.VW_Production_T1_STF_Delivery_Record.FindAll(x => x.Building == deptId)
                        .ProjectTo<VW_Production_T1_STF_Delivery_RecordDTO>(_configuration).ToListAsync();

            return data;
        }
    }
}