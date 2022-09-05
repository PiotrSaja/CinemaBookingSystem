using System.Collections.Generic;

namespace CinemaBookingSystem.Common.Mailer.Models
{
    public class MailData
    {
        public List<string> To { get; set; }
        public List<string> Bcc { get; }
        public List<string> Cc { get; }
        public string? From { get; }
        public string? DisplayName { get; set; }
        public string? ReplyTo { get; set; }
        public string? ReplyToName { get; set; }
        public string Subject { get; set; }
        public string? Body { get; set; }

        #region MailData()
        public MailData(List<string> to, string subject, string? body = null, string? from = null, string? displayName = null, string? replyTo = null, string? replyToName = null, List<string>? bcc = null, List<string>? cc = null)
        {
            To = to;
            Bcc = bcc ?? new List<string>();
            Cc = cc ?? new List<string>();
            From = from;
            DisplayName = displayName;
            ReplyTo = replyTo;
            ReplyToName = replyToName;
            Subject = subject;
            Body = body;
        }

        public MailData()
        {
        }
        #endregion
    }
}
