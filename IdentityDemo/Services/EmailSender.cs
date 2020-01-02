using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace IdentityDemo.Services
{
    /// <summary>
    /// 邮件发送器
    /// </summary>
    public class EmailSender : IEmailSender
    {

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            // Logging Options
            _logger = logger;
            _logger.LogInformation($"SendGridEmail: {Options.SendGridEmail}, SendGridUser: {Options.SendGridUser}, SendGridKey: {Options.SendGridKey}");
        }
        ILogger<EmailSender> _logger { get; }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            // SendGrid 需要注册的，有免费使用次数
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Options.SendGridEmail, Options.SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }

        /// <summary>
        /// 认证消息发送器选项
        /// </summary>
        public class AuthMessageSenderOptions
        {
            /// <summary>
            /// 电子邮箱 SendGrid
            /// </summary>
            public string SendGridEmail { get; set; }
            /// <summary>
            /// 用户 SendGrid
            /// </summary>
            public string SendGridUser { get; set; }
            /// <summary>
            /// 授权码 SendGrid
            /// </summary>
            public string SendGridKey { get; set; }
        }

        /// <summary>
        /// 电子邮件发送器配置
        /// </summary>
        public class EmailSenderOptions
        {
            /// <summary>
            /// 电子邮箱
            /// </summary>
            public string Email { get; set; }
            /// <summary>
            /// 用户名
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// 授权码
            /// </summary>
            public string AuthorizationCode { get; set; }
        }
    }
}
