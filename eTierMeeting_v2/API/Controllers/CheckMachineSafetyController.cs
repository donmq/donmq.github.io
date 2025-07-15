using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class CheckMachineSafetyController : ApiController
    {
        private readonly ICheckMachineSafetyService _checkMachineSafetyService;

        public CheckMachineSafetyController(ICheckMachineSafetyService checkMachineSafetyService)
        {
            _checkMachineSafetyService = checkMachineSafetyService;
        }

        [HttpPost("GetMachine")]
        public async Task<IActionResult> GetMachine(string idMachine, string lang)
        {
            var result = await _checkMachineSafetyService.GetMachine(idMachine, lang);
            return Ok(result);
        }

        [HttpGet("Getlistquestion")]
        public async Task<ActionResult> GetListQuestion(string lang)
        {
            var data = await _checkMachineSafetyService.GetListQuestion(lang);
            return Ok(data);
        }

        [HttpPost("SaveMachineSafetyCheck")]
        public async Task<IActionResult> SaveMachineSafetyCheck([FromForm] SurveyRequest request)
        {
            request.CheckDate = DateTime.Now;
            var data = await _checkMachineSafetyService.SaveMachineSafetyCheck(request);
            return Ok(data);
        }
    }
}