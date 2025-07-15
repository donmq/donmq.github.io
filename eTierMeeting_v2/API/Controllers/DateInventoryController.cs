using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class DateInventoryController : ApiController
    {
        private readonly IDateInventoryServive _dateInventory;

        public DateInventoryController(IDateInventoryServive dateInventory)
        {
            _dateInventory = dateInventory;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> GetAllBuildingByID(int id)
        {
            var result = await _dateInventory.GetDateInventory(id);
            return Ok(result);
        }

        [HttpPost("RemoveDateInventory")]
        public async Task<IActionResult> RemoveDateInventory(int id, string lang)
        {
            var result = await _dateInventory.RemoveDateInventory(id, lang);
            return Ok(result);
        }
        [HttpPost("AddDateInventory")]
        public async Task<IActionResult> AddDateInventory(AddDateDto addDate, string lang)
        {
            //fromat về thời thời gian local
            addDate.FromDate = addDate.FromDate.ToLocalTime();
            addDate.ToDate = addDate.ToDate.ToLocalTime();
            var userName = User.FindFirst("UserName").Value;
            var empName = User.FindFirst("EmpName").Value;
            var result = await _dateInventory.AddDateInventory(addDate, userName, empName, lang);
            return Ok(result);
        }

        [HttpPost("GetAllDateInventories")]
        public async Task<IActionResult> GetAllDateInventories([FromQuery] PaginationParams paginationParams)
        {
            var result = await _dateInventory.GetAllDateInventories(paginationParams);
            return Ok(result);
        }

        [HttpGet("Check")]
        public ActionResult CheckScheduleInventory()
        {
            var result = _dateInventory.CheckScheduleInventory();
            return Ok(result);
        }


    }
}