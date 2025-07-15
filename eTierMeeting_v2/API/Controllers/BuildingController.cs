using System.Security.Claims;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class BuildingController : ApiController
    {
        private readonly IBuildingService _buildingService;
        public BuildingController(IBuildingService buildingService)
        {
            _buildingService = buildingService;

        }

        [HttpGet("GetAllBuilding")]
        public async Task<IActionResult> GetAllBuilding()
        {
            var result = await _buildingService.GetAllBuilding();
            return Ok(result);
        }

        [HttpGet("GetBuildingByPdcID")]
        public async Task<IActionResult> GetBuildingByPdcID(int idPDC)
        {
            var result = await _buildingService.GetBuildingByPdcID(idPDC);
            return Ok(result);
        }

        [HttpGet("GetBuildingByCellCodeAndPDC")]
        public async Task<IActionResult> GetBuildingByCellCodeAndPDC(string cellCode, int? idPDC)
        {
            if (idPDC > 0)
            {
                var result = await _buildingService.GetBuildingByCellCodeAndPDC(cellCode, idPDC);
                return Ok(result);
            }
            else
            {
                var result = await _buildingService.GetBuildingByCellCode(cellCode);
                return Ok(result);
            }
        }

        [HttpPost("{idPDC}")]
        public async Task<IActionResult> GetAllBuildingByID(int idPDC)
        {
            var result = await _buildingService.GetAllBuildingByID(idPDC);
            return Ok(result);
        }

        [HttpGet("GetListBuilding")]
        public async Task<IActionResult> GetListBuilding([FromQuery] PaginationParams paginationParams)
        {
            var result = await _buildingService.GetListBuilding(paginationParams);
            return Ok(result);
        }

        [HttpPost("SearchBuilding")]
        public async Task<IActionResult> SearchBuilding([FromQuery] PaginationParams paginationParams, string keyword)
        {
            var result = await _buildingService.SearchBuilding(paginationParams, keyword);
            return Ok(result);
        }

        [HttpPost("AddBuilding")]
        public async Task<IActionResult> AddBuilding(BuildingDto model, string lang)
        {
            var userName = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _buildingService.AddBuilding(model, lang);
            return Ok(result);
        }

        [HttpPost("RemoveBuilding")]
        public async Task<IActionResult> RemoveBuilding(BuildingDto model, string lang)
        {
            var userName = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _buildingService.RemoveBuilding(model, lang);
            return Ok(result);
        }
        [HttpPost("UpdateBuilding")]
        public async Task<IActionResult> UpdateBuilding(BuildingDto model, string lang)
        {
            var userName = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _buildingService.UpdateBuilding(model, lang);
            return Ok(result);
        }
    }
}