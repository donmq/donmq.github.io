using System.Security.Claims;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class PDCController : ApiController
    {
        private readonly IPDCService _pDCService;
        public PDCController(IPDCService pDCService)
        {
            _pDCService = pDCService;
        }

        [HttpGet("GetAllPDC")]
        public async Task<IActionResult> GetAllPDC()
        {
            var result = await _pDCService.GetAllPDC();
            return Ok(result);
        }

        [HttpGet("GetListAllPDC")]
        public async Task<IActionResult> GetListAllPDC([FromQuery] PaginationParams paginationParams)
        {
            var result = await _pDCService.GetListAllPDC(paginationParams);
            return Ok(result);
        }

        [HttpPost("SearchPDC")]
        public async Task<IActionResult> SearchPDC([FromQuery] PaginationParams paginationParams, string keyword)
        {
            var result = await _pDCService.SearchPDC(paginationParams, keyword);
            return Ok(result);
        }

        [HttpPost("AddPDC")]
        public async Task<IActionResult> AddPDC(PdcDto model, string lang)
        {
            var userName = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _pDCService.AddPDC(model, lang);
            return Ok(result);
        }

        [HttpPost("RemovePDC")]
        public async Task<IActionResult> AddRemovePDCPDC(PdcDto model, string lang)
        {
            var userName = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _pDCService.RemovePDC(model, lang);
            return Ok(result);
        }

        [HttpPost("UpdatePDC")]
        public async Task<IActionResult> UpdatePDC(PdcDto model, string lang)
        {
            var userName = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _pDCService.UpdatePDC(model, lang);
            return Ok(result);
        }
    }
}