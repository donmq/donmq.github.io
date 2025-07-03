using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTierV2_API.Models;

namespace eTierV2_API.DTO
{
    public class ProductionT2SelectLineDTO
    {
        public List<eTM_Team_UnitIndexOC> ListC2B { get; set; }
        public List<eTM_Team_UnitIndexOC> ListSTF { get; set; }
        public List<eTM_Team_UnitIndexOC> ListUPF { get; set; }
    }
}