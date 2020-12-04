using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_Notify
{
    static class SendMail
    {
        public static async Task Notify(Serilog.Core.Logger logger)
        {
            var ip = PublicIPManager.GetPublicIP(logger, Properties.Settings.Default.PublicIP_URL);
            logger.Information("Public IP is {0}", ip);

            var machineName = Environment.MachineName;
            logger.Debug("Machine name is {0}", machineName);

            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY", EnvironmentVariableTarget.Machine);
            if (string.IsNullOrEmpty(apiKey))
            {
                logger.Error("SendGrid API key is empty");
                return;
            }

            var fromEmail = Environment.GetEnvironmentVariable("SENDGRID_EMAIL", EnvironmentVariableTarget.Machine);
            if (string.IsNullOrEmpty(fromEmail))
            {
                logger.Error("SendGrid Email is empty");
                return;
            }

            var fromName = Environment.GetEnvironmentVariable("SENDGRID_NAME", EnvironmentVariableTarget.Machine);
            if (string.IsNullOrEmpty(fromName))
            {
                logger.Error("SendGrid Name is empty");
                return;
            }

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, fromName);
            var subject = $"{machineName} Public IP is {ip}";

            var tos = new List<EmailAddress>();
            foreach (var item in Properties.Settings.Default.EmailTo)
            {
                tos.Add(new EmailAddress(item));
            }

            if (Properties.Settings.Default.EmailTo.Count == 0)
            {
                logger.Error("Email list is empty");
                return;
            }

            var plainTextContent = $"{machineName} Public IP is {ip}";
            var htmlContent = $"<strong>{machineName}</strong> Public IP is <strong>{ip}</strong>";

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            var result = response.Body.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                logger.Information("email sent");
            }
            else
            {
                logger.Error("Status Code {0} and response content: {1}", response.StatusCode, result);
            }
        }
    }
}
