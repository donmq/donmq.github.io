using System.Text.RegularExpressions;
using API._Repositories;
using API.Helpers.Enums;
using API.Models;
using Microsoft.EntityFrameworkCore;
namespace API.Helpers.Utilities
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface INotification
    {
        Task<string> StepbyStepApproval(Employee emp, int leaveID, string comment);
        Task<List<string>> GetPersonApproval(Employee emp);
        Task SendNotitoUser(string notitext, int? empid);
    }
    public class Notification : INotification
    {
        private readonly IRepositoryAccessor _repositoryAccessor;
        private readonly IFunctionUtility _functionUtility;
        private readonly IMailUtility _mailUtility;

        public Notification(
            IRepositoryAccessor repositoryAccessor,
            IFunctionUtility functionUtility,
            IMailUtility mailUtility)
        {
            _repositoryAccessor = repositoryAccessor;
            _functionUtility = functionUtility;
            _mailUtility = mailUtility;
        }

        public async Task<string> StepbyStepApproval(Employee emp, int leaveID, string comment)
        {
            string smtpsend = _functionUtility.HashPasswordUser("smtpsend");
            var smtp = await _repositoryAccessor.DefaultSetting.FindAll(q => q.GroupSett == 1 && q.KeySett == smtpsend).FirstOrDefaultAsync();
            string result = string.Empty;
            if (smtp.ValueSett.HasValue && smtp.ValueSett.Value)
            {
                try
                {
                    //Lấy danh sách những người nhận email
                    var listMail = await GetPersonApproval(emp);

                    //Gởi email thông báo
                    await NotificationApproval(listMail.Distinct().ToList(), comment);
                    return result = "ok";
                }
                catch (Exception ex)
                {
                    return result = ex.Message.ToString();
                }
            }
            else
            {
                return result = "smtp false";
            }
        }

        public async Task<List<string>> GetPersonApproval(Employee emp)
        {
            string partsym = emp.Part.PartSym;
            string deptsym = emp.Part.Dept.DeptSym;
            string buildsym = emp.Part.Dept.Building.BuildingSym;
            string areasym = emp.Part.Dept.Area.AreaSym;

            //Danh sách user có UserRank là 3 hoặc 5, email không trống và có đuôi "ssbshoes.com"
            var mails = await _repositoryAccessor.Users
            .FindAll(
                q => (q.UserRank == 3 || q.UserRank == 5) &&
                !string.IsNullOrWhiteSpace(q.EmailAddress) &&
                q.EmailAddress.Contains("ssbshoes.com")
            ).AsNoTracking()
            .Join(_repositoryAccessor.RolesUser.FindAll(
                    q => q.Role.RoleSym == partsym ||
                    q.Role.RoleSym == deptsym ||
                    q.Role.RoleSym == buildsym ||
                    q.Role.RoleSym == areasym).AsNoTracking(),
                x => x.UserID,
                y => y.UserID,
                (x, y) => new { User = x, Roles_User = y }
            ).Join(_repositoryAccessor.SetApproveGroupBase.FindAll(x => x.GBID == emp.GBID).AsNoTracking(),
                x => x.User.UserID,
                y => y.UserID,
                (x, y) => new { x.User, x.Roles_User, GroupBase = y }
            ).Select(x => x.User.EmailAddress).ToListAsync();

            if (!mails.Any())
            {
                string smtp_mail_null = _functionUtility.HashPasswordUser("smtp-mail-null");
                // đây là giá trị mặc định nếu không tìm được cấp trên xét duyệt
                return new List<string> { _repositoryAccessor.EmailContent.FindAll(q => q.GroupKey == 0 && q.KeyCont == smtp_mail_null).FirstOrDefault()?.Content };
            }
            else
            {
                return mails;
            }
        }

        private async Task NotificationApproval(List<string> mailto, string comment)
        {
            try
            {
                string factory = SettingsConfigUtility.GetCurrentSettings("AppSettings:Factory");
                string titlesmtp = EmailContentContants.title;
                string displaynamesmtp = EmailContentContants.displayname;

                var body = EmailContentContants.WaitingApprovalContent;
                body = Regex.Replace(body, "xx/xx/xxxx", DateTime.Now.ToString("MM/dd/yyyy"));
                body = Regex.Replace(body, "xxxxxxxxxx", comment);

                await _mailUtility.SendListMailAsync(mailto, titlesmtp, string.Format(body), "");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task SendNotitoUser(string notitext, int? empid)
        {
            string titlesmtp = _functionUtility.HashPasswordUser("smtpsend");

            var smtpSpec = await _repositoryAccessor.DefaultSetting.FindAll(x => x.GroupSett == 1 && x.KeySett == titlesmtp).FirstOrDefaultAsync();
            var mailto = await _repositoryAccessor.Users.FindAll(q => q.EmpID == empid).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(mailto.EmailAddress))
            {
                return;
            }
            else
            {
                if (smtpSpec.ValueSett.HasValue)
                {
                    var body = notitext;
                    await _mailUtility.SendMailAsync(mailto.EmailAddress.ToString(), "Thông báo từ nhân sự", string.Format(body), "");
                }
            }
        }
    }
}