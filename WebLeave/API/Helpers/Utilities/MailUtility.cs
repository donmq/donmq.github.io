using Aspose.Email.Clients;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using System.Text;
using System.Diagnostics;
namespace API.Helpers.Utilities
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IMailUtility
    {
        void SendMail(string toMail, string subject, string content, string filePath);
        Task SendMailAsync(string toMail, string subject, string content, string filePath = "");
        Task SendListMailAsync(List<string> toMail, string subject, string content, string filePath = "");
    }

    public class MailUtility : IMailUtility
    {
        private readonly MailSettingServer _mailSetting = SettingsConfigUtility<MailSettingServer>.GetCurrentSettings("MailSettingServer");
        public MailUtility()
        {
        }

        public void SendMail(string toMail, string subject, string content, string filePath)
        {
            MailMessage mail = new()
            {
                From = new MailAddress(_mailSetting.FromEmail, _mailSetting.FromName)
            };

            mail.To.Add(toMail);
            mail.Subject = subject;
            mail.Body = content;
            Attachment attachment;
            attachment = new Attachment(filePath);
            mail.Attachments.Add(attachment);

            try
            {
                ConfigSmtp().Send(mail);
            }
            catch (Exception ex)
            {
                StringBuilder builder = new();
                builder.AppendFormat("Error: \"{0}\"{1}", "Có lỗi xảy ra!", Environment.NewLine);
                builder.AppendLine();
                builder.AppendFormat("ErrorTime: {0}{1}", DateTime.Now, Environment.NewLine);
                builder.AppendLine();
                builder.AppendFormat("ErrorSendingMail: {0}{1}", ex.Message, Environment.NewLine);
                builder.AppendLine();
                builder.AppendFormat("Lỗi khi gửi mail: {0}{1}", TranslateUtility.TranslateText(ex.Message), Environment.NewLine);

                EventLog.WriteEntry(".NET Runtime", builder.ToString(), EventLogEntryType.Error, 1000);
            }
        }

        public async Task SendMailAsync(string toMail, string subject, string content, string filePath = "")
        {
            MailMessage mail = new()
            {
                From = new MailAddress(_mailSetting.FromEmail, _mailSetting.FromName)
            };
            mail.To.Add(toMail);
            mail.Subject = subject;
            mail.Body = content;
            mail.HtmlBody = mail.Body;
            mail.BodyEncoding = Encoding.UTF8;
            mail.Priority = MailPriority.High;

            try
            {
                await ConfigSmtp().SendAsync(mail);
            }
            catch (Exception ex)
            {
                StringBuilder builder = new();
                builder.AppendFormat("Error: \"{0}\"{1}", "Có lỗi xảy ra!", Environment.NewLine);
                builder.AppendLine();
                builder.AppendFormat("ErrorTime: {0}{1}", DateTime.Now, Environment.NewLine);
                builder.AppendLine();
                builder.AppendFormat("ErrorSendingMail: {0}{1}", ex.Message, Environment.NewLine);
                builder.AppendLine();
                builder.AppendFormat("Lỗi khi gửi mail: {0}{1}", TranslateUtility.TranslateText(ex.Message), Environment.NewLine);

                EventLog.WriteEntry(".NET Runtime", builder.ToString(), EventLogEntryType.Error, 1000);
            }
        }

        public async Task SendListMailAsync(List<string> toMail, string subject, string content, string filePath = "")
        {
            MailMessage mail = new()
            {
                From = new MailAddress(_mailSetting.FromEmail, _mailSetting.FromName)
            };

            foreach (var item in toMail)
            {
                mail.To.Add(item);
            }
            mail.Subject = subject;
            mail.Body = content;
            mail.HtmlBody = mail.Body;
            mail.BodyEncoding = Encoding.UTF8;
            mail.Priority = MailPriority.High;

            try
            {
                await ConfigSmtp().SendAsync(mail);
            }
            catch (Exception ex)
            {
                StringBuilder builder = new();
                builder.AppendFormat("Error: \"{0}\"{1}", "Có lỗi xảy ra!", Environment.NewLine);
                builder.AppendLine();
                builder.AppendFormat("ErrorTime: {0}{1}", DateTime.Now, Environment.NewLine);
                builder.AppendLine();
                builder.AppendFormat("ErrorSendingMail: {0}{1}", ex.Message, Environment.NewLine);
                builder.AppendLine();
                builder.AppendFormat("Lỗi khi gửi mail: {0}{1}", TranslateUtility.TranslateText(ex.Message), Environment.NewLine);

                EventLog.WriteEntry(".NET Runtime", builder.ToString(), EventLogEntryType.Error, 1000);
            }
        }

        public SmtpClient ConfigSmtp()
        {
            SmtpClient smtpServer = new(_mailSetting.Server)
            {
                Port = Convert.ToInt32(_mailSetting.Port),
                Username = _mailSetting.UserName,
                Password = _mailSetting.Password,
                SecurityOptions = SecurityOptions.Auto,
                UseAuthentication = false,
                Timeout = 3600
            };
            return smtpServer;
        }
        public class MailSettingServer
        {
            public string Server { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string FromEmail { get; set; }
            public string FromName { get; set; }
            public string Port { get; set; }
            public string EnableSsl { get; set; }
            public string DefaultCredentials { get; set; }
        }
    }
}