using API._Repositories;
using API.Controllers;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace API.Helpers.Jobs
{
    public class LoginExpriesJob : ApiController, IJob
    {
        private readonly IRepositoryAccessor _repoAccessor;

        public LoginExpriesJob(IRepositoryAccessor repoAccessor)
        {
            _repoAccessor = repoAccessor;
        }

        [HttpDelete("Execute")]
        public async Task Execute(IJobExecutionContext context)
        {
            List<LoginDetect> detects = await _repoAccessor.LoginDetect.FindAll(x => x.Expires < DateTime.Now).ToListAsync();
            if (detects.Any())
            {
                _repoAccessor.LoginDetect.RemoveMultiple(detects);
                await _repoAccessor.SaveChangesAsync();
            }
            await Task.CompletedTask;
        }
    }
}