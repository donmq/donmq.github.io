using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API.Helpers;

namespace eTierV2_API._Services.Interfaces.Production.T1.C2B
{
    public interface IMainService<T> where T : class
    {
        Task<bool> Add(T model);

        Task<bool> Update(T model);

        Task<bool> Delete(T model);
    }
}