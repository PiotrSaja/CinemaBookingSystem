using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Common.Mailer.Abstractions;
using CinemaBookingSystem.Common.Mailer.Configuration;
using CinemaBookingSystem.Common.Mailer.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorEngineCore;

namespace CinemaBookingSystem.Common.Mailer.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _settings;

        #region MailService()
        public MailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }
        #endregion

        #region SendAsync()
        public async Task<bool> SendAsync(MailData mailData, CancellationToken ct = default)
        {
            try
            {
                var mail = new MimeMessage();

                #region Sender / Receiver
                mail.From.Add(new MailboxAddress(_settings.DisplayName, mailData.From ?? _settings.From));
                mail.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName, mailData.From ?? _settings.From);

                foreach (string mailAddress in mailData.To)
                    mail.To.Add(MailboxAddress.Parse(mailAddress));

                if (!string.IsNullOrEmpty(mailData.ReplyTo))
                    mail.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName, mailData.ReplyTo));

                if (mailData.Bcc != null)
                {

                    foreach (string mailAddress in mailData.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                        mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }

                if (mailData.Cc != null)
                {
                    foreach (string mailAddress in mailData.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                        mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }
                #endregion

                #region Content

                var body = new BodyBuilder();
                mail.Subject = mailData.Subject;
                body.HtmlBody = mailData.Body;
                mail.Body = body.ToMessageBody();
                #endregion

                #region Send Mail
                using var smtp = new SmtpClient();
                if (_settings.UseSSL)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, ct);
                }
                else if (_settings.UseStartTls)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
                }
                await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, ct);
                await smtp.SendAsync(mail, ct);
                await smtp.DisconnectAsync(true, ct);

                #endregion

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region GetEmailTemplate()
        public string GetEmailTemplate<T>(string emailTemplate, T emailTemplateModel)
        {
            string mailTemplate = LoadTemplate(emailTemplate);
            IRazorEngine razorEngine = new RazorEngine();
            IRazorEngineCompiledTemplate modifiedMailTemplate = razorEngine.Compile(mailTemplate);
            return modifiedMailTemplate.Run(emailTemplateModel);
        }
        #endregion

        #region LoadTemplate()
        public string LoadTemplate(string emailTemplate)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string templateDir = Path.Combine(baseDir, "Mailer/Template");
            string templatePath = Path.Combine(templateDir, $"{emailTemplate}.cshtml");
            using FileStream fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
            string mailTemplate = streamReader.ReadToEnd();
            streamReader.Close();
            return mailTemplate;
        }
        #endregion
    }
}
