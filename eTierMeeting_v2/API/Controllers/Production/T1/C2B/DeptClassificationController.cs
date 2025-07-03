using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Params;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;

namespace eTierV2_API.Controllers.Production.T1.C2B
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class DeptClassificationController : ControllerBase
    {
        private readonly IDeptClassificationServcie _deptClassificationService;
        public DeptClassificationController(IDeptClassificationServcie deptClassificationService)
        {
            _deptClassificationService = deptClassificationService;
        }

        // 搜尋
        [HttpPost("search")]
        public async Task<IActionResult> DataSearch([FromQuery] PaginationParam param, DeptClassificationParam paramSearch)
        {
            var lists = await _deptClassificationService.SearchDeptClassification(param, paramSearch);
            Response.AddPagination(lists.CurrentPage, lists.PageSize, lists.TotalCount, lists.TotalPages);
            return Ok(lists);
        }

        [HttpGet("getdept")]
        public async Task<IActionResult> GetDept() => Ok(await _deptClassificationService.GetDept());

        // 抓classification部門值(會再修改)
        [HttpGet("getdeptforselectline")]
        public async Task<IActionResult> GetDeptInClassification()
        {
            var result  =await _deptClassificationService.getFactoryIndex();
            return Ok(result);
        }

        // 新增站台
        [HttpPost("addDeptClassification")]
        public async Task<IActionResult> AddStation(eTM_Dept_ClassificationDTO deptClassificationDTO)
        {
            string Dept_ID = await _deptClassificationService.ChkDeptIDBeforeInsert(deptClassificationDTO.Dept_ID);
            if (await _deptClassificationService.CheckDeptClassificationExists(deptClassificationDTO.Class_Kind, Dept_ID))
            {
                return BadRequest("DeptClassification already exists!");
            }
            deptClassificationDTO.Dept_ID = Dept_ID;
            deptClassificationDTO.Insert_By = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            deptClassificationDTO.Insert_At = DateTime.Now;
            deptClassificationDTO.Update_By = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            deptClassificationDTO.Update_At = DateTime.Now;
            if (await _deptClassificationService.Add(deptClassificationDTO))
            {
                return Ok();
            }
            throw new Exception("Creating the DeptClassification failed on save");
        }

        // 修改站台
        [HttpPost("updateDeptClassification")]
        public async Task<IActionResult> UpdateStation(eTM_Dept_ClassificationDTO deptClassificationDTO)
        {
            deptClassificationDTO.Update_By = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            deptClassificationDTO.Update_At = DateTime.Now;
            if (await _deptClassificationService.Update(deptClassificationDTO))
            {
                return Ok();
            }
            return BadRequest($"Updating DeptClassification failed on save");
        }

        // 刪除站台
        [HttpPost("deleteClassification")]
        public async Task<IActionResult> DeleteModelOperation(eTM_Dept_ClassificationDTO deptClassificationDTO)
        {
            if (await _deptClassificationService.Delete(deptClassificationDTO))
            {
                return Ok();
            }
            return BadRequest($"DeptClassification Delete Fail");
        }

    }
}