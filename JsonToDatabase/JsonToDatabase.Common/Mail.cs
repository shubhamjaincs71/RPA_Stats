using JsonToDatabase.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace JsonToDatabase.Common
{
    public class Mail
    {
        public bool Send(string mailFrom, string mailTo, string mailSubject, string mailBody)
        {
            bool isSuccess = false;
                try
                {
                    SmtpClient client = new SmtpClient
                    {
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Host = Assets.Mail_Host,
                        Port = Convert.ToInt32(Assets.Mail_Port)
                    };

                    MailMessage mail = new MailMessage(from: mailFrom, to: mailTo)
                    {
                        IsBodyHtml = true,
                        Subject = mailSubject
                    };
                    mail.Body = mailBody;

                    client.Send(mail);

                    isSuccess = true;
                }
                catch (Exception)
                {

                }

            return isSuccess;
        }

        public bool SendWithAttachment(bool isSendMail, string mailFrom, string mailTo, string mailCC, string mailBcc, string mailSubject, string mailBody, string username, Attachment attachment)
        {
            bool isSuccess = false;

            if (isSendMail)
            {
                try
                {
                    SmtpClient client = new SmtpClient
                    {
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Host = Assets.Mail_Host,
                        Port = Convert.ToInt32(Assets.Mail_Port)
                    };

                    MailMessage mail = new MailMessage(from: mailFrom, to: mailTo)
                    {
                        IsBodyHtml = true,
                        Subject = mailSubject,

                    };

                    if (!string.IsNullOrEmpty(mailCC))
                        mail.CC.Add(mailCC);

                    if (!string.IsNullOrEmpty(mailBcc))
                        mail.Bcc.Add(mailBcc);

                    if (attachment != null)
                    {
                        mail.Attachments.Add(attachment);
                    }

                    mail.Body = mailBody;

                    client.Send(mail);

                    isSuccess = true;
                }
                catch (Exception)
                {

                }
            }

            return isSuccess;
        }
    }
}
