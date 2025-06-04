using System.Globalization;
using API._Repositories;
using API._Services.Interfaces.Manage;
using API.Dtos.Manage.HolidayManage;
using API.Helpers.Enums;
using API.Models;
using Microsoft.EntityFrameworkCore;
namespace API._Services.Services.Manage
{
    public class HolidayService : IHolidayService
    {
        private readonly IRepositoryAccessor _repositoryAccessor;

        public HolidayService(IRepositoryAccessor repositoryAccessor)
        {
            _repositoryAccessor = repositoryAccessor;
        }

        public async Task<OperationResult> AddHoliday(HolidayDto Holidays)
        {
            DateTime.TryParseExact(Holidays.Date, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime holiday);
            Holiday data = await _repositoryAccessor.Holiday.FirstOrDefaultAsync(x => x.Date == holiday);
            if (data != null)
                return new OperationResult(false, MessageConstants.REMOVE_ERROR, MessageConstants.ERROR);

            Holiday item = new()
            {
                UserID = Holidays.UserID,
                Date = holiday,
                Description = Holidays.Description,
                CreateTime = Holidays.CreateTime
            };

            _repositoryAccessor.Holiday.Add(item);
            await _repositoryAccessor.SaveChangesAsync();
            return new OperationResult(true, MessageConstants.UPDATE_SUCCESS, MessageConstants.SUCCESS);
        }

        public async Task<List<HolidayAndUserDto>> GetHolidayData()
        {
            List<HolidayAndUserDto> data = await _repositoryAccessor.Holiday.FindAll(true)
            .Include(x => x.User)
            .OrderBy(x => x.Date)
            .Select(x => new HolidayAndUserDto
            {
                HolidayID = x.HolidayID,
                Date = x.Date,
                Description = x.Description,
                UserID = x.UserID,
                FullName = x.User.FullName,
                CreateTime = x.CreateTime
            }).ToListAsync();

            return data;
        }

        public async Task<OperationResult> RemoveHoliday(int IDHoliday)
        {
            Holiday checkID = _repositoryAccessor.Holiday.FirstOrDefault(x => x.HolidayID == IDHoliday);
            if (checkID == null)
                return new OperationResult(false, MessageConstants.REMOVE_ERROR, MessageConstants.ERROR);

            _repositoryAccessor.Holiday.Remove(checkID);

            await _repositoryAccessor.SaveChangesAsync();
            return new OperationResult(true, MessageConstants.UPDATE_SUCCESS, MessageConstants.SUCCESS);
        }
    }
}