using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.ComponentModel;
using Ini;

namespace SMTPClass
{
    public class MailClass
    {
        public static bool bSent;

        public static void SendMail(string path, string subject, string attachmentFile, string toEmailList, string ccEmailList, string bccEmailList)
        {
            string SMTPServer;

            bSent = false;

            //IniFile ini = new IniFile(path + @"PinExceptions.ini");
            IniFile ini = new IniFile(path + @"\PinExceptions.ini");
            SMTPServer = ini.IniReadValue("SMTPServer", "SMTPServerName");
            
            // Command line argument must the the SMTP host.
            SmtpClient client = new SmtpClient(SMTPServer, 25);

            MailAddress from = new MailAddress("cardsupport@euronetworldwide.com",
               "GR-Cards-Customer-Support-Dl", System.Text.Encoding.UTF8);
            // Set destinations for the e-mail message.
            string[] toList = GetAddresses(toEmailList);
            MailAddress to = new MailAddress(toList[0]);

            // Specify the message content.
            MailMessage message = new MailMessage(from, to);
            for (int i = 1; i < toList.Length; i++)
                message.To.Add(toList[i]);

            if (ccEmailList != "")
            {
                string[] ccList = GetAddresses(ccEmailList);
                for (int i = 0; i < ccList.Length; i++)
                    message.CC.Add(ccList[i]);
            }

            if (bccEmailList != "")
            {
                string[] bccList = GetAddresses(bccEmailList);
                for (int i = 0; i < bccList.Length; i++)
                    message.Bcc.Add(bccList[i]);
            }

            message.Body = "";

            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = subject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            Attachment attachment = new Attachment(attachmentFile);
            message.Attachments.Add(attachment);

            client.UseDefaultCredentials = true;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // Clean up.
            message.Dispose();

            bSent = true;
        }

        private static string[] GetAddresses(string addressList)
        {
            string[] addresses = addressList.Split(new char[] { ';' });

            return addresses;
        }
    }
}
