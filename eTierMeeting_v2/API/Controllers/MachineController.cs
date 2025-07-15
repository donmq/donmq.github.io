using System.Security.Claims;
using Machine_API._Service.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class MachineController : ApiController
    {
        private readonly IMachineService _machineService;
        public MachineController(IMachineService machineService)
        {
            _machineService = machineService;

        }

        [HttpPost("GetMachineByID")]
        public async Task<IActionResult> GetMachineByID(string idMachine, string lang)
        {
            var result = await _machineService.GetMachineByID(idMachine, lang);
            return Ok(result);
        }

        [HttpPost("MoveMachine")]
        public async Task<IActionResult> MoveMachine(string fromEmploy, string idMachine, string toEmploy, string fromPlno, string toPlno)
        {
            string userID = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _machineService.MoveMachine(fromEmploy, idMachine, toEmploy, fromPlno, toPlno, userID);
            return Ok(result);
        }
    }
}