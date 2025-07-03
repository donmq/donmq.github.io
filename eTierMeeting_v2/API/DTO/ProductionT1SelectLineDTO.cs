using System.Collections.Generic;
using eTierV2_API.Models;

namespace eTierV2_API.DTO
{
    public class ProductionT1SelectLineDTO
    {
        public List<VFactoryIndexOC> ListC2B { get; set; }
        public List<VFactoryIndexOC> ListSTF { get; set; }
    }
}