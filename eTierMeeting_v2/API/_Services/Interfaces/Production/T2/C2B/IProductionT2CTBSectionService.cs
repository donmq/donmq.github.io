using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTierV2_API.DTO;

namespace eTierV2_API._Services.Interfaces.Production.T2.CTB
{
    public interface IProductionT2CTBSectionService
    {
         Task<ProductionT2SelectLineDTO> getFactoryIndex();
    }
}