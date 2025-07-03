using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.DTO.UploadT1;
using eTierV2_API.Helpers.Params;
using eTierV2_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.C2B
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UploadVideoT1Controller : ControllerBase
    {
        private readonly IUploadT1Service _serviceUploadT1;
        public UploadVideoT1Controller(IUploadT1Service serviceUploadT1)
        {
            _serviceUploadT1 = serviceUploadT1;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromQuery] PaginationParam param, ProductVideoT1Param filterParam)
        {
            var result = await _serviceUploadT1.Search(param, filterParam);
            Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpGet("getAllVideoKind")]
        public async Task<IActionResult> GetAllVideoKind()
        {
            var videokinds = await _serviceUploadT1.GetListVideoKind();
            return Ok(videokinds);
        }


        [HttpPost("delete")]
        public async Task<IActionResult> DeleteProductVideoT1(TMVideoDto model) {
            var result = await _serviceUploadT1.DeleteProductVideoT1(model);
            return Ok(result);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadVideo(UploadVideoT1Dto data) {
            var insertBy = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _serviceUploadT1.UploadVideo(data, insertBy);
            return Ok(result);
        }


        [HttpPost("searchOfUser")]
        public async Task<IActionResult> SearchOfUser([FromQuery] PaginationParam param, BatchDeleteParam filterParam) {
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _serviceUploadT1.SearchOfBatch(param, filterParam, user);
            Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpPost("deleteAllBySearch")]
        public async Task<IActionResult> DeleteAllBySearch(List<eTM_Video> data) {
            var result = await _serviceUploadT1.DeleteAllBySearch(data);
            return Ok(result);
        }


        [HttpGet("getCenters")]
        public async Task<IActionResult> GetCenters() {
            var data = await _serviceUploadT1.GetCenters();
            return Ok(data);
        }
         [HttpGet("getTiers")]
        public async Task<IActionResult> GetTiers() {
            var data = await _serviceUploadT1.GetTiers();
            return Ok(data);
        }
         [HttpGet("getSections")]
        public async Task<IActionResult> GetSections() {
            var data = await _serviceUploadT1.GetSections();
            return Ok(data);
        }
         [HttpGet("getUnits")]
        public async Task<IActionResult> GetUnits() {
            var data = await _serviceUploadT1.GetUnits();
            return Ok(data);
        }

        [HttpGet("getUnitsInParam")]
        public async Task<IActionResult> GetUnits(string center, string tier, string section) {
            var data = await _serviceUploadT1.GetUnits(center,tier,section);
            return Ok(data);
        }
    }
}