using API._Services.Interfaces.SeaHr;
using API.Helpers.Params.SeaHr.EmployeeManager;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.SeaHr
{
    public class SeaEmployeeManagerController : ApiController
    {
        private readonly ISeaEmpManagerServices _seaEmployeeManagerServices;

        public SeaEmployeeManagerController(ISeaEmpManagerServices seaEmployeeManagerServices)
        {
            _seaEmployeeManagerServices = seaEmployeeManagerServices;
        }

        [HttpGet("area")]
        public async Task<IActionResult> GetAllArea() => Ok(await _seaEmployeeManagerServices.GetAreas());

        // lấy danh sách phòng ban theo khu vực (Department)
        [HttpGet("department")]
        public async Task<IActionResult> GetDepartmentByAreaId(int areaId) => Ok(await _seaEmployeeManagerServices.GetDepartments(areaId));

        // Lấy danh sách chi tiết phòng theo phòng ban (Part)
        [HttpGet("part")]
        public async Task<IActionResult> GetPartByAreaIdAndDepartmentId(int deptId) => Ok(await _seaEmployeeManagerServices.GetParts(deptId));

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] PaginationParam param, [FromQuery] SeaEmployeeFilter filter)
        {
            var data = await _seaEmployeeManagerServices.Search(param, filter);
            return Ok(data);
        }
    }
}