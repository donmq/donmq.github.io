using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.DTO;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API._Services.Services.Production.T1.C2B
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IRepositoryAccessor _repoAccessor;

        public DeliveryService(
        IRepositoryAccessor repoAccessor)
        {
            _repoAccessor = repoAccessor;
        }

        public async Task<List<DeliveryDTO>> GetAllDelivery(string deptId)
        {
            var data = await _repoAccessor.eTM_Dept_Classification.FindAll(x => x.Dept_ID == deptId)
                .Join(_repoAccessor.VW_Prod_T1_CTB_Delivery.FindAll(), 
                    x => x.Dept_Name, 
                    y => y.ASL_Team, 
                    (x, y) => new DeliveryDTO
                    {
                        Plan_Finish_Date = y.ETC.ToString(),
                        MO_No = y.PO_Batch,
                        Model_Name = y.Model_Name,
                        Article = y.Article,
                        Plan_Qty = y.Planned_Qty,
                        QTY = y.Balance,
                        Nation = y.Country
                    })
                .ToListAsync();
            return data;
        }
    }
}